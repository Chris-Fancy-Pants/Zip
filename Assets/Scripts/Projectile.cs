using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {


    public GameObject ExplosionCloud;
    public int damage = 1;
    Collider2D col;
    SpriteRenderer sRenderer;

	public AudioSource explosion;

	// Use this for initialization
	void Start () {
        col = GetComponent<Collider2D>();
        sRenderer = GetComponent<SpriteRenderer>();
	}

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<Player>().takeDamage(damage);
        }
        Instantiate(ExplosionCloud, transform.position, transform.rotation);
        RemoveSpriteAndCollider();
    }

    void RemoveSpriteAndCollider()
    {
		explosion.Play ();
        col.enabled = false;
        sRenderer.enabled = false;
        Destroy(this.gameObject, 2f);
    }




}
