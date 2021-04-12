// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using Firebase;
// using Firebase.Auth;
// using System;

// public class AuthResults {

//     public Firebase.Auth.FirebaseAuth auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
//     public  void loginAnno() {

//         auth.SignInAnonymouslyAsync().ContinueWith(task =>
//         {
//             if (task.IsCanceled) {
//                 Debug.LogError("SignInAnonymouslyAsync was canceled.");
//                 return;
//             }
//             if (task.IsFaulted)
//             {
               
//                 Debug.LogError("SignInAnonymouslyAsync encountered an error: " + task.Exception);
//                 return;
//             }

//             Firebase.Auth.FirebaseUser newUser = task.Result;
//             FirebaseFirestoreDatabaseX firebase = new FirebaseFirestoreDatabaseX(newUser.UserId);
//             firebase.addUserToDatabase();
//             Debug.LogFormat(
//                 newUser.UserId);
//         });
//     }
// }

  
