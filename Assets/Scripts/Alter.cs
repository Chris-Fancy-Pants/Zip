using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Alter : MonoBehaviour {

	public GameObject alterChargeBolt;
    public bool activated = false;
	public AudioSource alterSound;

	void OnTriggerEnter2D(Collider2D col) {

		if (!activated) {
			if (col.CompareTag ("Player")) {
				PlayerController player = col.gameObject.GetComponent<PlayerController> ();
				ActivateAlter (player);
				activated = true;
			}
		}

	}



	void ActivateAlter(PlayerController player) {


		alterSound.Play ();
		alterChargeBolt.SetActive (true);
	}

}
