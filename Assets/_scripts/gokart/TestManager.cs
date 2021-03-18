using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;
using System.Threading;
using UnityEngine.Video;

[RequireComponent(typeof(Canvas))]
[RequireComponent(typeof(Timer))]
public class TestManager : MonoBehaviour
{

    public Option optionPrefab;
    public Question questionPrefab;
    public GameObject dangerHighlighter;
    public IMediaDownloadManager mediaManager;
    public Stack<QuestionData> questionStack;
    public float fixedHeight;

    HoverManager hoverManager;

    private const float QUESTION_ASKING_MARGIN = 0.03f;
    private const int APP_PRESS_THRESHOLD = 10;

    private QuestionData currentQuestionData;
    private Question currentQuestion;
    private GameObject currentExplanation;
    private bool _coroutineRunning;
    private bool _hoverQ;
    private Button _floorBtn;
    //private GvrBasePointer _reticle;
   // private Renderer _reticleRenderer;
    private Timer _timer;

    void Start()
    {
        _timer = GetComponent<Timer>();
        _coroutineRunning = false;
        if (hoverManager != null)
        {
            hoverManager.SetActive(false);
        }


        /* Do we need floor button?
        // TODO: Target the floorButton better?
        _floorBtn = FindObjectOfType<Button> ();
        _floorBtn.gameObject.SetActive (false);
        */

        // TODO: Target the reticle better?
        // _reticle = FindObjectOfType<GvrBasePointer>();
        // _reticleRenderer = _reticle.gameObject.GetComponent<Renderer>();
        // _reticleRenderer.enabled = false;
        hoverManager = GameObject.Find("HoverManager").GetComponent<HoverManager>();
        hoverManager.SetActive(false);
        VideoTimer().player.loopPointReached += AdvanceVideo;
        ResetQuestions();
    }

    Timer VideoTimer()
    {
        return _timer;
    }

    void ResetQuestions()
    {
        Debug.Log("MediaLibrary.currentExperience : " +MediaLibrary.currentExperience);
        VideoTimer().SetPlaybackSpeed(1.0f);
        VideoTimer().elapsedTime = 0;
        if (mediaManager == null)
        {
            mediaManager = MediaLibrary.MediaDownloadManagerFor(MediaLibrary.currentExperience);
        }

        mediaManager.PrepareVideoPath(VideoTimer());
        questionStack = GetQuestionStack();
    }

    void Update()
    {
        //CheckForAppButtonPress();
        if (VideoTimer().TimerIsCounting())
        {
            CheckIfTimeToAskQuestion();
        }

    }

    public void OnOptionsAvailable()
    {
        if (currentQuestion.HasHoverMap())
        {
            hoverManager.ActivateForQuestion(currentQuestion);
        }
        else
        {
            hoverManager.SetActive(false);
        }
       // _reticleRenderer.enabled = true;
    }

    public void OnAnswerSelected(Option option)
    {
        hoverManager.SetActive(false);
        // Play sound effect!
        Debug.Log("Answer was selected");
        if (option.IsContinued())
        {
            Debug.Log("+++: Continuing video");
            VideoTimer().TimerStart();
        }
        else if (currentQuestion != null && currentQuestion.isBranching)
        {
            // JSON data stores an explanation text for each question response
            // if the text matches a video file we should play that.
            // else assume the text is a human readable explanation for that option choice

            if (mediaManager.HasVideo(option.NextVideoName()))
            {
                Debug.Log("+++ : Branching To: " + option.NextVideoName());
                BranchTo(option.NextVideoName());
            }
            else
            {
                Debug.Log("+++ : Not branching. Showing explanation text");
                StartCoroutine(ShowExplanationWithDelay(option));
            }
        }
        else
        {
            // Moved to enable a delay before explanation button appears
            Debug.Log("Showing delayed explanation: " + option.explanationText.Substring(0, 20));
            StartCoroutine(ShowExplanationWithDelay(option));
        }

        if (currentQuestion != null)
        {
            Destroy(currentQuestion.gameObject);
        }
    }

    private OptionData GetExplanationAsOptionData(string explanation)
    {
        var od = new OptionData();
        od.text = explanation;
        od.isCorrect = true;
        od.explanation = "";
        return od;
    }

    /// <summary>
    /// Stops the timer and asks a question.
    /// </summary>
    /// <param name="questionData">Question</param>
    public void AskQuestion(QuestionData questionData)
    {
        Debug.Log("Inside Ask Question");
        VideoTimer().TimerStop();
        currentQuestionData = questionData;
        currentQuestion = Instantiate(questionPrefab, transform) as Question;
        currentQuestion.Initialize(questionData);
    }

    private Stack<QuestionData> GetQuestionStack()
    {
        var questionnaireObject = JsonUtility.FromJson<QuestionnaireObject>(mediaManager.JSONQuestions());

        // Sort by appearance time and reverse since we're putting the questions in a stack so soonest is on top.
        var chronologicalQuestions = questionnaireObject.questions.OrderBy(q => q.time).Reverse();
        return new Stack<QuestionData>(chronologicalQuestions);
    }

    //  private void LogData(QuestionnaireObject data) {
    //    foreach (var question in data.questions) {
    //      Debug.Log ("-----");
    //      Debug.Log (question.text);
    //      Debug.Log (question.time);
    //      Debug.Log (question.numPoints);
    //      Debug.Log (question.isBranching);
    //      foreach (var option in question.options) {
    //        Debug.Log ("--" + option.text);
    //      }
    //    }
    //  }
    //
    private void CheckForAppButtonPress()
    {
        //if (GvrControllerInput.AppButtonDown)
        {
            if (currentExplanation != null)
            {
                var toggle = !currentExplanation.activeInHierarchy;
                currentExplanation.SetActive(toggle);
               // DynamicTooltipsForController.UpdateTooltips(toggle);
            }
            else if (currentQuestion != null)
            {
                var toggle = !currentQuestion.gameObject.activeInHierarchy;
                currentQuestion.gameObject.SetActive(toggle);
                //DynamicTooltipsForController.UpdateTooltips(toggle);
            }
        }
    }

    /// <summary>
    /// This is a hack method to have a show/hide question feature in cardboard.
    /// </summary>
    public void ManuallyShowOrHideCurrentUI(Button floorButton)
    {
        var toggle = false;
        if (currentExplanation != null)
        {
            toggle = !currentExplanation.activeInHierarchy;
            currentExplanation.SetActive(toggle);
            var buttonText = toggle ? "Hide Question" : "Show Question";
            floorButton.GetComponentInChildren<Text>().text = buttonText;
        }
        else if (currentQuestion != null)
        {
            toggle = !currentQuestion.gameObject.activeInHierarchy;
            currentQuestion.gameObject.SetActive(toggle);
            var buttonText = toggle ? "Hide Question" : "Show Question";
            floorButton.GetComponentInChildren<Text>().text = buttonText;
        }
    }

    /// <summary>
    /// Checks if time to ask question.
    /// </summary>
    private void CheckIfTimeToAskQuestion()
    {

        Debug.Log("Inside checkiftimetoaskquestion : " +questionStack.Count);
        // _floorBtn.gameObject.SetActive (false);

//        _reticleRenderer.enabled = false;
        if (VideoTimer().TimerIsPaused())
        {
            return;
        }

        if (questionStack.Count > 0)
        {
            var diff = questionStack.Peek().time - VideoTimer().elapsedTime;
            // Ask the question when it's time, or ASAP if it missed its deadline
            if (diff <= QUESTION_ASKING_MARGIN || diff < 0)
            {
                AskQuestion(questionStack.Pop());
                // _floorBtn.gameObject.SetActive (true);
            }
        }
    }

    private void Continue()
    {
        //HACK TO DISPLAY HIGHLIGHTS
        if (_hoverQ)
        {
            if (dangerHighlighter != null)
            {
                dangerHighlighter.SetActive(false);
            }
            _hoverQ = false;
        }
        if (currentExplanation != null)
        {
            Destroy(currentExplanation);
        }
        VideoTimer().TimerStart();
    }

    IEnumerator ShowExplanationWithDelay(Option opt, float delay = 2.5f)
    {

        currentExplanation = Instantiate(questionPrefab.gameObject, transform) as GameObject;
        currentExplanation.transform.Find("Question").GetComponent<Text>().text = opt.explanationText;

        yield return new WaitForSeconds(delay);

        var optParent = currentExplanation.transform.Find("Options");
        var contOption = Instantiate(optionPrefab, optParent) as Option;
        contOption.numPoints = 0;
        contOption.Initialize(GetExplanationAsOptionData(string.IsNullOrEmpty(opt.explanationBtnText) ? "Continue" : opt.explanationBtnText), 0);
        contOption.SetCustomListener(Continue);
    }

    public void AdvanceVideo(VideoPlayer player = null)
    {
        mediaManager.AdvanceVideo();
        ResetQuestions();
    }

    public void BranchTo(string branch)
    {
        if (branch == mediaManager.CurrentVideoName() || branch == "Current")
        {
            Debug.Log("not changing scene");
            return;
        }
        mediaManager.LoadVideo(branch);
        ResetQuestions();
    }
}
