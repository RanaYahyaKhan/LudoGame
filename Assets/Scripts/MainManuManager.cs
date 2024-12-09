using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class MainManuManager : MonoBehaviour
{
    [SerializeField]
    private TMP_Text pocketAmount;
    [SerializeField]
    private TMP_Text betAmountText;
    [SerializeField]
    private TMP_InputField betAmountManualText;
    [SerializeField]
    private TMP_Text totalBetAmountText;

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
       
        betAmountText.text = "Entry - $ " + PhotonManager.instance.betAmount.ToString();
        betAmountManualText.text = "$" + PhotonManager.instance.betAmount.ToString();
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
        CalculateTotalBet();

    }
    public void FourPlayers()
    {
        PhotonManager.instance.numberOfPlayers = 4;
        CalculateTotalBet();

    }
    public void BetAmout(int bet)
    {
        if (PhotonManager.instance.betAmount >=0)
        {
            PhotonManager.instance.betAmount += bet;
            Debug.Log(PhotonManager.instance.betAmount);
        }
        betAmountText.text = "Entry - $ " + PhotonManager.instance.betAmount.ToString();
        betAmountManualText.text = "$"+PhotonManager.instance.betAmount.ToString();
        CalculateTotalBet();
    }
    private void CalculateTotalBet()
    {
        double temp = PhotonManager.instance.betAmount * PhotonManager.instance.numberOfPlayers * 0.70;
        PhotonManager.instance.totalBet = temp;
        totalBetAmountText.text = temp.ToString();


    }
}
