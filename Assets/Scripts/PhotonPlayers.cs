using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.IO;

public class PhotonPlayers : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        PV = GetComponent<PhotonView>();
        myPassingData = GameObject.FindObjectOfType<PassingData>();
        if(PV.IsMine)
        {
            myAvatar = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs","PlayerAvatar"),
             GameSetup.GS.spawnPoints[myPassingData.myId - 1].position,  GameSetup.GS.spawnPoints[myPassingData.myId - 1].rotation, 0);
        }
    }

    private PhotonView PV;
    public GameObject myAvatar;
    PassingData myPassingData;
    // Update is called once per frame
    void Update()
    {
        
    }
}
