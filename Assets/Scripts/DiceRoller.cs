using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class DiceRoller : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        PV = GetComponent<PhotonView>();
        myPassingData = GameObject.FindObjectOfType<PassingData>();
        myDisplayController = GameObject.FindObjectOfType<DisplayController>();
    }

    DisplayController myDisplayController;
    PassingData myPassingData;
    private PhotonView PV;
    public int DiceValue;
    public int numberOfPlayers = 4;

    public int currentPlayerId = 0;
    private bool isLucky = false;
    private bool doneChoosing = false;
    public bool currentPlayerWantsToChoose = true;
    public bool nextPlayerWantsToChoose = true;
    public bool currentPlayerGetsToChoose = true;
    public bool nextPlayerGetsToChoose = true;
    public bool canChoose = false;
    public bool counterCanChoose = false;
    public bool hasChosen = false;
    public bool isLadder = false;
    public bool isSnake = false;
    public int[] Axe;
    public int[] SnakeCharmer; 
    public bool IsDoneRolling = false;
    private bool axesSynced = false;
    private bool diceSynced = false;
    private bool displaySynced = false;
    private bool snakeCharmerSynced = false;
    public bool CanMove = false;
    public bool counterPlayerChose = false;
    public GameObject luckyMenu1;
    public GameObject luckyMenu2;
    public GameObject myObject;

    // Update is called once per frame
    void Update()
    {   
        if(isLucky == false)
        {
            luckyMenu1.SetActive(false);
            luckyMenu2.SetActive(false);
        }
        if(myPassingData.myId - 1 != currentPlayerId)
        {
            myObject.SetActive(false);
        }
        else
        {
            myObject.SetActive(true);
        }
    }

    bool SnakeCharmerSynced()
    {
        if(snakeCharmerSynced == true)
        {
            return true;
        }
        else
        {
            return false;
        }    
    }

    [PunRPC]
    void RPC_SyncingSnakeCharmer(int localId)
    {
        myDisplayController.myString = "Player " + localId + " chose snakecharmer";
        SnakeCharmer[localId - 1] = SnakeCharmer[localId - 1] + 1;
        if((myPassingData.myId - 1) == currentPlayerId)
        {
            snakeCharmerSynced = true;
        }
    }

    public IEnumerator ChoosingSnakeCharmer()
    {
        PV.RPC("RPC_SyncingSnakeCharmer", RpcTarget.AllViaServer, myPassingData.myId);
        yield return new WaitUntil(SnakeCharmerSynced);
        snakeCharmerSynced = false;
        doneChoosing = true;
    }

    public void ChoseSnakeCharmer()
    {
        StartCoroutine(ChoosingSnakeCharmer());
        isLucky = false;
    }

    bool AxesSynced()
    {
        if(axesSynced == true)
        {
            return true;
        }
        else
        {
            return false;
        }    
    }

    [PunRPC]
    void RPC_SyncingAxe(int localId)
    {
        myDisplayController.myString = "Player " + localId + " chose axe";
        Axe[localId - 1] = Axe[localId - 1] + 1;
        if((myPassingData.myId - 1) == currentPlayerId)
        {
            axesSynced = true;
        }
    }

    IEnumerator ChoosingAxe()
    {
        PV.RPC("RPC_SyncingAxe", RpcTarget.AllViaServer, myPassingData.myId);
        yield return new WaitUntil(AxesSynced);
        axesSynced = false;
        doneChoosing = true;
    }

    public void ChoseAxe()
    {
        StartCoroutine(ChoosingAxe());
        isLucky = false;
    }

    bool DoneChoosing()
    {
        if(doneChoosing == true)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    IEnumerator IsLucky()
    {
        luckyMenu1.SetActive(true);
        luckyMenu2.SetActive(true);
        yield return new WaitUntil(DoneChoosing);
        doneChoosing = false;
        Debug.Log("Player Can Move");
        CanMove = true;
    }

    bool DisplaySynced()
    {
        if(displaySynced == true)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    [PunRPC]
    void RPC_SyncingLuckyDisplay(int localId)
    {
        myDisplayController.myString = "Player " + localId + " got lucky";
        if((myPassingData.myId - 1) == currentPlayerId)
        {
            displaySynced = true;
        }
    }

    IEnumerator DisplayLucky()
    {
        PV.RPC("RPC_SyncingLuckyDisplay", RpcTarget.AllViaServer, myPassingData.myId);
        yield return new WaitUntil(DisplaySynced);
        displaySynced = false;
        StartCoroutine(IsLucky());
    }

    void checkLucky()
    {
        int check = Random.Range(1,7);
        if(check == 4)
        {
            isLucky = true;
            StartCoroutine(DisplayLucky());
        }
        else{
            Debug.Log("NotLucky");
            CanMove = true;
        }
    }

    public void RollTheDice()
    {
        if(IsDoneRolling == true)
        {
            return;
        }
        IsDoneRolling = true;
        StartCoroutine(rollTheDice());
    }

    bool DiceSynced()
    {
        if(diceSynced == true)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    [PunRPC]
    void RPC_SyncingDiceValue(int localId, int localDiceValue)
    {
        DiceValue = localDiceValue;
        myDisplayController.myString = "Player " + localId + " rolled";
        if((myPassingData.myId - 1) == currentPlayerId)
        {
            diceSynced = true;
        }
    }

    IEnumerator rollTheDice()
    {        
        if(myPassingData.myId - 1 == currentPlayerId)
        {
            DiceValue = Random.Range(1, 7);
            PV.RPC("RPC_SyncingDiceValue", RpcTarget.AllViaServer, myPassingData.myId, DiceValue);
            yield return new WaitUntil(DiceSynced);
            diceSynced = false;
            checkLucky();
        }
        else 
        {
            Debug.Log("Not Your Turn");
        }
    }
}
