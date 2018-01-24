using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : MonoBehaviour {

	public AudioSource collect;
	public SpriteRenderer sprite;

    public int value = 1;


    private void Start()
    {
    }

    public void CollectItem(Player player) {

		PlayCollectSound ();
		sprite.enabled = false;
		GetComponent<Collider2D> ().enabled = false;
        GameManager.instance.boltsThisTrial++;
        player.AddBolt(this.value);

    }



	public void PlayCollectSound() {
		collect.Play();
	}


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            CollectItem(collision.gameObject.GetComponent<Player>());
        }
    }

}
