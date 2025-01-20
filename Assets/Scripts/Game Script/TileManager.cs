using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class TileManager : MonoBehaviour
{
    public List<Token> Tokens = new List<Token>();
    public bool isProtected;
    public bool isLast;
    public void AddToken(Token token)
    {
        Tokens.Add(token);
        CheckForWin();
        AdjustTokenPositions();
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
            foreach (var goti in Tokens)
            {
                goti.isWin = true;
            }
            if (Tokens.Count == 4)
            {
                GameManager.Instance.ShowWinner();
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
    private void Update()
    {
        AdjustTokenScale();
    }
    private void AdjustTokenScale()
    {
        int tokenCount = Tokens.Count;

        if (tokenCount == 0) return;

        float scale = Mathf.Clamp(1f / tokenCount, 0.5f, 1f); // Scale tokens based on count

       

        for (int i = 0; i < tokenCount; i++)
        {
            Token token = Tokens[i];
            token.transform.localScale = new Vector3(scale, scale, scale); // Adjust scale
            
        }
    }
    private void AdjustTokenPositions()
    {
        int tokenCount = Tokens.Count;
        Vector2[] positions = GetTokenOffsets(tokenCount,10); // Get token offsets based on count
        for (int i = 0; i < tokenCount; i++)
        {
            Token token = Tokens[i];
            Vector2 orignalPos = token.transform.localPosition;
            token.transform.localPosition = orignalPos + positions[i]; // Adjust position
        }

    }
    private Vector2[] GetTokenOffsets(int tokenCount , float pos)
    {
        switch (tokenCount)
        {
            case 2:
                return new Vector2[] { new Vector2(pos, pos), new Vector2(-pos, pos) };
            case 3:
                return new Vector2[] { new Vector2(pos, pos), new Vector2(-pos, pos), new Vector2(-pos, -pos) };
            case 4:
                return new Vector2[] { new Vector2(pos, pos), new Vector2(-pos, pos), new Vector2(-pos, -pos), new Vector2(-pos, -pos) };
            default:
                return new Vector2[] { Vector2.zero }; // Default to center
        }
    }
}//end of Class
