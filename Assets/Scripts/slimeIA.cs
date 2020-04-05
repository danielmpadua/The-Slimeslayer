using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class slimeIA : MonoBehaviour {

    private gameController _gameController;
    private Rigidbody2D slimeRigidBody;
    private Animator slimeAnimator;

    public float speed;
    public float timeToWalk;

    public GameObject hitBox;

    private int horizontalMoviment;
    public bool isLookLeft;


	// Use this for initialization
	void Start () {
        _gameController = FindObjectOfType(typeof(gameController)) as gameController;
        slimeAnimator = GetComponent<Animator>();
        slimeRigidBody = GetComponent<Rigidbody2D>();
        StartCoroutine("slimeWalk");
	}
	
	// Update is called once per frame
	void Update () {

        if (_gameController.currentState != gameState.GAMEPLAY){
            return;
        }

            if ((horizontalMoviment > 0 && isLookLeft) || (horizontalMoviment < 0 && !isLookLeft))
        {
            Flip();
        }

        slimeRigidBody.velocity = new Vector2(horizontalMoviment * speed, slimeRigidBody.velocity.y);

        if (horizontalMoviment != 0)
        {
            slimeAnimator.SetBool("isWalk", true);
        } else {
            slimeAnimator.SetBool("isWalk", false);
        }
    }
    
    void OnTriggerEnter2D(Collider2D collider){
        if(collider.gameObject.tag == "hitBox"){

            horizontalMoviment = 0;
            StopCoroutine("slimeWalk");
            Destroy(hitBox);
            _gameController.playSoundEffect(_gameController.soundEffectEnemyDead, 0.2f);
            slimeAnimator.SetTrigger("dead");
        }
    }

    IEnumerator slimeWalk(){
        int random = Random.Range(0, 100);

        if (random < 33) {
            horizontalMoviment = -1;
        } else if(random < 66){
            horizontalMoviment = 0;
        } else{
            horizontalMoviment = 1;
        }

        yield return new WaitForSeconds(timeToWalk);
        StartCoroutine("slimeWalk");
    }

    void OnDead(){
        Destroy(this.gameObject);
    }

    void Flip(){
        isLookLeft = !isLookLeft;
        float scaleX = transform.localScale.x * -1;
        transform.localScale = new Vector3(scaleX, transform.localScale.y, transform.localScale.z);

    }
}
