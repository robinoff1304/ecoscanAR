using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class ARPlacementIndicator : MonoBehaviour
{
    private ARRaycastManager raycastManager;

    static List<ARRaycastHit> hits = new();

    public GameObject placementIndicator;

    void Start()
    {
        raycastManager =
            FindObjectOfType<ARRaycastManager>();
    }

    void Update()
    {
        Vector2 screenCenter =
            new Vector2(
                Screen.width / 2,
                Screen.height / 2);

        if (raycastManager.Raycast(
            screenCenter,
            hits,
            TrackableType.PlaneWithinPolygon))
        {
            Pose pose = hits[0].pose;

            placementIndicator.SetActive(true);

            placementIndicator.transform.SetPositionAndRotation(
                pose.position,
                pose.rotation);
        }
        else
        {
            placementIndicator.SetActive(false);
        }
    }
}