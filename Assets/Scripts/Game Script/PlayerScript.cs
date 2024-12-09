using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerScript : MonoBehaviour
{
    public string playerName;
    public bool isTurn = false; // Tracks if it is this player's turn
    public Token token;


    private void Start()
    {
        
    }

    public void StartTurn()
    {
        isTurn = true;
    }

   
    public void CanMove()
    {
        if (!isTurn ) return;
        Debug.Log(GameManager.Instance.currentPlayerIndex + " is to move");
        token.CanMove();
    }

   
}
