using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BetWalletManager : MonoBehaviour
{
    [SerializeField] private Button walletBtn;
    [SerializeField] private Button withDrawBtn;
    [SerializeField] private Button addMonyBtn;
    
    [SerializeField] private Button showDepositBtn;
    [SerializeField] private Button showWithDrawBtn;

    [SerializeField] private GameObject depositActive;
    [SerializeField] private GameObject withDrawActive;

    [SerializeField] private GameObject walletScreen;
    [SerializeField] private GameObject withdrawScreen;
    [SerializeField] private GameObject depositScreen;
    [SerializeField] private GameObject addMonyScreen;

    private void Start()
    {
        walletBtn.onClick.AddListener(ShowWallet);
        addMonyBtn.onClick.AddListener(ShowAddMonyScreen);
        showDepositBtn.onClick.AddListener(ShowDepositScreen);
        showWithDrawBtn.onClick.AddListener(ShowWithDrawScreen);
    }
    void ShowWallet() 
    {
        walletScreen.SetActive(true);
    }
    void ShowDepositScreen()
    {
        depositScreen.SetActive(true);
        withdrawScreen.SetActive(false);

        depositActive.SetActive(true);
        showWithDrawBtn.gameObject.SetActive(true);
        showDepositBtn.gameObject.SetActive(false);
        withDrawActive.SetActive(false);
    }
    void ShowWithDrawScreen()
    {
        depositScreen.SetActive(false);
        withdrawScreen.SetActive(true);

        depositActive.SetActive(false);
        showWithDrawBtn.gameObject.SetActive(false);
        showDepositBtn.gameObject.SetActive(true);
        withDrawActive.SetActive(true);

    }
    void ShowAddMonyScreen()
    {
        addMonyScreen.SetActive(true);
        walletScreen.SetActive(false);
    }
}
