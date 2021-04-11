using Firebase.Auth;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WinningMenu : MonoBehaviour
{

    void Start()
    {
        theStateManager = GameObject.FindObjectOfType<StateManager>();
        theWinnerText = GameObject.FindObjectOfType<WinnerText>();
    }
    public static bool hasWon = false;
    public GameObject winMenu;
    StateManager theStateManager;
    WinnerText theWinnerText;
    


    // Update is called once per frame
    void Update()
    {
        if(theStateManager.isGameOver == true)
        {
            winMenu.SetActive(true);
            Time.timeScale = 0f;
            hasWon = true;
        }
        else
        {
            hasWon = false;
        }
    }

    public void Replay()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("SampleScene");
    }

    public void Exit()
    {
        FirebaseAuth a = FirebaseAuth.DefaultInstance;
        FirebaseFirestoreDatabaseX database = new FirebaseFirestoreDatabaseX(a.CurrentUser.UserId);
        database.removeUserFromDatabase();
        SceneManager.LoadScene("Menu");
    }
}
