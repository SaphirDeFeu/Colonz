using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour {

    public Camera cameraFreeWalk;
    public GameObject CurrentPlayer;

    public float zoomSpeed = 10f;
    public float minZoomFOV = 80f;
    public float maxZoomFOV = 160f;
    public float defaultFOV = 150f;

    // Start is called before the first frame update
    void Start() {
        cameraFreeWalk.fieldOfView = defaultFOV;
        Debug.Log("CameraMovement.cs successfully loaded!");
    }

    // Update is called once per frame
    void LateUpdate() {
        transform.position = new Vector3(CurrentPlayer.transform.position.x, CurrentPlayer.transform.position.y, transform.position.z);
        if (Input.GetAxis("Mouse ScrollWheel") > 0) {
            ZoomIn();
        }
        else if (Input.GetAxis("Mouse ScrollWheel") < 0) {
            ZoomOut();
        }
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
