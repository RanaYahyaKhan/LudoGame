using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    public Button rollButton; // Reference to the Roll Dice button
    public Transform[] player1BoardPositions;
    public Transform[] player2BoardPositions;
    public Transform[] player3BoardPositions;
    public Transform[] player4BoardPositions;
    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
        DisableDiceRoll();
    }

    public void EnableDiceRoll()
    {
        
        rollButton.interactable = true; rollButton.enabled = true;
        
    }
    public void DisableDiceRoll()
    {
        rollButton.interactable = false; rollButton.enabled = false;
    }
    public void SetBoardPosition(int playerIndex)
    {
        if (playerIndex == 0)
        {
            GameManager.Instance.players[playerIndex].playerToken.boardPositions = player1BoardPositions;
        }
        if (playerIndex == 1)
        {
            GameManager.Instance.players[playerIndex].playerToken.boardPositions = player3BoardPositions;
        }
    }

}
