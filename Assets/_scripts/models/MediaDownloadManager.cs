using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Video;
using System.Linq;
using UnityEngine.Networking;
using System.Globalization;
using UnityEngine.Events;
using System.Threading;

[RequireComponent(typeof(DownloadTracker))]
public class MediaDownloadManager : MonoBehaviour, IMediaDownloadManager
{
    Thread fileCopyThread;
    string nextVideoPath;
    Timer videoTimer;


    /// <summary>
    /// Indicates whether or not the current device is running Android 10+. We
    /// currently detect this, so we can copy media to a safe directory for
    /// playback. More information on this issue can be found at the following
    /// links:
    ///   - https://forum.unity.com/threads/error-videoplayer-on-android.742451/
    ///   - https://issuetracker.unity3d.com/issues/android-video-player-cannot-play-files-located-in-the-persistent-data-directory-on-android-10
    ///   - https://commonsware.com/blog/2019/06/07/death-external-storage-end-saga.html
    /// </summary>
    /// <returns><c>true</c>, if this device is running Android 10 or higher, <c>false</c> otherwise.</returns>
    private static bool CanReadFromPermanentStorage()
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        using (var version = new AndroidJavaClass("android.os.Build$VERSION"))
        {
            return version.GetStatic<int>("SDK_INT") < 29; // Android 10 is SDK level 29.
        }
#else
        return true;
#endif
    }


    public string subfolder;
    public TextAsset[] questionDataFiles;

    public IDownloadTracker downloadTracker;

    [SerializeField]
    private Experience _experience;

    public Experience experience
    {
        get { return _experience; }
        set
        {
            _experience = value;
            DownloadTracker().Experience = value;

        }
    }

    private int mediaIndex;

    public int MediaIndex()
    {
        return mediaIndex;
    }

    public TextAsset[] QuestionDataFiles()
    {
        return questionDataFiles;
    }

    private void Awake()
    {
        questionDataFiles = new TextAsset[0];
        fileCopyThread = null;
    }

    void Update()
    {
        // Wait for fileCopyThread to finish and then activate Timer
        if (fileCopyThread != null && !fileCopyThread.IsAlive)
        {
            LoadingIndicator.Deactivate();
            Debug.Log("nextVideoPath  : " + nextVideoPath);
            videoTimer.InitializeTimer(nextVideoPath);
            fileCopyThread = null;
        }
    }

    public void Initialize()
    {
        mediaIndex = 0;
        PopulateQuestionData();
    }

    public int QuestionsFileCount()
    {
        return QuestionDataFiles().Length;
    }

    public string Subfolder()
    {
        return "homevisit";
    }

    public void PopulateQuestionData()
    {
        var questionResources = Resources.LoadAll("vrMedia/" + Subfolder(), typeof(TextAsset));
        questionDataFiles = questionResources.OrderBy(file => file.name).Cast<TextAsset>().ToArray();
    }

    public IDownloadTracker DownloadTracker()
    {
        if (downloadTracker == null)
        {
            downloadTracker = (IDownloadTracker)GetComponent<DownloadTracker>();
        }
        return downloadTracker;
    }

    public IDownloadTracker StartDownloads()
    {
        DownloadTracker().StartDownloads(
            QuestionDataFiles().Select(dataFile => dataFile.name)
        );
        return downloadTracker;
    }

    public IDownloadTracker ResumeDownloads()
    {
        DownloadTracker().ResumeDownloads(
            QuestionDataFiles().Select(dataFile => dataFile.name)
        );
        return downloadTracker;
    }

    public void DeleteVideos()
    {
        DownloadTracker().Experience = experience;
        DownloadTracker().DeleteVideos();
    }

    public bool HasVideo(string videoName)
    {
        for (int i = 0; i < QuestionsFileCount(); i++)
        {
            if (QuestionDataFiles()[i].name == videoName)
            {
                return true;
            }
        }
        return false;
    }

    public string CurrentVideoName()
    {
        return QuestionDataFiles()[mediaIndex].name;
    }

    public void PrepareVideoPath(Timer videoTimer)
    {
        string videoKey = CurrentVideoName() + ".mp4";
        string storagePath = MediaUtils.VideoPath(videoKey);
        string tempLocation = MediaUtils.TemporaryPath(videoKey);
        storagePath = MediaUtils.VideoStreamPath(videoKey);
        tempLocation = MediaUtils.VideoStreamPath(videoKey);
        Debug.Log("Path : " + storagePath);
        PrepareTimer(videoTimer, storagePath, tempLocation);
    }

    void PrepareTimer(Timer videoTimer, string storagePath, string tempLocation)
    {

        // As of Android 10, Unity does not allow reading from the download
        // path, so it's necessary to copy media to a temporary location. Other
        // versions and platforms are not affected.
        if (!HasCompleteVideos())
        {
            videoTimer.InitializeTimer(storagePath);
        }
        else
        {
            if (CanReadFromPermanentStorage())
            {
                videoTimer.InitializeTimer(storagePath);
            }
            else
            {
                this.videoTimer = videoTimer;
                nextVideoPath = tempLocation;
                LoadingIndicator.Activate();
                fileCopyThread = new Thread(() => CopyPath(storagePath, tempLocation));
                fileCopyThread.Start();
            }
        }
    }

    void CopyPath(string storagePath, string tempLocation)
    {
        if (File.Exists(tempLocation))
        {
            File.Delete(tempLocation);
        }
        File.Copy(storagePath, tempLocation);
    }

    public string JSONQuestions()
    {
        return QuestionDataFiles()[mediaIndex].text;
    }

    public void AdvanceVideo()
    {
        for (int i = mediaIndex + 1; i < QuestionsFileCount(); i++)
        {
           
            {
                if (QuestionDataFiles()[i].name.IndexOf("", 0, QuestionDataFiles()[i].name.Length, System.StringComparison.Ordinal) >= 0)
                {
                    mediaIndex = i;
                    Debug.Log("mediaIndex : " + mediaIndex);
                    return;
                }
            }
            
        }

        mediaIndex = 0;
    }

    public void LoadVideo(string videoName)
    {
        for (int i = 0; i < QuestionsFileCount(); i++)
        {
            if (QuestionDataFiles()[i].name == videoName)
            {
                mediaIndex = i;
                return;
            }
        }
        // else (video not found)
        AdvanceVideo();
    }

    public UnityEvent FinishedDownloadingEvent()
    {
        return DownloadTracker().FinishedDownloadingEvent();
    }

    public UnityEvent NoVideosEvent()
    {
        return DownloadTracker().NoVideosEvent();
    }

    public bool IsDownloading()
    {
        return DownloadTracker().IsDownloading();
    }

    public bool HasDownloadErrors()
    {
        return DownloadTracker().HasDownloadErrors();
    }

    public bool HasCompleteVideos()
    {
        return DownloadTracker().HasCompleteVideos();
    }
}
