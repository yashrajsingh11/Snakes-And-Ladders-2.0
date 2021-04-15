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
    public bool allVariablesSet = false;
    public string player2iD = "";
    public string ekAurPlayer2id = "";
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

        if (allVariablesSet == true)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            Debug.Log("Game Start");
            allVariablesSet = false;
        }

        if (ekAurBoolean == true)
        {
            Debug.Log(ekAurPlayer2id);
            createMovesRoom(useridXXXX, ekAurPlayer2id);
            player1inroom = false;
            ekAurBoolean = false;
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
                    ekAurPlayer2id = cxxxxxity["player2Uid"].ToString();
                    ekAurBoolean = bool.Parse(cxxxxxity["allOk"].ToString());
                    Debug.Log(cxxxxxity.Count);
                    Debug.Log(ekAurPlayer2id);
                    Debug.Log(ekAurBoolean);

                }
            });
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
            isEveryonePresent = true;
            secondPlayerAdded = false;
        }

        if (isConnectedToServer == true && isEveryonePresent == false && isInRoom == false)
        {
            FirebaseFirestore _refrence = FirebaseFirestore.DefaultInstance;

            for (int i = 0; i < userDataAll.Count; i++)
            {
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
                        });
                        isInRoom = true;
                    }
                    break;
                }
            }
        }
    }

    public void createARooom()
    {

        FirebaseFirestore _refrence = FirebaseFirestore.DefaultInstance;
        UserModelClassX userClassModel = new UserModelClassX(useridXXXX, true);
        _refrence.Collection("Room").Document(useridXXXX).SetAsync(userClassModel.toJsonRoomCreateXX(useridXXXX)
        ).ContinueWith(task =>
        {
            Debug.Log("Entering room");
            player1inroom = true;
            isInRoom = true;
        });
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
        _refrence.Collection("Users").Document(useridXXXX).SetAsync(userClassModel.toJsonX()).ContinueWith(task =>
        {
            Debug.Log("Connected To Server");
            isConnectedToServer = true;
            abc();
            searchForRooms();
        });
    }

    public void createMovesRoom(string uidofUserCreatedtheRoom, string secondUserUID)
    {
        FirebaseAuth _firebase = FirebaseAuth.DefaultInstance;
        Dictionary<string, object> userStab = new Dictionary<string, object>{
        {"player1Result",0},
        {"player2Result",0},
        {"player1DiceValue",0},
        {"player2DiceValue",0},
        {"player1AXE",0},
        {"player2AXE",0},
        {"player1SnakeC",0},
        {"player2SnakeC",0},
        {"player1Turn",true},
        {"player2Turn",false},
    };
        if (_firebase.CurrentUser.UserId != secondUserUID)
        {
            FirebaseFirestore _refrence = FirebaseFirestore.DefaultInstance;
            _refrence.Collection("Room").Document(useridXXXX).Collection("HelloD")
            .Document(uidofUserCreatedtheRoom+secondUserUID).SetAsync(userStab).ContinueWith(task =>
            {
                Debug.Log("Dono User Ke variables");
                allVariablesSet = true;
            });
        }

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