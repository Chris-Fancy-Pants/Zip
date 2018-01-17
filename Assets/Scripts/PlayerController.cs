using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DigitalRuby.LightningBolt;
using UnityEngine.SceneManagement;

public class PlayerController : PhysicsObject
{

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


	public Vector3 endCameraOffset;
	public float normalCameraZoom = 5f;
	public float endCameraZoom = 10f;

	bool endRun = false;
	bool gameOver = false;

    SpriteRenderer zipIndicatorSpriteRenderer;
	bool doingZip = false;

	bool zipBuildingCharge = false;

    public GameObject zipIndicator;


	public GameObject LightningObject;
	LightningBoltScript lightningScript;

	public int bolts = 0;


	public GameObject gameOverScreenObject;
	GameOverScreen gameOverScreen;

	public float timeinc = 0.1f;
	float timeTaken = 0;
	public Text timeText;
    // Use this for initialization
    void Awake()
    {
        zipIndicatorSpriteRenderer = zipIndicator.GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        zipChargeText.text = "Zip: " + maxZipCharge;
        zipCharge = maxZipCharge;
        deadZoneCam = _camera.GetComponent<DeadzoneCamera>();
		lightningScript = LightningObject.GetComponent<LightningBoltScript> ();
		gameOverScreen = gameOverScreenObject.GetComponent<GameOverScreen> ();

		Scene s = SceneManager.GetSceneByName ("TEst");
		if (s.IsValid()) {


			print ("Something loaded");



		} else {


			print ("Noppppppe");

		}

	
		StartCoroutine ("TimeKeeper");
		//print ("GameManager: " + GameManager.instance.trials);
    }



	IEnumerator TimeKeeper() {

		while(!gameOver) {

			timeTaken += timeinc;
			timeText.text = "Time: " + timeTaken.ToString ();
			yield return new WaitForSeconds (timeinc);
		}

	}


    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(zipIndicator.transform.position, zipIndicatorHitRadius);
    }

    protected override void ComputeVelocity()
    {
		if (!gameOver) {
			
			Vector2 move = Vector2.zero;
			bool overObject = true;

			if (shouldFall == 0) {
				overObject = Physics2D.OverlapCircle (zipIndicator.transform.position, zipIndicatorHitRadius, zipIndicatorLayerMask);
			} else {
				move.x = Input.GetAxis ("Horizontal");
			}
        

			if (Input.GetButtonDown ("Jump") && grounded) {
				jumpSound.Play ();
				velocity.y = jumpTakeOffSpeed;
				shouldFall = 1;
				animator.SetTrigger ("jump");
			} else if (Input.GetButtonUp ("Jump")) {
				if (velocity.y > 0) {
					velocity.y = velocity.y * 0.5f;
					shouldFall = 1;
				}
			}


			if (Input.GetButtonDown ("Fire1") && !grounded) {

				StartZip ();

			} else if (Input.GetButtonUp ("Fire1") && zipBuildingCharge) {


	
				if (!overObject) {
					transform.position = zipIndicator.transform.position;
					zipCompleteSound.Play ();
				}
				StopZip ();


			}

			if (zipBuildingCharge) {

				zipCharge--;
				lightningScript.SetLightningPos (transform.position, zipIndicator.transform.position);
				float zipIndicatorHorizontal = Input.GetAxis ("Horizontal");
				float zipIndicatorVertical = Input.GetAxis ("Vertical");
				float zipModifierHorizontal = zipIndicatorHorizontal * zipSpeed * Time.deltaTime;
				float zipModifierVertical = zipIndicatorVertical * zipSpeed * Time.deltaTime;

				Vector2 nextIndicatorPosition = new Vector2 (zipIndicator.transform.position.x + zipModifierHorizontal, zipIndicator.transform.position.y + zipModifierVertical);

				if (Vector2.Distance (transform.position, nextIndicatorPosition) < 5) {

					_cam.orthographicSize += 0.3f;
					zipIndicator.transform.position = nextIndicatorPosition;

				}

				if (zipCharge <= 0) {
					zipBuildingCharge = false;
				}
			}

			if (zipCharge <= 0) {
				StopZip ();
			
		
			}



			if (shouldFall == 0) { // Zip Active
           
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

	void StartZip() {
		LightningObject.SetActive (true);
		lightningScript.FireLightning (transform.position, zipIndicator.transform.position);
		zipIndicator.SetActive(true);
		deadZoneCam.ChangeTarget(zipIndicator);
		if (!zipIndicator.activeInHierarchy)
		{
			zipIndicator.SetActive(true);
		}
		zipBuildingCharge = true;
		shouldFall = 0;

		// Play Zip arc sound
		zipArcSound.Play();


	}

	void StopZip() {
		LightningObject.SetActive (false);
		zipIndicator.SetActive (false);
		zipBuildingCharge = false;
		zipIndicator.transform.position = transform.position;
		shouldFall = 1;
		StartCoroutine ("RechargeZip");
		deadZoneCam.ChangeTarget (this.gameObject);
		_cam.orthographicSize = 5;
		// Stop zip arcing sound
		zipArcSound.Stop();



	}


	void NoZip() {
		zipIndicator.SetActive(false);

		shouldFall = 1;
		zipIndicator.transform.position = transform.position;
		deadZoneCam.ChangeTarget(this.gameObject);
		if (!zipRecharging)
		{
			StartCoroutine("RechargeZip");
		}
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
        shouldFall = 1;
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
            zipChargeText.text = "Zip: " + zipCharge;
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
			collectable.CollectItem ();
			bolts++;


		}

		if (col.CompareTag ("InsideCastle")) {

			gameOverScreen.UpdateGameOver (bolts, timeTaken);

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
