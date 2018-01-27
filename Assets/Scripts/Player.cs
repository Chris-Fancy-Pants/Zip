using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DigitalRuby.LightningBolt;

public class Player : MonoBehaviour {



    [Header("Player Speeds")]
    public float moveSpeed = 10f;
    public Vector2 jumpForce = new Vector2(0, 100f);

    [Header("Ground Check Transforms")]
    public Transform frontGroundCheck;
    public Transform backGroundCheck;
    public Transform centreGroundCheck;
    public LayerMask groundCheckLayerMask;
    public float groundCheckRadius = 0.1f;
    float previousDirection = 1;
    public float zipCoolDown = 2f;

    LightningBoltScript lightningScript; 
    public GameObject lightningObject;

    public GameObject zipMoveObject;

    bool zipping = false;
    public float zipIndicatorRadius = 0.2f;

    public GameObject zipIndicatorObject;
    public SpriteRenderer spriteRenderer;



    public Text healthText;
    public Text zipCharge;
    public AudioSource zipArcSound;

    bool zipIndicatorOverObject = false;
    bool doingZip = false;

    public float zipSpeed;

    Rigidbody2D _rigidbody;
    Animator _animator;

    bool jumping = false;
    public bool grounded = false;

    public int bolts = 0;
    public int health = 3;

    bool canZip = true;


	public AudioSource jumpSound;
    public AudioSource footStepAudio;
    bool gameOverRunStarted = false;

    bool hurt = false;


	public GameObject deathPanel;

    // Use this for initialization
    void Start () {
        healthText.text = "Health: " + health.ToString();
        zipMoveObject.SetActive(false);
        _rigidbody = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        lightningScript = lightningObject.GetComponent<LightningBoltScript>();
		GameManager.instance.trialRunning = true;

    }



    private void OnDrawGizmos()
    {
       // Gizmos.DrawSphere(frontGroundCheck.position, groundCheckRadius);
       // Gizmos.DrawSphere(backGroundCheck.position, groundCheckRadius);
       // Gizmos.DrawSphere(zipIndicatorObject.transform.position, zipIndicatorRadius);

    }
    // Update is called once per frame
    void Update () {

        if (GameManager.instance.trialRunning)
        {


            float horizontal = Input.GetAxis("Horizontal");

            grounded = IsGrounded();

            float direction = 1f;
            float directionToMove = 1f;


            if (horizontal > 0.01)
            {
                direction = 1;
                directionToMove = direction;
                spriteRenderer.flipX = false;

            }
            else if (horizontal < -0.01)
            {
                direction = -1;
                directionToMove = direction;
                spriteRenderer.flipX = true;

            }
            else
            {
                directionToMove = 0;
            }


            if (!doingZip)
            {
                _rigidbody.velocity = new Vector2(moveSpeed * directionToMove, _rigidbody.velocity.y);
                _rigidbody.gravityScale = 2;
            }


            previousDirection = direction;



            HandleInput();


            if (zipping)
            {
                HandleZip();
            }


            HandleAnimator();
        }
        else
        {
			if(!gameOverRunStarted && health > 0)
            {
                gameOverRunStarted = true;
                _rigidbody.velocity = Vector2.zero;
                _animator.SetFloat("velocityX", 0);
                Invoke("GameOverRun", 3f);
            
            }
            
        }




    }


    void GameOverRun()
    {
       
        _rigidbody.velocity = new Vector2(10, 0);
        _animator.SetFloat("velocityX", Mathf.Abs(_rigidbody.velocity.x));
    }

    public void EndRunOver()
    {
        _animator.SetFloat("velocityX", 0);
    }



    void HandleInput()
    {
        if (Input.GetButtonDown("Jump") && grounded)
        {
            Jump();
        }


        if (Input.GetButtonDown("Fire1") && !grounded)
        {

            StartZip();

        }

        if (Input.GetButtonUp("Fire1") && zipping)
        {
            StopZip();
        }
    }

    bool IsGrounded()
    {
        bool groundedFront = Physics2D.OverlapCircle(frontGroundCheck.position, groundCheckRadius, groundCheckLayerMask);
        bool groundedBack = Physics2D.OverlapCircle(backGroundCheck.position, groundCheckRadius, groundCheckLayerMask);
        bool groundedCentre = Physics2D.OverlapCircle(centreGroundCheck.position, groundCheckRadius, groundCheckLayerMask);


      
        {

            return ((groundedBack || groundedFront) && groundedCentre);
        }
   
    }

    void HandleAnimator()
    {
        _animator.SetFloat("velocityX", Mathf.Abs(_rigidbody.velocity.x));
        _animator.SetBool("grounded", grounded);
        _animator.SetFloat("velocityY", _rigidbody.velocity.y);
    }


    void HandleZip()
    {
        _rigidbody.gravityScale = 0;
        _rigidbody.velocity = Vector2.zero;
        lightningScript.SetLightningPos(transform.position, zipIndicatorObject.transform.position);
        zipIndicatorOverObject = Physics2D.OverlapCircle(zipIndicatorObject.transform.position, zipIndicatorRadius);

        float zipIndicatorHorizontal = Input.GetAxis("Horizontal");
        float zipIndicatorVertical = Input.GetAxis("Vertical");

        Vector2 newIndicatorPosition = new Vector2(zipIndicatorObject.transform.position.x + zipIndicatorHorizontal, zipIndicatorObject.transform.position.y + zipIndicatorVertical);
        // TODO: Add vertical movement to zipIndicator
        if (Vector2.Distance(transform.position, newIndicatorPosition) < 5)
        {
            zipIndicatorObject.transform.position = newIndicatorPosition;
        }


        if (zipIndicatorOverObject)
        {
            zipIndicatorObject.GetComponent<SpriteRenderer>().color = Color.red;
        }
        else
        {
            zipIndicatorObject.GetComponent<SpriteRenderer>().color = Color.white;
        }
    }


    void StartZip()
    {
        if (canZip)
        {
            zipping = true;
            zipIndicatorObject.SetActive(true);
            lightningObject.SetActive(true);
            lightningScript.FireLightning(transform.position, zipIndicatorObject.transform.position);
            zipArcSound.Play();
            canZip = false;
        }
    }


    IEnumerator DoZip()
    {


        spriteRenderer.enabled = false;
        zipMoveObject.SetActive(true);

        zipIndicatorObject.transform.SetParent(null);
        zipMoveObject.transform.SetParent(null);
        _rigidbody.gravityScale = 0;
        _rigidbody.isKinematic = true;



        
        while (Vector2.Distance(zipMoveObject.transform.position, zipIndicatorObject.transform.position) > 0.1f)
        {
            zipMoveObject.transform.position = Vector2.MoveTowards(zipMoveObject.transform.position, zipIndicatorObject.transform.position, zipSpeed);
            yield return new WaitForEndOfFrame();
        }

        zipIndicatorObject.transform.position = transform.position;
        zipIndicatorObject.transform.SetParent(transform);
        doingZip = false;
        zipMoveObject.SetActive(false);

        transform.position = zipMoveObject.transform.position;
        zipMoveObject.transform.position = transform.position;
        zipMoveObject.transform.SetParent(transform);
        spriteRenderer.enabled = transform;

        //_rigidbody.gravityScale = 2;
        _rigidbody.isKinematic = false;
        _animator.SetBool("doindZip", false);
    }

    void StopZip()
    {
        zipping = false;
        zipIndicatorObject.SetActive(false);
        if (!zipIndicatorOverObject)
        {
           
            //transform.position = zipIndicatorObject.transform.position;
            doingZip = true;
            StartCoroutine("DoZip");
        }

        lightningScript.StopLightning();
        lightningObject.SetActive(false);
        zipArcSound.Stop();
        StartCoroutine("ResetZip");
    }


    IEnumerator ResetZip()
    {

        float count = 0;

        zipCharge.text = "Charge: " + count.ToString();

        while (count < zipCoolDown)
        {
            count += Time.deltaTime;
            zipCharge.text = "Charge: " + count.ToString();
            yield return new WaitForEndOfFrame();
        }
        canZip = true;
        zipCharge.text = "ZIP!";
    }




    void Jump()
    {

    	_rigidbody.AddForce(jumpForce);
		jumpSound.Play ();

    }

    public void PlayFootStep()
    {
        footStepAudio.Play();
    }



    public void AddBolt(int boltAmount)
    {
        bolts += boltAmount;

    }


    public void takeDamage(int damage)
    {

        if (!hurt)
        {
			health -= damage;

			if (health <= 0) {
				HandleDeath ();
			} else {
				_animator.SetTrigger ("flicker");

				healthText.text = "Health: " + health.ToString ();
				hurt = true;
			}
        }
    }

	void HandleDeath() {

		deathPanel.SetActive (true);
		DeathPanel dPanel = deathPanel.GetComponent<DeathPanel> ();
		dPanel.ShowDeathPanel ();
		GameManager.instance.trialRunning = false;

	}


    public void FlickerEnd()
    {
        hurt = false;
    }


}


/// DM-CGS-33 - zip spund
/// DM-CGS-34 - zip spund
/// /// DM-CGS-39 - zip spund
/// 
/// DM-CGS-48 metal door close