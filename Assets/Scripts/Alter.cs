using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Alter : MonoBehaviour {

	public GameObject alterChargeBolt;


	void OnTriggerEnter2D(Collider2D col) {


		if (col.CompareTag ("Player")) {

			ActivateAlter ();

		}

	}



	void ActivateAlter() {

		alterChargeBolt.SetActive (true);
	}

}
