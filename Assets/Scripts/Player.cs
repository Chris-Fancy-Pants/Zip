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

    public float maxZipLength = 5f;

	public float fallGravityModifier = 2.5f;

    LightningBoltScript lightningScript; 
    public GameObject lightningObject;

    public GameObject zipMoveObject;

    bool zipping = false;
    public float zipIndicatorRadius = 0.2f;

    public GameObject zipIndicatorObject;
    public SpriteRenderer spriteRenderer;


	public LayerMask zipIndicatorOverMask;

   
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

    public GameObject healthPanelObject;
    HealthPanel healthPanel;

	public GameObject deathPanel;

    public GameObject playerSprite;
    public ParticleSystem deathParticles;
    public Text angleText;
    // Use this for initialization
    void Start () {
     
        zipMoveObject.SetActive(false);
        _rigidbody = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        lightningScript = lightningObject.GetComponent<LightningBoltScript>();
		GameManager.instance.trialRunning = true;
        healthPanel = healthPanelObject.GetComponent<HealthPanel>();
        UpdateHealth();
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
            //ShowDebug();
            float horizontal = 0;


            if (!doingZip)
            {
                horizontal = Input.GetAxis("Horizontal");
            }


            grounded = IsGrounded();

            if(grounded)
            {
                canZip = true;
            }


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

            /*
            if (!doingZip && grounded)
            {

                float moveSpeed = 5000f;

                print("Move speed * horizontal: " + moveSpeed * horizontal);
                
                if (Mathf.Abs(_rigidbody.velocity.x) < 10)
                {
                    _rigidbody.AddForce(new Vector2(moveSpeed * horizontal, 0));
                }
                _rigidbody.gravityScale = 1;
            }


            else if (!doingZip && !grounded)
            {

                float moveSpeed = 1000f;

                //print("Move speed * horizontal: " + moveSpeed * horizontal);

                print("Sign: " + Mathf.Sign(horizontal));
                
                if(Mathf.Sign(horizontal) != Mathf.Sign(_rigidbody.velocity.x))
                {
                    moveSpeed = 7000f;
                }


                if (Mathf.Abs(_rigidbody.velocity.x) < 7)
                {
                    _rigidbody.AddForce(new Vector2(moveSpeed * horizontal , 0));
                }
                _rigidbody.gravityScale = 1;
            }
            */


            if(!doingZip && !zipping)
            {
                //_rigidbody.velocity = new Vector2(Mathf.Sign(horizontal) * moveSpeed, _rigidbody.velocity.y);
                _rigidbody.position = new Vector2(_rigidbody.position.x + horizontal * moveSpeed * Time.deltaTime, _rigidbody.position.y);
                _animator.SetFloat("velocityX", Mathf.Abs(horizontal));
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


	void FixedUpdate() {

		

	}

    void ShowDebug()
    {
        print("ZipIndicatorPos: " + zipIndicatorObject.transform.position);
        print("ZipIndicatorPos: local:" + zipIndicatorObject.transform.localPosition);
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

        RaycastHit2D hitFront = Physics2D.Raycast(frontGroundCheck.position, -Vector2.up);


        float distance = Mathf.Abs(hitFront.point.y - transform.position.y);

        

        bool groundedFront = Physics2D.OverlapCircle(frontGroundCheck.position, groundCheckRadius, groundCheckLayerMask);
        bool groundedBack = Physics2D.OverlapCircle(backGroundCheck.position, groundCheckRadius, groundCheckLayerMask);
        bool groundedCentre = Physics2D.OverlapCircle(centreGroundCheck.position, groundCheckRadius, groundCheckLayerMask);


      
        {

            return ((groundedBack || groundedFront) && groundedCentre);
        }
   
    }

    void HandleAnimator()
    {
        //_animator.SetFloat("velocityX", Mathf.Abs(_rigidbody.velocity.x));
        _animator.SetBool("grounded", grounded);
        _animator.SetFloat("velocityY", _rigidbody.velocity.y);
    }


    void HandleZip()
    {
        _rigidbody.gravityScale = 0;
        _rigidbody.velocity = Vector2.zero;
        lightningScript.SetLightningPos(transform.position, zipIndicatorObject.transform.position);




        HandleIndicatorMovement();

        if (zipIndicatorOverObject)
        {
            zipIndicatorObject.GetComponent<SpriteRenderer>().color = Color.red;
        }
        else
        {
            zipIndicatorObject.GetComponent<SpriteRenderer>().color = Color.white;
        }
    }

    private void HandleIndicatorMovement()
    {


        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);



        float horizontal = mousePos.x - transform.position.x;
        float vertical = mousePos.y - transform.position.y;


        if (Vector2.Distance(transform.position, zipIndicatorObject.transform.position) < maxZipLength)
        {
            zipIndicatorObject.transform.position = mousePos;
        }
        else
        {
            float horizontalModifier = 1;
            float verticalModifier = 1;

            float hypotenuse = Mathf.Sqrt(horizontal * horizontal + vertical * vertical);

            // Get angle

            // Opposite / Adjacent

            float oa = vertical / hypotenuse;

            float angleTheta = Mathf.Asin(oa) * 180 / Mathf.PI;
            float ang = Mathf.Asin(oa);
            // print("**************");
            // print("Hypotenuse: " + hypotenuse);
            // print("Angle Theta: " + angleTheta);

            // find other angle

            float angleBeta = 90 - angleTheta;
            // print("Angle Beta: " + angleBeta);



            float horizontalLength = (maxZipLength * Mathf.Sin(DegreeToRadian(angleBeta))) / Mathf.Sin(DegreeToRadian(90));

            // print("Horizontal length: " + horizontalLength);


            float verticalLength = Mathf.Sqrt(maxZipLength * maxZipLength - horizontalLength * horizontalLength);

            // print("Vertical length: " + verticalLength);
            if (horizontal < 0)
            {
                horizontalModifier = -1;
                print("Horizontal length: " + horizontalLength);
            }

            if (vertical < 0)
            {
                verticalModifier = -1;
            }

            zipIndicatorObject.transform.position = new Vector2(transform.position.x + horizontalLength * horizontalModifier,
                                                                transform.position.y + verticalLength * verticalModifier);
        }
        //Get hypotenuse



        //zipIndicator = Physics2D.OverlapCircle(zipIndicator.transform.position, zipIndicatorRadius, zipIndicatorOverMask);
        zipIndicatorOverObject = Physics2D.OverlapCircle(zipIndicatorObject.transform.position, zipIndicatorRadius, zipIndicatorOverMask);

    }


    private void HandleIndicatorMovementOld()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");


        Vector2 newPos = new Vector2(zipIndicatorObject.transform.position.x + horizontal * Time.deltaTime * zipSpeed,
                                     zipIndicatorObject.transform.position.y + vertical * Time.deltaTime * zipSpeed);

        if(Vector2.Distance(transform.position, zipIndicatorObject.transform.position) < maxZipLength)
        {
            zipIndicatorObject.transform.position = newPos;
        }
        else
        {
            float horizontalModifier = 1;
            float verticalModifier = 1;




            float hypotenuse = Mathf.Sqrt(horizontal * horizontal + vertical * vertical);

            // Get angle

            // Opposite / Adjacent

            float oa = vertical / hypotenuse;

            float angleTheta = Mathf.Asin(oa) * 180 / Mathf.PI;
            float ang = Mathf.Asin(oa);
            // print("**************");
            // print("Hypotenuse: " + hypotenuse);
            // print("Angle Theta: " + angleTheta);

            // find other angle

            float angleBeta = 90 - angleTheta;
            // print("Angle Beta: " + angleBeta);



            float horizontalLength = (maxZipLength * Mathf.Sin(DegreeToRadian(angleBeta))) / Mathf.Sin(DegreeToRadian(90));

            // print("Horizontal length: " + horizontalLength);


            float verticalLength = Mathf.Sqrt(maxZipLength * maxZipLength - horizontalLength * horizontalLength);

            // print("Vertical length: " + verticalLength);
            if (horizontal < 0)
            {
                horizontalModifier = -1;
                print("Horizontal length: " + horizontalLength);
            }

            if (vertical < 0)
            {
                verticalModifier = -1;
            }

            zipIndicatorObject.transform.position = new Vector2(transform.position.x + horizontalLength * horizontalModifier,
                                                                transform.position.y + verticalLength * verticalModifier);
        }
        //Get hypotenuse

       

        zipIndicatorOverObject = Physics2D.OverlapCircle(zipIndicatorObject.transform.position, zipIndicatorRadius, zipIndicatorOverMask);


    }



    private float RadianToDegree(float angle)
    {
        return angle * (180.0f / Mathf.PI);
    }


    private float DegreeToRadian(float angle)
    {
        return Mathf.PI * angle / 180.0f;
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
        _rigidbody.gravityScale = 1;

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


				
				hurt = true;
			}


            UpdateHealth();

        }
    }

	void HandleDeath() {

		
		GameManager.instance.trialRunning = false;
        playerSprite.SetActive(false);
        _rigidbody.velocity = Vector2.zero;
        deathParticles.Play();
        Invoke("ShowDeathPanel", 2f);

	}

    void ShowDeathPanel()
    {
        deathPanel.SetActive(true);
        DeathPanel dPanel = deathPanel.GetComponent<DeathPanel>();
        dPanel.ShowDeathPanel();
    }


    public void FlickerEnd()
    {
        hurt = false;
    }


    void UpdateHealth()
    {
        healthPanel.UpdateHealth(health);
    }


}


/// DM-CGS-33 - zip spund
/// DM-CGS-34 - zip spund
/// /// DM-CGS-39 - zip spund
/// 
/// DM-CGS-48 metal door close