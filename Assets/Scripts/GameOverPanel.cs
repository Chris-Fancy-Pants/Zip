using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class GameOverPanel : MonoBehaviour {


    public Text bolts;
    public Text time;

    public Image alter1;
    public Image alter2;
    public Image alter3;


    bool gameOverActive = false;


    public GameObject gameOverWindow;

    public void UpdateGameOver(int bolts, float time, bool alter1, bool alter2, bool alter3)
    {
        this.bolts.text = "Bolts: " + bolts.ToString();
        this.time.text = "Time: " + time.ToString("N2");

        if(alter1)
        {
            this.alter1.color = Color.white;
        }
        if (alter2)
        {
            this.alter2.color = Color.white;
        }
        if (alter3)
        {
            this.alter3.color = Color.white;
        }

        print(time.ToString());


        gameOverWindow.SetActive(true);
        gameOverActive = true;

    }

	// Use this for initialization
	void Start () {
		
	}



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
