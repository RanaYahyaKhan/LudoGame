using Photon.Pun;
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
        if (PhotonNetwork.CurrentRoom.PlayerCount == 2)
        {
            if (playerIndex == 0)
            {
                for (int i = 0; i < 4; i++)
                {
                    GameManager.Instance.players[playerIndex].playerToken[i].boardPositions = player1BoardPositions;
                }

            }
            if (playerIndex == 1)
            {
                for (int i = 0; i < 4; i++)
                {
                    GameManager.Instance.players[playerIndex].playerToken[i].boardPositions = player3BoardPositions;
                }
            }
        }
        if (PhotonNetwork.CurrentRoom.PlayerCount == 4)
        {
            if (playerIndex == 0)
            {
                for (int i = 0; i < 4; i++)
                {
                    GameManager.Instance.players[playerIndex].playerToken[i].boardPositions = player1BoardPositions;
                }

            }
            if (playerIndex == 1)
            {
                for (int i = 0; i < 4; i++)
                {
                    GameManager.Instance.players[playerIndex].playerToken[i].boardPositions = player2BoardPositions;
                }
            }
            if (playerIndex == 2)
            {
                for (int i = 0; i < 4; i++)
                {
                    GameManager.Instance.players[playerIndex].playerToken[i].boardPositions = player3BoardPositions;
                }
            }
            if (playerIndex == 3)
            {
                for (int i = 0; i < 4; i++)
                {
                    GameManager.Instance.players[playerIndex].playerToken[i].boardPositions = player4BoardPositions;
                }
            }
        }
    }
    public void StopScaling()
    {
        if(PhotonNetwork.CurrentRoom.PlayerCount == 2)
        {
            for (int i = 0; i < 4; i++)
            {
                GameManager.Instance.players[0].playerToken[i].GetComponent<TokenBlinker>().StopScaling();
                GameManager.Instance.players[1].playerToken[i].GetComponent<TokenBlinker>().StopScaling();
            }
        }
        if (PhotonNetwork.CurrentRoom.PlayerCount == 4)
        {
            for (int i = 0; i < 4; i++)
            {
                GameManager.Instance.players[0].playerToken[i].GetComponent<TokenBlinker>().StopScaling();
                GameManager.Instance.players[1].playerToken[i].GetComponent<TokenBlinker>().StopScaling();
                GameManager.Instance.players[2].playerToken[i].GetComponent<TokenBlinker>().StopScaling();
                GameManager.Instance.players[3].playerToken[i].GetComponent<TokenBlinker>().StopScaling();
            }
        }

    }



}//end of class

