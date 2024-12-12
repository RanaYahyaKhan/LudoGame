using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BetManager : MonoBehaviour
{
    [SerializeField] private TMP_Text betAmounttext;
    [SerializeField] private TMP_Text totalBetAmount;
    [SerializeField] private TMP_InputField betAmountInput;

    public static BetManager instance;
    private void Awake()
    {
        if (instance == null) { instance = this; }
    }
    private void OnEnable()
    {
        showBet();
    }
    public void showBet()
    {
        if (betAmountInput.gameObject.activeInHierarchy)
        {
            PhotonManager.instance.betAmount=int.Parse(betAmounttext.text);
        }
        if (betAmountInput.gameObject.activeInHierarchy)
        {
            betAmountInput.text = "Entry - $" + PhotonManager.instance.betAmount.ToString();
        }
        betAmounttext.text="Entry - $" + PhotonManager.instance.betAmount.ToString();
        totalBetAmount.text = " $ "+PhotonManager.instance.totalBet.ToString();
    }
    public void BetAmout(int bet)
    {
        if (PhotonManager.instance.betAmount >= 0)
        {
            PhotonManager.instance.betAmount += bet;
            Debug.Log(PhotonManager.instance.betAmount);
        }
        betAmounttext.text = "Entry - $ " + PhotonManager.instance.betAmount.ToString();
        betAmountInput.text = "$" + PhotonManager.instance.betAmount.ToString();
        CalculateTotalBet();
    }
    public void CalculateTotalBet()
    {
        double temp = PhotonManager.instance.betAmount * PhotonManager.instance.numberOfPlayers * 0.70;
        PhotonManager.instance.totalBet = temp;
        totalBetAmount.text = temp.ToString();


    }
}
