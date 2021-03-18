using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class Timer : MonoBehaviour
{
    public VideoPlayer player;

    public float elapsedTime;

    public bool videoEnded;
    public bool hasVideo;

    bool timerIsCounting;
    public IMediaDownloadManager mediaManager;

    void Start()
    {
        player.loopPointReached += EndOfVideo;
        InitMediaManager();
    }

    void Update()
    {
        if (hasVideo)
        {
            elapsedTime = (float)player.time;
        }
        else
        {
            elapsedTime += Time.deltaTime;
        }
    }

    void EndOfVideo(VideoPlayer player)
    {
        videoEnded = true;
    }

    public void SetPlaybackSpeed(float _val)
    {
        player.playbackSpeed = _val;
    }

    public void InitializeTimer(string videoPath = null)
    {
        SetPlaybackSpeed(1.0f);
        timerIsCounting = false;
        elapsedTime = 0.0f;
        videoEnded = false;


        if (videoPath != null)
        {
            hasVideo = true;
            player.url = videoPath;
            TimerStart();
        }
        else
        {
            hasVideo = false;
            TimerStart();
        }

        CheckForHomeVisitVideo();
    }

    public void InitMediaManager()
    {
        mediaManager = GetComponent<TestManager>().mediaManager;
    }

    private void CheckForHomeVisitVideo()
    {
        if (mediaManager != null)
        {
                if (mediaManager.MediaIndex() == 1)
                {
                    SetPlaybackSpeed(5.0f);
                }
        }
    }

    public void TimerStart()
    {
        if (hasVideo)
        {
            player.Play();
        }

        timerIsCounting = true;
    }

    public void TimerStop()
    {
        if (hasVideo)
        {
            player.Pause();
        }

        timerIsCounting = false;
    }

    public bool VideoFinished()
    {
        return player.url == null || videoEnded;
    }

    public bool TimerIsCounting()
    {
        return timerIsCounting && !videoEnded;
    }

    public bool TimerIsPaused()
    {
        return !TimerIsCounting();
    }
}
