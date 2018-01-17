using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverScreen : MonoBehaviour {


	public TextMesh boltsValue;
	public TextMesh timeValue;
	public GameObject cam;

	SpriteRenderer transitionScreenSprite;


	public GameObject ContinueText;
	bool canContinue  = false;


	// Use this for initialization
	void Start () {


		transitionScreenSprite = GameObject.Find ("TransitionScreen").GetComponent<SpriteRenderer> ();
		cam = GameObject.Find ("Camera");

	}


	public void UpdateGameOver(int bolts, float timeTaken) {
		
		cam.GetComponent<DeadzoneCamera> ().enabled = false;

		cam.transform.position = new Vector3(transform.position.x, transform.position.y, -20);

		StartCoroutine (UpdateBolts(bolts, timeTaken));

	}


	IEnumerator UpdateBolts(int bolts, float timeTaken) {
		int b = 0;

		while (b < bolts) {
			b++;
			boltsValue.text = b.ToString ();

			yield return new WaitForSeconds (0.05f);

		}


		StartCoroutine (UpdateTime (timeTaken));



	}


	IEnumerator UpdateTime(float timeTaken) {

		float t = 0;

		while (t < timeTaken) {

			t += 0.5f;

			timeValue.text = "Time: " + t.ToString ("N2");
			yield return new WaitForSeconds (0.0001f);
		}



		ContinueText.SetActive (true);
		canContinue = true;

	}


	IEnumerator TransitionScene() {

		print ("Start Coroutine");
		while (transitionScreenSprite.color.a < 1) {
			print ("In Coroutine");
			transitionScreenSprite.color = new Vector4 (transitionScreenSprite.color.r, transitionScreenSprite.color.g, transitionScreenSprite.color.b, transitionScreenSprite.color.a + 0.1f);

			yield return new WaitForSeconds (0.1f);
		}
		print("End Coroutine");

		GameManager.instance.LoadLevel (1, 0);
		ContinueText.SetActive (false);
		canContinue = false;

	}
	
	// Update is called once per frame
	void Update () {

		if (canContinue && Input.GetButtonDown ("Jump")) {

			StartCoroutine ("TransitionScene");

		}

	}
}
