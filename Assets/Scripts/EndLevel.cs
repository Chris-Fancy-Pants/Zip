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
            print("End Level Collision");
            GameManager.instance.trialRunning = false;

            GameObject hazards = GameObject.Find("Hazards");

            foreach(Transform t in hazards.transform)
            {
                Destroy(t.gameObject);
            }

        }
    }


    public void PlaySound()
    {

		GetComponent<Collider2D> ().enabled = false;
		GetComponent<AudioSource>().Play ();

	}

}
