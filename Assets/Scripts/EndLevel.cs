using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndLevel : MonoBehaviour {





	public void PlaySound() {

		GetComponent<Collider2D> ().enabled = false;
		GetComponent<AudioSource>().Play ();

	}

}
