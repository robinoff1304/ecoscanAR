using UnityEngine;

using UnityEngine.SceneManagement;

using UnityEngine.InputSystem;



public class TapToSpawnPrefab : MonoBehaviour

{


    private Camera arCamera;



    public GameObject objectToShow;



    private void Awake()

    {

        // nimmt automatisch die AR-Kamera aus der Szene

        arCamera = Camera.main;

    }



    private void Update()

    {

        if (arCamera == null)

            arCamera = Camera.main; // fallback falls Szene neu geladen wurde



        if (Touchscreen.current == null)

            return;



        var touch = Touchscreen.current.primaryTouch;



        if (!touch.press.wasPressedThisFrame)

            return;



        Vector2 touchPosition = touch.position.ReadValue();



        Ray ray = arCamera.ScreenPointToRay(touchPosition);



        if (Physics.Raycast(ray, out RaycastHit hit))

        {

            if (hit.transform == transform)

            {


                objectToShow.SetActive(true);



            }

        }

    }

}