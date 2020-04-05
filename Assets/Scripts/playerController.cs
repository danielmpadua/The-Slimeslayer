using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerController : MonoBehaviour {
    private gameController _gameController;

	private Rigidbody2D playerRigidBody;
    private Animator playerAnimator;
    private SpriteRenderer playerSpriteRenderer;

    public float speed;
    public float jumpForce;

    public bool isLookLeft;

    public Transform groundCheck;
    private bool isGrounded;
    private bool isAtack;

    public Transform hand;
    public GameObject hitBoxPrefab;

    public Color hitColor;
    public Color invencibleColor;

    public int maxHP;

	// Funçoes Nativas
	void Start () {
        playerRigidBody = GetComponent < Rigidbody2D>();
        playerAnimator = GetComponent<Animator>();
        playerSpriteRenderer = GetComponent<SpriteRenderer>();

        _gameController = FindObjectOfType(typeof(gameController)) as gameController;
        _gameController.playerTransform = this.transform;

    }
	
	void Update () {
        playerAnimator.SetBool("isGrounded", isGrounded);

        if(_gameController.currentState != gameState.GAMEPLAY){
            playerRigidBody.velocity = new Vector2(0, playerRigidBody.velocity.y);
            playerAnimator.SetInteger("horizontal", 0);
            return;
        }

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
            _gameController.playSoundEffect(_gameController.soundEffectJump,0.5f);
            playerRigidBody.AddForce(new Vector2(0, jumpForce));
        }

        if (Input.GetButtonDown("Fire1") && !isAtack) {
            playerAnimator.SetTrigger("atack");
            isAtack = true;
            _gameController.playSoundEffect(_gameController.soundEffectAtack, 0.5f);
        }

        playerRigidBody.velocity = new Vector2(horizontalMoviment * speed, speedY);

        playerAnimator.SetInteger("horizontal", (int)horizontalMoviment);
        playerAnimator.SetFloat("speedY", speedY);
        playerAnimator.SetBool("isAtack", isAtack);
	}

    void FixedUpdate() {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, 0.02f);
    }

    void OnTriggerEnter2D(Collider2D collider){
        if (collider.gameObject.tag == "Coletavel") {

            _gameController.playSoundEffect(_gameController.soundEffectCoin, 0.5f);
            _gameController.getCoin();
            Destroy(collider.gameObject);

        } else if (collider.gameObject.tag == "Damage") {
            _gameController.getHit();
            if (_gameController.lifePlayer > 0) {
                StartCoroutine("damageController");
            }
        } else if (collider.gameObject.tag == "Abism") {
            _gameController.playSoundEffect(_gameController.soundEffectDamage, 0.5f);
            _gameController.lifePlayer = 0;
            _gameController.heartController();
            _gameController.gameOverPanel.SetActive(true);
            _gameController.currentState = gameState.GAMEOVER;
            _gameController.selectMusic(musicScene.GAMEOVER);
        }else if(collider.gameObject.tag == "Flag"){
            _gameController.theEnd();
        }
    }

    // Funções criadas
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

    void footStep(){
        _gameController.playSoundEffect(_gameController.soundEffectStep[Random.Range(0, _gameController.soundEffectStep.Length)], 1f);
    }

    IEnumerator damageController(){
        maxHP --;

        if(maxHP <= 0){
            Debug.LogError("pause");
        }

        this.gameObject.layer = LayerMask.NameToLayer("Invencible");

        _gameController.playSoundEffect(_gameController.soundEffectDamage,0.5f);
        playerSpriteRenderer.color = hitColor;
        yield return new WaitForSeconds(0.3f);
        playerSpriteRenderer.color = invencibleColor;

        for (int i = 0; i < 5; i++){
            playerSpriteRenderer.enabled = false;
            yield return new WaitForSeconds(0.2f);
            playerSpriteRenderer.enabled = true;
            yield return new WaitForSeconds(0.2f);
        }

        playerSpriteRenderer.color = Color.white;
        this.gameObject.layer = LayerMask.NameToLayer("Player");
    }

}
