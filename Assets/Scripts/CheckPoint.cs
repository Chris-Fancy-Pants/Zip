using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour {


    Animator _animator;
    bool isSet = false;

	// Use this for initialization
	void Start () {
        _animator = GetComponent<Animator>();
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            if(!isSet)
            {
                _animator.SetTrigger("RaiseFlag");
            }

            isSet = true;
        }
    }



}
