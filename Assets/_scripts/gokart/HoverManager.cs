using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HoverManager : MonoBehaviour
{
    [Range(0.01f, 1)]
    public float similarityThresh; // Acceptable range of variance between two colors to be considered the same.
    public HoverDisplayPanel hoverDisplayPanel;
    public HoverMap hoverMap;
    public GameObject testManager;

    void Update()
    {
        HoverCheck(GetGazeRay());
    }

    public void SetActive(bool active)
    {
        gameObject.SetActive(active);
    }

    public void ActivateHoverMap(HoverMap map)
    {
        // disabling the renderer makes it invisible, but we can still detect its color map
        GetComponent<Renderer>().enabled = false;
        SetActive(true);
        GetComponent<Renderer>().material = map.hoverMask;
    }

    public void ActivateForQuestion(Question question)
    {
        ActivateHoverMap(question.hoverMap);
    }

    /// <summary>
    /// Gets the gaze point ray. This extends from the camera fwd vector.
    /// </summary>
    /// <returns>The gaze ray.</returns>
    private Ray GetGazeRay()
    {
        return new Ray(Camera.main.transform.position, Camera.main.transform.forward);
    }

    /// <summary>
    /// This runs a raycast to see what's being hit, then checks the color of the texture coordinate at the hit point.
    /// It then maps the color to colors provided by the ColorMap array of hoverables.
    /// </summary>
    /// <param name="ray">Ray to cast.</param>
    private void HoverCheck(Ray ray)
    {
        // The raycaster can't collide from inside the sphere, we need to reverse the ray's direction
        ray.origin = ray.GetPoint(200);
        ray.direction = -ray.direction;

        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            Renderer renderer = hit.transform.GetComponent<Renderer>();

            if (renderer == null || renderer.sharedMaterial == null || renderer.sharedMaterial.mainTexture == null)
            {
                return;
            }

            // Get the color of the texture at the hit point.
            Texture2D tex = renderer.material.mainTexture as Texture2D;
            Vector2 pixelUV = hit.textureCoord;
            pixelUV.x *= tex.width;
            pixelUV.y *= tex.height;

            var texColor = tex.GetPixel((int)pixelUV.x, (int)pixelUV.y);
            if (texColor.a > 0.0f)
            {
                foreach (var entry in hoverMap.HoverMapEntries)
                {
                    if (ColorCompare(texColor, entry.color))
                    {
                        testManager.SetActive(false);
                        hoverDisplayPanel.ShowTooltip(entry.name, entry.description);
                    }
                }
            }
            else
            {
                testManager.SetActive(true);
                hoverDisplayPanel.HideTooltip();
            }
        }
    }

    /// <summary>
    /// Compares the rgb values of two colors to see how similar they are.
    /// Due to rounding errors, two colors are almost never the same, so this is a convenience method
    /// to check if the difference falls within a pre-set acceptable range.
    /// </summary>
    /// <returns><c>true</c>, if colors are acceptably similar, <c>false</c> otherwise.</returns>
    /// <param name="c1">color 1</param>
    /// <param name="c2">color 2</param>
    private bool ColorCompare(Color c1, Color c2)
    {
        if (Mathf.Abs(c1.r - c2.r) > similarityThresh)
        {
            return false;
        }
        if (Mathf.Abs(c1.g - c2.g) > similarityThresh)
        {
            return false;
        }
        if (Mathf.Abs(c1.b - c2.b) > similarityThresh)
        {
            return false;
        }
        return true;
    }
}
