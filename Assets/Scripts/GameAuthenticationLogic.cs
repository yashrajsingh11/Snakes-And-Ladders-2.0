using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Auth;
using Firebase.Firestore;
using System;
using UnityEngine.SceneManagement;

public class GameAuthenticationLogic : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    public bool isConnectedToServer = false;
    public bool isEveryonePresent = false;
    public bool isInRoom = false;
    public bool player1inroom = false;
    public bool secondPlayerAdded = false;
    public bool roomSearched = false;
    public bool ekAurBoolean = false;
    public string useridXXXX;
    public string player2iD = "";
    public List<UserDeserializeData> userDataAll = new List<UserDeserializeData>();
    public List<RoomsDataXXXX> roomInfromation = new List<RoomsDataXXXX>();


    // Update is called once per frame
    void Update()
    {

        if (isConnectedToServer == true && isEveryonePresent == true && isInRoom == true)
        {
            isConnectedToServer = false;
            isEveryonePresent = false;
            isInRoom = false;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            Debug.Log("Game Start");
        }

        if(ekAurBoolean == true) {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            Debug.Log("Game Start");
        }

        if (player1inroom == true && ekAurBoolean == false)
        {
            FirebaseFirestore _refrence = FirebaseFirestore.DefaultInstance;
            _refrence.Collection("Room").Document(useridXXXX).GetSnapshotAsync().ContinueWith(task =>
            {
                DocumentSnapshot snapshot = task.Result;
                if (snapshot.Exists)
                {
                    Dictionary<string, object> cxxxxxity = snapshot.ToDictionary();
                    foreach (KeyValuePair<string, object> pair in cxxxxxity)
                    {
                        if (pair.Key == "allOk" && bool.Parse(pair.Value.ToString()) == true)
                        ekAurBoolean = true;
                        Debug.Log(String.Format("{0}: {1}", pair.Key, pair.Value));
                        break;
                    }
                }
                else
                {
                   // Debug.Log(String.Format("Document {0} does not exist!", snapshot.Id));
                }
            });


            // searchForRooms();
            // Debug.Log("waiting for other user");
            // if(roomInfromation.Exists(element => element.player2Uid == player2iD)) {
            //     SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            //     Debug.Log("Game Start");
            // }
        }

        if (roomSearched == true)
        {
            if (roomInfromation.Count == 0)
            {
                createARooom();
            }
            roomSearched = false;
        }

        if (secondPlayerAdded == true)
        {
            // Debug.Log(useridXXXX);
            // Debug.Log(player2iD);
            isEveryonePresent = true;
            secondPlayerAdded = false;
        }

        if (isConnectedToServer == true && isEveryonePresent == false && isInRoom == false)
        {
            FirebaseFirestore _refrence = FirebaseFirestore.DefaultInstance;

            for (int i = 0; i < userDataAll.Count; i++)
            {
                //Player 1 and Player 2
                abc();
                if (userDataAll[i].isOnline && userDataAll[i].uid != useridXXXX && userDataAll.Count > 1)
                {
                    player2iD = userDataAll[i].uid;
                    Debug.Log(player2iD);

                    if (!roomInfromation.Exists(element => element.player2Uid == player2iD) || roomInfromation.Exists(element => String.IsNullOrEmpty(element.player2Uid)))
                    {

                        Debug.Log("hello");
                        Dictionary<string, object> asufs = new Dictionary<string, object>{
                            {"player2Uid", useridXXXX},
                            {"allOk",true},
                        };
                        _refrence.Collection("Room").Document(player2iD).UpdateAsync(asufs).ContinueWith(task =>
                        {
                            Debug.Log("Second UserAdded to First User Room ");
                            secondPlayerAdded = true;
                            ///Game Begin.
                        });
                        isInRoom = true;
                    }
                    break;
                }
                //  else {

                //     Debug.Log("Waiting for second user");
                // }
            }
        }
    }
    //new Code
    public void createARooom()
    {
        FirebaseFirestore _refrence = FirebaseFirestore.DefaultInstance;
        UserModelClassX userClassModel = new UserModelClassX(useridXXXX, true);
        _refrence.Collection("Room").Document(useridXXXX).SetAsync(userClassModel.toJsonRoomCreateXX(useridXXXX)
        ).ContinueWith(task =>
        {
            Debug.Log("Entering room");
            player1inroom = true;
        });
        //searchForRooms();
    }
    public void searchForRooms()
    {
        FirebaseFirestore _refrence = FirebaseFirestore.DefaultInstance;
        _refrence.Collection("Room").GetSnapshotAsync().ContinueWith(task =>
        {
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
            roomSearched = true;
        });
    }
    //

    public void abc()
    {

        FirebaseFirestore _refrence = FirebaseFirestore.DefaultInstance;
        if (userDataAll.Count <= 1 && !userDataAll.Exists(element => element.uid == useridXXXX))
        {
            _refrence.Collection("Users").GetSnapshotAsync().ContinueWith(task =>
            {
                QuerySnapshot snapshot = task.Result;
                foreach (DocumentSnapshot document in snapshot.Documents)
                {
                    Dictionary<string, object> resultsHere = document.ToDictionary();
                    ///deserizz the data code
                    UserDeserializeData dsData = new UserDeserializeData
                    {
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

    public void loginAnno()
    {

        Firebase.Auth.FirebaseAuth auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
        auth.SignInAnonymouslyAsync().ContinueWith(task =>
        {
            if (task.IsCanceled)
            {
                Debug.LogError("SignInAnonymouslyAsync was canceled.");
                return;
            }
            if (task.IsFaulted)
            {
                Debug.LogError("SignInAnonymouslyAsync encountered an error: " + task.Exception);
                return;
            }
            Firebase.Auth.FirebaseUser newUser = task.Result;
            useridXXXX = newUser.UserId;
            addUserToDatabase();
        });
    }

    public void addUserToDatabase()
    {
        FirebaseFirestore _refrence = FirebaseFirestore.DefaultInstance;
        UserModelClassX userClassModel = new UserModelClassX(useridXXXX, true);
        _refrence.Collection("Users").Document(useridXXXX)
            .SetAsync(userClassModel.toJsonX())
            .ContinueWith(task =>
            {
                Debug.Log("Connected To Server");
                isConnectedToServer = true;
                abc();
                searchForRooms();
            });
    }
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