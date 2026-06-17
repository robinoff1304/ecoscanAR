using System.Collections.Generic;

using UnityEngine;

using UnityEngine.InputSystem;

using UnityEngine.XR.ARFoundation;

using UnityEngine.XR.ARSubsystems;



[RequireComponent(typeof(ARRaycastManager))]

public class ARTapToPlaceObject : MonoBehaviour

{

    [SerializeField]

    private GameObject gameObjectToInstantiate;



    private GameObject spawnedObject;



    private ARRaycastManager arRaycastManager;

    private Camera arCamera;



    private static readonly List<ARRaycastHit> arHits = new();



    // Gibt an, ob das Objekt aktuell gezogen wird

    private bool isDragging = false;



    private void Awake()

    {

        arRaycastManager = GetComponent<ARRaycastManager>();

        arCamera = Camera.main;

    }



    private void Update()

    {

        if (Touchscreen.current == null)

            return;



        var touch = Touchscreen.current.primaryTouch;



        // Fingerposition lesen

        Vector2 touchPosition = touch.position.ReadValue();



        //--------------------------------------------------

        // TOUCH BEGIN

        //--------------------------------------------------

        if (touch.press.wasPressedThisFrame)

        {

            // Prüfen, ob auf das vorhandene Objekt getippt wurde

            if (spawnedObject != null)

            {

                Ray ray = arCamera.ScreenPointToRay(touchPosition);



                if (Physics.Raycast(ray, out RaycastHit hit))

                {

                    if (hit.collider.gameObject == spawnedObject)

                    {

                        isDragging = true;

                        return;

                    }

                }

            }



            // Kein Objekt getroffen -> Plane prüfen

            if (arRaycastManager.Raycast(

                    touchPosition,

                    arHits,

                    TrackableType.PlaneWithinPolygon))

            {

                Pose hitPose = arHits[0].pose;



                if (spawnedObject == null)

                {

                    // Erstes Objekt erzeugen

                    spawnedObject = Instantiate(

                        gameObjectToInstantiate,

                        hitPose.position,

                        hitPose.rotation);

                }

                else

                {

                    // Objekt an neue Position setzen

                    spawnedObject.transform.SetPositionAndRotation(

                        hitPose.position,

                        hitPose.rotation);

                }

            }

        }



        //--------------------------------------------------

        // DRAGGING

        //--------------------------------------------------

        if (isDragging && touch.press.isPressed)

        {

            if (arRaycastManager.Raycast(

                    touchPosition,

                    arHits,

                    TrackableType.PlaneWithinPolygon))

            {

                Pose hitPose = arHits[0].pose;



                spawnedObject.transform.position = hitPose.position;

            }

        }



        //--------------------------------------------------

        // TOUCH END

        //--------------------------------------------------

        if (touch.press.wasReleasedThisFrame)

        {

            isDragging = false;

        }

    }

}