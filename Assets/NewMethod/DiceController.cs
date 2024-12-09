using Photon.Pun;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class DiceController : MonoBehaviourPun
{
    [Header("Dice Settings")]
    [SerializeField]
    private Sprite[] diceFaces; // Array of dice face sprites (must have 6 sprites for a standard die)
    [SerializeField]
    private Sprite diceImage;    // UI Image to display the dice
    public int diceValue;
    public bool diceRolled = false;


    private float rollDuration = 2.0f; // Duration of the dice rolling animation
    private float rollSpeed = 0.1f;    // Speed of dice face switching during rolling
    private int selectedFace; // The index of the final dice face

    private SpriteRenderer spriteRenderer;

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
        // Get the SpriteRenderer component attached to this object
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (spriteRenderer == null)
        {
            Debug.LogError("No SpriteRenderer found on this GameObject.");
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
            spriteRenderer.sprite = diceFaces[randomIndex];

            // Wait for the roll speed duration
            yield return new WaitForSeconds(rollSpeed);

            elapsedTime += rollSpeed;
        }

        // Stop on the final face received via RPC
        selectedFace = finalFace;
        spriteRenderer.sprite = diceFaces[selectedFace];
        diceValue = selectedFace + 1;
        //GameManager.Instance.players.MoveToken();
        //GameManager.Instance.players[GameManager.Instance.currentPlayerIndex].MoveToken();
        //GameManager.Instance.players[GameManager.Instance.currentPlayerIndex].tkn.Move(diceValue);
        Debug.Log($"Dice roll finished. Final face: {diceValue}");

        //Invoke("ChangeTurn", 3f);
    }

    private void ChangeTurn()
    {
        GameManager.Instance.EndTurn();
    }
}
