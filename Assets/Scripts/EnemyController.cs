using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : PhysicsObject {

    public Transform frontCheck;
    public LayerMask frontCheckLayerMask;


    public float xCheckRadius = 0.2f;
    // Use this for initialization
    void Start()
    {
        targetVelocity = new Vector2(1, 0);
    }

    private void OnDrawGizmos()
    {

        Gizmos.DrawSphere(frontCheck.position, xCheckRadius);
    }


    // Update is called once per frame
    void Update () {

        bool frontHit = Physics2D.OverlapCircle(frontCheck.position, xCheckRadius, frontCheckLayerMask);
        if(frontHit)
        {

            targetVelocity = new Vector2(targetVelocity.x * -1, 0);
            transform.localScale = new Vector2(transform.localScale.x * -1, transform.localScale.y);
        }
    }
}
