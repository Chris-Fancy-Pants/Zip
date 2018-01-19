using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour {


    bool moving = false;
    float arrowSpeed = 0;

    public void FireArrow(float speed, bool flip = false)
    {

        int flipInt = 1;

        if(flip)
        {
            flipInt = -1;
        }


        transform.localScale = new Vector3(transform.localScale.x * flipInt, transform.localScale.y, transform.localScale.z);

        arrowSpeed = speed;
        moving = true;
    }


    private void Update()
    {
       
        if (moving)
        {
            transform.position = new Vector2(transform.position.x + arrowSpeed * Time.deltaTime, transform.position.y);
        }
    }

}
