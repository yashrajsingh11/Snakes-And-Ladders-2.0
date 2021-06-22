using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerMovement : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        PV = GetComponent<PhotonView>();
        myPassingData = GameObject.FindObjectOfType<PassingData>();
        myDiceRoller = GameObject.FindObjectOfType<DiceRoller>();
         myDisplayController = GameObject.FindObjectOfType<DisplayController>();
    }

    private PhotonView PV;
    DisplayController myDisplayController;
    PassingData myPassingData;
    DiceRoller myDiceRoller;
    Tile currentTile;
    private bool choosingDisplaySynced  =false;
    private bool canChooseSynced = false;
    private bool currentPlayerIdSynced = false;
    private bool hasChosenSynced = false;
    private bool resultSynced = false;

    // Update is called once per frame
    void Update()
    {
        if(myDiceRoller.CanMove == false)
        {
            return;
        }

        if(PV.IsMine && myPassingData.myId - 1 == myDiceRoller.currentPlayerId && myDiceRoller.CanMove == true)
        {
            MovePlayer();
        }
    }

    bool CanChooseSynced()
    {
        if(canChooseSynced == true)
        {
            return true;
        }
        else
        {
            return false;
        }    
    }

    [PunRPC]
    void RPC_SyncCanChooseForLadder(int localId)
    {   
        Debug.Log("canChoose Synced");
        myDisplayController.myString = "Player " + ((localId % 4) + 1) + " is choosing";
        myDiceRoller.canChoose = true;
        myDiceRoller.isLadder = true;
        if((myPassingData.myId - 1) == myDiceRoller.currentPlayerId)
        {
            canChooseSynced = true;
        }
    }

    bool HasChosen()
    {
        if(myDiceRoller.hasChosen == true)
        {
            return true;
        }
        else
        {
            return false;
        }    
    }

    IEnumerator SecondLadderCoroutine()
    {
        PV.RPC("RPC_SyncCanChooseForLadder", RpcTarget.AllViaServer, myPassingData.myId);
        yield return new WaitUntil(CanChooseSynced);
        canChooseSynced = false;
        Debug.Log("Second Player Can Choose Now");
        yield return new WaitUntil(HasChosen);
        Debug.Log("Can Choose - " + myDiceRoller.canChoose);
        Debug.Log("Has Chosen - " + myDiceRoller.hasChosen);
        Debug.Log("Current player gets to choose - " + myDiceRoller.currentPlayerGetsToChoose);
        Debug.Log("Next player gets to choose - " + myDiceRoller.nextPlayerGetsToChoose);
        Debug.Log("Next player wants to choose - " + myDiceRoller.nextPlayerWantsToChoose);
    }

    [PunRPC]
    void RPC_SyncCurrentPlayerId(int CurrentPlayerId)
    {
        Debug.Log("currentPlayerId Synced");
        if((myPassingData.myId - 1) == CurrentPlayerId)
        {
            currentPlayerIdSynced = true;
        }
        myDiceRoller.currentPlayerWantsToChoose = true;
        myDiceRoller.nextPlayerWantsToChoose = true;
        myDiceRoller.currentPlayerGetsToChoose = true;
        myDiceRoller.nextPlayerGetsToChoose = true;
        myDiceRoller.currentPlayerId = ((CurrentPlayerId + 1) % 4);
        if((myPassingData.myId - 1) == myDiceRoller.currentPlayerId)
        {
            myDisplayController.myString = "Your turn";
        }
        else
        {
            myDisplayController.myString = "Player " + (myDiceRoller.currentPlayerId + 1) + "'s turn";
        }
    }

    bool CurrentPlayerIdSynced()
    {
        if(currentPlayerIdSynced == true)
        {
            return true;
        }
        else
        {
            return false;
        }    
    }

    IEnumerator CompleteTheTurn(Tile finalTile)
    {
        currentTile = finalTile;
        PV.RPC("RPC_SyncCurrentPlayerId", RpcTarget.AllViaServer, myDiceRoller.currentPlayerId);
        yield return new WaitUntil(CurrentPlayerIdSynced);
        currentPlayerIdSynced = false;
        Debug.Log("Resetting variables");
        myDiceRoller.IsDoneRolling = false;
        Debug.Log("Turn Complete");
    }

    [PunRPC]
    void RPC_SyncHasChosen()
    {
        Debug.Log("hasChosen Synced");
        myDiceRoller.hasChosen = false;
        if((myPassingData.myId - 1) == myDiceRoller.currentPlayerId)
        {
            hasChosenSynced = true;
        }
    }

    bool HasChosenSynced()
    {
        if(hasChosenSynced == true)
        {
            return true;
        }
        else
        {
            return false;
        }    
    }
    
    IEnumerator ThirdLadderCoroutine()
    {
        PV.RPC("RPC_SyncingChoosingDisplay", RpcTarget.AllViaServer, myPassingData.myId);
        yield return new WaitUntil(ChoosingDisplaySynced);
        choosingDisplaySynced = false;
        myDiceRoller.counterCanChoose = true;
        myDiceRoller.isLadder = true;
        yield return new WaitUntil(CounterPlayerChose);
        myDiceRoller.counterPlayerChose = false;
    }

    IEnumerator LadderCoroutine(Tile finalTile)
    {
        while(myDiceRoller.Axe[(myDiceRoller.currentPlayerId + 1) % 4] != 0 
            && myDiceRoller.Axe[myDiceRoller.currentPlayerId] != 0 && myDiceRoller.currentPlayerWantsToChoose == true &&
            myDiceRoller.nextPlayerWantsToChoose == true) // player1 and player2 fight for the axe
            {
                if(myDiceRoller.nextPlayerGetsToChoose == true)
                {
                    yield return StartCoroutine(SecondLadderCoroutine());
                    Debug.Log("Second Player Has Chosen");
                    PV.RPC("RPC_SyncHasChosen", RpcTarget.AllViaServer);
                    yield return new WaitUntil(HasChosenSynced);
                    hasChosenSynced = false;
                    Debug.Log("Resetting hasChosen - " + myDiceRoller.hasChosen);
                }
                if(myDiceRoller.currentPlayerGetsToChoose == true)
                {
                    Debug.Log("Countering now");
                    yield return StartCoroutine(ThirdLadderCoroutine());
                    Debug.Log("Can Choose - " + myDiceRoller.canChoose);
                    Debug.Log("Has Chosen - " + myDiceRoller.hasChosen);
                    Debug.Log("Current player gets to choose - " + myDiceRoller.currentPlayerGetsToChoose);
                    Debug.Log("Next player gets to choose - " + myDiceRoller.nextPlayerGetsToChoose);
                    Debug.Log("Next player wants to choose - " + myDiceRoller.nextPlayerWantsToChoose);
                    Debug.Log("Current Player Has Chosen");
                }
            }
        if(myDiceRoller.currentPlayerWantsToChoose == false && myDiceRoller.nextPlayerWantsToChoose == true) //current player skipped
        {
            Debug.Log("Current Player Skipped");
        } 
        else if(myDiceRoller.nextPlayerWantsToChoose == false && myDiceRoller.currentPlayerWantsToChoose == true) //opponent skipped
        {
            Debug.Log("Opponent Skipped");
            finalTile = finalTile.NextTiles[1];
            this.transform.position = finalTile.transform.position;
        }
        else if(myDiceRoller.nextPlayerWantsToChoose == true && myDiceRoller.currentPlayerWantsToChoose == true) // nobody skipped checking values of axes now
        {
            if((myDiceRoller.Axe[(myDiceRoller.currentPlayerId + 1) % 4] == 0 && myDiceRoller.Axe[myDiceRoller.currentPlayerId] != 0) ||
                (myDiceRoller.Axe[(myDiceRoller.currentPlayerId + 1) % 4] == 0 && myDiceRoller.Axe[myDiceRoller.currentPlayerId] == 0)) //opponent doesnt have axe
            {
                Debug.Log("Opponent out of Axes");
                finalTile = finalTile.NextTiles[1];
                this.transform.position = finalTile.transform.position;
            } 
            else if(myDiceRoller.Axe[(myDiceRoller.currentPlayerId + 1) % 4] != 0 && myDiceRoller.Axe[myDiceRoller.currentPlayerId] == 0)//current player doesnt have axe
            {
                Debug.Log("Current Player Out of Axes");
                yield return StartCoroutine(SecondLadderCoroutine());
                Debug.Log("Second Player Has Chosen At Last");
                PV.RPC("RPC_SyncHasChosen", RpcTarget.AllViaServer);
                yield return new WaitUntil(HasChosenSynced);
                hasChosenSynced = false;
                Debug.Log("Resetting hasChosen - " + myDiceRoller.hasChosen);
                if(myDiceRoller.currentPlayerGetsToChoose == false)
                {
                    Debug.Log("Opponent Skipped at last");
                    finalTile = finalTile.NextTiles[1];
                    this.transform.position = finalTile.transform.position;
                }
                else if(myDiceRoller.currentPlayerGetsToChoose == true)
                {
                    Debug.Log("Opponent used Axe at last");
                }
            }
        }
        yield return StartCoroutine(CompleteTheTurn(finalTile));
    }

    void LadderHere(Tile finalTile)
    {
        if(myDiceRoller.Axe[(myDiceRoller.currentPlayerId + 1) % 4] != 0) // checking if next player has axe to stop it from climbing
        {
            Debug.Log("Opponent Has Axe");
            StartCoroutine(LadderCoroutine(finalTile));
        }
        else
        {
            Debug.Log("Opponent Doesnt Have Axe");
            finalTile = finalTile.NextTiles[1];
            this.transform.position = finalTile.transform.position;
            StartCoroutine(CompleteTheTurn(finalTile));
        }
    }

    bool CounterPlayerChose()
    {
        if(myDiceRoller.counterPlayerChose == true)
        {
            return true;
        }
        else
        {
            return false;
        }    
    }

    bool ChoosingDisplaySynced()
    {
        if(choosingDisplaySynced == true)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    [PunRPC]
    void RPC_SyncingChoosingDisplay(int localId)
    {
        myDisplayController.myString = "Player " + localId + " is choosing";
        if((myPassingData.myId - 1) == myDiceRoller.currentPlayerId)
        {
            choosingDisplaySynced = true;
        }
    }

    IEnumerator SecondSnakeCoroutine()
    {
        PV.RPC("RPC_SyncingChoosingDisplay", RpcTarget.AllViaServer, myPassingData.myId);
        yield return new WaitUntil(ChoosingDisplaySynced);
        choosingDisplaySynced = false;
        myDiceRoller.counterCanChoose = true;
        myDiceRoller.isSnake = true;
        yield return new WaitUntil(CounterPlayerChose);
        myDiceRoller.counterPlayerChose = false;
    }

    [PunRPC]
    void RPC_SyncCanChooseForSnakeCharmer(int localId)
    {   
        Debug.Log("canChoose Synced");
        myDisplayController.myString = "Player " + ((localId % 4) + 1) + " is choosing";
        myDiceRoller.canChoose = true;
        myDiceRoller.isSnake = true;
        if((myPassingData.myId - 1) == myDiceRoller.currentPlayerId)
        {
            canChooseSynced = true;
        }
    }


    IEnumerator ThirdSnakeCoroutine()
    {
        PV.RPC("RPC_SyncCanChooseForSnakeCharmer", RpcTarget.AllViaServer, myPassingData.myId);
        yield return new WaitUntil(CanChooseSynced);
        canChooseSynced = false;
        Debug.Log("Second Player Can Choose Now");
        yield return new WaitUntil(HasChosen);
        Debug.Log("Can Choose - " + myDiceRoller.canChoose);
        Debug.Log("Has Chosen - " + myDiceRoller.hasChosen);
        Debug.Log("Current player gets to choose - " + myDiceRoller.currentPlayerGetsToChoose);
        Debug.Log("Next player gets to choose - " + myDiceRoller.nextPlayerGetsToChoose);
        Debug.Log("Next player wants to choose - " + myDiceRoller.nextPlayerWantsToChoose);
    }

    IEnumerator SnakeCoroutine(Tile finalTile)
    {
        while(myDiceRoller.SnakeCharmer[(myDiceRoller.currentPlayerId + 1) % 4] != 0 
            && myDiceRoller.SnakeCharmer[myDiceRoller.currentPlayerId] != 0 && myDiceRoller.currentPlayerWantsToChoose == true &&
            myDiceRoller.nextPlayerWantsToChoose == true) // player1 and player2 fight for the snakecharmer
            {
                if(myDiceRoller.currentPlayerGetsToChoose == true)
                {
                    yield return StartCoroutine(SecondSnakeCoroutine());
                    Debug.Log("Can Choose - " + myDiceRoller.canChoose);
                    Debug.Log("Has Chosen - " + myDiceRoller.hasChosen);
                    Debug.Log("Current player gets to choose - " + myDiceRoller.currentPlayerGetsToChoose);
                    Debug.Log("Next player gets to choose - " + myDiceRoller.nextPlayerGetsToChoose);
                    Debug.Log("Next player wants to choose - " + myDiceRoller.nextPlayerWantsToChoose);
                    Debug.Log("Current Player Has Chosen");
                }
                if(myDiceRoller.nextPlayerGetsToChoose == true)
                {
                    yield return StartCoroutine(ThirdSnakeCoroutine());
                    Debug.Log("Second Player Has Chosen");
                    PV.RPC("RPC_SyncHasChosen", RpcTarget.AllViaServer);
                    yield return new WaitUntil(HasChosenSynced);
                    hasChosenSynced = false;
                    Debug.Log("Resetting hasChosen - " + myDiceRoller.hasChosen);
                }
            }
        if(myDiceRoller.currentPlayerWantsToChoose == false && myDiceRoller.nextPlayerWantsToChoose == true) //current player skipped
        {
            Debug.Log("Current Player Skipped");
            finalTile = finalTile.NextTiles[2];
            this.transform.position = finalTile.transform.position;
        } 
        else if(myDiceRoller.nextPlayerWantsToChoose == false && myDiceRoller.currentPlayerWantsToChoose == true) //opponent skipped
        {
            Debug.Log("Opponent Skipped");
        }
        else if(myDiceRoller.nextPlayerWantsToChoose == true && myDiceRoller.currentPlayerWantsToChoose == true) // nobody skipped checking values of snakecharmers now
        {
            if((myDiceRoller.SnakeCharmer[(myDiceRoller.currentPlayerId + 1) % 4] != 0 && myDiceRoller.SnakeCharmer[myDiceRoller.currentPlayerId] == 0) ||
                (myDiceRoller.SnakeCharmer[(myDiceRoller.currentPlayerId + 1) % 4] == 0 && myDiceRoller.SnakeCharmer[myDiceRoller.currentPlayerId] == 0)) //current player doesnt have snakecharmer
            {
                Debug.Log("Current Player Out of Snakecharmers");
                finalTile = finalTile.NextTiles[2];
                this.transform.position = finalTile.transform.position;
            } 
            else if(myDiceRoller.SnakeCharmer[(myDiceRoller.currentPlayerId + 1) % 4] == 0 && myDiceRoller.SnakeCharmer[myDiceRoller.currentPlayerId] != 0)//opponent doesnt have snakecharmer
            {
                Debug.Log("Opponent out of snakecharmer");
                yield return StartCoroutine(SecondSnakeCoroutine());
                Debug.Log("Can Choose - " + myDiceRoller.canChoose);
                Debug.Log("Has Chosen - " + myDiceRoller.hasChosen);
                Debug.Log("Current player gets to choose - " + myDiceRoller.currentPlayerGetsToChoose);
                Debug.Log("Next player gets to choose - " + myDiceRoller.nextPlayerGetsToChoose);
                Debug.Log("Next player wants to choose - " + myDiceRoller.nextPlayerWantsToChoose);
                Debug.Log("Current Player Has Chosen");
                if(myDiceRoller.nextPlayerGetsToChoose == false)
                {
                    Debug.Log("Current Player Skipped at last");
                    finalTile = finalTile.NextTiles[2];
                    this.transform.position = finalTile.transform.position;
                }
                else if(myDiceRoller.nextPlayerGetsToChoose == true)
                {
                    Debug.Log("Current Player used Snakecharmer at last");
                }
            }
        }
        yield return StartCoroutine(CompleteTheTurn(finalTile));
    }

    void SnakeHere(Tile finalTile)
    {
        if(myDiceRoller.SnakeCharmer[myDiceRoller.currentPlayerId] != 0) // checking if next player has snakecharmer to stop from going down
        {
            Debug.Log("Current Player Has Snakecharmer");
            StartCoroutine(SnakeCoroutine(finalTile));
        }
        else
        {
            Debug.Log("Current Player Doesnt Have SnakeCharmer");
            finalTile = finalTile.NextTiles[2];
            this.transform.position = finalTile.transform.position;
            StartCoroutine(CompleteTheTurn(finalTile));
        }
    }

    void CheckForSnakesOrLadder(Tile finalTile)
    {
        if(finalTile.NextTiles.Length > 1) // this tile has more than 1 next tiles i.e. snake or ladder here
        {
            if(finalTile.NextTiles[1] != null) // ladder present here
            {
                Debug.Log("Ladder Here");
                LadderHere(finalTile);
            }
            else if(finalTile.NextTiles[2] != null) //snake present here
            {
                Debug.Log("Snake Here");
                SnakeHere(finalTile);
            }
        }
        else
        {
            Debug.Log("No Snake Or Ladder Here");
            StartCoroutine(CompleteTheTurn(finalTile));
        }
    }

    bool ResultSynced()
    {
        if(resultSynced == true)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    [PunRPC]
    void RPC_SyncingResult(int localId)
    {
        myDisplayController.myString = "Player " + localId + " won";
        if((myPassingData.myId - 1) == myDiceRoller.currentPlayerId)
        {
            resultSynced = true;
        }
    }

    IEnumerator displayResult()
    {
        PV.RPC("RPC_SyncingResult", RpcTarget.AllViaServer, myDiceRoller.currentPlayerId);
        yield return new WaitUntil(ResultSynced);
        resultSynced = false;
        Destroy(gameObject);
    }

    void MovePlayer()
    {
        Tile finalTile = currentTile;
        Tile tempTile = finalTile;
        for(int i = 0; i < myDiceRoller.DiceValue; i++)
        {
            if(finalTile == null)
            {
                finalTile = GameSetup.GS.StartingTile;
            }
            else
            {
                finalTile = finalTile.NextTiles[0];
                if(finalTile.NextTiles == null || finalTile.NextTiles.Length == 0) 
                {
                    if(i == (myDiceRoller.DiceValue - 1))
                    {
                        Debug.Log("Player " + ((myDiceRoller.currentPlayerId + 1) % 4) + " Won!!");
                        StartCoroutine(displayResult());
                        return;
                    }
                    else
                    {
                        Debug.Log("stay here");
                        finalTile = tempTile;
                        myDiceRoller.CanMove = false;
                        tempTile = null;
                        StartCoroutine(CompleteTheTurn(finalTile));
                        return;
                    }
                } 
            }
        }
        this.transform.position = finalTile.transform.position;
        myDiceRoller.CanMove = false;
        CheckForSnakesOrLadder(finalTile);
    }
}