using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class RadialProgressBar : MonoBehaviour
{
    public Image progressBar;
    public float secondsToFill;

    private static Coroutine _progressBarCoroutine;

    private static RadialProgressBar _instance;

    void Start()
    {
        _instance = this;
        progressBar.fillAmount = 0;
        _instance.gameObject.SetActive(false);
        gameObject.SetActive(false);
    }

    public static void StartProgress(Button hoveredButton)
    {
        _instance.gameObject.SetActive(true);
        _progressBarCoroutine = _instance.StartCoroutine(_instance.RadialFill(hoveredButton));
    }

    public static void StopProgress()
    {
        if (_progressBarCoroutine == null)
        {
            return;
        }
        _instance.StopCoroutine(_progressBarCoroutine);
        _progressBarCoroutine = null;
        _instance.gameObject.SetActive(false);
        _instance.progressBar.fillAmount = 0;
    }

    private IEnumerator RadialFill(Button hoveredButton)
    {
        var timeElapsed = 0.0f;
        var stepSize = 0.05f;
        var fillPerFrame = 1.0f / (secondsToFill / stepSize);
        while (timeElapsed < secondsToFill)
        {
            yield return new WaitForSeconds(stepSize);
            progressBar.fillAmount += fillPerFrame;
            timeElapsed += stepSize;
        }
        if (hoveredButton == null)
        {
            Debug.LogError("Gaze selected button is null!");
            yield break;
        }
        hoveredButton.onClick.Invoke();
        _instance.gameObject.SetActive(false);
    }
}
