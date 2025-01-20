using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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
        if (betAmountInput.gameObject.activeInHierarchy)
        {
            betAmountInput.onValueChanged.AddListener(OnValueChanged);
            betAmountInput.onEndEdit.AddListener(OnEndEdit);
        }
    }
    void OnValueChanged(string value)
    {
        Debug.Log("Value Changed: " + value);
        PhotonManager.instance.betAmount = int.Parse(value);
    }

    // Called when the user finishes editing (e.g., presses Enter or moves focus away)
    void OnEndEdit(string value)
    {
        Debug.Log("Editing Ended: " + value);
        PhotonManager.instance.betAmount = int.Parse(value);
    }
    public void showBet()
    {
        if (betAmountInput.gameObject.activeInHierarchy)
        {
            betAmountInput.text = "$ " + PhotonManager.instance.betAmount.ToString();
            //PhotonManager.instance.betAmount=int.Parse(betAmounttext.text);
        }
        if (betAmounttext.gameObject.activeInHierarchy)
        {
            betAmountInput.text = "Entry - $" + PhotonManager.instance.betAmount.ToString();
            totalBetAmount.text = " $ " + PhotonManager.instance.totalBet.ToString();

        }
        //betAmounttext.text="Entry - $" + PhotonManager.instance.betAmount.ToString();
        //totalBetAmount.text = " $ " + PhotonManager.instance.totalBet.ToString();
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
