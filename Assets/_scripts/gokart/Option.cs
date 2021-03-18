using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Option : MonoBehaviour
{
    public bool isCorrectAnswer;
    public string explanationText;
    public string explanationBtnText;
    public int numPoints;

    // Use this for initialization
    void Start()
    {
        SetCustomListener(AnswerSelected);
    }

    public void AnswerSelected()
    {
        SendMessageUpwards("OnAnswerSelected", this);
        Debug.Log("OnAnswerSelected");
    }

    public void Initialize(OptionData optionData, int numQuestionPoints)
    {
        SetOptionText(optionData.text);
        explanationText = optionData.explanation;
        explanationBtnText = optionData.explanationBtn;
        isCorrectAnswer = optionData.isCorrect;
        numPoints = numQuestionPoints;
    }

    private void SetOptionText(string optionText)
    {
        transform.Find("Text").GetComponent<Text>().text = optionText;
    }

    public void SetCustomListener(UnityEngine.Events.UnityAction callback)
    {
        var btn = GetComponent<Button>();
        btn.onClick.AddListener(callback);
    }

    public string NextVideoName()
    {
        return explanationText;
    }

    public bool IsContinued()
    {
        return explanationText.Equals("") || explanationText.Equals("_CONTINUE_");
    }

}
