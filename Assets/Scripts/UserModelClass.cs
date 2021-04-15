using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Firebase.Firestore;

public class UserModelClassX {

    public string uidT;
    public bool isPresnt;
    public UserModelClassX(string uid,bool isP) {
        uidT = uid;
        isPresnt = isP;
    }

    public Dictionary<string, object> toJsonX() {
        
        Dictionary<string, object> gamePlayer = new Dictionary<string, object> {
            { "uid", uidT },
            { "isOnline", true },
            { "dateTime",DateTime.Now.ToString()}
        };
        return gamePlayer;
    }

    public Dictionary<string, object> toJsonRoomCreateXX(string userId1) {

        Dictionary<string, object> PlayersRoom = new Dictionary<string, object> {
            { "player1Uid", userId1 },
            { "player2Uid", "" },
            { "player1Move", true },
            { "player2Move", false },
            { "dateTime",DateTime.Now.ToString()},
            { "allOk",false}
        };
        return PlayersRoom;
    }
}

[FirestoreData]
public class UserDeserializeData {
    [FirestoreProperty]
    public string uid { get; set; }

    [FirestoreProperty]
    public string dateTime { get; set; }

    [FirestoreProperty]
    public bool isOnline { get; set; }
}

[FirestoreData]
public class UserMovesListData
{
    [FirestoreProperty]
    public int player1Result { get; set; }

    [FirestoreProperty]
    public int player2Result { get; set; }

    [FirestoreProperty]
    public int player1DiceValue { get; set; }

    [FirestoreProperty]
    public int player2DiceValue { get; set; }

    
    [FirestoreProperty]
    public int player1AXE { get; set; }

    
    [FirestoreProperty]
    public int player2AXE { get; set; }

    
    [FirestoreProperty]
    public int player1SnakeC { get; set; }

    
    [FirestoreProperty]
    public int player2SnakeC { get; set; }



    

    [FirestoreProperty]
    public bool player1Turn { get; set; }

    [FirestoreProperty]
    public bool player2Turn { get; set; }

    [FirestoreProperty]
    public string player1uid { get; set; }

    [FirestoreProperty]
    public string player2uid { get; set; }
}
