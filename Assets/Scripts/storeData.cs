using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class storeData : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
    	DontDestroyOnLoad (transform.gameObject);
    }

    public string player1uid;
    public string player2uid; 
    // Update is called once per frame
    void Update()
    {

    }
}
