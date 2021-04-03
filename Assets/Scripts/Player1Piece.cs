using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player1Piece : MonoBehaviour {
    // Start is called before the first frame update
    void Start() {
        theStateManager = GameObject.FindObjectOfType<StateManager>();
        theWinnerText = GameObject.FindObjectOfType<WinnerText>();
        thePromptText = GameObject.FindObjectOfType<PromptText>();
    }

    public Tile StartingTile;
    public AudioSource myFx;
    public AudioClip moveFx;
    Tile currentTile;
    StateManager theStateManager;
    WinnerText theWinnerText;
    PromptText thePromptText;

    // Update is called once per frame
    void Update() {
        if(theStateManager.IsDoneRolling == false || theStateManager.HasToChoose == true) {
            return;
        }
        
        if(theStateManager.CurrentPlayerId == 0) {     
        
            int spacesToMove = theStateManager.diceValue;
            Tile finalTile = currentTile;
        
            for(int i = 0; i < spacesToMove; i++) {
                
                if(finalTile == null) {
            
                    finalTile = StartingTile;
            
                } else {
                
                    if(finalTile.NextTiles == null || finalTile.NextTiles.Length == 0) {
                        theStateManager.isGameOver = true;
                        Debug.Log("Player 1 Won!!");
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
            myFx.PlayOneShot(moveFx);

            if(finalTile.NextTiles.Length > 1) {
                if(finalTile.NextTiles[1] != null) {
                //now player one has reached a ladder 
                // if player two has an axe then he use it and player 1 uses its axe to counter it
                //as long as any players number of axes does not reach zero this process continues
                //if player 1 reaches 0 first then player 2 uses his next axe and he cannot go up 
                //if player 2 reaches 0 first then player 1 uses the ladder and goes up    
                    if(theStateManager.PlayerTwoAxe != 0) {
                        while(theStateManager.PlayerTwoAxe != 0 || theStateManager.PlayerOneAxe != 0) {
                            theStateManager.PlayerTwoAxe = theStateManager.PlayerTwoAxe - 1;
                            theStateManager.hasUsedSpecialItem = true;
                            thePromptText.promptText("Player 2 used Axe");
                            Debug.Log("Player 2 used Axe");
                            theStateManager.PlayerOneAxe = theStateManager.PlayerOneAxe - 1;
                            theStateManager.hasUsedSpecialItem = true;
                            thePromptText.promptText("Player 1 used Axe");
                            Debug.Log("Player 1 used Axe");
                            }
                        if(theStateManager.PlayerTwoAxe == 0 && theStateManager.PlayerOneAxe != 0) {
                            finalTile = finalTile.NextTiles[1];
                            this.transform.position = finalTile.transform.position;
                            myFx.PlayOneShot(moveFx);
                            theStateManager.hasUsedSpecialItem = true;
                            thePromptText.promptText("Player 2 didnt had axe so you moved up");
                            Debug.Log("Player 2 didnt had axe so you moved up");
                        } else if(theStateManager.PlayerTwoAxe != 0 && theStateManager.PlayerOneAxe == 0) {
                            theStateManager.PlayerTwoAxe = theStateManager.PlayerTwoAxe - 1;
                            theStateManager.hasUsedSpecialItem = true;
                            thePromptText.promptText("Player 2 used the final Axe");
                            Debug.Log("Player 2 used the final Axe");
                        } else if(theStateManager.PlayerTwoAxe == 0 && theStateManager.PlayerOneAxe == 0)  {
                            finalTile = finalTile.NextTiles[1];
                            this.transform.position = finalTile.transform.position;
                            myFx.PlayOneShot(moveFx);
                            theStateManager.hasUsedSpecialItem = true;
                            thePromptText.promptText("Player 2 didnt had axe so you moved up");
                            Debug.Log("Player 2 didnt had axe so you moved up");
                        }
                    }
                } else if(finalTile.NextTiles[2] != null) {
                    if(theStateManager.PlayerOneSnakeCharmer != 0) {
                         theStateManager.PlayerOneSnakeCharmer = theStateManager.PlayerOneSnakeCharmer - 1;
                        theStateManager.hasUsedSpecialItem = true;
                        thePromptText.promptText("You(player1) got saved by snakecharmer");
                         Debug.Log("You(player1) got saved by snakecharmer");
                    } else {
                        finalTile = finalTile.NextTiles[2];
                        this.transform.position = finalTile.transform.position;
                        myFx.PlayOneShot(moveFx);
                        theStateManager.hasUsedSpecialItem = true;
                        thePromptText.promptText("No snakecharmer sorry you(player1) have to go down");
                        Debug.Log("No snakecharmer sorry you(player1) have to go down");
                    }
                }
            }

            currentTile = finalTile;
            theStateManager.NewTurn();
    
        }

    }

}
