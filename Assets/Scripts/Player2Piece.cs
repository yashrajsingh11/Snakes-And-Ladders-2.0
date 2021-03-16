using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player2Piece : MonoBehaviour {
    // Start is called before the first frame update
    void Start() {
        theStateManager = GameObject.FindObjectOfType<StateManager>();
    }

    public Tile StartingTile;
    Tile currentTile;
    StateManager theStateManager;

    // Update is called once per frame
    void Update() {
        if(theStateManager.IsDoneRolling == false) {
            return;
        }
    
    	if(theStateManager.CurrentPlayerId == 1) {    
    
	        int spacesToMove = theStateManager.diceValue;
    	    Tile finalTile = currentTile;
        	
        	for(int i = 0; i < spacesToMove; i++) {
            
	            if(finalTile == null) {
            
    	            finalTile = StartingTile;
            
        	    } else {
                
            	    if(finalTile.NextTiles == null || finalTile.NextTiles.Length == 0) {
                
                	    Debug.Log("Player 2 Won!!");
                    	Destroy(gameObject);
                	    return;
                
                	} else {
                
                		finalTile = finalTile.NextTiles[0];
            
                	}
        
            	}

        	}
        
        	if(finalTile == null) {
            	return;
        	}
        
        	this.transform.position = finalTile.transform.position; 

        	if(finalTile.NextTiles.Length > 1) {
                finalTile = finalTile.NextTiles[1];
            }
        	
            this.transform.position = finalTile.transform.position; 

        	currentTile = finalTile;  
        	theStateManager.NewTurn();

    	}

	}

}
