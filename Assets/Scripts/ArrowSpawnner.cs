using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowSpawnner : MonoBehaviour {


    public GameObject arrowPrefab;
    public Transform arrowSpawnPosition;
    public float minSpeed = 1f;
    public float maxSpeed = 2f;

	// Use this for initialization
	void Start () {
        FireArrow();
	}
	
	// Update is called once per frame
	void Update () {
		
	}


    void FireArrow()
    {
        float speed = Random.Range(minSpeed, maxSpeed);
        GameObject arrowObject = Instantiate(arrowPrefab, arrowSpawnPosition.position, transform.rotation) as GameObject;
        Arrow arrow = arrowObject.GetComponent<Arrow>();
        arrow.FireArrow(speed);
        Invoke("FireArrow", 3f);


    }


}
