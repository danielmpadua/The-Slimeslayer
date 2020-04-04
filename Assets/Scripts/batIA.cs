using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class batIA : MonoBehaviour {

    private gameController _gameController;
    private Rigidbody2D batRigidBody;
    private Animator batAnimator;

    private bool isFollow;

    public GameObject hitBox;

    public bool isLookLeft;
    public float speed;

	void Start () {
        _gameController = FindObjectOfType(typeof(gameController)) as gameController;
        batAnimator = GetComponent<Animator>();
    }
	
	void Update () {
		if(isFollow == true){
            transform.position = Vector3.MoveTowards(transform.position, _gameController.playerTransform.position, speed * Time.deltaTime);
        }

        if(transform.position.x < _gameController.playerTransform.position.x && isLookLeft == true){
            Flip();
        }else if(transform.position.x > _gameController.playerTransform.position.x && isLookLeft == false){
            Flip();
        }
    }

    void OnBecameVisible() {
        isFollow = true;
    }

    void OnBecameInvisible()
    {
        isFollow = false;
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "hitBox")
        {

            isFollow = false;
            Destroy(hitBox);
            _gameController.playSoundEffect(_gameController.soundEffectEnemyDead, 0.2f);
            batAnimator.SetTrigger("dead");
        }
    }

    void OnDead()
    {
        Destroy(this.gameObject);
    }

    void Flip()
    {
        isLookLeft = !isLookLeft;
        float scaleX = transform.localScale.x * -1;
        transform.localScale = new Vector3(scaleX, transform.localScale.y, transform.localScale.z);

    }
}
