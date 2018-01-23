using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Switch : MonoBehaviour {


    public GameObject buttonPromptObject;
    public GameObject doorObject;
    SteelDoor door;

    bool inTrigger = false;

    private void Start()
    {
        door = doorObject.GetComponent<SteelDoor>();
    }


    private void Update()
    {
        if(inTrigger)
        {
            if (Input.GetButtonDown("Fire2"))
            {
                print("Open Door");
                door.DoAction();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        buttonPromptObject.SetActive(true);
        inTrigger = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        buttonPromptObject.SetActive(false);
        inTrigger = false;
    }


}
