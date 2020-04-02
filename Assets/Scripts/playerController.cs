using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerController : MonoBehaviour {

	private Rigidbody2D playerRigidBody;
    private Animator playerAnimator;

    public float speed;
    public float jumpForce;

    public bool isLookLeft;

    public Transform groundCheck;
    private bool isGrounded;
    private bool isAtack;

    public Transform hand;
    public GameObject hitBoxPrefab;

	
	void Start () {
        playerRigidBody = GetComponent < Rigidbody2D>();
        playerAnimator = GetComponent<Animator>();
    }
	
	void Update () {
        float horizontalMoviment = Input.GetAxisRaw("Horizontal");
        float speedY = playerRigidBody.velocity.y;


        if (isAtack && isGrounded) {
            horizontalMoviment = 0;
        }

        if((horizontalMoviment > 0 && isLookLeft) || (horizontalMoviment < 0 && !isLookLeft))
        {
            Flip();
        }

        if (Input.GetButtonDown("Jump") && isGrounded){
            playerRigidBody.AddForce(new Vector2(0, jumpForce));
        }

        if (Input.GetButtonDown("Fire1") && !isAtack) {
            playerAnimator.SetTrigger("atack");
            isAtack = true;
        }

        playerRigidBody.velocity = new Vector2(horizontalMoviment * speed, speedY);

        playerAnimator.SetInteger("horizontal", (int)horizontalMoviment);
        playerAnimator.SetBool("isGrounded", isGrounded);
        playerAnimator.SetFloat("speedY", speedY);
        playerAnimator.SetBool("isAtack", isAtack);
	}

    void FixedUpdate() {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, 0.02f);
    }

    void Flip() {
        isLookLeft = !isLookLeft;
        float scaleX = transform.localScale.x * -1;
        transform.localScale = new Vector3(scaleX, transform.localScale.y, transform.localScale.z);

    }

    void onEndAtack() {
        isAtack = false;
    }
    
    void hitBoxAtack() {
        GameObject hitBoxTemp = Instantiate(hitBoxPrefab, hand.position, transform.localRotation);
        Destroy(hitBoxTemp, 0.2f);
    }
}
