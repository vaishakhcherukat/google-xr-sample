using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SimpleJSON;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class MediaLibrary : MonoBehaviour
{

    public static Experience currentExperience;
    public HorizontalOrVerticalLayoutGroup mediaSelectorGroup;


    void Start()
    {
        LoadFromLocal();
    }

    /// <summary>
    /// Loads metadata for each VR Experience from Local JSON.
    /// to load JSON from the 'local_api_sample' TextAsset instead of a remote server.
    /// </summary>
    /// <returns></returns>
    public void LoadFromLocal()
    {
        var textName = "_development/api_sample";
        var downloadStats = Resources.Load<TextAsset>(textName);
        string json = downloadStats.text;
        json = "{ \"data\": " + json + "}";
        PopulateMediaFromJSON(json);
    }

    public void PopulateMediaFromJSON(string json)
    {
        var mediaSelectors = new List<MediaSelector>();
        var apiContent = JsonUtility.FromJson<ApiContent>(json);

        foreach (var course in apiContent.data)
        {
           
            foreach (var experience in course.Experiences())
            { Debug.Log("$$$$$$$$$$" + experience);
                // experience.courseId = course.courseCode;
                var selector = PopulateMediaSelector(MediaDownloadManagerFor(experience));
                mediaSelectors.Add(selector);
            }
                
        }

    }
    MediaSelector PopulateMediaSelector(MediaDownloadManager mediaManager)
    {
     
        Debug.Log("Media manager :  " + mediaManager);
        GameObject selectorPanel = Object.Instantiate(Resources.Load<GameObject>("_prefabs/MediaSelectorPanel"));
        var mediaSelector = selectorPanel.GetComponent<MediaSelector>();
        selectorPanel.name = mediaManager.Subfolder() + " SelectionPanel";
        selectorPanel.SetActive(true);
        selectorPanel.transform.SetParent(mediaSelectorGroup.transform, false);
        mediaSelector.SetMediaManager(mediaManager);
        mediaManager.transform.SetParent(mediaSelector.transform);
        return mediaSelector;

    }

    static public void SelectMedia(IMediaDownloadManager mediaManager)
    {
        currentExperience = mediaManager.experience;
        Debug.Log("currentExperience : " + currentExperience);
    }

    static public MediaDownloadManager MediaDownloadManagerFor(Experience experience)
    {
        MediaDownloadManager manager = Object.Instantiate(Resources.Load<MediaDownloadManager>("_prefabs/MediaDownloadManager"));
        experience.title = "Home Visit";
        manager.name = experience.title + " MediaDownloadManager";

        manager.experience = experience;
        manager.Initialize();
        return manager;
    }
}
