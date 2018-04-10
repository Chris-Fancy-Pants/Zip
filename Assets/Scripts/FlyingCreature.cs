using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingCreature : MonoBehaviour {

    Transform playerTransform;
    public float minEngageDistance = 10f;
    Animator _animator;
    public Transform projectileSpawn;

    public GameObject projectile;

    bool attacking = false;
	// Use this for initialization
	void Start () {

        playerTransform = GameObject.Find("PlayerNew").transform;
        _animator = GetComponent<Animator>();

	}
	
	// Update is called once per frame
	void Update () {

        //Get Distance to player
        if (!attacking)
        {
            float distance = Vector2.Distance(transform.position, playerTransform.position);


            if (distance < minEngageDistance)
            {
                // Attack Player
                AttackPlayer();
            }
        }
		
	}


    void AttackPlayer()
    {
        attacking = true;
        _animator.SetTrigger("OpenMouth");
        print("ATTACK");

        Invoke("SetAttackingFalse", 3f);
        
    }

    void SpawnProjectile()
    {
        GameObject proj = Instantiate(projectile, projectileSpawn.position, Quaternion.identity) as GameObject; 
        Vector3 moveVector = playerTransform.position - transform.position;
        Rigidbody2D projBody = proj.GetComponent<Rigidbody2D>();
        projBody.gravityScale = 0;
        projBody.velocity = moveVector;

    }





    void SetAttackingFalse ()
    {
        attacking = false;
    }


}
