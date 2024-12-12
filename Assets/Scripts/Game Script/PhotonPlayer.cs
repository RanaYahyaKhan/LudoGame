using Photon.Pun;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PhotonPlayer : MonoBehaviourPun
{
    public List<Token> playerToken =new List<Token>();
    public bool isTurn = false;
    public Image[] piece;
    public Sprite[] goties;
    [SerializeField] private GameObject thisObj;
    public bool canBlink;
    public GameObject[] tokens;
    [SerializeField] private Button[] tokenBtns;

    public bool isSixCome = false;



    private void Start()
    {
        GameManager.Instance.instanciatedPlayers.Add(thisObj);
        ButtonsInteractable(false);
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
        if (PhotonNetwork.LocalPlayer.ActorNumber - 1 == GameManager.Instance.currentPlayerIndex)
        {
            isTurn = true;
            MoveToken();
        }
    }
    public void MoveToken()
    {

        if (isTurn && photonView.IsMine)
        {
            CanMove();
        }
    }
    private void Update()
    {
        if (canBlink)
        {
            HighlightEligibleTokens();
        }

    }
    public void CanMove()
    {

        if (DiceRoller.instance.diceValue == 6) { isSixCome = true; Debug.Log("Six is True"); }
        // Check if the token is eligible to move
        if (IsTokenEligible(tokens[0]) || IsTokenEligible(tokens[1]) || IsTokenEligible(tokens[2]) || IsTokenEligible(tokens[3]))
        {
            canBlink = true;

            ButtonsInteractable(true);
        }
        else
        {
            Debug.Log("End of turn is Calling");
            ButtonsInteractable(false);
            GameManager.Instance.EndTurn();
        }
    }
    public void ButtonsInteractable( bool set)
    {
        foreach (var button in tokenBtns)
        {
            button.interactable = set;
            button.enabled = set;
        }
    }
    private void HighlightEligibleTokens()
    {
        foreach (var token in tokens)
        {
            TokenBlinker tokenBlinker = token.GetComponent<TokenBlinker>();
            if (tokenBlinker != null)
            {
                // Check if the token is eligible to move
                if (IsTokenEligible(token))
                {
                    tokenBlinker.StartScaling();

                }
                else
                {
                    tokenBlinker.StopScaling();
                }
            }
        }
    }

    private bool IsTokenEligible(GameObject token)
    {
        int tokenIndex = System.Array.IndexOf(tokens, token);
        if (tokenIndex < 0 || tokenIndex >= tokens.Length) return false;

        int currentTokenPosition = playerToken[tokenIndex].tokenPositions;

        // Token is eligible to move if it's off the board and a 6 is rolled
        // or if it's already on the board
        return currentTokenPosition == -1 || currentTokenPosition >= 0;
        //return (currentTokenPosition == -1 && DiceRoller.instance.diceValue == 6) || currentTokenPosition >= 0;
    }


}//end of class

