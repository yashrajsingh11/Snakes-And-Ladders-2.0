using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DiceRoller : MonoBehaviour {
    // Start is called before the first frame update
    void Start() {
        diceValue = 6;
        diceTotal = 0;
    }

    // Update is called once per frame
    void Update() {
        
    }

    public int diceValue;
    public int diceTotal;
    public Sprite DiceFace1;
    public Sprite DiceFace2;
    public Sprite DiceFace3;
    public Sprite DiceFace4;
    public Sprite DiceFace5;
    public Sprite DiceFace6;

    public void RollTheDice() {

    	diceValue = Random.Range(1, 7);
    	diceTotal = diceTotal + diceValue;

    	if(diceValue == 1) {		
    		this.GetComponent<Image>().sprite = DiceFace1;
    	} else if(diceValue == 2) {
    		this.GetComponent<Image>().sprite = DiceFace2;
    	} else if(diceValue == 3) {
    		this.GetComponent<Image>().sprite = DiceFace3;
    	} else if(diceValue == 4) {
    		this.GetComponent<Image>().sprite = DiceFace4;
    	} else if(diceValue == 5) {
    		this.GetComponent<Image>().sprite = DiceFace5;
    	} else if(diceValue == 6) {
    		this.GetComponent<Image>().sprite = DiceFace6;
    	}     

    	Debug.Log(diceValue);
    }


}
