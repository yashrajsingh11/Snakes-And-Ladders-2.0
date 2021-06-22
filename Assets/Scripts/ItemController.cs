using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class ItemController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        PV = GetComponent<PhotonView>();
        myPassingData = GameObject.FindObjectOfType<PassingData>();
        myDiceRoller = GameObject.FindObjectOfType<DiceRoller>();
        myDisplayController = GameObject.FindObjectOfType<DisplayController>();
    }

    DisplayController myDisplayController;
    PassingData myPassingData;
    private PhotonView PV;
    DiceRoller myDiceRoller;
    public GameObject choseAxe;
    public GameObject choseSnakecharmer;
    public GameObject choseAxeSkip;
    public GameObject choseSnakecharmerSkip;
    public GameObject counterchoseAxe;
    public GameObject counterchoseSnakecharmer;
    public GameObject counterchoseAxeSkip;
    public GameObject counterchoseSnakecharmerSkip;
    private bool localCanChoose = false;
    private bool doneChoosing = false;
    private bool allBooleanSynced = false;
    private bool allCounterBooleanSynced = false;

    // Update is called once per frame
    void Update()
    {
        if(localCanChoose == false)
        {
            choseAxe.SetActive(false);
            choseAxeSkip.SetActive(false);
            choseSnakecharmer.SetActive(false);
            choseSnakecharmerSkip.SetActive(false);
            counterchoseAxe.SetActive(false);
            counterchoseAxeSkip.SetActive(false);
            counterchoseSnakecharmer.SetActive(false);
            counterchoseSnakecharmerSkip.SetActive(false);
        }

        if(myDiceRoller.canChoose == true && (myPassingData.myId - 1) == ((myDiceRoller.currentPlayerId + 1) % 4) && myDiceRoller.isLadder == true)
        {
            Debug.Log("Choosing For Axe");
            StartCoroutine(GetsToChooseForAxe());
            myDiceRoller.canChoose = false;
            myDiceRoller.isLadder = false;
        }

        if(myDiceRoller.canChoose == true && (myPassingData.myId - 1) == ((myDiceRoller.currentPlayerId + 1) % 4) && myDiceRoller.isSnake == true)
        {
            Debug.Log("Choosing For SnakeCharmer");
            StartCoroutine(GetsToChooseForSnakeCharmer());
            myDiceRoller.canChoose = false;
            myDiceRoller.isSnake = false;
        }

        if(myDiceRoller.counterCanChoose == true && (myPassingData.myId - 1) == myDiceRoller.currentPlayerId && myDiceRoller.isSnake == true)
        {
            Debug.Log("Choosing For Counter SnakeCharmer");
            StartCoroutine(GetsToChooseForCounterSnakeCharmer());
            myDiceRoller.counterCanChoose = false;
            myDiceRoller.isSnake = false;
        }

        if(myDiceRoller.counterCanChoose == true && (myPassingData.myId - 1) == myDiceRoller.currentPlayerId && myDiceRoller.isLadder == true)
        {
            Debug.Log("Choosing For Counter Axe");
            StartCoroutine(GetsToChooseForCounterAxe());
            myDiceRoller.counterCanChoose = false;
            myDiceRoller.isLadder = false;
        }
    }

    bool DoneChoosing()
    {
        if(doneChoosing == true)
        {
            return true;
        }
        else{
            return false;
        }
    }

    IEnumerator GetsToChooseForCounterAxe()
    {
        localCanChoose = true;
        counterchoseAxe.SetActive(true);
        counterchoseAxeSkip.SetActive(true);
        yield return new WaitUntil(DoneChoosing);
        doneChoosing = false;
        myDiceRoller.counterPlayerChose = true;
    }

    IEnumerator GetsToChooseForCounterSnakeCharmer()
    {
        localCanChoose = true;
        counterchoseSnakecharmer.SetActive(true);
        counterchoseSnakecharmerSkip.SetActive(true);
        yield return new WaitUntil(DoneChoosing);
        doneChoosing = false;
        myDiceRoller.counterPlayerChose = true;
    }

    IEnumerator GetsToChooseForAxe()
    {
        localCanChoose = true;
        choseAxe.SetActive(true);
        choseAxeSkip.SetActive(true);
        yield return new WaitUntil(DoneChoosing);
        doneChoosing = false;
    }

    IEnumerator GetsToChooseForSnakeCharmer()
    {
        localCanChoose = true;
        choseSnakecharmer.SetActive(true);
        choseSnakecharmerSkip.SetActive(true);
        yield return new WaitUntil(DoneChoosing);
        doneChoosing = false;
    }

    bool AllCounterBooleanSynced()
    {
        if(allCounterBooleanSynced == true)
        {
            return true;
        }
        else
        {
            return false;
        }    
    }

    [PunRPC]
    void RPC_SyncingCounterBooleanForAxe(int localId)
    {
        Debug.Log("Counter Booleans Synced and Axe Used By Current Player");
        myDisplayController.myString = "Player " + localId + " chose axe";
        myDiceRoller.Axe[localId - 1] = myDiceRoller.Axe[localId - 1] - 1;
        Debug.Log("Player " + (localId - 1) + " has " + myDiceRoller.Axe[localId - 1] + " axes");
        myDiceRoller.currentPlayerGetsToChoose = false;
        myDiceRoller.currentPlayerWantsToChoose = true;
        myDiceRoller.nextPlayerGetsToChoose = true;
        myDiceRoller.nextPlayerWantsToChoose = true;
        if((myPassingData.myId - 1) == myDiceRoller.currentPlayerId)
        {
            allCounterBooleanSynced = true;
        }
    }

    IEnumerator CounterPlayerChoseAxe()
    {
        Debug.Log("Current Player Chose To Use Axe");
        PV.RPC("RPC_SyncingCounterBooleanForAxe", RpcTarget.AllViaServer, myPassingData.myId);
        yield return new WaitUntil(AllCounterBooleanSynced);
        allCounterBooleanSynced = false;
        doneChoosing = true;
    }

    public void counterPlayerChoseAxe()
    {
        StartCoroutine(CounterPlayerChoseAxe());
        localCanChoose = false;
    } 

    [PunRPC]
    void RPC_SyncingCounterBooleanForSkip(int localId)
    {
        Debug.Log("Counter Booleans Synced");
        myDisplayController.myString = "Player " + localId + " chose skip";
        myDiceRoller.currentPlayerGetsToChoose = false;
        myDiceRoller.currentPlayerWantsToChoose = false;
        myDiceRoller.nextPlayerGetsToChoose = false;
        myDiceRoller.nextPlayerWantsToChoose = true;
        if((myPassingData.myId - 1) == myDiceRoller.currentPlayerId)
        {
            allCounterBooleanSynced = true;
        }
    }

    IEnumerator CounterPlayerSkipped()
    {
        Debug.Log("Current Player Chose To Skip");
        PV.RPC("RPC_SyncingCounterBooleanForSkip", RpcTarget.AllViaServer, myPassingData.myId);
        yield return new WaitUntil(AllCounterBooleanSynced);
        allCounterBooleanSynced = false;
        doneChoosing = true;
    }

    public void counterPlayerSkipped()
    {
        StartCoroutine(CounterPlayerSkipped());
        localCanChoose = false;
    }


    [PunRPC]
    void RPC_SyncingCounterBooleanForSnakeCharmer(int localId)
    {
        Debug.Log("Counter Booleans Synced and SnakeCharmer Used By Current Player");
        myDisplayController.myString = "Player " + localId + " chose snakecharmer";
        myDiceRoller.SnakeCharmer[localId - 1] = myDiceRoller.SnakeCharmer[localId - 1] - 1;
        Debug.Log("Player " + (localId - 1) + " has " + myDiceRoller.SnakeCharmer[localId - 1] + " snakecharmers");
        myDiceRoller.currentPlayerGetsToChoose = false;
        myDiceRoller.currentPlayerWantsToChoose = true;
        myDiceRoller.nextPlayerGetsToChoose = true;
        myDiceRoller.nextPlayerWantsToChoose = true;
        if((myPassingData.myId - 1) == myDiceRoller.currentPlayerId)
        {
            allCounterBooleanSynced = true;
        }
    }

    IEnumerator CounterPlayerChoseSnakeCharmer()
    {
        Debug.Log("Current Player Chose To Use SnakeCharmer");
        PV.RPC("RPC_SyncingCounterBooleanForSnakeCharmer", RpcTarget.AllViaServer, myPassingData.myId);
        yield return new WaitUntil(AllCounterBooleanSynced);
        allCounterBooleanSynced = false;
        doneChoosing = true;
    }

    public void counterPlayerChoseSnakeCharmer()
    {
        StartCoroutine(CounterPlayerChoseSnakeCharmer());
        localCanChoose = false;
    }

    bool AllBooleanSynced()
    {
        if(allBooleanSynced == true)
        {
            return true;
        }
        else
        {
            return false;
        }    
    }

    [PunRPC]
    void RPC_SyncingBooleanForAxe(int localId)
    {
        Debug.Log("Booleans Synced and Axe Used By Opponent");
        myDisplayController.myString = "Player " + localId + " chose axe";
        myDiceRoller.canChoose = false;
        myDiceRoller.hasChosen = true;
        myDiceRoller.isLadder = false;
        myDiceRoller.Axe[localId - 1] = myDiceRoller.Axe[localId - 1] - 1;
        Debug.Log("Player " + (localId - 1) + " has " + myDiceRoller.Axe[localId - 1] + " axes");
        myDiceRoller.nextPlayerGetsToChoose = false;
        myDiceRoller.nextPlayerWantsToChoose = true;
        myDiceRoller.currentPlayerGetsToChoose = true;
        myDiceRoller.currentPlayerWantsToChoose = true;
        if((myPassingData.myId - 1) == ((myDiceRoller.currentPlayerId + 1) % 4))
        {
            allBooleanSynced = true;
        }
    }

    IEnumerator PlayerChoseAxe()
    {
        Debug.Log("Chose To Use Axe");
        PV.RPC("RPC_SyncingBooleanForAxe", RpcTarget.AllViaServer, myPassingData.myId);
        yield return new WaitUntil(AllBooleanSynced);
        allBooleanSynced = false;
        doneChoosing = true;
    }

    public void playerChoseAxe()
    {
        StartCoroutine(PlayerChoseAxe());
        localCanChoose = false;
    }

    [PunRPC]
    void RPC_SyncingBooleanForSnakeCharmer(int localId)
    {
        Debug.Log("Booleans Synced and SnakeCharmer Used By Opponent");
        myDisplayController.myString = "Player " + localId + " chose snakecharmer";
        myDiceRoller.canChoose = false;
        myDiceRoller.hasChosen = true;
        myDiceRoller.isSnake = false;
        myDiceRoller.SnakeCharmer[localId - 1] = myDiceRoller.SnakeCharmer[localId - 1] - 1;
        Debug.Log("Player " + (localId - 1) + " has " + myDiceRoller.SnakeCharmer[localId - 1] + " snakecharmers");
        myDiceRoller.nextPlayerGetsToChoose = false;
        myDiceRoller.nextPlayerWantsToChoose = true;
        myDiceRoller.currentPlayerGetsToChoose = true;
        myDiceRoller.currentPlayerWantsToChoose = true;
        if((myPassingData.myId - 1) == ((myDiceRoller.currentPlayerId + 1) % 4))
        {
            allBooleanSynced = true;
        }
    }


    IEnumerator PlayerChoseSnakeCharmer()
    {
        Debug.Log("Chose To Use SnakeCharmer");
        PV.RPC("RPC_SyncingBooleanForSnakeCharmer", RpcTarget.AllViaServer, myPassingData.myId);
        yield return new WaitUntil(AllBooleanSynced);
        allBooleanSynced = false;
        doneChoosing = true;
    }

    public void playerChoseSnakeCharmer()
    {
        StartCoroutine(PlayerChoseSnakeCharmer());
        localCanChoose = false;
    }

    [PunRPC]
    void RPC_SyncingBooleanForSkip(int localId)
    {
        Debug.Log("Booleans Synced");
        myDisplayController.myString = "Player " + localId + " chose skip";
        myDiceRoller.canChoose = false;
        myDiceRoller.hasChosen = true;
        myDiceRoller.isSnake = false;
        myDiceRoller.isLadder = false;
        myDiceRoller.nextPlayerGetsToChoose = false;
        myDiceRoller.nextPlayerWantsToChoose = false;
        myDiceRoller.currentPlayerGetsToChoose = false;
        myDiceRoller.currentPlayerWantsToChoose = true;
        if((myPassingData.myId - 1) == ((myDiceRoller.currentPlayerId + 1) % 4))
        {
            allBooleanSynced = true;
        }
    }

    IEnumerator PlayerSkipped()
    {
        Debug.Log("Chose To Skip");
        PV.RPC("RPC_SyncingBooleanForSkip", RpcTarget.AllViaServer, myPassingData.myId);
        yield return new WaitUntil(AllBooleanSynced);
        allBooleanSynced = false;
        doneChoosing = true;
    }

    public void playerChoseSkip()
    {
        StartCoroutine(PlayerSkipped());
        localCanChoose = false;
    }

}
