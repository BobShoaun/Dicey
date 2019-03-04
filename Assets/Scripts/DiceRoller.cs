using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;

public class DiceRoller : MonoBehaviour
{

    public GameObject closupShotCamera;
    public TextMeshProUGUI resultText;
    public Dice [] dices;
    private int dicesDoneRolling = 0;
    private int dicesDoneReseting = 0;

    private bool canRoll = true;

    private void Start()
    {
        RollDices ();
    }
    private void Update()
    {
         if (Input.GetKeyDown (KeyCode.V)) {
           
        }

        if (Input.GetKeyDown (KeyCode.Space)) {
      
        }

        

    }

    public void RollDices () {
        if (!canRoll)
            return;

        closupShotCamera.SetActive (false);
        canRoll = false;
        Array.ForEach (dices, d => d.Roll (DoneRolling));
    }

    private void DoneRolling () {
        if (++dicesDoneRolling < dices.Length)
            return;
        dicesDoneRolling = 0;
        int result = 0;
        Array.ForEach (dices, d => result += d.Result ());
        resultText.text = "Result: " + result;
        Array.ForEach (dices, d => d.ResetPosition (DoneReseting));

        closupShotCamera.SetActive (true);
    }

    private void DoneReseting () {
        if (++dicesDoneReseting < dices.Length)
            return;
        dicesDoneReseting = 0;
        canRoll = true;
    }

}
