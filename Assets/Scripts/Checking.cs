using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Checking : MonoBehaviour
{

    public GameObject playerPrefab ;
    //public GameObject playerPrefab2 ;
    // Start is called before the first frame update
    void Start()
    {
        PhotonNetwork.Instantiate(playerPrefab.name,Vector3.zero,Quaternion.identity);
        //Debug.Log(PhotonPlayer.Get(ID)); 
        //PhotonNetwork.Instantiate(playerPrefab2.name,Vector3.zero,Quaternion.identity);
    }

    // Update is called once per frame       
    void Update()
    {
        
    }
}
