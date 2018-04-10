using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YellowSpikeBall : MonoBehaviour {


    float speed = 1f;

	// Update is called once per frame
	void Update () {

        transform.Rotate(Vector3.right * Time.deltaTime);

    }
}
