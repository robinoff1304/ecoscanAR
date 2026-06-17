using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.InputSystem;

[RequireComponent(typeof(ARRaycastManager))]

public class TapToPlaceObject : MonoBehaviour

{
    public GameObject gameObjectToInstatiate;
    private GameObject spawnedObject;
    private ARRaycastManager _arRaycastmanager;
    static List<ARRaycastHit> hits = new List<ARRaycastHit>();
    private Camera arCam;
    private bool islocked = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
        _arRaycastmanager = GetComponent<ARRaycastManager>();
        arCam = Camera.main;

    }

    bool TryGetTouchPosition(out Vector2 touchPosition)
    {
        if (Touchscreen.current != null && Touchscreen.current.primaryTouch.press.isPressed)
        {
            touchPosition = Touchscreen.current.primaryTouch.position.ReadValue();
            return true;
        }
        touchPosition = default;
        return false;
    }

    private void Update()
    {
        if (!TryGetTouchPosition(out Vector2 touchPosition))
        {
            islocked = false;
            spawnedObject = null;
            return;
        }

        RaycastHit _raycasthit;
        Ray ray = arCam.ScreenPointToRay(touchPosition);

        if (Physics.Raycast(ray, out _raycasthit))
        {
            if (_raycasthit.collider.gameObject.tag == "Spawnable")
            {
                spawnedObject = _raycasthit.collider.gameObject;
                islocked = true;
            }
        }

        if (_arRaycastmanager.Raycast(touchPosition, hits, trackableTypes: TrackableType.PlaneWithinPolygon))
        {
            var hitpose = hits[0].pose;

            if (spawnedObject == null && islocked == false)
            {
                spawnedObject = Instantiate(gameObjectToInstatiate, hitpose.position, hitpose.rotation);
                islocked = true;

            }
            else if (spawnedObject != null)
            {

                spawnedObject.transform.position = hitpose.position;
            }
        }


    }
}
