using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fokat : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(this.gameObject);   
    }
    public bool isHost = false;
    // Update is called once per frame
    void Update()
    {
      //  Debug.Log(isHost);
    }
}
