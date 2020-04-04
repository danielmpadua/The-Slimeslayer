using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum musicScene{
    SCENE0,SCENE1
}

public class gameController : MonoBehaviour {

    private Camera cam;
    public Transform playerTransform;
    public Transform limitCameraLeft, limitCameraRight, limitCameraTop, limitCameraBottom;

    public float speedCamera;

    [Header("Audio")]
    public AudioSource soundEffectSource;
    public AudioSource musicSource;

    public AudioClip soundEffectCoin;

    public AudioClip soundEffectDamage;
    public AudioClip soundEffectJump;
    public AudioClip soundEffectAtack;

    public AudioClip soundEffectEnemyDead;
    
    public AudioClip[] soundEffectStep;

    public musicScene currentMusic;

    public AudioClip musicScene0, musicScene1;

    public GameObject[] scene;

    void Start() {
        cam = Camera.main;

        /*foreach(GameObject sceneControl in scene){
            sceneControl.SetActive(false);
        }

        scene[0].SetActive(true);*/
    }

    void Update() {

    }

    void LateUpdate() {
        cameraController();
    }

    void cameraController() {
        float camPositionX = playerTransform.position.x;
        float camPositionY = playerTransform.position.y;

        if (cam.transform.position.x < limitCameraLeft.transform.position.x && playerTransform.position.x < limitCameraLeft.position.x)
        {
            camPositionX = limitCameraLeft.transform.position.x;
        }
        else if (cam.transform.position.x > limitCameraRight.transform.position.x && playerTransform.position.x > limitCameraRight.position.x)
        {
            camPositionX = limitCameraRight.transform.position.x;
        }

        if (cam.transform.position.y < limitCameraBottom.transform.position.y && playerTransform.position.y < limitCameraBottom.position.y)
        {
            camPositionY = limitCameraBottom.transform.position.y;
        }
        else if (cam.transform.position.y > limitCameraTop.transform.position.y && playerTransform.position.y > limitCameraTop.position.y)
        {
            camPositionY = limitCameraTop.transform.position.y;
        }

        Vector3 camPosition = new Vector3(camPositionX, camPositionY, cam.transform.position.z);
        cam.transform.position = Vector3.Lerp(cam.transform.position, camPosition, speedCamera * Time.deltaTime);
    }

    public void playSoundEffect(AudioClip soundClip, float volume){
        soundEffectSource.PlayOneShot(soundClip, volume);
    }

    public void selectMusic(musicScene newMusic){
        AudioClip clip = null;

        switch (newMusic) {
            case musicScene.SCENE0:
                clip = musicScene0;
                break;
            case musicScene.SCENE1:
                clip = musicScene1;
                break;
        }

        StartCoroutine("switchMusic", clip);
    }

    IEnumerator switchMusic(AudioClip music){
        float maxVolume = musicSource.volume;

        for (float volume = maxVolume; volume > 0; volume -= 0.01f){
            musicSource.volume = volume;
            yield return new WaitForEndOfFrame();
        }

        musicSource.clip = music;
        musicSource.Play();

        for (float volume = 0; volume < maxVolume; volume += 0.01f)
        {
            musicSource.volume = volume;
            yield return new WaitForEndOfFrame();
        }
    }
    
}
