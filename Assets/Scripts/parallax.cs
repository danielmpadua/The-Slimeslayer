using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class parallax : MonoBehaviour {

    public Transform background;
    public float speed;
    private Transform cam;
    private Vector3 previewCameraPosition;

    void Start()
    {
        cam = Camera.main.transform;
        previewCameraPosition = cam.position;
    }

    void LateUpdate()
    {
        float parallaxX = previewCameraPosition.x - cam.position.x;
        float backgroundTargetX = background.position.x + parallaxX;

        Vector3 backgroundPosition = new Vector3(backgroundTargetX, background.position.y, background.position.z);
        background.position = Vector3.Lerp(background.position, backgroundPosition, speed * Time.deltaTime);
        previewCameraPosition = cam.position;
    }
}
