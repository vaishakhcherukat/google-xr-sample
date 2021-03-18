using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Use this component to add comments and documentation a scene file.
// Attach to an empty game object or any other object in the scene hierarchy.
public class DocumentationNote : MonoBehaviour
{
    public string title;

    [TextArea(4, 10)]
    public string comment;
}
