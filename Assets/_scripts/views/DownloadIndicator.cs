using UnityEngine;
using UnityEngine.UI;

public enum DownloadState
{
    New,
    Downloading,
    Error,
    Pause,
    Downloaded
}

public class DownloadIndicator : MonoBehaviour
{
    public IMediaDownloadManager mediaManager;

    public DownloadState downloadState = DownloadState.New;


    public IMediaDownloadManager MediaManager
    {
        set
        {
            mediaManager = value;
            UpdateIndicatorState();
        }
    }

    public IDownloadTracker DownloadTracker()
    {
        return mediaManager.DownloadTracker();
    }

   

    void UpdateIndicatorState()
    {
        Debug.Log("Inside Update");
    }


    // public SVGImage MainIcon()
    // {
    //     return GetComponent<SVGImage>();
    // }

}
