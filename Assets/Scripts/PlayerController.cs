using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DigitalRuby.LightningBolt;
using UnityEngine.SceneManagement;

public class PlayerController : PhysicsObject
{


    public int health = 3;

	[Header("Sound Effects")]
	public AudioSource jumpSound;
	public AudioSource footstep;
	public AudioSource zipArcSound;
	public AudioSource zipCompleteSound;

    public float maxSpeed = 7;
    public float jumpTakeOffSpeed = 7;

    public SpriteRenderer spriteRenderer;
    private Animator animator;

	public Camera _cam;
    public GameObject _camera;
    DeadzoneCamera deadZoneCam;

    public float zipSpeed = 5;
    public float zipCharge = 0;
    public int maxZipCharge = 100;
    public bool isJumping = false;
    public LayerMask zipIndicatorLayerMask;
    bool zipping = false;
    public float zipIndicatorHitRadius = 0.7f;
    bool zipRecharging = false;
    public Text zipChargeText;
    public float zipRechargeSpeed = 0.7f;
	public float zipChargeCost = 1.5f;
	public float zipMoveSpeed  = 4f;


    public Transform frontCheck;
    public Transform rearCheck;

	public Vector3 endCameraOffset;
	public float normalCameraZoom = 5f;
	public float endCameraZoom = 10f;

	bool endRun = false;
	bool gameOver = false;

    SpriteRenderer zipIndicatorSpriteRenderer;
	bool doingZip = false;

	bool zipBuildingCharge = false;

    bool canZip = false;

    public GameObject zipIndicator;


	public GameObject LightningObject;
	LightningBoltScript lightningScript;

	public int bolts = 0;


	public GameObject gameOverScreenObject;
    GameOverPanel gameOverScreen;

	public float timeinc = 0.1f;
	float timeTaken = 0;
	public Text timeText;

    [Header("Alters")]
    public GameObject alter1Object;
    public GameObject alter2Object;
    public GameObject alter3Object;

    Alter alter1;
    Alter alter2;
    Alter alter3;

    public float maxZipLength = 5f;

    [Header("HUD")]
    public Text healthText;

    // Use this for initialization
    void Awake()
    {
        zipIndicatorSpriteRenderer = zipIndicator.GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        
        zipCharge = maxZipCharge;
        deadZoneCam = _camera.GetComponent<DeadzoneCamera>();
		lightningScript = LightningObject.GetComponent<LightningBoltScript> ();
      
        gameOverScreen = gameOverScreenObject.GetComponent<GameOverPanel> ();

        alter1 = alter1Object.GetComponent<Alter>();
        alter2 = alter2Object.GetComponent<Alter>();
        alter3 = alter3Object.GetComponent<Alter>();

        
        
    }



    protected override void ComputeVelocity()
    {
        

        if (!gameOver) {
			

            if(grounded)
            {
                canZip = true;
            }


			Vector2 move = Vector2.zero;
			bool overObject = true;

			if (!shouldFall) {
				overObject = Physics2D.OverlapCircle (zipIndicator.transform.position, zipIndicatorHitRadius, zipIndicatorLayerMask);
            } else {
				move.x = Input.GetAxis ("Horizontal");
			}
        

			if (Input.GetButtonDown ("Jump") && CanJump(move.x)) {
				jumpSound.Play ();
				velocity.y = jumpTakeOffSpeed;
			
			} else if (Input.GetButtonUp ("Jump")) {
				if (velocity.y > 0) {
					velocity.y = velocity.y * 0.5f;
				}
			}


			if (Input.GetButtonDown ("Fire1") && !grounded && canZip) {

				StartZip ();

			} else if (Input.GetButtonUp ("Fire1") && zipBuildingCharge) {


	
				if (!overObject) {
					transform.position = zipIndicator.transform.position;
					zipCompleteSound.Play ();
				}
				StopZip ();


			}

			

			



			if (!shouldFall) { // Zip Active
           
				if (overObject) {
					zipIndicatorSpriteRenderer.color = Color.red;
				} else {
					zipIndicatorSpriteRenderer.color = Color.white;
				}
			}


			//bool flipSprite = (spriteRenderer.flipX ? (move.x > 0.01f) : (move.x < 0.01f));
			if (velocity.x < 0) {
				spriteRenderer.flipX = true;
			} else {
				spriteRenderer.flipX = false;
			}

			animator.SetBool("grounded", grounded);
			animator.SetFloat ("velocityX", Mathf.Abs (velocity.x) / maxSpeed);
			animator.SetFloat ("velocityY", velocity.y);
			targetVelocity = move * maxSpeed;




			if (zipCharge < 0) {
				zipCharge = 0;
			}

			if (doingZip) {
				transform.position = Vector2.MoveTowards (transform.position, zipIndicator.transform.position, zipMoveSpeed);
			}
		}


		if (endRun) {

			animator.SetFloat ("velocityX", Mathf.Abs (velocity.x) / maxSpeed);
			spriteRenderer.flipX = false;
			targetVelocity = new Vector2 (10, 0);


		}


    }

    private void LateUpdate()
    {
        if (zipBuildingCharge)
        {

            /*
             zipCharge--;
             lightningScript.SetLightningPos (transform.position, zipIndicator.transform.position);
             float zipIndicatorHorizontal = Input.GetAxis ("Horizontal");
             float zipIndicatorVertical = Input.GetAxis ("Vertical");
             float zipModifierHorizontal = zipIndicatorHorizontal * zipSpeed * Time.deltaTime;
             float zipModifierVertical = zipIndicatorVertical * zipSpeed * Time.deltaTime;

             Vector2 nextIndicatorPosition = new Vector2 (zipIndicator.transform.position.x + zipModifierHorizontal, zipIndicator.transform.position.y + zipModifierVertical);

             if (Vector2.Distance (transform.position, nextIndicatorPosition) < 5) {

                 //_cam.orthographicSize += 0.3f;
                 zipIndicator.transform.position = nextIndicatorPosition;

             }

             if (zipCharge <= 0) {
                 zipBuildingCharge = false;
             }
             */
            HandleZip();

        }
    }

    void HandleZip()
    {
     



        lightningScript.SetLightningPos(transform.position, zipIndicator.transform.position);



        
        HandleIndicatorMovement();

        if (zipIndicator)
        {
            zipIndicator.GetComponent<SpriteRenderer>().color = Color.red;
        }
        else
        {
            zipIndicator.GetComponent<SpriteRenderer>().color = Color.white;
        }
    }


    private void HandleIndicatorMovement()
    {
        

        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);



        float horizontal = mousePos.x - transform.position.x;
        float vertical = mousePos.y - transform.position.y;


        if (Vector2.Distance(transform.position, zipIndicator.transform.position) < maxZipLength)
        {
            zipIndicator.transform.position = mousePos;
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

            zipIndicator.transform.position = new Vector2(transform.position.x + horizontalLength * horizontalModifier,
                                                                transform.position.y + verticalLength * verticalModifier);
        }
        //Get hypotenuse



        //zipIndicator = Physics2D.OverlapCircle(zipIndicator.transform.position, zipIndicatorRadius, zipIndicatorOverMask);


    }




    private void HandleIndicatorMovementOld()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");




        Vector2 newPos = new Vector2(zipIndicator.transform.position.x + horizontal * Time.deltaTime * zipSpeed,
                                     zipIndicator.transform.position.y + vertical * Time.deltaTime * zipSpeed);

        if (Vector2.Distance(transform.position, zipIndicator.transform.position) < maxZipLength)
        {
            zipIndicator.transform.position = newPos;
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

            zipIndicator.transform.position = new Vector2(transform.position.x + horizontalLength * horizontalModifier,
                                                                transform.position.y + verticalLength * verticalModifier);
        }
        //Get hypotenuse



        //zipIndicator = Physics2D.OverlapCircle(zipIndicator.transform.position, zipIndicatorRadius, zipIndicatorOverMask);


    }



    private float RadianToDegree(float angle)
    {
        return angle * (180.0f / Mathf.PI);
    }


    private float DegreeToRadian(float angle)
    {
        return Mathf.PI * angle / 180.0f;
    }


    bool CanJump(float move)
    {
        if(grounded)
        {
            return true;
        }

        bool canJump = false;

        if(move < 0 )
        {
            print("Checking Front");
            canJump = Physics2D.OverlapCircle(frontCheck.position, 0.1f);
        } else if(move > 0)
        {
            print("Checking Back");
            canJump = Physics2D.OverlapCircle(rearCheck.position, 0.1f);
        }



        return canJump;
    }

	void StartZip() {

        SetShouldFall(false);

		LightningObject.SetActive (true);
		lightningScript.FireLightning (transform.position, zipIndicator.transform.position);
		zipIndicator.SetActive(true);
		//deadZoneCam.ChangeTarget(zipIndicator);
		if (!zipIndicator.activeInHierarchy)
		{
			zipIndicator.SetActive(true);
		}
		zipBuildingCharge = true;

        print("Setting shouldFall to false");

		shouldFall = false;

        print("shouldFall set to false");

		// Play Zip arc sound
		zipArcSound.Play();


	}

	void StopZip() {

        

		LightningObject.SetActive (false);
		zipIndicator.SetActive (false);
		zipBuildingCharge = false;
		zipIndicator.transform.position = transform.position;
        //print("Setting shouldFall to true");
        SetShouldFall(true);
        //print("shouldFall set to true");
        StartCoroutine ("RechargeZip");
		deadZoneCam.ChangeTarget (this.gameObject);
		//_cam.orthographicSize = 5;
		// Stop zip arcing sound
		zipArcSound.Stop();



	}




	void DoZip() {

		doingZip = true;

//		transform.position = zipIndicator.transform.position;
//		zipIndicator.transform.position = transform.position;

	}


    void EndZip(bool overObject)
    {
        zipIndicator.SetActive(false);
        if (!overObject)
        {
			DoZip ();
        }
        shouldFall = true;
        zipIndicator.transform.position = transform.position;
        deadZoneCam.ChangeTarget(this.gameObject);
        if (!zipRecharging)
        {
            StartCoroutine("RechargeZip");
        }
    }

    IEnumerator RechargeZip()
    {
        print("Start Coroutine");
        zipRecharging = true;
        while (zipCharge < maxZipCharge)
        {
            zipCharge++;
           // zipChargeText.text = "Zip: " + zipCharge;
            yield return new WaitForSeconds(zipRechargeSpeed);
        }
        zipRecharging = false;
        yield return null;
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Enemy"))
        {
            print("Hit Enemy");
            health -= 1;
            Enemy enemy = collision.gameObject.GetComponent<Enemy>();
            enemy.DestroyEnemy();
            
            
        }

        print("HIT");

    }


	void OnTriggerEnter2D(Collider2D col) {

		if (col.CompareTag ("End Level")) {

			print ("End Level");
			EndLevel endLevel = col.gameObject.GetComponent<EndLevel> ();
			endLevel.PlaySound ();
			DoGameOver ();
		}


		if (col.CompareTag ("Bolt")) {


			Collectable collectable = col.GetComponent<Collectable> ();
		//	collectable.CollectItem ();
			bolts++;


		}

		if (col.CompareTag ("InsideCastle")) {

			//gameOverScreen.UpdateGameOver (bolts, timeTaken, alter1.activated, alter2.activated, alter3.activated);

		}


	}


	void DoGameOver() {

		gameOver = true;
		animator.SetFloat ("velocityX", 0);
		animator.SetBool ("grounded", true);
		GameObject endCastleObject = GameObject.Find ("End Castle");
		EndCastle endCastle = endCastleObject.GetComponent<EndCastle> ();
		endCastle.AwakeEndCastle ();
		StartCoroutine ("GameOverRun");
		Vector3 endPosCam = transform.position + endCameraOffset;
		deadZoneCam.EndGame (endPosCam);





	}

	public void PlayFootstep() {
		footstep.Play ();

	}




	IEnumerator GameOverRun() {
		//_cam.orthographicSize = endCameraZoom;
		yield return new WaitForSeconds (3f);

		endRun = true;

	}





}
