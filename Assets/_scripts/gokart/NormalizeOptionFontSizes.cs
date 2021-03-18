using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NormalizeOptionFontSizes : MonoBehaviour
{

    void Awake()
    {
        var optionTexts = GetComponentsInChildren<Text>();
        StartCoroutine(NormalizeFontSizes(optionTexts));
    }

    void OnTransformChildrenChanged()
    {
        var optionTexts = GetComponentsInChildren<Text>();
        StartCoroutine(NormalizeFontSizes(optionTexts));
    }

    private IEnumerator NormalizeFontSizes(Text[] texts)
    {
        // Wait one frame so Unity can compute best fit font size
        yield return null;
        int minFontSize = 1000000; //arbitrary large number that's bigger than any expected font size
        foreach (var t in texts)
        {
            var bestFitFontSize = t.cachedTextGenerator.fontSizeUsedForBestFit;
            minFontSize = Mathf.Min(minFontSize, bestFitFontSize);
        }
        foreach (var t in texts)
        {
            t.resizeTextMaxSize = minFontSize;
        }
    }

}
