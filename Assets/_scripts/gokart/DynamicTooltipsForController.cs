using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(MeshRenderer))]
public class DynamicTooltipsForController : MonoBehaviour
{
    public Texture optionToHideQ;
    public Texture optionToShowQ;

    private static Material _tooltips;
    private static DynamicTooltipsForController _instance;

    void Start()
    {
        _tooltips = GetComponent<MeshRenderer>().material;
        _instance = this;
    }

    /// <summary>
    /// The app button shows/hides the UI question. This method dynamically updates the tooltips so it says "show question"
    /// if the question is hidden, and "hide question" if the question is currently showing.
    /// </summary>
    /// <param name="isUIVisible">If set to <c>true</c>, the user interface is visible.</param>
    public static void UpdateTooltips(bool isUIVisible)
    {
        if (isUIVisible)
        {
            _tooltips.mainTexture = _instance.optionToHideQ;
        }
        else
        {
            _tooltips.mainTexture = _instance.optionToShowQ;
        }
    }
}
