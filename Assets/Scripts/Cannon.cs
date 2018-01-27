using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cannon : MonoBehaviour {

    public GameObject cannonBallObject;
    GameObject player;
    public float minFireDistance = 10f;
    bool firing = false;

	public float waitTime = 3f;
    public float xFire = 90f;
    public float yFire = 500f;

	Transform hazards;

	// Use this for initialization
	void Start () {
        player = GameObject.Find("PlayerNew");
		hazards = GameObject.Find ("Hazards").transform;
	}


    private void OnDrawGizmos()
    {
        
    }

    // Update is called once per frame
    void Update () {

        Vector2 dis = transform.position - player.transform.position;
		if(dis.magnitude < minFireDistance && !firing && GameManager.instance.trialRunning)
        {
            //Fire Cannon
            GameObject cBall = Instantiate(cannonBallObject, transform.position, Quaternion.identity) as GameObject;
			cBall.transform.SetParent (hazards);
            Rigidbody2D rb2d = cBall.GetComponent<Rigidbody2D>();

            rb2d.AddForce(new Vector2(xFire * dis.x * -1,yFire));

            firing = true;

            Invoke("ResetFiring", waitTime);
        }

	}

    void ResetFiring()
    {
        firing = false;
    }


}
