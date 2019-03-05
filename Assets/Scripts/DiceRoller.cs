using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;

public class DiceRoller : MonoBehaviour
{

    public GameObject oneDie;
    public GameObject twoDices;
    public GameObject threeDices;
    public GameObject fourDices;
    public GameObject closupShotCamera;
    public TextMeshProUGUI resultText;
    private Dice [] dices;
    private int dicesDoneRolling = 0;
    private int dicesDoneReseting = 0;

    private bool canRoll = true;

    private void Start()
    {
        SetNumberOfDices (2);
        RollDices ();
       
    }
    private void Update()
    {
   

        if (Input.acceleration.sqrMagnitude > 5) {
            RollDices ();
        }
        

    }

    public void SetNumberOfDices (int number) {
        if (!canRoll)
            return;
        oneDie.SetActive (false);
        twoDices.SetActive (false);
        threeDices.SetActive (false);
        fourDices.SetActive (false);
        switch (number) {
            case 1:
                dices = oneDie.GetComponentsInChildren<Dice> ();
                oneDie.SetActive (true);
                break;
            case 2:
                dices = twoDices.GetComponentsInChildren<Dice> ();
                twoDices.SetActive (true);
                break;
            case 3:
                dices = threeDices.GetComponentsInChildren<Dice> ();
                threeDices.SetActive (true);
                break;
            case 4:
                dices = fourDices.GetComponentsInChildren<Dice> ();
                fourDices.SetActive (true);
                break;

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
        resultText.text = result.ToString ();
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
