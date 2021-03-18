using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Question : MonoBehaviour
{
    public Option optionPrefab;
    public Transform optionsContainer;
    public Text questionText;
    public bool isBranching;
    public int numPoints;
    public float optionDelay;
    public HoverMap hoverMap;

    public void Initialize(QuestionData questionData)
    {
        isBranching = questionData.isBranching;
        numPoints = questionData.numPoints;
        questionText.text = questionData.text;
        optionDelay = (questionData.optionDelay >= 0) ? questionData.optionDelay : 1.0f;
        if (!string.IsNullOrEmpty(questionData.hoverMapName))
        {
            hoverMap = Resources.Load<HoverMap>("_hoverables/" + questionData.hoverMapName);
        }
        StartCoroutine(CreateOptions(questionData.options, optionDelay));
    }

    public bool HasHoverMap()
    {
        return hoverMap != null;
    }

    IEnumerator CreateOptions(List<OptionData> options, float delay)
    {

        yield return new WaitForSeconds(delay);
        Debug.Log("Created options");
        // dispatch options available event
        SendMessageUpwards("OnOptionsAvailable");

        foreach (var option in options)
        {
            var curOption = Instantiate(optionPrefab, optionsContainer) as Option;
            curOption.Initialize(option, numPoints);
        }

    }

}
