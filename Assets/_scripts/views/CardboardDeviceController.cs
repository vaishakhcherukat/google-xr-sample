using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.SceneManagement;

public class CardboardDeviceController : MonoBehaviour
{
    void Start()
    {

        StartCoroutine("SwitchToVR");
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene("LoginUI");
        }
        // To view framerate logging on android: `adb logcat | grep -i Frame`
        //Debug.Log("Frames: " + (1.0f / Time.smoothDeltaTime));
    }


    // More Info: https://developers.google.com/vr/develop/unity/guides/hybrid-apps
    // Call via `StartCoroutine(SwitchToVR())` from your code. Or, use
    // `yield SwitchToVR()` if calling from inside another coroutine.
    IEnumerator SwitchToVR()
    {
        string desiredDevice = "cardboard";
        Screen.orientation = ScreenOrientation.LandscapeLeft;


        // Some VR Devices do not support reloading when already active, see
        // https://docs.unity3d.com/ScriptReference/XR.XRSettings.LoadDeviceByName.html
        if (String.Compare(XRSettings.loadedDeviceName, desiredDevice, true) != 0)
        {
            XRSettings.LoadDeviceByName(desiredDevice);
            // Must wait one frame after calling `XRSettings.LoadDeviceByName()`.
            yield return null;
        }

        // Now it's ok to enable VR mode.
        XRSettings.enabled = true;

        // Need to set this directly for iOS instead of relying on Quality Settings from Unity:
        Application.targetFrameRate = 60;
        QualitySettings.vSyncCount = 0;
#if UNITY_ANDROID
        Screen.fullScreen = true;
#endif
    }
}
