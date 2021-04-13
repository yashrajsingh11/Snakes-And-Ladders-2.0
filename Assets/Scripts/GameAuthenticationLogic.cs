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
    public bool allPlayerPresent = false;
    public string useridXXXX;
    public string player2iD = "";

    public List<UserDeserializeData> userDataAll = new List<UserDeserializeData>();


    // Update is called once per frame
    void Update()
    {

        if (isConnectedToServer == true && allPlayerPresent == true)
        {
            SceneManager.LoadSceneAsync("SampleScene");
            Debug.Log("Hey Its Me");
            isConnectedToServer = false;
            allPlayerPresent = false;  
        }
        if(isConnectedToServer == true && allPlayerPresent == false) {
        FirebaseFirestore _refrence = FirebaseFirestore.DefaultInstance;
        for (int i = 0; i < userDataAll.Count; i++) {
          //Player 1 and Player 2
          if (userDataAll[i].isOnline && userDataAll[i].uid != useridXXXX && userDataAll.Count > 1 && isConnectedToServer == true) {
            player2iD = userDataAll[i].uid;
            Debug.Log(useridXXXX);
                  Debug.Log(player2iD);
                  UserModelClassX userClassModel = new UserModelClassX(useridXXXX, true);
                  if(!string.IsNullOrEmpty(player2iD)) {
                  _refrence.Collection("Room").Document(useridXXXX + player2iD).SetAsync(
   userClassModel.toJsonRoomCreateXX(useridXXXX, player2iD)

                   ).ContinueWith(task =>
               {
                   Debug.Log("Hello Rooms");
                   allPlayerPresent = true;
               });
            break;
          }
          } else {
            Debug.Log("Ruk jao bhai");
          }
        }
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
            // Debug.Log(useridXXXX);
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
              Debug.Log("Mast");
               ///NewCodeLine....
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
                 isConnectedToServer = true;
              });
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


