using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using UnityEngine.SceneManagement;
using Firebase.Firestore;
using Firebase.Auth;

public class FirebaseFirestoreDatabaseX {

    public string useridXXXX;
    public FirebaseFirestoreDatabaseX(string userId) {
        useridXXXX = userId;
    }
    FirebaseFirestore _refrence = FirebaseFirestore.DefaultInstance;
    //Addding User to Firestore when game starts...
    public void addUserToDatabase() {
        UserModelClassX userClassModel = new UserModelClassX(useridXXXX,true);
         _refrence.Collection("Users").Document(useridXXXX)
            .SetAsync(userClassModel.toJsonX())
           .ContinueWith(task => {
            Debug.Log("Mast");
        });
    }

    public List<UserModelClassX> getDataXX() {

        List < UserModelClassX > userDataAll = new List<UserModelClassX>();
        _refrence.Collection("Users").GetSnapshotAsync().ContinueWith(task=> {
            
            QuerySnapshot snapshot = task.Result;
            foreach (DocumentSnapshot document in snapshot.Documents) {
                Dictionary<string, object> resultsHere = document.ToDictionary();
                string uid = "";
                bool isPresent = false;

                foreach (KeyValuePair<string,object> pairX in resultsHere) {

                    uid = (string)pairX.Value;
                    isPresent = (bool)pairX.Value;
                    Debug.Log(System.String.Format("{0}: {1}", pairX.Key, pairX.Value));
                }
                UserModelClassX data = new  UserModelClassX(uid,isPresent);
                userDataAll.Add(data);
            };
        });
        return userDataAll;
    }
    //Read UserData
   


    //Removing users from database when game Exits....
    public void removeUserFromDatabase() {

        FirebaseAuth auth = FirebaseAuth.DefaultInstance;
        _refrence.Collection("Users").Document(useridXXXX).DeleteAsync();
        auth.CurrentUser.DeleteAsync();
        Debug.Log("User Deleted");
    }
}
