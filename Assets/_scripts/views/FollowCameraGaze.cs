using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// FollowCameraGaze keeps its transform in front of the camera as the camera rotates.
/// FollowCameraGaze keeps the object's height at the same level if the camera rotates up or down.
/// </summary>
public class FollowCameraGaze : MonoBehaviour
{
    [Range(1.0f, 2.5f)]
    public float distanceFromCamera;
    [Range(0f, 90f)]
    public float angleBelowHorizon;
    public bool scaleByDistanceFromCamera;

    void Update()
    {
        PositionOnCircle();
        transform.RotateAround(Camera.main.transform.position, Vector3.up, Camera.main.transform.localRotation.eulerAngles.y + 90);
        if (scaleByDistanceFromCamera && transform.parent != null)
        {
            transform.parent.localScale = Vector3.one * distanceFromCamera;
        }
    }

    private void PositionOnCircle()
    {
        var radiansBelowHorizon = (360 - angleBelowHorizon) * (Mathf.PI / 180.0f);
        var pointOnCircle = new Vector3(0, Mathf.Sin(radiansBelowHorizon), Mathf.Cos(radiansBelowHorizon)) * distanceFromCamera;
        transform.position = Camera.main.transform.position + pointOnCircle;
        // keep other localRotation the same, just tilt by angleBelowHorizon
        var planeRotation = new Vector3(angleBelowHorizon, transform.localRotation.y, transform.localRotation.z);
        transform.localRotation = Quaternion.Euler(planeRotation);
    }

}