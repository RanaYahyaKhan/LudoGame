using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class MainManuManager : MonoBehaviour
{
    

    [Header("UIScreens")]
    [SerializeField]
    private GameObject ManualPannel;
    [SerializeField]
    private GameObject homeScreen;
    [SerializeField]
    private GameObject AutoPlayScreen;
    [SerializeField]
    private GameObject lobbyScreen;
    [SerializeField]
    private GameObject playWithFriends;

    private void Start()
    {
        ManualPannel.SetActive(false);
        lobbyScreen.SetActive(false);
    }
    public void PlayOnline()
    {
        AutoPlay();
        TwoPlayers();
        homeScreen.SetActive(false);
    }
    public void PlayWithFriends()
    {
        playWithFriends.SetActive(true);
        homeScreen.SetActive(true);
        AutoPlayScreen.SetActive(false);


    }
    public void AutoPlay()
    {
        ManualPannel.gameObject.SetActive(false);
        AutoPlayScreen.SetActive(true);
        homeScreen.SetActive(false);
        ToggleScript.Instance.AutoChecked();
        TwoPlayers();
    }
    public void ManualPlay()
    {
        ManualPannel.gameObject.SetActive(true);
        homeScreen.SetActive(true);
        AutoPlayScreen.SetActive(false);
        ToggleScript.Instance.ManualChecked();
        TwoPlayers();
    }
    public void TwoPlayers()
    {
        PhotonManager.instance.numberOfPlayers = 2;
        ToggleForPlayers.instance.TwoPlayerCheck();
        BetManager.instance.CalculateTotalBet();

    }
    public void FourPlayers()
    {
        PhotonManager.instance.numberOfPlayers = 4;
        BetManager.instance.CalculateTotalBet();

    }


}
