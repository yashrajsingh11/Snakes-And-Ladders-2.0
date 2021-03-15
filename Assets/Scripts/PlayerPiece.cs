using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPiece : MonoBehaviour {
    // Start is called before the first frame update
    void Start() {
        theDiceRoller = GameObject.FindObjectOfType<DiceRoller>();
    }

    // Update is called once per frame
    void Update() {
        
    }

    public Tile StartingTile;
    Tile currentTile;

    DiceRoller theDiceRoller;

    void OnMouseUp() {

    	if(theDiceRoller.IsDoneRolling == false) {
    		return;
    	}
    	
    	int spacesToMove = theDiceRoller.diceValue;
        Tile finalTile = currentTile;
        
        for(int i = 0; i < spacesToMove; i++) {
            
            if(finalTile == null) {
            
                finalTile = StartingTile;
            
            } else {
            
                finalTile = finalTile.NextTiles[0];
            
            }
        
        }
        
        if(finalTile == null) {
            return;
        }
        
        this.transform.position = finalTile.transform.position; 
        currentTile = finalTile;  
	    theDiceRoller.NewTurn();
    
    }

}
