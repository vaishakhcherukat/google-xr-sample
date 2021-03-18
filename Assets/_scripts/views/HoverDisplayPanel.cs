using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HoverDisplayPanel : MonoBehaviour
{
    public GameObject hoverTooltip;
    public Text title;
    public Text description;

    // Start is called before the first frame update
    void Start()
    {
       // hoverTooltip.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ShowTooltip(string newTitle, string newDescription)
    {
        title.text = newTitle;
        description.text = newDescription;
        hoverTooltip.SetActive(true);
    }

    public void HideTooltip()
    {
        hoverTooltip.SetActive(false);
    }
}
