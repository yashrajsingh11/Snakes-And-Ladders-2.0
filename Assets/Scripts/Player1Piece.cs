using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Firestore;
using UnityEngine.UI;

public class Player1Piece : MonoBehaviour {
    // Start is called before the first frame update
    void Start() {
        theStateManager = GameObject.FindObjectOfType<StateManager>();
        theWinnerText = GameObject.FindObjectOfType<WinnerText>();
        thePromptText = GameObject.FindObjectOfType<PromptText>();
        theStoreData = GameObject.FindObjectOfType<storeData>();
        checkForUserMoves();
    }

    public Tile StartingTile;
    public AudioSource myFx;
    public AudioClip moveFx;
    Tile currentTile;
    StateManager theStateManager;
    storeData theStoreData;
    WinnerText theWinnerText;
    PromptText thePromptText;
    //get green signal for that both players are in
    bool allOK = true;
    bool isMoveUpdated = false;
    bool userMoveChecked = false;
    bool movementCompleted = false;
    //List that checks the details...
    List<UserMovesListData> userMoves = new List<UserMovesListData>();

    // Update is called once per frame
    void Update() {

        if(theStateManager.IsDoneRolling == false || theStateManager.HasToChoose == true) {
            return;
        }
        if(isMoveUpdated == true) {
            checkForUserMoves();
            isMoveUpdated = false;
        }
        if(userMoveChecked == true) {
            theStateManager.NewTurn();
            movementCompleted = false;
        }
        
        if(theStateManager.CurrentPlayerId == 0 && userMoves[0].player1Turn == true && movementCompleted == false) {     
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
                    if(theStateManager.PlayerTwoAxe != 0) {
                        while(theStateManager.PlayerTwoAxe != 0 && theStateManager.PlayerOneAxe != 0) {
                            theStateManager.PlayerTwoAxe = theStateManager.PlayerTwoAxe - 1;
                            theStateManager.hasUsedSpecialItem = true;
                            thePromptText.promptText("Player 2 used Axe");
                            Debug.Log("Player 2 used Axe");
                            theStateManager.PlayerOneAxe = theStateManager.PlayerOneAxe - 1;
                            theStateManager.hasUsedSpecialItem = true;
                            thePromptText.promptText("Player 1 used its Axe as a counter");
                            Debug.Log("Player 1 used its Axe as a counter");
                        }
                        if(theStateManager.PlayerTwoAxe == 0 && theStateManager.PlayerOneAxe != 0) {
                            finalTile = finalTile.NextTiles[1];
                            this.transform.position = finalTile.transform.position;
                            myFx.PlayOneShot(moveFx);
                            theStateManager.hasUsedSpecialItem = true;
                            thePromptText.promptText("Player 2 didnt had axe so Player 1 moved up");
                            Debug.Log("Player 2 didnt had axe so Player 1 moved up");
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
                            thePromptText.promptText("Player 2 didnt had axe so Player 1 moved up");
                            Debug.Log("Player 2 didnt had axe so Player 1 moved up");
                        }
                    } else {
                    	finalTile = finalTile.NextTiles[1];
                        this.transform.position = finalTile.transform.position;
                        myFx.PlayOneShot(moveFx);
                        theStateManager.hasUsedSpecialItem = true;
                        thePromptText.promptText("Player 2 didnt had axe so Player 1 moved up");
                        Debug.Log("Player 2 didnt had axe so Player 1 moved up");
                    }
                } else if(finalTile.NextTiles[2] != null) { 
                    if(theStateManager.PlayerOneSnakeCharmer != 0) {
                        while(theStateManager.PlayerTwoSnakeCharmer != 0 && theStateManager.PlayerOneSnakeCharmer != 0) {
                            theStateManager.PlayerOneSnakeCharmer = theStateManager.PlayerOneSnakeCharmer - 1;
                            theStateManager.hasUsedSpecialItem = true;
                            thePromptText.promptText("Player 1 got saved by snakecharmer");
                            Debug.Log("Player 1 got saved by snakecharmer");
                            theStateManager.PlayerTwoSnakeCharmer = theStateManager.PlayerTwoSnakeCharmer - 1;
                            theStateManager.hasUsedSpecialItem = true;
                            thePromptText.promptText("Player 2 used its snakecharmer as a counter");
                            Debug.Log("Player 2 used its snakecharmer as a counter");
                        }
                        if(theStateManager.PlayerTwoSnakeCharmer == 0 && theStateManager.PlayerOneSnakeCharmer != 0) {
                            theStateManager.PlayerOneSnakeCharmer = theStateManager.PlayerOneSnakeCharmer - 1;
                            theStateManager.hasUsedSpecialItem = true;
                            thePromptText.promptText("Player 1 finally got saved by snakecharmer");
                            Debug.Log("Player 1 finally got saved by snakecharmer");
                        } else if(theStateManager.PlayerTwoSnakeCharmer != 0 && theStateManager.PlayerOneSnakeCharmer == 0) {
                            finalTile = finalTile.NextTiles[2];
                            this.transform.position = finalTile.transform.position;
                            myFx.PlayOneShot(moveFx);
                            theStateManager.hasUsedSpecialItem = true;
                            thePromptText.promptText("No snakecharmer Player 1 goes down");
                            Debug.Log("No snakecharmer Player 1 goes down");
                        } else if(theStateManager.PlayerTwoSnakeCharmer == 0 && theStateManager.PlayerOneSnakeCharmer == 0)  {
                            finalTile = finalTile.NextTiles[2];
                            this.transform.position = finalTile.transform.position;
                            myFx.PlayOneShot(moveFx);
                            theStateManager.hasUsedSpecialItem = true;
                            thePromptText.promptText("No snakecharmer Player 1 goes down");
                            Debug.Log("No snakecharmer Player 1 goes down");
                        }
                    } else {
                    	finalTile = finalTile.NextTiles[2];
                        this.transform.position = finalTile.transform.position;
                        myFx.PlayOneShot(moveFx);
                        theStateManager.hasUsedSpecialItem = true;
                        thePromptText.promptText("No snakecharmer Player 1 goes down");
                        Debug.Log("No snakecharmer Player 1 goes down");
                    }
                }
            }
            movementCompleted = true;
            currentTile = finalTile;
            playerUpadtes(false, true, theStateManager.PlayerOneAxe, theStateManager.PlayerTwoAxe, theStateManager.PlayerOneSnakeCharmer,
            theStateManager.PlayerTwoSnakeCharmer, 0, 0, spacesToMove, 0, theStoreData.player1uid, theStoreData.player2uid);
        }
    }

    public void playerUpadtes(bool player1Turn, bool player2Turn, int player1AXE, int player2AXE, int player1SnakeC, int player2SnakeC,
    int player1Result, int player2Result, int player1DiceValue, int player2DiceValue, string uidofUserCreatedtheRoom, string secondUserUID)
    {
        FirebaseFirestore _refrence = FirebaseFirestore.DefaultInstance;
        Dictionary<string, object> updates = new Dictionary<string, object>
                        {
                            {"player1DiceValue",  player1DiceValue},
                            {"player2DiceValue",  player1DiceValue},
                            {"player1Result",player1Result},
                            {"player2Result",player1Result},
                            {"player1AXE",player1AXE},
                            {"player2AXE",player2AXE},
                            {"player1SnakeC",player1SnakeC},
                            {"player2SnakeC",player2SnakeC},
                            {"player1Turn",player1Turn},
                            {"player2Turn",player2Turn},
                        };
        _refrence.Collection("Room").Document(uidofUserCreatedtheRoom).Collection("HelloD").Document(uidofUserCreatedtheRoom + secondUserUID)
        .UpdateAsync(updates).ContinueWith(task => { 
            Debug.Log("Update Moves"); 
            isMoveUpdated = true;    
        });
    }

    public void checkForUserMoves() {
        FirebaseFirestore _refrence = FirebaseFirestore.DefaultInstance;
        _refrence.Collection("Room").Document(theStoreData.player1uid).Collection("HelloD").GetSnapshotAsync().ContinueWith(task =>
        {
            QuerySnapshot snapshots = task.Result;
            foreach (DocumentSnapshot document in snapshots.Documents)
            {
                Dictionary<string, object> aLLLData = document.ToDictionary();
                UserMovesListData daata = new UserMovesListData
                {
                    player1Result = Convert.ToInt32(aLLLData["player1Result"].ToString()),
                    player2Result = Convert.ToInt32(aLLLData["player2Result"].ToString()),
                    player1DiceValue = Convert.ToInt32(aLLLData["player1DiceValue"].ToString()),
                    player2DiceValue = Convert.ToInt32(aLLLData["player2DiceValue"].ToString()),
                    player1Turn = bool.Parse(aLLLData["player1Turn"].ToString()),
                    player2Turn = bool.Parse(aLLLData["player2Turn"].ToString()),
                    player1AXE = Convert.ToInt32(aLLLData["player1AXE"].ToString()),
                    player2AXE = Convert.ToInt32(aLLLData["player2AXE"].ToString()),
                    player1SnakeC = Convert.ToInt32(aLLLData["player1SnakeC"].ToString()),
                    player2SnakeC = Convert.ToInt32(aLLLData["player2SnakeC"].ToString()),
                    player1uid = Convert.ToInt32(aLLLData["player1uid"].ToString()),
                    player2uid = Convert.ToInt32(aLLLData["player2uid"].ToString()),
                };
                if(userMoves.Count > 1) {
                    userMoves.Clear();
                }
                userMoves.Add(daata);
            }
            if(userMoves[0].player1Result != -1) {
                userMoveChecked = true;
            }
        });
    }
}
