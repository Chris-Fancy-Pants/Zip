using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndCastle : MonoBehaviour {


	public GameObject[] castleObjects;

	public GameObject castleLeft;
	public GameObject castleRight;
	public GameObject castleEntrance;


    public Transform leftEmergencePoint;
    public Transform rightEmergencePoint;


    public GameObject explosion;


    public GameOverPanel gameOverPanelObject;
    GameOverPanel gameOverPanel;

    bool castleRaised = false;

	Animator animator;


	Vector2 startPos;
	Vector2 targetPos;


	void Awake() {


        gameOverPanel = gameOverPanelObject.GetComponent<GameOverPanel>();



		castleLeft.SetActive (false);
		castleRight.SetActive (false);
		castleEntrance.SetActive (false);

		

		animator = GetComponent<Animator> ();


		startPos = transform.position;
		targetPos = new Vector2 (startPos.x, startPos.y + 8);


	}


	public void AwakeEndCastle() {
		
		castleLeft.SetActive (true);
		castleRight.SetActive (true);
		castleEntrance.SetActive (true);

	

		StartCoroutine (RaiseCastle ());
		//animator.SetTrigger ("RaiseCastle");

	}


    IEnumerator RaiseCastleGroundEffects()
    {
        while(!castleRaised)
        {
            float waitTime = Random.Range(0, 0.5f);

            float xPos = Random.Range(leftEmergencePoint.position.x, rightEmergencePoint.position.x);
            float yPos = Random.Range(leftEmergencePoint.position.y, rightEmergencePoint.position.y);
            Vector2 spawnPosition = new Vector2(xPos, yPos);

            Instantiate(explosion, spawnPosition, Quaternion.identity);

            yield return new WaitForSeconds(waitTime);
        }


    }



	IEnumerator RaiseCastle() {

        leftEmergencePoint.SetParent(null);
        rightEmergencePoint.SetParent(null);

        StartCoroutine(RaiseCastleGroundEffects());

        while (transform.position.y < targetPos.y) {

			transform.position = new Vector2 (transform.position.x, transform.position.y + 0.2f);

			yield return new WaitForSeconds (0.05f);

		}

		castleRight.GetComponent<SpriteRenderer> ().sortingLayerName = "Foreground1";
        castleRaised = true;
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {

        print("End Castle Collision");


        if(collision.gameObject.CompareTag("Player"))
        {
            Player player = collision.gameObject.GetComponent<Player>();
            gameOverPanel.UpdateGameOver();
            player.EndRunOver();
        }
    }
}
