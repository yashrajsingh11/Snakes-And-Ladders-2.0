using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PromptMenu : MonoBehaviour {
    // Start is called before the first frame update
    void Start() {
        theStateManager = GameObject.FindObjectOfType<StateManager>();        
    }

    public GameObject promptMenu;
    StateManager theStateManager;
    // Update is called once per frame
    void Update() {
        if(theStateManager.hasUsedSpecialItem == false) {           
            promptMenu.SetActive(false);   
            Time.timeScale = 1f;
        }
        else {
            promptMenu.SetActive(true);
        }
    }

    public void ok() {
        theStateManager.hasUsedSpecialItem = false;        
        Time.timeScale = 1f;
    }
}
