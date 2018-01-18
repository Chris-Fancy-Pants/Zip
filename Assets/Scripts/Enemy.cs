using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {


    public GameObject explosionCloud;

	// Use this for initialization
	void Start () {


   
    }



    public void DestroyEnemy()
    {
        Instantiate(explosionCloud, transform.position, transform.rotation);
        Destroy(transform.parent.gameObject);
       
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
