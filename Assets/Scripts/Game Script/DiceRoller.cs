using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Photon.Pun;

public class DiceRoller : MonoBehaviourPun
{
    public static DiceRoller instance;
    [Header("Dice Settings")]
    [SerializeField]
    private Sprite[] diceFaces; // Array of dice face sprites (must have 6 sprites for a standard die)
    [SerializeField]
    private Image diceImage;    // UI Image to display the dice
    public int diceValue;
    public bool diceRolled = false;


    private float rollDuration = 2.0f; // Duration of the dice rolling animation
    private float rollSpeed = 0.1f;    // Speed of dice face switching during rolling
    private int selectedFace; // The index of the final dice face
    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        if (diceFaces.Length != 6)
        {
            Debug.LogError("Dice faces array must contain 6 sprites.");
        }

        if (diceImage == null)
        {
            Debug.LogError("Dice Image is not assigned.");
        }
    }
   
    public void RollDice()
    {
        if (PhotonNetwork.IsConnected)
        {
            // MasterClient generates the random dice value
            diceRolled = true;
            int result = Random.Range(0, diceFaces.Length);
            photonView.RPC("ShowDice", RpcTarget.All, result);
        }
       

    }

    [PunRPC]
    private void ShowDice(int finalFace)
    {
        StartCoroutine(RollAnimation(finalFace));
    }

    private IEnumerator RollAnimation(int finalFace)
    {
        float elapsedTime = 0f;
        while (elapsedTime < rollDuration)
        {
            // Pick a random dice face and update the dice image
            int randomIndex = Random.Range(0, diceFaces.Length);
            diceImage.sprite = diceFaces[randomIndex];

            // Wait for the roll speed duration
            yield return new WaitForSeconds(rollSpeed);

            elapsedTime += rollSpeed;
        }

        // Stop on the final face received via RPC
        selectedFace = finalFace;
        diceImage.sprite = diceFaces[selectedFace];
        diceValue = selectedFace + 1;
        Debug.Log($"Dice roll finished. Final face: {diceValue}");
        GameManager.Instance.currentPlayer.StartTurn();    //0 for first player of every scene



        //Invoke("ChangeTurn", 1f);
    }

    private void ChangeTurn()
    {

    }

}//end of class
