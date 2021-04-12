using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class MainMenu : MonoBehaviour {
   
	// public void PlayGame() {
 //        // AuthResults auth = new AuthResults();
 //        // auth.loginAnno();
 //        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
 //        Debug.Log("Hey !! It's me");
 //    }
	
    public void QuitGame()
    {
        Debug.Log("Quit!");
        Application.Quit();
    }
    
}



