using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class HoverMapEntry
{
    public UnityEngine.Color color;
    public string name;
    public string description;
}

[CreateAssetMenu]
public class HoverMap : ScriptableObject
{
    public Material hoverMask;
    public HoverMapEntry[] HoverMapEntries;

}
