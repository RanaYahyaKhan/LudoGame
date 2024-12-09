using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class TurnManager : MonoBehaviourPunCallbacks
{
    // Current turn player index
    private const string CurrentTurnKey = "CurrentTurn";
    

    // Start the turn system
    public void StartTurnSystem()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            // Master client initializes the turn system
            ExitGames.Client.Photon.Hashtable turnData = new ExitGames.Client.Photon.Hashtable();
            turnData[CurrentTurnKey] = 0; // First player (index 0)
            PhotonNetwork.CurrentRoom.SetCustomProperties(turnData);
        }
    }

    // Get the current turn player index
    public int GetCurrentTurn()
    {
        if (PhotonNetwork.CurrentRoom.CustomProperties.ContainsKey(CurrentTurnKey))
        {
            return (int)PhotonNetwork.CurrentRoom.CustomProperties[CurrentTurnKey];
        }
        return -1;
    }

    // Advance to the next turn
    public void NextTurn()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            int currentTurn = GetCurrentTurn();
            int nextTurn = (currentTurn + 1) % PhotonNetwork.CurrentRoom.PlayerCount;

            // Update turn in room properties
            ExitGames.Client.Photon.Hashtable turnData = new ExitGames.Client.Photon.Hashtable();
            turnData[CurrentTurnKey] = nextTurn;
            PhotonNetwork.CurrentRoom.SetCustomProperties(turnData);
        }
    }

    // Check if it's the local player's turn
    public bool IsMyTurn()
    {
        return GetCurrentTurn() == PhotonNetwork.LocalPlayer.ActorNumber - 1;
    }

    public override void OnRoomPropertiesUpdate(ExitGames.Client.Photon.Hashtable propertiesThatChanged)
    {
        if (propertiesThatChanged.ContainsKey(CurrentTurnKey))
        {
            int currentTurn = GetCurrentTurn();
            Debug.Log($"It's now Player {currentTurn}'s turn.");

            // Perform actions if it's the local player's turn
            if (IsMyTurn())
            {
                Debug.Log("It's my turn!");
                UIManager.Instance.EnableDiceRoll();

                // Enable player's turn-based actions here
            }
            else
            {
                // Disable player's actions
                UIManager.Instance.DisableDiceRoll();

            }
        }
    }
}
