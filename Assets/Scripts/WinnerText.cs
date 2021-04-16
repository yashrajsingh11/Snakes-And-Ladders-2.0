using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WinnerText : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        theStateManager = GameObject.FindObjectOfType<StateManager>();
        myText = GetComponent<Text>();
    }

    StateManager theStateManager;
    Text myText;
    string[] numberWords = { "One", "Two" };

    // Update is called once per frame
    void Update()
    {
        if(theStateManager.isGameOver == true)
        {
            WinnerDisplay();
        }
    }

    public void WinnerDisplay()
    {

        myText.text = "Conrats!! Player " + numberWords[theStateManager.CurrentPlayerId] + "\n You Won";

    }
}
