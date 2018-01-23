using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndLevel : MonoBehaviour {


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            PlaySound();
            GameObject endCastleObject = GameObject.Find("End Castle");
            EndCastle endCastle = endCastleObject.GetComponent<EndCastle>();
            endCastle.AwakeEndCastle();
        }
    }


    public void PlaySound()
    {

		GetComponent<Collider2D> ().enabled = false;
		GetComponent<AudioSource>().Play ();

	}

}
