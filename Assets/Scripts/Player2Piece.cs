using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player2Piece : MonoBehaviour {
    // Start is called before the first frame update
    void Start() {
        theStateManager = GameObject.FindObjectOfType<StateManager>();
        theWinnerText = GameObject.FindObjectOfType<WinnerText>();
        thePromptText = GameObject.FindObjectOfType<PromptText>();
    }

    public Tile StartingTile;
    Tile currentTile;
    StateManager theStateManager;
    WinnerText theWinnerText;
    PromptText thePromptText;
    // Update is called once per frame
    void Update() {
        if(theStateManager.IsDoneRolling == false || theStateManager.HasToChoose == true) {
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
                        theStateManager.isGameOver = true;
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
                if(finalTile.NextTiles[1] != null) {
                    if(theStateManager.PlayerOneAxe != 0) {
                        theStateManager.PlayerOneAxe = theStateManager.PlayerOneAxe - 1;
                        theStateManager.hasUsedSpecialItem = true;
                        thePromptText.promptText("Player 1 used Axe");
                        Debug.Log("Player 1 used Axe");
                    } else {
                        finalTile = finalTile.NextTiles[1];
                        this.transform.position = finalTile.transform.position;
                        theStateManager.hasUsedSpecialItem = true;
                        thePromptText.promptText("Player 1 didnt had axe so you moved up");
                        Debug.Log("Player 1 didn't had axe so you moved up");
                    }
                } else if(finalTile.NextTiles[2] != null) {
                    if(theStateManager.PlayerTwoSnakeCharmer != 0) {
                         theStateManager.PlayerTwoSnakeCharmer = theStateManager.PlayerTwoSnakeCharmer - 1;
                        theStateManager.hasUsedSpecialItem = true;
                        thePromptText.promptText("You(player2) got saved by snakecharmer");
                        Debug.Log("Player 2 got saved by snakecharmer");
                    } else {
                        finalTile = finalTile.NextTiles[2];
                        this.transform.position = finalTile.transform.position;
                        theStateManager.hasUsedSpecialItem = true;
                        thePromptText.promptText("No snakecharmer sorry you(player2) have to go down");
                        Debug.Log("No snakecharmer sorry you(player2) have to go down");
                    }
                }
            }

        	currentTile = finalTile;  
        	theStateManager.NewTurn();

    	}

	}

}
