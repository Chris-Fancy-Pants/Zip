using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cannon : MonoBehaviour {

    public GameObject cannonBallObject;
    GameObject player;
    float minFireDistance = 10f;
    bool firing = false;

    public float xFire = 90f;
    public float yFire = 500f;


	// Use this for initialization
	void Start () {
        player = GameObject.Find("Player");
	}


    private void OnDrawGizmos()
    {
        
    }

    // Update is called once per frame
    void Update () {

        Vector2 dis = transform.position - player.transform.position;
        if(dis.magnitude < minFireDistance && !firing)
        {
            //Fire Cannon
            GameObject cBall = Instantiate(cannonBallObject, transform.position, Quaternion.identity) as GameObject;

            Rigidbody2D rb2d = cBall.GetComponent<Rigidbody2D>();

            rb2d.AddForce(new Vector2(xFire * dis.x * -1,yFire));

            firing = true;

            Invoke("ResetFiring", 1f);
        }

	}

    void ResetFiring()
    {
        firing = false;
    }


}
