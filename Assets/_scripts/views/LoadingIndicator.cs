using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadingIndicator : MonoBehaviour
{
    public Image loadIndicator;
    public float loadingDelay = 3.0f;
    public float rotationSpeed;

    static LoadingIndicator instance;

    public static void Activate()
    {
        instance = Instance();
        if (instance != null)
        {
            instance.SetLoadIndicatorActive(true);
        }
    }

    public static void Deactivate()
    {
        instance = Instance();
        if (instance != null)
        {
            instance.SetLoadIndicatorActive(false);
        }
    }

    public static LoadingIndicator Instance()
    {
        GameObject indicator = GameObject.Find("LoadingIndicator");
        if (indicator != null)
        {
            return indicator.GetComponent<LoadingIndicator>();
        }
        else
        {
            return null;
        }
    }

    private void Awake()
    {
        SetLoadIndicatorActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        loadIndicator.transform.Rotate(new Vector3(0, 0, Time.deltaTime * rotationSpeed));
    }

    void SetLoadIndicatorActive(bool active)
    {
        loadIndicator.gameObject.SetActive(active);
    }
}
