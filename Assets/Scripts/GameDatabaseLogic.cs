using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Firestore;
using Firebase.Auth;

public class FirebaseFirestoreDatabaseX
{
    public string useridXXXX;
   // public FirebaseUser user;
    public FirebaseFirestoreDatabaseX(string userId)
        // FirebaseUser cure
    {
        useridXXXX = userId;
       // user = cure;
    }
    FirebaseFirestore _refrence = FirebaseFirestore.DefaultInstance;
    //Addding User to Firestore when game starts...


    public void addUserToDatabase()
    {
        UserModelClassX userClassModel = new UserModelClassX(useridXXXX);
        _refrence.Collection("Users").Document(useridXXXX).SetAsync(userClassModel.toJsonX()).ContinueWith(task => {
            Debug.Log("Mast");
        });
    }
   /* public List<UserModelClassX> getDataXX()
    {
        _refrence.Collection("Users").GetSnapshotAsync().ContinueWith(task=> {
            Debug.Log(task.Result);

        });
    }
   */
    //Read UserData
   


    //Removing users from database when game Exits....
    public void removeUserFromDatabase()
    {
        Debug.Log("I am In");
        FirebaseAuth auth = FirebaseAuth.DefaultInstance;
        _refrence.Collection("Users").Document(useridXXXX).DeleteAsync();
        auth.CurrentUser.DeleteAsync();
        Debug.Log("User Deleted");
    }
}
