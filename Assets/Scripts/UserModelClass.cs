using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Firebase.Firestore;

public class UserModelClassX
{
    public string uidT;
    public bool isPresnt;
    public UserModelClassX(string uid,bool isP)
    {
        uidT = uid;
        isPresnt = isP;
    }

    public Dictionary<string, object> toJsonX()
    {
        Dictionary<string, object> gamePlayer = new Dictionary<string, object>
    {
        { "uid", uidT },
        { "isOnline", true },
        { "dateTime",DateTime.Now.ToString()}
    };
        return gamePlayer;
    }
    public Dictionary<string, object> toJsonRoomCreateXX(string userId1,string userId2)
    {
        Dictionary<string, object> PlayersRoom = new Dictionary<string, object>
    {
        { "player1Uid", userId1 },
        { "player2Uid", userId2 },
        { "player1Move", true },
        { "player2Move", false },
        { "dateTime",DateTime.Now.ToString()}
    };
        return PlayersRoom;
    }
}

[FirestoreData]
public class UserDeserializeData
{
    [FirestoreProperty]
    public string uid { get; set; }

    [FirestoreProperty]
    public string dateTime { get; set; }

    [FirestoreProperty]
    public bool isOnline { get; set; }
}
