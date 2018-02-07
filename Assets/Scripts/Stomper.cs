using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stomper : MonoBehaviour {


    public Transform openPosition;
    public Transform closedPosition;

    public AudioSource doorStopSound;

    public float speed = 10f;

    public enum MoveDirection
    {
        up,
        down
    };

    public MoveDirection direction = MoveDirection.down;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        if(direction == MoveDirection.down)
        {
            if(transform.position.y > closedPosition.position.y)
            {
                //move Down
                transform.position = new Vector2(transform.position.x, transform.position.y - speed * Time.deltaTime);
            } else
            {
                direction = MoveDirection.up;
                doorStopSound.Play();
            }
        } else if(direction == MoveDirection.up)
        {
            if(transform.position.y < openPosition.position.y)
            {
                // move up
                transform.position = new Vector2(transform.position.x, transform.position.y + speed * Time.deltaTime);
            } else
            {
                direction = MoveDirection.down;
            }
        }
		
	}

	void OnCollisionEnter2D(Collision2D col) {

		if (col.gameObject.CompareTag ("Player")) {

			col.gameObject.GetComponent<Player> ().takeDamage (10);


		}


	}


}
