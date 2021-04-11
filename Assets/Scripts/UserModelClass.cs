using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class UserModelClassX
{
    public string uidT;

    public UserModelClassX(string uid)
    {
        uidT = uid;
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
  /*  UserModelClass fromJson(Dictionary<string, object> data)
    {
        if(data == null)
        {
            return null;
        }
        else
        {
            return UserModelClass(data["uid"] == null ? "" : data["uid"],data["isOnline"] == null ? false : data["isOnline"]); 
        }
    }
  */

    
}