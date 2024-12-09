using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhotonDataManager : MonoBehaviour
{
    public static PhotonDataManager Instance;
    public List<string> onlinePlayer = new List<string>();
    public int betAmount;
    public double totalBet;
    private void Awake()
    {
        if (Instance == null)
          Instance = this;
    }

}
