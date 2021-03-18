using System.IO;
using UnityEngine;

public class MediaUtils
{

    public static string VideoPath(string videoName)
    {
        return Path.Combine(Application.persistentDataPath, videoName);
    }

    public static string TemporaryPath(string videoName)
    {
        return Path.Combine(Application.temporaryCachePath, videoName);
    }
    public static string VideoStreamPath(string videoName)
    {
        return Path.Combine(Application.streamingAssetsPath,videoName);
    }
}
