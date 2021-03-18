using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class FloorControlsHelper : MonoBehaviour
{


    // Use this for initialization
    void Start()
    {
        Button btn = GetComponent<Button>();
        var testMan = FindObjectOfType<TestManager>();
        if (testMan == null)
        {
            Debug.LogError("There's no Test Manager in the scene! The floor controls to show/hide the questions will not work");
            return;
        }
        // Onclick event passed with parameter.
        btn.onClick.AddListener(delegate
        {
            testMan.ManuallyShowOrHideCurrentUI(btn);
        });
    }
}
