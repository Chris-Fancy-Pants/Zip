using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPanel : MonoBehaviour {

    public GameObject heart1;
    public GameObject heart2;
    public GameObject heart3;
    public GameObject heart4;


    public int currentHealth = 0;

	public void UpdateHealth(int health)
    {

        if (health > currentHealth)
        {

            if (!heart1.activeInHierarchy && health > 0)
            {
                heart1.SetActive(true);
            }
            if (!heart2.activeInHierarchy && health > 1)
            {
                heart2.SetActive(true);
            }

            if (!heart3.activeInHierarchy && health > 2)
            {
                heart3.SetActive(true);
            }
            if (!heart4.activeInHierarchy && health > 3)
            {
                heart4.SetActive(true);
            }
        } else if(health < currentHealth)
        {
            if (heart1.activeInHierarchy && health < 1)
            {
                heart1.SetActive(false);
            }
            if (heart2.activeInHierarchy && health < 2)
            {
                heart2.SetActive(false);
            }

            if (heart3.activeInHierarchy && health < 3)
            {
                heart3.SetActive(false);
            }
            if (heart4.activeInHierarchy && health < 4)
            {
                heart4.SetActive(false);
            }
        }






        currentHealth = health;
    }
}
