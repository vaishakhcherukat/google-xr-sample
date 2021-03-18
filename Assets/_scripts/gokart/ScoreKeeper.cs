using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ScoreBucket
{
    Awareness,
    Deescalation
}

public struct ScorePair
{
    public int userScore;
    public int totalScore;
    public ScorePair(int u, int t)
    {
        userScore = u;
        totalScore = t;
    }
}

public class Score
{
    public string sceneName;
    public ScoreBucket scoreBucket;
    private int _userScore;
    private int _totalScore;
    public void UpdateScore(bool answeredCorrectly, int points)
    {
        if (answeredCorrectly)
        {
            _userScore += points;
        }
        _totalScore += points;
        Debug.Log(ScoreKeeper.GetScoreString(_userScore, _totalScore));
    }
    public void ResetScore()
    {
        _userScore = 0;
        _totalScore = 0;
    }
    public ScorePair GetScorePair()
    {
        return new ScorePair(_userScore, _totalScore);
    }
    public Score(string sName, ScoreBucket sBucket)
    {
        sceneName = sName;
        scoreBucket = sBucket;
        ResetScore();
    }
    public override string ToString()
    {
        return "[" + sceneName + ": " + _userScore.ToString() + "/" + _totalScore.ToString() + "]";
    }
}

public class ScoreKeeper : MonoBehaviour
{
    private static ScoreKeeper _instance;
    private static Hashtable _scoresByBucket = new Hashtable();
    private static Score _currentScore;

    void Awake()
    {
        if (_instance == null)
        {
            Object.DontDestroyOnLoad(gameObject);
            _instance = this;
        }
        else
        {
            // In case of duplicate ScoreKeeper objects, keep original so we don't lose point tallies.
            Destroy(this.gameObject);
        }
    }

    /// <summary>
    /// Updates the score. Points are always added to the total score, and only added to the user's score if they answered correctly.
    /// </summary>
    /// <param name="answeredCorrectly">If the question was answered correctly.</param>
    /// <param name="points">Number of points.</param>
    public static void UpdateScore(bool answeredCorrectly, int points)
    {
        //    _currentScore.UpdateScore (answeredCorrectly, points);
    }

    public static void ResetAllScores()
    {
        //    _scoresByBucket = new Hashtable ();
    }

    /// <summary>
    /// Formats the score into a string with the number correct and percentage.
    /// </summary>
    /// <returns>" [user's score] / [total possible score] | [percentage correct] "</returns>
    public static string GetScoreString(int userScore, int totalScore)
    {
        if (totalScore == 0)
        {
            return "0/0 | 100%";
        }
        var percent = ((float)userScore / (float)totalScore) * 100;
        return userScore.ToString() + "/" + totalScore.ToString() + " | " + percent.ToString("#0.#") + "%";
    }

    public static string GetFinalScoreString()
    {
        var finalScoreString = "";
        foreach (var key in _scoresByBucket.Keys)
        {
            var cumulativeUserScore = 0;
            var cumulativeTotalScore = 0;
            var scoreListForBucket = _scoresByBucket[key] as List<Score>;
            foreach (var score in scoreListForBucket)
            {
                var scorePair = score.GetScorePair();
                cumulativeUserScore += scorePair.userScore;
                cumulativeTotalScore += scorePair.totalScore;
            }
            finalScoreString += key.ToString() + " Score: " + GetScoreString(cumulativeUserScore, cumulativeTotalScore) + "\n";
        }
        return finalScoreString;
    }

    /// <summary>
    /// Resets the score when the game restarts.
    /// </summary>
    /// <param name="sceneName">Name of the current scene</param>
    /// <param name="scoreBucket">The type of scoring (e.g. "Awareness" vs "Deescalation").</param>
    public static void CreateScoreForScene(string sceneName, ScoreBucket scoreBucket)
    {
        LogScoresToConsole();
        List<Score> scoreList;
        if (_scoresByBucket.ContainsKey(scoreBucket))
        {
            scoreList = _scoresByBucket[scoreBucket] as List<Score>;
        }
        else
        {
            scoreList = new List<Score>();
            _scoresByBucket.Add(scoreBucket, scoreList);
        }
        // if scene already exists, reset that score
        foreach (var score in scoreList)
        {
            if (score.sceneName == sceneName)
            {
                score.ResetScore();
                _currentScore = score;
                return;
            }
        }
        // otherwise, make a new Score object and add it to the list
        var scoreForCurrentScene = new Score(sceneName, scoreBucket);
        scoreList.Add(scoreForCurrentScene);
        _currentScore = scoreForCurrentScene;
    }

    private static void LogScoresToConsole()
    {
        foreach (var bucket in _scoresByBucket.Keys)
        {
            Debug.Log(bucket.ToString());
            var scoreList = _scoresByBucket[bucket] as List<Score>;
            foreach (var score in scoreList)
            {
                Debug.Log("---" + score.ToString());
            }
        }
    }
}
