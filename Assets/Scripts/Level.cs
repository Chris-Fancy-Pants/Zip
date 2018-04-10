using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour {


    public int currentTrial = 1;
    public int currentLevel = 1;
    public int nextTrial = 1;
    public int nextLevel = 1;

	// Use this for initialization
	void Start () {
        Dictionary<string, string> levelDetails = GameManager.instance.GetLevelDetails();

        currentLevel = int.Parse(levelDetails["Level"]);
        currentTrial = int.Parse(levelDetails["Trial"]);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
