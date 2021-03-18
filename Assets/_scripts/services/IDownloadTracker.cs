using System.Collections.Generic;
using UnityEngine.Events;

public interface IDownloadTracker
{
    Experience Experience
    {
        get;
        set;
    }

    string Subfolder();
    void StartDownloads(IEnumerable<string> questionDataKeys);

    void ResumeDownloads(IEnumerable<string> questionDataKeys);

    bool IsDownloading();
    void PauseDownload(bool status);
    float Progress();
    void DeleteVideos();

    bool HasDownloadErrors();
    bool HasCompleteVideos();

    UnityEvent FinishedDownloadingEvent();
    UnityEvent NoVideosEvent();
}
