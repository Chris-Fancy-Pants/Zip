using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cloud : MonoBehaviour {


    float resetXPos = 0;
    float startPos = 0;
    public float speed = -1f;
	// Use this for initialization
	void Start () {

        resetXPos = GameObject.Find("CloudResetPoint").transform.position.x;
        startPos = GameObject.Find("CloudStartPos").transform.position.x;
        speed = Random.Range(-2f, -0.7f);
	}
	
	// Update is called once per frame
	void Update () {

        transform.position = new Vector2(transform.position.x + speed * Time.deltaTime, transform.position.y);

        if(transform.position.x < resetXPos)
        {
            transform.position = new Vector2(startPos, transform.position.y);
        }
		
	}
}
