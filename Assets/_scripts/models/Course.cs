using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Course
{
    public List<Experience> assets;

    public List<Experience> Experiences()
    {
        return assets;
    }
}
