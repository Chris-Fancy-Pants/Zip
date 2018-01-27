using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class DeathPanel : MonoBehaviour {

	public Button retryButton;

	public void ShowDeathPanel() {

		retryButton.Select ();
	}

	public void GoToScene(string scene) {

		SceneManager.LoadScene (scene);

	}

	public void RestartLevel() {

		SceneManager.LoadScene (SceneManager.GetActiveScene ().name);

	}

}
