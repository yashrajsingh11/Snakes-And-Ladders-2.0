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
    public bool allPlayerPresent = false;
    public string useridXXXX;
    public List <Dictionary<string, object>> userDataAll = new List<Dictionary<string, object>>();


    // Update is called once per frame
    void Update() {

		if(isConnectedToServer == true || allPlayerPresent == true) {
			SceneManager.LoadSceneAsync("SampleScene");
			Debug.Log("Hey Its Me");
		}
		isConnectedToServer = false;
	}

    public  void loginAnno() {

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
            Debug.Log(useridXXXX);
            addUserToDatabase();
    	});
    }

    public void addUserToDatabase() {

    	FirebaseFirestore _refrence = FirebaseFirestore.DefaultInstance;
        UserModelClassX userClassModel = new UserModelClassX(useridXXXX,true);
         _refrence.Collection("Users").Document(useridXXXX)
            .SetAsync(userClassModel.toJsonX())
           .ContinueWith(task => {
            Debug.Log("Mast");

        	_refrence.Collection("Users").GetSnapshotAsync().ContinueWith(task=> {

            	QuerySnapshot snapshot = task.Result;
            	foreach (DocumentSnapshot document in snapshot.Documents) {
                	Dictionary<string, object> resultsHere = document.ToDictionary();
                	userDataAll.Add(resultsHere);
            	};
            // for(int i = 0; i < userDataAll.Count; i++) {
            // 	UserModelClassX p = getDetails(userDataAll[i]);
            // 	Debug.Log(p.uidT);
            // }
            		Debug.Log(userDataAll.Count);
        	isConnectedToServer = true;
        	});
        });
    }

    // public UserModelClassX getDetails(Dictionary<string, object> data) {
    // 	if(data == null) {
    // 		return null;
    // 	}
    // 	return UserModelClassX(data["uid"] == null ? "" : data["uid"], data["isOnline"] == null ? "" : data["isOnline"]);
    // }

}
