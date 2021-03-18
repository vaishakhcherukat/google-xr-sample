using UnityEngine.Events;

public interface IMediaDownloadManager
{
    Experience experience { get; set; }
    string name { get; }

    int MediaIndex();
    int QuestionsFileCount();
    IDownloadTracker DownloadTracker();
    
    void PrepareVideoPath(Timer videoTimer);
    bool HasVideo(string videoName);
    string JSONQuestions();
    void LoadVideo(string videoName);
    string CurrentVideoName();
    void AdvanceVideo();


    UnityEvent FinishedDownloadingEvent();
    UnityEvent NoVideosEvent();

}
