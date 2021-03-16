using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DiceRoller : MonoBehaviour {
    // Start is called before the first frame update
    void Start() {
    	theStateManager = GameObject.FindObjectOfType<StateManager>();
    }

    // Update is called once per frame
    void Update() {
        
    }

    StateManager theStateManager;
    public Sprite DiceFace1;
    public Sprite DiceFace2;
    public Sprite DiceFace3;
    public Sprite DiceFace4;
    public Sprite DiceFace5;
    public Sprite DiceFace6;

    public void RollTheDice() {

    	if(theStateManager.IsDoneRolling == true)  {
    		return;
    	}

    	theStateManager.diceValue = Random.Range(1, 7);

    	if(theStateManager.diceValue == 1) {		
    		this.GetComponent<Image>().sprite = DiceFace1;
    	} else if(theStateManager.diceValue == 2) {
    		this.GetComponent<Image>().sprite = DiceFace2;
    	} else if(theStateManager.diceValue == 3) {
    		this.GetComponent<Image>().sprite = DiceFace3;
    	} else if(theStateManager.diceValue == 4) {
    		this.GetComponent<Image>().sprite = DiceFace4;
    	} else if(theStateManager.diceValue == 5) {
    		this.GetComponent<Image>().sprite = DiceFace5;
    	} else {
    		this.GetComponent<Image>().sprite = DiceFace6;
    	}     

    	theStateManager.IsDoneRolling = true;

    }

}
