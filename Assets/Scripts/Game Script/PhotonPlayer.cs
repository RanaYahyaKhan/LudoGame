using Photon.Pun;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting.Antlr3.Runtime;
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
        //if (PhotonNetwork.LocalPlayer.ActorNumber - 1 == GameManager.Instance.currentPlayerIndex)
        if (TurnManager.instance.IsMyTurn())
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
        if (canBlink && DiceRoller.instance.diceRolled)
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
            //GameManager.Instance.EndTurn();
            TurnManager.instance.NextTurn();
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
            Token tkn = token.GetComponent<Token>();
            if (tokenBlinker != null)
            {
                // Check if the token is eligible to move
                if (IsTokenEligible(token))
                {
                    tokenBlinker.StartScaling();
                    tkn.eligibaleToMove = true;


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
        //return currentTokenPosition == -1 || currentTokenPosition >= 0;
        return ((currentTokenPosition == -1 && DiceRoller.instance.diceValue == 6) || currentTokenPosition >= 0) && currentTokenPosition + DiceRoller.instance.diceValue <=UIManager.Instance.player1BoardPositions.Length && !token.GetComponent<Token>().isWin;
    }

    public void MoveAPlayer()
    {
        Invoke("PlayerMovement", 3f);
    }
    private void PlayerMovement()
    {
        if (tokens[0].GetComponent<Token>().eligibaleToMove && DiceRoller.instance.diceValue==6 && tokens[0].GetComponent<Token>().isHome && !tokens[0].GetComponent<Token>().isWin)
        {
            tokens[0].GetComponent<Token>().MoveSelectedToken();
        }else if(tokens[1].GetComponent<Token>().eligibaleToMove && DiceRoller.instance.diceValue == 6 && tokens[1].GetComponent<Token>().isHome && !tokens[1].GetComponent<Token>().isWin)
        {
            tokens[1].GetComponent<Token>().MoveSelectedToken();
        }
        else if (tokens[2].GetComponent<Token>().eligibaleToMove && DiceRoller.instance.diceValue == 6 && tokens[2].GetComponent<Token>().isHome && !tokens[2].GetComponent<Token>().isWin)
        {
            tokens[2].GetComponent<Token>().MoveSelectedToken();
        }
        else if (tokens[3].GetComponent<Token>().eligibaleToMove && DiceRoller.instance.diceValue == 6 && tokens[3].GetComponent<Token>().isHome && !tokens[3].GetComponent<Token>().isWin)
        {
            tokens[3].GetComponent<Token>().MoveSelectedToken();
        }
        else
        {
            foreach (var token in tokens)
            {
                Token tkn = token.GetComponent<Token>();

                if (tkn.eligibaleToMove)
                {
                    tkn.MoveSelectedToken();
                    break;
                }
            }
        }
        /*
        foreach (var token in tokens)
        {
            Token tkn = token.GetComponent<Token>();
       
            if (tkn.eligibaleToMove)
            {
                if (DiceRoller.instance.diceValue == 6 && tkn.isHome)
                {
                    tkn.MoveSelectedToken();
                    break;
                }
                else
                {
                    tkn.MoveSelectedToken();
                    break;
                }
            }
        }
        */
    }
}//end of class

