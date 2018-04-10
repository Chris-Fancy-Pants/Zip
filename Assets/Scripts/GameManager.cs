using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour {


	public static GameManager instance = null;

    public int[] trialCounts;



    public int boltsThisTrial;
    public bool trialRunning = true;

    void Awake()
	{
		//Check if instance already exists
		if (instance == null)

			//if not, set instance to this
			instance = this;

		//If instance already exists and it's not this:
		else if (instance != this)

			//Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a GameManager.
			Destroy(gameObject);    

		//Sets this to not be destroyed when reloading scene
		DontDestroyOnLoad(gameObject);

	}

	/// <summary>
    /// 
    /// </summary>
    void Start () {
        /*
		Scene scene = SceneManager.GetActiveScene ();
		string levelNum = scene.name.Substring (5);
		print ("Level number " + levelNum);

		string[] LSSplit = levelNum.Split (',');
		print ("Trial: " + LSSplit[0]);
		print ("Level: " + LSSplit[1]);
		//int levelInt = int.Parse (levelNum);
	*/

	}
	
	// Update is called once per frame
	void Update () {
		
	}


	public void LoadLevel(int trial, int level) {

		string levelToLoad = "level" + trial.ToString() + "," + level.ToString ();


       
        SceneManager.LoadScene (levelToLoad);


	}


    public Dictionary<string, string> GetLevelDetails()
    {
        Dictionary<string, string> dict = new Dictionary<string, string>();
        Scene scene = SceneManager.GetActiveScene();
        string levelNum = scene.name.Substring(5);


        string[] LSSplit = levelNum.Split(',');

        dict.Add("Trial", LSSplit[0]);
        dict.Add("Level", LSSplit[1]);
        return dict;

    }

}
