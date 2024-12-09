using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileManager : MonoBehaviour
{
    public List<GameObject> Tokens = new List<GameObject>();
    public bool isProtected;

    public void AddToken(GameObject token)
    {
        Tokens.Add(token);
    }
    public void RemoveToken(GameObject token)
    {
        Tokens.Remove(token);
    }
}
