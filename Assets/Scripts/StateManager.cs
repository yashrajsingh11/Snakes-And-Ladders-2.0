﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateManager : MonoBehaviour {
    // Start is called before the first frame update
    void Start() {
        
    }

    // Update is called once per frame
    void Update() {
        
    }

    public int NumberOfPlayers = 2;
    public int CurrentPlayerId = 0;
    public int diceValue;
    public bool IsDoneRolling = false;
    public bool IsLucky = false;
    public bool HasToChoose = false;
    public int PlayerOneAxe = 0;
    public int PlayerOneSnakeCharmer = 0;
    public int PlayerTwoAxe = 0;
    public int PlayerTwoSnakeCharmer = 0;

    public void NewTurn() {
    	IsDoneRolling = false;
    	CurrentPlayerId = (CurrentPlayerId + 1) % NumberOfPlayers;
    }

}
