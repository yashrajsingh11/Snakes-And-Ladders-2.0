using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Auth;
using Firebase.Firestore;
using System;
using UnityEngine.SceneManagement;

public class GameAuthenticationLogic : MonoBehaviour {
    // Start is called before the first frame update
    void Start() {

    }

    public bool isConnectedToServer = false;
    public bool isEveryonePresent = false;
    public bool isInRoom = false;
    public string useridXXXX;
    public string player2iD = "";


    public List<UserDeserializeData> userDataAll = new List<UserDeserializeData>();
    public List<RoomsDataXXXX> roomInfromation = new List<RoomsDataXXXX>();


    // Update is called once per frame
    void Update() {

        if (isConnectedToServer == true && isEveryonePresent == true && isInRoom == true) {
            isConnectedToServer = false;
            isEveryonePresent = false; 
            isInRoom = false; 
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            Debug.Log("Game Start");
        }

        if (isConnectedToServer == true && isEveryonePresent == false) {
            FirebaseFirestore _refrence = FirebaseFirestore.DefaultInstance;
            for (int i = 0; i < userDataAll.Count; i++) {
            //Player 1 and Player 2
                abc();
                if (userDataAll[i].isOnline && userDataAll[i].uid != useridXXXX && userDataAll.Count > 1) {
                    player2iD = userDataAll[i].uid;
                    Debug.Log(useridXXXX);
                    Debug.Log(player2iD);
                    UserModelClassX userClassModel = new UserModelClassX(useridXXXX, true);
                    _refrence.Collection("Room").Document(useridXXXX + player2iD).SetAsync(userClassModel.toJsonRoomCreateXX(useridXXXX, player2iD)
                    ).ContinueWith(task => {
                        Debug.Log("Entering room");
                        isInRoom = true;
                    });
                        isEveryonePresent = true;
                    break;
                } else {
                    Debug.Log("Waiting for second user");
                }
            }
        }
    }

    public void abc() {

        FirebaseFirestore _refrence = FirebaseFirestore.DefaultInstance;
        //If not already in list then add to list and list must have less than or equal to 2 players
        if(userDataAll.Count <= 1 && !userDataAll.Exists(element => element.uid == useridXXXX)) {
            _refrence.Collection("Users").GetSnapshotAsync().ContinueWith(task => {
                QuerySnapshot snapshot = task.Result;
                foreach (DocumentSnapshot document in snapshot.Documents) {
                    Dictionary<string, object> resultsHere = document.ToDictionary();
                    ///deserizz the data code
                    UserDeserializeData dsData = new UserDeserializeData {
                        uid = resultsHere["uid"].ToString(),
                        isOnline = bool.Parse(resultsHere["isOnline"].ToString()),
                        dateTime = resultsHere["dateTime"].ToString(),
                    };
                    userDataAll.Add(dsData);
                };
            });
            Debug.Log("abc ran");
        }
    }

    public void loginAnno() {

        Firebase.Auth.FirebaseAuth auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
        auth.SignInAnonymouslyAsync().ContinueWith(task => {
            if (task.IsCanceled) {
                Debug.LogError("SignInAnonymouslyAsync was canceled.");
                return;
            }
            if (task.IsFaulted) {
                Debug.LogError("SignInAnonymouslyAsync encountered an error: " + task.Exception);
                return;
            }
            Firebase.Auth.FirebaseUser newUser = task.Result;
            useridXXXX = newUser.UserId;
            addUserToDatabase();
        });
    }

    public void addUserToDatabase() {

        FirebaseFirestore _refrence = FirebaseFirestore.DefaultInstance;
        UserModelClassX userClassModel = new UserModelClassX(useridXXXX, true);
        _refrence.Collection("Users").Document(useridXXXX)
            .SetAsync(userClassModel.toJsonX())
            .ContinueWith(task => {
                Debug.Log("Connected To Server");
                isConnectedToServer = true;
                abc();
        });
    }
}

    public void searchForRooms()
    {
        FirebaseFirestore _refrence = FirebaseFirestore.DefaultInstance;
        _refrence.Collection("Room").GetSnapshotAsync().ContinueWith(task => {
            QuerySnapshot snapshot = task.Result;
            foreach (DocumentSnapshot document in snapshot.Documents)
            {
                Dictionary<string, object> resultsHere = document.ToDictionary();
                ///deserizz the data code
                RoomsDataXXXX romxx = new RoomsDataXXXX
                {
                    player1Uid = resultsHere["player1Uid"].ToString(),
                    player2Uid = resultsHere["player2Uid"].ToString(),
                    player1Move = bool.Parse(resultsHere["player1Move"].ToString()),
                    player2Move = bool.Parse(resultsHere["player2Move"].ToString()),
                    dateTime = resultsHere["dateTime"].ToString(),
                    roomKey = resultsHere["roomKey"].ToString(),
                };
                roomInfromation.Add(romxx);
            };
        });
    } 

//update PlayerMove
/*
void updatePlayerMover(bool player1Move, bool player2Move)
{

    Dictionary<string, object> updates = new Dictionary<string, object>
                        {
                            { "player1Move",  player1Move},
                            {"player2Move",player2Move }
                        };


    _refrence.Collection("Room").Document(useridXXXX + player2iD)
    .UpdateAsync(updates).ContinueWith(task => { Debug.Log("Update Moves"); });
}
*/


