using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class LuckyTextDisplay : MonoBehaviour {
    // Start is called before the first frame update
    void Start() {
        theStateManager = GameObject.FindObjectOfType<StateManager>();
        myText = GetComponent<Text>();
    }

    StateManager theStateManager;
    Text myText;
    string[] numberWords = {"One", "Two"};

    // Update is called once per frame
    void Update() {

    }

    public void LuckyDisplay() {

    	myText.text = "Player " + numberWords[theStateManager.CurrentPlayerId] + " got lucky \n Choose one of the following:";
    	
    }
}
