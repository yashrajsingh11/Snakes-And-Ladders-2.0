using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplayController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        myDiceRoller = GameObject.FindObjectOfType<DiceRoller>();
    }

    DiceRoller myDiceRoller;
    [SerializeField]
    private Text player1Axe;
    [SerializeField]
    private Text player1SC;
    [SerializeField]
    private Text player2Axe;
    [SerializeField]
    private Text player2SC;
    [SerializeField]
    private Text player3Axe;
    [SerializeField]
    private Text player3SC;
    [SerializeField]
    private Text player4Axe;
    [SerializeField]
    private Text player4SC;
    [SerializeField]
    private Text diceValue;
    [SerializeField]
    private Text feed;
    public string myString = "Welcome!!";

    // Update is called once per frame
    void Update()
    {
        player1Axe.text  = "Axe : " + myDiceRoller.Axe[0];
        player1SC.text  = "SC : " + myDiceRoller.SnakeCharmer[0];
        player2Axe.text  = "Axe : " + myDiceRoller.Axe[1];
        player2SC.text  = "SC : " + myDiceRoller.SnakeCharmer[1];
        player3Axe.text  = "Axe : " + myDiceRoller.Axe[2];
        player3SC.text  = "SC : " + myDiceRoller.SnakeCharmer[2];
        player4Axe.text  = "Axe : " + myDiceRoller.Axe[3];
        player4SC.text  = "SC : " + myDiceRoller.SnakeCharmer[3];
        diceValue.text = myDiceRoller.DiceValue + " rolled";
        feed.text = myString;
    }
}
