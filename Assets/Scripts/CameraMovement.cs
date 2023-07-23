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
        loadText.text = "";
        Debug.Log("CameraMovement.cs successfully loaded!");
    }

    // Update is called once per frame
    void LateUpdate() {
        cameraFreeWalk.transform.position = new Vector3(CurrentPlayer.transform.position.x, CurrentPlayer.transform.position.y, cameraFreeWalk.transform.position.z);
        if(scrollInput.y > 0) {
            ZoomIn();
        } else if(scrollInput.y < 0) {
            ZoomOut();
        }
    }

    void OnScrollWheel(InputValue value) {
        scrollInput = value.Get<Vector2>();
    }

    void ZoomIn() {
        cameraFreeWalk.fieldOfView -= zoomSpeed;
        if (cameraFreeWalk.fieldOfView < minZoomFOV) {
            cameraFreeWalk.fieldOfView = minZoomFOV;
        }
    }

    void ZoomOut() {
        cameraFreeWalk.fieldOfView += zoomSpeed;
        if (cameraFreeWalk.fieldOfView > maxZoomFOV) {
            cameraFreeWalk.fieldOfView = maxZoomFOV;
        }
    }
}
