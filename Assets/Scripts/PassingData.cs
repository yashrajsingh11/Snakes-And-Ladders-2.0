using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassingData : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    public int myId;

    // Update is called once per frame
    void Update()
    {
        
    }
}
