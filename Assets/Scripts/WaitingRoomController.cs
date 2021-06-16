using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class WaitingRoomController : MonoBehaviourPunCallbacks
{
    // Start is called before the first frame update
    void Start()
    {
        myPhotonView = GetComponent<PhotonView>();
        myPassingData = GameObject.FindObjectOfType<PassingData>();
        fullGameTimer = maxFullGameWaitTime;
        notFullGameTimer = maxWaitTime;
        timerToStartGame = maxWaitTime;
        
        PlayerCountUpdate();
    }

    private PhotonView myPhotonView;
    PassingData myPassingData;
    [SerializeField]
    private int gameIndex;
    [SerializeField]
    private int startingMenuIndex;
    private int playerCount;
    private int roomSize;
    [SerializeField]
    private int minPlayersToStart;
    [SerializeField]
    private Text roomCountDisplay;
    [SerializeField]
    private Text timerToStartDisplay;
    private bool readyToCountDown;
    private bool readyToStart;
    private bool pehlaBoolean;

    private bool startingGame;
    private float timerToStartGame;
    private float notFullGameTimer;
    private float fullGameTimer;
    [SerializeField]
    private float maxWaitTime;
    [SerializeField]
    private float maxFullGameWaitTime;

    void PlayerCountUpdate()
    {
        playerCount = PhotonNetwork.PlayerList.Length;
        if(pehlaBoolean == false) 
        {
            myPassingData.myId = playerCount;
            pehlaBoolean = true;
        }
        roomSize = PhotonNetwork.CurrentRoom.MaxPlayers;
        roomCountDisplay.text  = "Players: " + playerCount + "/" + roomSize;

        if(playerCount == roomSize)
        {
            readyToStart = true;
        }
        else if (playerCount >= minPlayersToStart)
        {
            readyToCountDown = true;
        }
        else
        {
            readyToCountDown = false;
            readyToStart = false;
        }
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        PlayerCountUpdate();
        if(PhotonNetwork.IsMasterClient)
            myPhotonView.RPC("RPC_SendTimer", RpcTarget.Others, timerToStartGame);        
    }

    [PunRPC]
    private void RPC_SendTimer(float timeIn)
    {
        timerToStartGame = timeIn;
        notFullGameTimer = timeIn;
        if(timeIn < fullGameTimer)
        {
            fullGameTimer = timeIn;
        }
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        PlayerCountUpdate();
    }

    // Update is called once per frame
    void Update()
    {
        WaitingForMorePlayers();
    }

    void WaitingForMorePlayers()
    {
        if(playerCount < 1)
        {
            ResetTimer();
        }

        if(readyToStart)
        {
            fullGameTimer -= Time.deltaTime;
            timerToStartGame = fullGameTimer;
        }
        else if(readyToCountDown)
        {
            notFullGameTimer -= Time.deltaTime;
            timerToStartGame = notFullGameTimer;
        }

        string tempTimer = string.Format("{0:00}", timerToStartGame);
        timerToStartDisplay.text = "Timer: " + tempTimer;

        if(timerToStartGame <= 0f)
        {
            if(startingGame)
                return;
            StartGame();
        }
    }

    void ResetTimer()
    {
        timerToStartGame = maxWaitTime;
        notFullGameTimer = maxWaitTime;
        fullGameTimer = maxWaitTime;
    }

    public void StartGame() 
    {
        startingGame = true;
        if(!PhotonNetwork.IsMasterClient)
            return;
        PhotonNetwork.CurrentRoom.IsOpen = false;
        if(playerCount == roomSize)
        {
            PhotonNetwork.LoadLevel(gameIndex);
        }
        else 
        {
            PhotonNetwork.LeaveRoom();
            SceneManager.LoadScene(startingMenuIndex);
        }
    }

    public void MyCancel() {
        PhotonNetwork.LeaveRoom();
        SceneManager.LoadScene(startingMenuIndex);
    }

}
