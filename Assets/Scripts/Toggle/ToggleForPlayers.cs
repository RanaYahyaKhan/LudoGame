using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleForPlayers : MonoBehaviour
{
    [SerializeField] private GameObject _2PlayerCheck;
    [SerializeField] private GameObject _4PlayersCheck;
    [SerializeField] private GameObject _2PlayerUnCheck;
    [SerializeField] private GameObject _4PlayersUnCheck;
    public static ToggleForPlayers instance;
    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        TwoPlayerCheck();
    }
    public void TwoPlayerCheck()
    {
        _2PlayerCheck.SetActive(true); _2PlayerUnCheck.SetActive(false);
        _4PlayersCheck.SetActive(false); _4PlayersUnCheck.SetActive(true);
    }
    public void FourPlayerCheck()
    {
        _2PlayerCheck.SetActive(false); _2PlayerUnCheck.SetActive(true);
        _4PlayersCheck.SetActive(true); _4PlayersUnCheck.SetActive(false);
    }
}
