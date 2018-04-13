using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fan : MonoBehaviour {

	public Transform fanBlades;
	public float speed = 1f;
	float rot = 0;

	// Use this for initialization
	void Start () {
		rot = Random.Range (0, 360);
	}
	
	// Update is called once per frame
	void Update () {
		rot += speed * Time.deltaTime;

		if (rot > 360) {
			rot = rot - 360;
		}

		fanBlades.eulerAngles = new Vector3 (0, 0, rot);
	

	}
}
