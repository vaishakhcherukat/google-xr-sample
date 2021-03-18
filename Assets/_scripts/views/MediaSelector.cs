using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MediaSelector : MonoBehaviour
{
    public IMediaDownloadManager mediaManager;

    public static IMediaDownloadManager mediaManagerDesc;

    public RectTransform centerPanel;
    public Experience experience = null;

    public Button playButton;

    public static MediaSelector selectionPanel;

    const string isToastDisplayed = "isToastDisplayed";

    // Use this for initialization
    void Start()
    {
        playButton.gameObject.SetActive(true);
        playButton.onClick.AddListener(PlayMedia);
    }

    public DownloadIndicator DownloadIndicator()
    {
        return GetComponentInChildren<DownloadIndicator>();
    }

    public void InitializeUI()
    {
        
        if (mediaManager != null)
        {
            //DownloadIndicator().MediaManager = mediaManager;

            LayoutRebuilder.ForceRebuildLayoutImmediate(centerPanel.GetComponent<RectTransform>());

            var currentSize = GetComponent<RectTransform>().sizeDelta;

            if (currentSize.y < centerPanel.sizeDelta.y)
            {
                // normally we could add a SizeFitter component to handle this automatically,
                // but we can't because our main parent contains a VerticalLayoutGroup
                GetComponent<RectTransform>().sizeDelta += new Vector2(0, centerPanel.sizeDelta.y - currentSize.y);
            }
        }

    }

    void PlayMedia()
    {
        Debug.Log("******************");
        Debug.Log(mediaManager.QuestionsFileCount());
        Debug.Log(mediaManager.CurrentVideoName());
        Debug.Log("******************");
       
        UIManager2D.MediaSelected(this);
            
    }




    public void SetMediaManager(IMediaDownloadManager manager)
    {
        mediaManager = manager;
        experience = manager.experience;
        InitializeUI();
    }


}

