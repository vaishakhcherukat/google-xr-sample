using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class CardboardGazeSelectable : MonoBehaviour
{
    private float _gazeTime;
    private bool _isGazing;

    public void GazeSelectionStart()
    {
        RadialProgressBar.StartProgress(GetComponent<Button>());
    }

    public void GazeSelectionStop()
    {
        RadialProgressBar.StopProgress();
    }

    void OnDestroy()
    {
        GazeSelectionStop();
    }

}