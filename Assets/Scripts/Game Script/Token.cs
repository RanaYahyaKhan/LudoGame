using Photon.Pun;
using System.Collections;
using TMPro;
using UnityEngine;

public class Token : MonoBehaviourPun
{
    public GameObject token;
    public Token tokenScer;
    public int  tokenPositions; // Tracks each token's position on the board
    public Transform[] boardPositions; // Array of all board positions
    [SerializeField] public PhotonPlayer player;
    public Transform startingPosition;
    public bool isHome;
    public bool eligibaleToMove;

    void Start()
    {
        // Initialize token positions
        isHome = true;
        if (isHome)
        {
            tokenPositions = -1;
        }
    }
   
    // Triggered by the move button
    public void MoveSelectedToken()
    {
        Debug.Log("token is click");
        //GameManager.Instance.EndTurn();
        photonView.RPC("MoveToken", RpcTarget.All);
    }
    [PunRPC]
    private void MoveToken()
    {
        player.canBlink = false;
        eligibaleToMove=false;
        UIManager.Instance.StopScaling();
        Debug.Log(GameManager.Instance.currentPlayerIndex + "is moving do you Know");
        // Stop blinking for all tokens

        if (!DiceRoller.instance.diceRolled)
        {
            Debug.LogWarning("Roll the dice before selecting a token.");
            return;
        }


        int currentTokenPosition = tokenPositions;
        // Check if the move is valid
       if (currentTokenPosition == -1 && DiceRoller.instance.diceValue == 6) // Token can enter the board on rolling a 6
        {
            //tokenPositions += DiceRoller.instance.diceValue;
            tokenPositions = 0; // Move to the starting position
            isHome = false;
        }
        else if (currentTokenPosition >= 0) // Token is already on the board
        {
            RemoveTokenToTile(currentTokenPosition);
            tokenPositions += DiceRoller.instance.diceValue;

            // Ensure the token doesn't exceed the board length
            if (tokenPositions >= UIManager.Instance.player1BoardPositions.Length)
            {
                tokenPositions = UIManager.Instance.player1BoardPositions.Length - 1; // Clamp to last position
            }
        }
        else
        {
            Debug.LogWarning("Invalid move. Token cannot enter the board unless a 6 is rolled.");
            return;
        }

        // Update the token's position on the board
        //selectedToken.transform.position = boardPositions[tokenPositions[tokenIndex]].position;
        AddTokenToTile(tokenPositions);

        StartCoroutine(MoveToPosition(currentTokenPosition));
        photonView.RPC("SyncTokenPosition", RpcTarget.Others, tokenPositions);

        Debug.Log($"Token {token.name} moved to position {tokenPositions}");

        // Reset dice for the next turn
        

    }
    private IEnumerator MoveToPosition(int currentPosition)
    {
        while (currentPosition < tokenPositions)
        {
            currentPosition++;
            token.transform.position = boardPositions[currentPosition].position;
            yield return new WaitForSeconds(0.3f);
        }
        //Invoke("EndTurn", 1f);
        EndTurn();
    }

    private void EndTurn()
    {

        if (player.isSixCome)
        {
            player.isSixCome = false;
            DiceRoller.instance.diceRolled = false;
            Debug.Log("Is Six is false");
        }
        else
        {
            GameManager.Instance.players[0].isTurn = false;
            player.ButtonsInteractable(false);
            GameManager.Instance.EndTurn();
            //GameManager.Instance.players[GameManager.Instance.currentPlayerIndex].isTurn=false;
            Debug.Log("Changing the turn");

        }
    }
    [PunRPC]
    public void SyncTokenPosition(int newPosition)
    {
        if ((newPosition-DiceRoller.instance.diceValue) >= 0) // Token is already on the board
        {
            RemoveTokenToTile(newPosition - DiceRoller.instance.diceValue);
        }
        tokenPositions = newPosition;

        token.transform.position = boardPositions[newPosition].position;
        Debug.Log("Other Position " + boardPositions[newPosition]);
        AddTokenToTile(newPosition);
        Debug.Log($"Token {token.name} moved to position {newPosition}");
    }
   
    //Add the token to the tile
    private void AddTokenToTile(int index)
    {
        TileManager tileManager = boardPositions[index].GetComponent<TileManager>();
        tileManager.AddToken(tokenScer);
        //check for kill mechanism
        var strike = tileManager.CheckKill(player);
        if (strike.isKilled)
        {
            Debug.Log("Goti galti mar");
            tileManager.BackToHome();
            
        }
    }
    //Remove the Token To Tile
    private void RemoveTokenToTile(int index)
    {
        TileManager tileRemove = boardPositions[index].GetComponent<TileManager>();
        tileRemove.RemoveToken(tokenScer);
    }
    
    

    public int GetTokenPosition(int tokenIndex)
    {
        return tokenPositions;
    }

}//end of Class
