using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


/// <summary>
/// This displays the score to the first question asked in a scene. It's used in the "End" scene to append the user score to the wrap-up text.
/// </summary>
public class DisplayScore : MonoBehaviour
{

    void Start()
    {
        StartCoroutine(AppendScoreToQuestion());
    }

    // HACK: -ish Finds the first question asked and appends the score to it.
    private IEnumerator AppendScoreToQuestion()
    {
        Question question = null;
        while (question == null)
        {
            question = FindObjectOfType<Question>();
            yield return new WaitForSeconds(0.1f);
        }
        question.questionText.text += ScoreKeeper.GetFinalScoreString();
    }
}
