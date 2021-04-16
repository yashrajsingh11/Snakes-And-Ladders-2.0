using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player2Details : MonoBehaviour {
    // Start is called before the first frame update
    void Start() {
        theStateManager = GameObject.FindObjectOfType<StateManager>();
        myText = GetComponent<Text>();
    }

    StateManager theStateManager;
    Text myText;

    // Update is called once per frame
    void Update() {

    }

    public void PlayerTwoDetails() {

        myText.text = "Axe: " + theStateManager.PlayerTwoAxe  + "\nSnake Charmer: " + theStateManager.PlayerTwoSnakeCharmer;
        
    }
}
