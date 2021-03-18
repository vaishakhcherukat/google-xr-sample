using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;

public class DownloadTracker : MonoBehaviour, IDownloadTracker
{
    public float MINIMUM_PROGRESS = 0.1f;

    float updateCounter;

    public string Subfolder()
    {
        return "homevisit";
    }

    public Experience Experience { get; set; }

    public List<UnityWebRequest> downloaders;

    List<Coroutine> downloadRoutines;

    long totalBytesNeeded = 0;
    float progress = 0;

    public bool downloading = false;
    int remainingFiles;
    bool errors = false;

    public UnityEvent finishedDownloading;
    public UnityEvent noVideos;
    public UnityEvent downloadError;

    void Awake()
    {
        Application.runInBackground = true;
        downloaders = new List<UnityWebRequest>();
        downloadError.AddListener(HandleDownloadError);
        downloadRoutines = new List<Coroutine>();
        ResetDownloadStats();
    }

    void Update()
    {
        if ((!Application.runInBackground) || (Input.GetKeyDown(KeyCode.Escape)))
        {
            Application.runInBackground = true;
        }

        if (!downloading)
        {
            return;
        }

        if (progress < MINIMUM_PROGRESS)
        {
            progress += Time.deltaTime * MINIMUM_PROGRESS;
        }

        // recalculate progress only once per second
        updateCounter -= Time.deltaTime;

        if (updateCounter <= 0)
        {
            float totalDownloaded = downloaders.Select(request => (float)request.downloadedBytes).Sum();

            if (totalBytesNeeded > 0)
            {
                float newProgress = totalDownloaded / totalBytesNeeded;
                if (newProgress > MINIMUM_PROGRESS)
                {
                    progress = newProgress;
                }
            }
            else
            {
                progress = 1.0f;
            }
            updateCounter = 1;

        }
    }

    public UnityEvent FinishedDownloadingEvent()
    {
        return finishedDownloading;
    }

    public UnityEvent NoVideosEvent()
    {
        return noVideos;
    }

    public void PauseDownloadVideo()
    {
        errors = false;
        lock (downloaders)
        {
            foreach (var downloadRoutine in downloadRoutines)
            {
                StopCoroutine(downloadRoutine);
            }
            foreach (var downloader in downloaders)
            {
                downloader.Abort();
            }
            // ResumeDownloadVideo();
            //downloadRoutines.Clear();
            //downloaders.Clear();

        }
        //finishedDownloading.Invoke();
    }

    public void HandleDownloadError()
    {
        errors = true;
        lock (downloaders)
        {
            foreach (var downloadRoutine in downloadRoutines)
            {
                StopCoroutine(downloadRoutine);
            }
            foreach (var downloader in downloaders)
            {
                downloader.Abort();
            }
            downloadRoutines.Clear();
            downloaders.Clear();

        }
        finishedDownloading.Invoke();
    }

    public void ResetDownloadStats()
    {
        totalBytesNeeded = 0;
        progress = 0;
        remainingFiles = 0;
        errors = false;
    }

    public void StartDownloads(IEnumerable<string> questionDataKeys)
    {
        ResetDownloadStats();
        downloading = true;
        downloaders.Clear();
        remainingFiles = Experience.Downloads().Count;
        lock (downloaders)
        {
            foreach (var download in Experience.Downloads())
            {
                string videoPath = MediaUtils.VideoPath(download.name);
                var fileInfo = new FileInfo(videoPath);

                if (fileInfo.Exists && fileInfo.Length == download.bytes)
                {
                    //                    Debug.Log("File size " + download.bytes + " already downloaded: " + videoPath);

                    remainingFiles -= 1;
                    CheckForCompleteDownload();

                }
                else
                {
                    totalBytesNeeded += download.bytes;
                    downloadRoutines.Add(StartCoroutine("DownloadVideo", download.name));
                }
            }
        }
    }

    public void ResumeDownloads(IEnumerable<string> questionDataKeys)
    {
        Debug.Log("Inside ResumeDownloads");
        // ResetDownloadStats();
        downloading = true;
        // downloaders.Clear();
        remainingFiles = Experience.Downloads().Count;
        Debug.Log("remainingFiles : "+ remainingFiles);

        lock (downloaders)
        {
            foreach (var download in Experience.Downloads())
            {
                string videoPath = MediaUtils.VideoPath(download.name);
                Debug.Log("videoPath : " + videoPath);
                var fileInfo = new FileInfo(videoPath);
                // Debug.Log("fileInfo.Length : " + fileInfo.Length);
                // Debug.Log("download.bytes : " + download.bytes);
                // Debug.Log("fileInfo.Exists : " + fileInfo.Exists);
                if (fileInfo.Exists && fileInfo.Length == download.bytes)
                {
                    Debug.Log("File size " + download.bytes + " already downloaded: " + videoPath);

                    remainingFiles -= 1;
                    CheckForCompleteDownload();
                }
                else
                {
                    totalBytesNeeded += download.bytes;
                    downloadRoutines.Add(StartCoroutine("DownloadVideo", download.name));
                }
            }
        }
    }
    public void DeleteVideos()
    {
        noVideos.Invoke();
        foreach (var download in Experience.Downloads())
        {
            string videoPath = MediaUtils.VideoPath(download.name);
            var fileInfo = new FileInfo(videoPath);

            if (fileInfo.Exists)
            {
                File.Delete(videoPath);
            }
        }
    }

    public void CheckForCompleteDownload()
    {
        if (remainingFiles != 0)
        {
            return;
        }

        float totalDownloaded = downloaders.Select(request => (float)request.downloadedBytes).Sum();

        if (Mathf.Approximately(totalDownloaded, totalBytesNeeded))
        {
            //            Debug.Log("Download complete for : " + Subfolder());
            finishedDownloading.Invoke();
        }
    }

    public void PauseDownload(bool status)
    {
        downloading = !status;
    }

    public bool IsDownloading()
    {
        lock (downloaders)
        {
            return downloaders.All(item => item.isDone);
        }
    }

    /// <summary>
    /// Returns true if any files were partially downloaded.
    /// Assumes a download is not currently in progress.
    /// Also assumes zero files indicates no errors even though a download
    /// could have errored out before writing data to any of the files.
    /// </summary>
    public bool HasDownloadErrors()
    {
        if (errors)
        {
            return true;
        }
        int completeFiles = 0;
        if (Experience == null)
        {
            Debug.Log("null experience: ");
            return false;
        }

        foreach (var download in Experience.Downloads())
        {
            string videoPath = MediaUtils.VideoPath(download.name);
            var fileInfo = new FileInfo(videoPath);

            if (fileInfo.Exists)
            {
                if (fileInfo.Length == download.bytes)
                {
                    completeFiles++;
                }
                else
                {
                    Debug.Log("File size mismatch: " + videoPath);
                    // HasError
                    return true;
                }
            }
        }
        return (completeFiles > 0) && (completeFiles != Experience.Downloads().Count);
    }

    public bool HasCompleteVideos()
    {
        int countedFiles = 0;
        foreach (var download in Experience.Downloads())
        {
            string videoPath = MediaUtils.VideoPath(download.name);
            var fileInfo = new FileInfo(videoPath);

            if (fileInfo.Exists && fileInfo.Length == download.bytes)
            {
                countedFiles++;
            }
            else
            {
                return false;
            }
        }

        return countedFiles == Experience.Downloads().Count;
    }

    public float Progress()
    {
        // recalculate progress only once per second
        return progress;
    }

    IEnumerator DownloadVideo(string videoName)
    {
        yield return true;
    }
}