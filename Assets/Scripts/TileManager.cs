using System.Collections.Generic;
using UnityEngine;

public class TileManager : MonoBehaviour
{
    public List<Token> Tokens = new List<Token>();
    public bool isProtected;
    public bool isLast;
    public void AddToken(Token token)
    {
        Tokens.Add(token);
        CheckForWin();
    }
    public void RemoveToken(Token token)
    {
        if (Tokens.Contains(token))
        {
            Tokens.Remove(token);
        }
    }
    public (bool isKilled, Token tokenKilled) CheckKill(PhotonPlayer strikingPlayer)
    {
        if (Tokens.Count < 2 || isProtected) return (false, null);

        if (Tokens.Count == 2)
        {
            foreach (Token token in Tokens)
            {
                if (token.player != strikingPlayer)
                {
                    return (true, token);
                }
            }
        }
        return (false, null);

    }
    private void CheckForWin()
    {
        if (isLast)
        {
            if (Tokens.Count == 4)
            {
                GameManager.Instance.WininScreen.SetActive(true);
            }
        }

    }
    public void BackToHome()
    {
        Debug.Log("Change Position");
        Tokens[0].token.transform.position = Tokens[0].startingPosition.transform.position;
        Tokens[0].isHome = true;
        Tokens[0].tokenPositions = -1;
    }
}//end of Class
