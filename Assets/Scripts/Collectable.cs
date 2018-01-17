using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : MonoBehaviour {

	public AudioSource collect;
	public SpriteRenderer sprite;



	public void CollectItem() {

		PlayCollectSound ();
		sprite.enabled = false;
		GetComponent<Collider2D> ().enabled = false;

	}



	public void PlayCollectSound() {
		collect.Play();
	}


}
