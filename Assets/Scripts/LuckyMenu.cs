using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LuckyMenu : MonoBehaviour {
    // Start is called before the first frame update
    void Start() {
        theStateManager = GameObject.FindObjectOfType<StateManager>();
    }

    StateManager theStateManager;
    public GameObject luckyMenu;

    // Update is called once per frame
    void Update() {
        
    	if(theStateManager.IsLucky == false) {
    		luckyMenu.SetActive(false);
    	}

    }

    public void RunLuckyMenu() {
    	theStateManager.HasToChoose = true;
		luckyMenu.SetActive(true);
    }

    public void Axe() {
    	// Debug.Log("CHose axe");
    	if(theStateManager.CurrentPlayerId == 0) {
			theStateManager.PlayerOneAxe = theStateManager.PlayerOneAxe + 1;
		} else {
			theStateManager.PlayerTwoAxe = theStateManager.PlayerTwoAxe + 1;
		}
		theStateManager.IsLucky = false;
		theStateManager.HasToChoose = false;
    }

    public void SnakeCharmer() {
    	// Debug.Log("Chose snkchrmr");
    	if(theStateManager.CurrentPlayerId == 0) {
			theStateManager.PlayerOneSnakeCharmer = theStateManager.PlayerOneSnakeCharmer + 1;
		} else {
			theStateManager.PlayerTwoSnakeCharmer = theStateManager.PlayerTwoSnakeCharmer + 1;
		}
		theStateManager.IsLucky = false;
		theStateManager.HasToChoose = false;
    }

}
