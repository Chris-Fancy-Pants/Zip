using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class GameOverPanel : MonoBehaviour {


    public Text bolts;
    public Text time;

    public Image alter1Image;
    public GameObject alter1Object;
    Alter alter1;


    public Image alter2Image;
    public GameObject alter2Object;
    Alter alter2;


    public Image alter3Image;
    public GameObject alter3Object;
    Alter alter3;

    bool gameOverActive = false;


    public GameObject gameOverWindow;

    void Start()
    {
        GameManager.instance.boltsThisTrial = 0;
        alter1 = alter1Object.GetComponent<Alter>();
        alter2 = alter2Object.GetComponent<Alter>();
        alter3 = alter3Object.GetComponent<Alter>();
    }



    public void UpdateGameOver()
    {

        print("UpdateGameOver");

        this.bolts.text = "Bolts: " + GameManager.instance.boltsThisTrial.ToString();

        if(alter1.activated)
        {
            this.alter1Image.color = Color.white;
        }
        if (alter2.activated)
        {
            this.alter2Image.color = Color.white;
        }
        if (alter3.activated)
        {
            this.alter3Image.color = Color.white;
        }



        gameOverWindow.SetActive(true);
        gameOverActive = true;

    }

	// Use this for initialization



    void NextLevel()
    {

        Scene scene = SceneManager.GetActiveScene();
        string levelNum = scene.name.Substring(5);
        string[] LSSplit = levelNum.Split(',');

        string trial = LSSplit[0];
        string level = LSSplit[1];

        /// create level name
        /// 
        int levelNumber = int.Parse(level) + 1;
        int trialNumber = int.Parse(trial);


        if(levelNumber <= GameManager.instance.trialCounts[trialNumber -1])
        {
            GameManager.instance.LoadLevel(trialNumber, levelNumber);
        } else
        {
            SceneManager.LoadScene("Hub");
        }

        



     

    }

	
	// Update is called once per frame
	void Update () {
		if(gameOverActive && Input.GetButtonDown("Jump"))
        {
            NextLevel();
            
        }
	}
}
