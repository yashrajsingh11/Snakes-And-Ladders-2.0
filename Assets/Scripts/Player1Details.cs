using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player1Details : MonoBehaviour {
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

    public void PlayerOneDetails() {

        myText.text = "Axe: " + theStateManager.PlayerOneAxe  + "\nSnake Charmer: " + theStateManager.PlayerOneSnakeCharmer;
        
    }
}
