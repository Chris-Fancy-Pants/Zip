using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndCastle : MonoBehaviour {


	public GameObject[] castleObjects;

	public GameObject castleLeft;
	public GameObject castleRight;
	public GameObject castleEntrance;



	Animator animator;


	Vector2 startPos;
	Vector2 targetPos;


	void Awake() {


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



	IEnumerator RaiseCastle() {

		while (transform.position.y < targetPos.y) {

			transform.position = new Vector2 (transform.position.x, transform.position.y + 0.2f);

			yield return new WaitForSeconds (0.05f);

		}

		castleRight.GetComponent<SpriteRenderer> ().sortingLayerName = "Foreground1";
	}

}
