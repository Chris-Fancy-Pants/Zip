using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowSpawnner : MonoBehaviour {


    public GameObject arrowPrefab;
    public Transform arrowSpawnPosition;
    public float minSpeed = 1f;
    public float maxSpeed = 2f;
    public float spawnWait = 5f;

    GameObject hazards;

	// Use this for initialization
	void Start () {
        

        hazards = GameObject.Find("Hazards");

        FireArrow();
    }
	
	// Update is called once per frame
	void Update () {
		
	}


    void FireArrow()
    {
        if (GameManager.instance.trialRunning)
        {
            float speed = Random.Range(minSpeed, maxSpeed);
            GameObject arrowObject = Instantiate(arrowPrefab, arrowSpawnPosition.position, transform.rotation) as GameObject;
            Arrow arrow = arrowObject.GetComponent<Arrow>();

            if (hazards.transform)
            {
                arrowObject.transform.SetParent(hazards.transform);
            }
            else
            {
                print("Error");
            }
            arrow.FireArrow(speed);
        }
        Invoke("FireArrow", spawnWait);


    }


}
