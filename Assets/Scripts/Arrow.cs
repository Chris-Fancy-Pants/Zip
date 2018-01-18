using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour {

    public void FireArrow(float speed, bool flip = false)
    {
        GetComponentInChildren<SpriteRenderer>().flipX = flip;

   

        GetComponent<Rigidbody2D>().velocity = new Vector2(speed, 0);
    }
}
