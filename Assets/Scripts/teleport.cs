using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class teleport : MonoBehaviour {

    private gameController _gameController;

    public Transform end;
    public Transform cameraPosition;

    public Transform limitCameraLeft, limitCameraRight, limitCameraTop, limitCameraBottom;

    public musicScene newMusic;

    void Start () {
        _gameController = FindObjectOfType(typeof(gameController)) as gameController;
	}
	
	void OnTriggerEnter2D (Collider2D collider) {
		if(collider.gameObject.tag == "Player"){

            _gameController.selectMusic(musicScene.SCENE1);
            
            collider.transform.position = end.position;

            Camera.main.transform.position = cameraPosition.position;

            _gameController.limitCameraLeft = limitCameraLeft;
            _gameController.limitCameraRight = limitCameraRight;
            _gameController.limitCameraTop = limitCameraTop;
            _gameController.limitCameraBottom = limitCameraBottom;
         
        }
	}
}
