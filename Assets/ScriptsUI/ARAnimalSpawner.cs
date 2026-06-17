using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.InputSystem;

[RequireComponent(typeof(ARRaycastManager))]
public class ARAnimalSpawner : MonoBehaviour
{
    private ARRaycastManager raycastManager;

    static List<ARRaycastHit> hits = new List<ARRaycastHit>();

    public GameObject placementIndicator;

    [Header("Prefabs")]
    public GameObject lionPrefab;
    public GameObject penguinPrefab;
    public GameObject pandaPrefab;
    public GameObject dolphinPrefab;
    public GameObject camelPrefab;

    private GameObject spawnedAnimal;

    void Awake()
    {
        raycastManager = GetComponent<ARRaycastManager>();
    }

    void Update()
    {
        if (!TryGetTouchPosition(out Vector2 touchPosition))
            return;

        // 🔍 AR Raycast auf Fläche
        if (raycastManager.Raycast(touchPosition, hits, TrackableType.PlaneWithinPolygon))
        {
            Pose hitPose = hits[0].pose;

            // 📍 Indicator immer bewegen
            placementIndicator.SetActive(true);
            placementIndicator.transform.SetPositionAndRotation(hitPose.position, hitPose.rotation);

            // 👆 Spawn nur bei erstem Tap
            if (spawnedAnimal == null)
            {
                GameObject prefab = GetSelectedAnimalPrefab();
                if (prefab == null) return;

                spawnedAnimal = Instantiate(prefab, hitPose.position, hitPose.rotation);
            }
            else
            {
                // optional: follow indicator
                spawnedAnimal.transform.position = placementIndicator.transform.position;
            }
        }
        else
        {
            placementIndicator.SetActive(false);
        }
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

    GameObject GetSelectedAnimalPrefab()
    {
        switch (AnimalSelectionManager.SelectedAnimal)
        {
            case "Lion": return lionPrefab;
            case "Penguin": return penguinPrefab;
            case "Panda": return pandaPrefab;
            case "Dolphin": return dolphinPrefab;
            case "Camel": return camelPrefab;
        }
        return null;
    }
}