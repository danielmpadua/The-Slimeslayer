using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gameController : MonoBehaviour {

    private Camera camera;
    public Transform playerTransform;
    public Transform limitCameraLeft, limitCameraRight, limitCameraTop, limitCameraBottom;

    public float speedCamera;

    void Start() {
        camera = Camera.main;
    }

    void Update() {

    }

    void LateUpdate() {
        cameraController();
    }

    void cameraController() {
        float camPositionX = playerTransform.position.x;
        float camPositionY = playerTransform.position.y;

        if (camera.transform.position.x < limitCameraLeft.transform.position.x && playerTransform.position.x < limitCameraLeft.position.x)
        {
            camPositionX = limitCameraLeft.transform.position.x;
        }
        else if (camera.transform.position.x > limitCameraRight.transform.position.x && playerTransform.position.x > limitCameraRight.position.x)
        {
            camPositionX = limitCameraRight.transform.position.x;
        }

        if (camera.transform.position.y < limitCameraBottom.transform.position.y && playerTransform.position.y < limitCameraBottom.position.y)
        {
            camPositionY = limitCameraBottom.transform.position.y;
        }
        else if (camera.transform.position.y > limitCameraTop.transform.position.y && playerTransform.position.y > limitCameraTop.position.y)
        {
            camPositionY = limitCameraTop.transform.position.y;
        }

        Vector3 camPosition = new Vector3(camPositionX, camPositionY, camera.transform.position.z);
        camera.transform.position = Vector3.Lerp(camera.transform.position, camPosition, speedCamera * Time.deltaTime);
    }
}
