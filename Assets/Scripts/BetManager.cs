using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BetManager : MonoBehaviour
{
    [SerializeField] private TMP_Text betAmounttext;
    [SerializeField] private TMP_Text totalBetAmount;
    [SerializeField] private TMP_InputField betAmount;

    private void OnEnable()
    {
        showBet();
    }
    public void showBet()
    {
        if (betAmount.gameObject.activeInHierarchy)
        {
            PhotonManager.instance.betAmount=int.Parse(betAmounttext.text);
        }
        betAmounttext.text="Entry - $" + PhotonManager.instance.betAmount.ToString();
        totalBetAmount.text = " $ "+PhotonManager.instance.totalBet.ToString();
    }

}
