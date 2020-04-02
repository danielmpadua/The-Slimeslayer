using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerController : MonoBehaviour {

	private Rigidbody2D playerRigidBody;
    public float speed;
    public float jumpForce;
    public bool isLookLeft;
	
	void Start () {
        playerRigidBody = GetComponent < Rigidbody2D>();
	}
	
	void Update () {
        float horizontalMoviment = Input.GetAxisRaw("Horizontal");
        float speedY = playerRigidBody.velocity.y;

        if((horizontalMoviment > 0 && isLookLeft) || (horizontalMoviment < 0 && !isLookLeft))
        {
            Flip();
        }

        if (Input.GetButtonDown("Jump")){
            playerRigidBody.AddForce(new Vector2(0, jumpForce));
        }

        playerRigidBody.velocity = new Vector2(horizontalMoviment * speed, speedY);
	}

    void Flip() {
        isLookLeft = !isLookLeft;
        float scaleX = transform.localScale.x * -1;
        transform.localScale = new Vector3(scaleX, transform.localScale.y, transform.localScale.z);

    }
}
