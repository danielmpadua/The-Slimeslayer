using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public enum musicScene{
    SCENE0,SCENE1,GAMEOVER, THEEND
}

public enum gameState{
    TITLE, GAMEPLAY, GAMEOVER, THEEND
}

public class gameController : MonoBehaviour {

    public gameState currentState;

    public GameObject titlePanel, gameOverPanel, theEndPanel;

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

    public AudioClip musicScene0, musicScene1, musicGameOver, musicTheEnd;

    public GameObject[] scene;

    public int coinCounter;
    public Text coinPrint;
    public Image[] hearts;
    public int lifePlayer;

    void Start() {
        cam = Camera.main;
        heartController();
    }

    void Update() {
        if(currentState == gameState.TITLE && Input.GetButtonDown("Jump")) {
            currentState = gameState.GAMEPLAY;
            titlePanel.SetActive(false);
        }else if (currentState == gameState.GAMEOVER && Input.GetButtonDown("Jump")){
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }else if (currentState == gameState.THEEND && Input.GetButtonDown("Jump")){
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
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
                this.currentMusic = musicScene.SCENE0;
                break;
            case musicScene.SCENE1:
                this.currentMusic = musicScene.SCENE1;
                clip = musicScene1;
                break;
            case musicScene.GAMEOVER:
                this.currentMusic = musicScene.GAMEOVER;
                clip = musicGameOver;
                break;
            case musicScene.THEEND:
                this.currentMusic = musicScene.THEEND;
                clip = musicTheEnd;
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

    public void heartController() {
        foreach(Image heart in hearts){
            heart.enabled = false;
        }

        for(int i = 0; i < lifePlayer; i++){
            hearts[i].enabled = true;
        }
    }

    public void getHit(){
        lifePlayer -= 1;
        heartController();
        if(lifePlayer <= 0){
            playerTransform.gameObject.SetActive(false);
            gameOverPanel.SetActive(true);
            currentState = gameState.GAMEOVER;
            selectMusic(musicScene.GAMEOVER);
        }
    }

    public void getCoin(){
        coinCounter += 1;
        coinPrint.text = coinCounter.ToString();
    }

    public void theEnd(){
        currentState = gameState.THEEND;
        theEndPanel.SetActive(true);
        selectMusic(musicScene.THEEND);
    }
    
}
