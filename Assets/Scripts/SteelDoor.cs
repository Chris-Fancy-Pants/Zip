using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SteelDoor : MonoBehaviour {

    public Transform openPosition;
    public float closedYPosition;

    public GameObject doorObject;

    public float speed = 0.001f;


    public AudioSource doorMoving;

    public enum Action
    {
        Open,
        Close,
        Toggle
    };

    public Action action = Action.Open;

	public AudioSource stopSound;

    public void DoAction()
    {
        StartCoroutine("OpenDoor");
    }

    IEnumerator OpenDoor()
    {
        doorMoving.Play();

        while(doorObject.transform.position.y < openPosition.position.y)
        {
            doorObject.transform.position = new Vector2(doorObject.transform.position.x, doorObject.transform.position.y + speed * Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }

        doorMoving.Stop();
		stopSound.Play ();
        yield return null;
    }


}
