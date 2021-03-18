using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Contains data for a single VR experience (such as "Addiction" or "Child Welfare").
[System.Serializable]
public class Experience
{
    public string title;


    public List<Download> downloads;


    public List<Download> Downloads()
    {
        if (downloads == null || downloads.Count == 0)
        {
            var stats_resource = "vrMedia/homevisit_stats";
            var downloadStats = Resources.Load<TextAsset>(stats_resource);

            if (downloadStats != null)
            {
                JsonUtility.FromJsonOverwrite(downloadStats.text, this);
            }
            // else
            // {
            //     Debug.Log("Unable to find download stats for " + stats_resource);
            //     downloads = new List<Download>();
            // }
        }
        return downloads;
    }

}
