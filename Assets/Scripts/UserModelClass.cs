using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

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
        { "dateTime",DateTime.Now}
    };
        return gamePlayer;
    }    
}