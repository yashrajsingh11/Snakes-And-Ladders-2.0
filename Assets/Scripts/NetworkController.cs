using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class NetworkController : MonoBehaviourPunCallbacks 
{
    // Start is called before the first frame update
    void Start() 
    {
        PhotonNetwork.ConnectUsingSettings();    
    }

    // Update is called once per frame
    void Update() 
    {
        
    }

    public override void OnConnectedToMaster()
    { 
        Debug.Log("We are connected to " + PhotonNetwork.CloudRegion + " server");
    }

}
