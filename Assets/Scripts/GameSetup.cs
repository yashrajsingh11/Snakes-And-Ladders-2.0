using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSetup : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    public static GameSetup GS;
    public Transform[] spawnPoints;
    public Tile StartingTile;

    private void OnEnable()
    {
        if(GameSetup.GS == null)
        {
            GameSetup.GS = this;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
