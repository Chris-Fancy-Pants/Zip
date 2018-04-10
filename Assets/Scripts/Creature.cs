using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Creature : MonoBehaviour {


    public ContactFilter2D contactFilter;
    public Transform rayStart;
    public float speed = 1.2f;
    public Vector2 currentVector = Vector2.right;
    Rigidbody2D _rigidbody;
	// Use this for initialization
	void Start () {
        _rigidbody = GetComponent<Rigidbody2D>();
        _rigidbody.velocity = currentVector * speed;
	}
	
	// Update is called once per frame
	void FixedUpdate () {

        RaycastHit2D hit = Physics2D.Raycast(rayStart.position, currentVector);
        if (hit.collider != null)
        {
            Debug.DrawRay(rayStart.position, currentVector, Color.green);


           // if (hit.collider.CompareTag("Player") || hit.collider.CompareTag("Map"))
            //{
               
                float distance = Mathf.Abs(hit.point.x - rayStart.position.x);


           

                if (distance < 0.1f)
                {
                   
                    transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
                    currentVector *= -1;
                    _rigidbody.velocity = currentVector * speed;
                }
            }
           // float distance = Mathf.Abs(hit.point.x - rayStart.position.x);
           // 
           // print(hit.transform.name + "is " + distance + " away.");
        //}

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            Player player = collision.gameObject.GetComponent<Player>();
            player.takeDamage(1);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        _rigidbody.velocity = currentVector * speed;
    }
}
