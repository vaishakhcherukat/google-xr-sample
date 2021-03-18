using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR;
using UnityEngine.UI;

public class UIManager2D : MonoBehaviour
{

    public static UIManager2D Instance = null;


    // Use this for initialization
    void Start()
    {
        // Set framerate to 60 for better scrolling performance
        Application.targetFrameRate = 60;
        StartCoroutine("SwitchTo2D");

    }
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    
    // public void Authenticate()
    // {
    //     //authenticationPrompt.SetActive(false);
    // }

    public static void MediaSelected(MediaSelector mediaSelector)
    {
        Debug.Log("mediaSelector.mediaManager : " + mediaSelector.mediaManager);
        MediaLibrary.SelectMedia(mediaSelector.mediaManager);
        SceneManager.LoadScene("XRMigrated");

    }

    IEnumerator SwitchTo2D()
    {
        // Empty string loads the "None" device.
      //  XRSettings.LoadDeviceByName("");

        // Must wait one frame after calling `XRSettings.LoadDeviceByName()`.
        yield return null;

        Screen.orientation = ScreenOrientation.Portrait;

        // Not needed, since loading the None (`""`) device takes care of this.
        // XRSettings.enabled = false;

        // Restore 2D camera settings.
        ResetCameras();
#if UNITY_ANDROID
        Screen.fullScreen = false;
#endif
    }
  

    void ResetCameras()
    {
        // Camera looping logic copied from GvrEditorEmulator.cs
        for (int i = 0; i < Camera.allCameras.Length; i++)
        {
            Camera cam = Camera.allCameras[i];
            if (cam.enabled && cam.stereoTargetEye != StereoTargetEyeMask.None)
            {

                // Reset local position.
                // Only required if you change the camera's local position while in 2D mode.
                cam.transform.localPosition = Vector3.zero;

                // Reset local rotation.
                // Only required if you change the camera's local rotation while in 2D mode.
                cam.transform.localRotation = Quaternion.identity;

                // No longer needed, see issue github.com/googlevr/gvr-unity-sdk/issues/628.
                // cam.ResetAspect();

                // No need to reset `fieldOfView`, since it's reset automatically.
            }
        }
    }
}
