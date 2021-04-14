using System;
using System.Collections;
using System.Collections.Generic;
using Firebase.Firestore;
using UnityEngine;

public class MovesNMoves
{
    //get green signal for that both players are in
    bool allOK = true;
    //List that checks the details...
    List<UserMovesListData> userMoves = new List<UserMovesListData>();
    public void createMovesRoom(string uidofUserCreatedtheRoom, string secondUserUID)
    {
        Dictionary<string, object> userStab = new Dictionary<string, object>{
        {"player1Result",12},
        {"player2Result",2},
        {"player1DiceValue",1},
        {"player2DiceValue",1},
        {"player1AXE",0},
        {"player2AXE",0},
        {"player1SnakeC",0},
        {"player2SnakeC",0},
        {"player1Turn",true},
        {"player2Turn",false},
    };
        FirebaseFirestore _refrence = FirebaseFirestore.DefaultInstance;
        _refrence.Collection("Room").Document(uidofUserCreatedtheRoom).Collection("HelloD")
        .Document(secondUserUID + uidofUserCreatedtheRoom).SetAsync(userStab).ContinueWith(task =>
        {
            Debug.Log("Dono User Ke variables");
        });
    }

    public void checkForUserMoves(string uidofUserCreatedtheRoom)
    {
        FirebaseFirestore _refrence = FirebaseFirestore.DefaultInstance;
        _refrence.Collection("Room").Document(uidofUserCreatedtheRoom).Collection("HelloD").GetSnapshotAsync().ContinueWith(task =>
        {
            QuerySnapshot snapshots = task.Result;
            foreach (DocumentSnapshot document in snapshots.Documents)
            {
                Dictionary<string, object> aLLLData = document.ToDictionary();
                UserMovesListData daata = new UserMovesListData
                {
                    player1Result = Convert.ToInt32(aLLLData["player1Result"].ToString()),
                    player2Result = Convert.ToInt32(aLLLData["player2Result"].ToString()),
                    player1DiceValue = Convert.ToInt32(aLLLData["player1DiceValue"].ToString()),
                    player2DiceValue = Convert.ToInt32(aLLLData["player2DiceValue"].ToString()),
                    player1Turn = bool.Parse(aLLLData["player1Turn"].ToString()),
                    player2Turn = bool.Parse(aLLLData["player2Turn"].ToString()),
                    player1AXE = Convert.ToInt32(aLLLData["player1AXE"].ToString()),
                    player2AXE = Convert.ToInt32(aLLLData["player2AXE"].ToString()),
                    player1SnakeC = Convert.ToInt32(aLLLData["player1SnakeC"].ToString()),
                    player2SnakeC = Convert.ToInt32(aLLLData["player2SnakeC"].ToString()),
                };
                userMoves.Add(daata);
            }
        });
    }
    public void player1Upadtes(bool player1Move, int player1AXE, int player1SnakeC,
    int player1Result, int player1DiceValue, string uidofUserCreatedtheRoom, string secondUserUID)
    {
        FirebaseFirestore _refrence = FirebaseFirestore.DefaultInstance;
        Dictionary<string, object> updates = new Dictionary<string, object>
                        {
                            {"player1Move",  player1Move},
                              {"player1DiceValue",  player1DiceValue},
                            {"player1Result",player1Result},
                             {"player1AXE",player1AXE},
                              {"player1SnakeC",player1SnakeC},
                        };
        _refrence.Collection("Room").Document(uidofUserCreatedtheRoom).Collection("HelloD").Document(uidofUserCreatedtheRoom + secondUserUID)
        .UpdateAsync(updates).ContinueWith(task => { Debug.Log("Update Moves"); });
    }
    public void player2Upadtes(bool player1Move, int player1Result, int player2AXE, int player2SnakeC,
     int player1DiceValue, string uidofUserCreatedtheRoom, string secondUserUID)
    {
        FirebaseFirestore _refrence = FirebaseFirestore.DefaultInstance;
        Dictionary<string, object> updates = new Dictionary<string, object>
                        {
                            {"player2Move",  player1Move},
                              {"player2DiceValue",  player1DiceValue},
                            {"player2Result",player1Result},
                              {"player2AXE",player2AXE},
                              {"player2SnakeC",player2SnakeC},
                        };
        _refrence.Collection("Room").Document(uidofUserCreatedtheRoom).Collection("HelloD").Document(uidofUserCreatedtheRoom + secondUserUID)
        .UpdateAsync(updates).ContinueWith(task => { Debug.Log("Update Moves"); });
    }

}
