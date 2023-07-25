using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class CameraMovement : MonoBehaviour {

    public Camera cameraFreeWalk;
    public GameObject CurrentPlayer;

    public float zoomSpeed = 10f;
    public float minZoomFOV = 80f;
    public float maxZoomFOV = 160f;
    public float defaultFOV = 150f;

    public TextMeshProUGUI loadText;

    private Vector2 scrollInput;

    // Start is called before the first frame update
    void Start() {
        cameraFreeWalk.fieldOfView = defaultFOV;
        loadText.text = ""; // Une fois tout est chargé, on retire le Loading
        Debug.Log("CameraMovement.cs successfully loaded!");
    }

    // Update is called once per frame
    void LateUpdate() {
        // On se place au dessus du joueur qu'on joue
        cameraFreeWalk.transform.position = new Vector3(CurrentPlayer.transform.position.x, CurrentPlayer.transform.position.y, cameraFreeWalk.transform.position.z);
        if(scrollInput.y > 0) {
            ZoomIn();
        } else if(scrollInput.y < 0) {
            ZoomOut();
        }
    }

    void OnScrollWheel(InputValue value) { // Récupérer la valeur du scrolling
        scrollInput = value.Get<Vector2>();
    }

    void ZoomIn() { // On modifie le FOV de la caméra pour zommer
        cameraFreeWalk.fieldOfView -= zoomSpeed;
        if (cameraFreeWalk.fieldOfView < minZoomFOV) {
            cameraFreeWalk.fieldOfView = minZoomFOV;
        }
    }

    void ZoomOut() { // On modifie le FOV de la caméra pour dézoomer
        cameraFreeWalk.fieldOfView += zoomSpeed;
        if (cameraFreeWalk.fieldOfView > maxZoomFOV) {
            cameraFreeWalk.fieldOfView = maxZoomFOV;
        }
    }
}
