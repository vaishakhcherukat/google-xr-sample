using System.Collections;
using System.Collections.Generic;

// ***Helper Classes for Parsing JSON Text Assets***
// These are intermediate classes used to bring info from the Json question files,
// then parsed into the multiple choice prefab.
[System.Serializable]
public class QuestionnaireObject
{
    public List<QuestionData> questions;
}

[System.Serializable]
public class QuestionData
{
    public string text;
    public List<OptionData> options;
    public float time;
    public bool isBranching;
    public string hoverMapName;
    public int numPoints;
    public float optionDelay = -1;
}

[System.Serializable]
public class OptionData
{
    public string text;
    public string explanation;
    public string explanationBtn;
    public bool isCorrect;
}

[System.Serializable]
public class ApiContent
{
    public List<Course> data;
}


[System.Serializable]
public class FileData
{
    public string url;
}

