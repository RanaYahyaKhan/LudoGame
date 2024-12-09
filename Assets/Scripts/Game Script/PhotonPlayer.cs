using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PhotonPlayer : MonoBehaviourPun
{
    public Token playerToken;
    public bool isTurn = false;
    public Image[] piece;
    public Sprite[] goties;
    [SerializeField] private GameObject thisObj;

    private void Start()
    {
        GameManager.Instance.instanciatedPlayers.Add(thisObj);
    }
    public void GotiFace(int pieceColor)
    {
        for (int i = 0; i < piece.Length; i++)
        {
            piece[i].sprite = goties[pieceColor];
        }
    }
   
    public void StartTurn()
    {
        if (PhotonNetwork.LocalPlayer.ActorNumber - 1 == GameManager.Instance.currentPlayerIndex && photonView.IsMine)
        {
            isTurn = true;
            MoveToken();
        }
    }
    public void MoveToken()
    {
        
        if (/*isTurn && */photonView.IsMine)
        {
            playerToken.CanMove();
        }
    }

}
