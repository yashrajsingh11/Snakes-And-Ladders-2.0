using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.IO;

public class GameController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        myPassingData = GameObject.FindObjectOfType<PassingData>();
        CreatePlayer();
    }

    PassingData myPassingData;

    private void CreatePlayer()
    {
        Debug.Log("Creating Player");
        PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs","PhotonPlayer"), Vector3.zero, Quaternion.identity);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
