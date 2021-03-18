using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// This script provides a smooth fade-in of text components from 0 to its original opacity.
/// The transition takes `fadeDuration` seconds to complete.
/// </summary>
[RequireComponent(typeof(Text))]
[RequireComponent(typeof(Shadow))]
[RequireComponent(typeof(Outline))]
public class FadeInText : MonoBehaviour
{
    // <value>Duration of the desired fade.</value>
    public float fadeDuration;

    private Text textDisplay;
    private Shadow textShadow;
    private Outline textOutline;

    private float originalDisplayOpacity;
    private float originalOutlineOpacity;
    private float originalShadowOpacity;

    private float opacityPercentage;

    void Awake()
    {
        textShadow = GetComponent<Shadow>();
        textOutline = GetComponent<Outline>();
        textDisplay = GetComponent<Text>();

        // store original opacities
        originalDisplayOpacity = textDisplay.color.a;
        originalOutlineOpacity = textOutline.effectColor.a;
        originalShadowOpacity = textShadow.effectColor.a;

        // set new opacity to 0
        opacityPercentage = 0;
        UpdateOpacities(opacityPercentage);
    }

    private void Update()
    {
        var finished = false;
        if (fadeDuration <= 0)
        {
            opacityPercentage = 1.0f;
        }
        else
        {
            opacityPercentage += Time.deltaTime / fadeDuration;
        }
        if (opacityPercentage >= 1.0f)
        {
            finished = true;
            opacityPercentage = 1.0f;
        }
        UpdateOpacities(opacityPercentage);
        if (finished)
        {
            // disabling the component prevents further updates.
            this.enabled = false;
        }
    }

    public void UpdateOpacities(float opacityPercentage)
    {
        textDisplay.color = BumpColorOpacity(textDisplay.color, originalDisplayOpacity * opacityPercentage);
        textOutline.effectColor = BumpColorOpacity(textOutline.effectColor, originalOutlineOpacity * opacityPercentage);
        textShadow.effectColor = BumpColorOpacity(textShadow.effectColor, originalShadowOpacity * opacityPercentage);
    }

    /// <summary>
    /// You can't directly modify the opacity (alpha) of a color, so this is a convenience method to make code more readable.
    /// It also clamps the opacity to be between 0 and 1.
    /// </summary>
    /// <returns>Color with bumped opacity.</returns>
    /// <param name="color">Color.</param>
    /// <param name="newOpacity">Desired opacity value.</param>
    private Color BumpColorOpacity(Color color, float newOpacity)
    {
        var bumpedOpacity = Mathf.Clamp01(newOpacity);
        return new Color(color.r, color.g, color.b, bumpedOpacity);
    }

}
