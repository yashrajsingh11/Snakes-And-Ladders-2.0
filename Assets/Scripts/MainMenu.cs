using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using UnityEngine.UI;
using TMPro;
using System;
using Photon.Realtime;

public class MainMenu : MonoBehaviourPunCallbacks
{

     public GameObject findOpponent = null;
     public GameObject waitingStatus = null;
    public Text waitingStatusText = null;
    fokat thefokt;
    
    private bool isConnecting = false;
    private const string GameVersion = "0.1";
    private const int MaxPlayersPerRoom = 2;

    public void Start(){
        thefokt = GameObject.FindObjectOfType<fokat>();
    }

    private void Awake()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    public void findopponent()
    {
        
        isConnecting = true;
        findOpponent.SetActive(false);
        waitingStatus.SetActive(true);
        waitingStatusText.text = "Loading...";
        if (PhotonNetwork.IsConnected)
        {
            PhotonNetwork.JoinRandomRoom();

        }
        else
        {
            PhotonNetwork.GameVersion = GameVersion;
            PhotonNetwork.ConnectUsingSettings();
        }
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected to Master");
        if (isConnecting)
        {
            PhotonNetwork.JoinRandomRoom();
        }
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        waitingStatus.SetActive(false);
        findOpponent.SetActive(true);
        Debug.Log("Disconnected");
    }

    public override void OnJoinRandomFailed(short returnCode,string message)
    {
        Debug.Log("No clients are waiting for an opponent, creating a new room");
        PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = MaxPlayersPerRoom });
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("Client Successfully joined a room");
        int playerCount = PhotonNetwork.CurrentRoom.PlayerCount;
        if(playerCount != MaxPlayersPerRoom)
        {
            thefokt.isHost = true;
            waitingStatusText.text = "Waiting";
            Debug.Log("Client is waiting");
        }
        else
        {
            waitingStatusText.text = "Found";
            Debug.Log("Matching Began");
        }
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        if(PhotonNetwork.CurrentRoom.PlayerCount == MaxPlayersPerRoom)
        {
            PhotonNetwork.CurrentRoom.IsOpen = false;
            waitingStatusText.text = "Opponent Found";
            Debug.Log("Match is ready");
            PhotonNetwork.LoadLevel("SampleScene");
        }
    }

    public void PlayGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);  
    }

    public void QuitGame()
    {
        Debug.Log("Quit!");
        Application.Quit();
    }
}
