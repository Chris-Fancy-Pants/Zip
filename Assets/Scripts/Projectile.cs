using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {


    public GameObject ExplosionCloud;

    Collider2D col;
    SpriteRenderer sRenderer;

	// Use this for initialization
	void Start () {
        col = GetComponent<Collider2D>();
        sRenderer = GetComponent<SpriteRenderer>();
	}

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Instantiate(ExplosionCloud, transform.position, transform.rotation);
        RemoveSpriteAndCollider();
    }

    void RemoveSpriteAndCollider()
    {
        col.enabled = false;
        sRenderer.enabled = false;
    }




}
