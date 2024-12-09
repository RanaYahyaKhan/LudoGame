using Photon.Pun;
using System.Collections;
using System.Runtime.InteropServices;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class Token : MonoBehaviourPun
{
    public GameObject[] tokens; // Array to hold all four tokens
    public int[] tokenPositions; // Tracks each token's position on the board
    public Transform[] boardPositions; // Array of all board positions
    private GameObject selectedToken; // Token currently selected
    public TokenBlinker[] blinker;
    private bool canBlink;


    void Start()
    {
        // Initialize token positions
        tokenPositions = new int[tokens.Length];
        for (int i = 0; i < tokenPositions.Length; i++)
        {
            tokenPositions[i] = -1; // Set initial positions to -1 (off the board)
        }
        
    }
    private void Update()
    {
        if (canBlink)
        {
            HighlightEligibleTokens();
        }
        
    }
    private bool isSixCome =false;
    public void CanMove()
    {
       
        if (DiceRoller.instance.diceValue == 6) { isSixCome = true; Debug.Log("Six is True"); }
        // Check if the token is eligible to move
        if (IsTokenEligible(tokens[0]) || IsTokenEligible(tokens[1]) || IsTokenEligible(tokens[2]) || IsTokenEligible(tokens[3]))
        {
            canBlink = true;

        }
        else
        {
            EndTurn();
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

        int currentTokenPosition = tokenPositions[tokenIndex];

        // Token is eligible to move if it's off the board and a 6 is rolled
        // or if it's already on the board
        //return currentTokenPosition == -1  || currentTokenPosition >= 0;
        return (currentTokenPosition == -1 && DiceRoller.instance.diceValue == 6) || currentTokenPosition >= 0;
    }

    // Triggered by the move button
    public void MoveSelectedToken(int tokenIndex)
    {
        Debug.Log("token is click");
        //GameManager.Instance.EndTurn();
        photonView.RPC("MoveToken", RpcTarget.All, tokenIndex);
    }
    [PunRPC]
    private void MoveToken(int tokenIndex)
    {
        TileManager tileManager = boardPositions[tokenPositions[tokenIndex]].GetComponent<TileManager>();
        tileManager.RemoveToken(selectedToken);
        canBlink = false;
        Debug.Log(GameManager.Instance.currentPlayerIndex + "is moving do you Know");
        // Stop blinking for all tokens
        foreach (var blink in blinker)
        {
            blink.StopScaling();
        }

        if (!DiceRoller.instance.diceRolled)
        {
            Debug.LogWarning("Roll the dice before selecting a token.");
            return;
        }
        if (tokenIndex < 0 || tokenIndex >= tokens.Length)
        {
            Debug.LogWarning("Invalid token index.");
            return;
        }

        selectedToken = tokens[tokenIndex];
        int currentTokenPosition = tokenPositions[tokenIndex];
        // Check if the move is valid
        if (currentTokenPosition == -1 && DiceRoller.instance.diceValue == 6) // Token can enter the board on rolling a 6
        {
            //tokenPositions[tokenIndex] += DiceRoller.instance.diceValue;
            tokenPositions[tokenIndex] = 0; // Move to the starting position
        }
        else if (currentTokenPosition >= 0) // Token is already on the board
        {
            tokenPositions[tokenIndex] += DiceRoller.instance.diceValue;

            // Ensure the token doesn't exceed the board length
            if (tokenPositions[tokenIndex] >= UIManager.Instance.player1BoardPositions.Length)
            {
                tokenPositions[tokenIndex] = UIManager.Instance.player1BoardPositions.Length - 1; // Clamp to last position
            }
        }
        else
        {
            Debug.LogWarning("Invalid move. Token cannot enter the board unless a 6 is rolled.");
            return;
        }

        // Update the token's position on the board
        //selectedToken.transform.position = boardPositions[tokenPositions[tokenIndex]].position;
        tileManager.AddToken(selectedToken);

        StartCoroutine(MoveToPosition(currentTokenPosition, tokenIndex));
        Debug.Log($"Token {selectedToken.name} moved to position {tokenPositions[tokenIndex]}");

        // Reset dice for the next turn
        

    }
    private IEnumerator MoveToPosition(int currentPosition, int tIndex)
    {
        while (currentPosition < tokenPositions[tIndex])
        {
            currentPosition++;
            selectedToken.transform.position = boardPositions[currentPosition].position;
            yield return new WaitForSeconds(0.3f);
        }
        photonView.RPC("SyncTokenPosition", RpcTarget.Others, tIndex, currentPosition);
        Invoke("EndTurn", 1f);
    }

    private void EndTurn()
    {

        if (isSixCome)
        {
            isSixCome = false;
            canBlink = false;
            DiceRoller.instance.diceRolled = false;
            Debug.Log("Is Six is false");
        }
        else
        {
            GameManager.Instance.players[0].isTurn = false;
            GameManager.Instance.EndTurn();
            //GameManager.Instance.players[GameManager.Instance.currentPlayerIndex].isTurn=false;
            Debug.Log("Changing the turn");

        }
    }
    [PunRPC]
    public void SyncTokenPosition(int tokenIndex, int newPosition)
    {
        tokenPositions[tokenIndex] = newPosition;
        tokens[tokenIndex].transform.position = boardPositions[newPosition].position;
        TileManager tileManager = boardPositions[newPosition].GetComponent<TileManager>();
        tileManager.AddToken(selectedToken);
        Debug.Log($"Token {tokenIndex} moved to position {newPosition}");
    }

    public int GetTokenPosition(int tokenIndex)
    {
        return tokenPositions[tokenIndex];
    }

}//end of Class
