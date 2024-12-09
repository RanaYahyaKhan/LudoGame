using Photon.Pun;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
public class LudoManager : MonoBehaviourPunCallbacks
{
    public static LudoManager Instance;
    public GameObject playerPrefab; // The prefab name in Resources folder
    public Transform[] spawnPoints;

    private void Start()
    {
        if (PhotonNetwork.IsConnected && PhotonNetwork.InRoom)
        {
            SpawnMultipalPlayer();
        }
    }

    private void SpawnMultipalPlayer()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            // Instantiate Player1 at spawnPoint1
            PhotonNetwork.Instantiate("RedPlayer", spawnPoints[0].position, Quaternion.identity);

            // Instantiate Player2 at spawnPoint2
            PhotonNetwork.Instantiate("YellowPlayer", spawnPoints[1].position, Quaternion.identity);
        }
    }

    private void SpawnPlayer()
    {
        if (spawnPoints == null || spawnPoints.Length == 0)
        {
            Debug.LogError("No spawn points found in the scene.");
            return;
        }

        // Get the actor number of the player
        int playerIndex = PhotonNetwork.LocalPlayer.ActorNumber - 1; // ActorNumber starts from 1, array index starts from 0

        // Ensure the index is within the bounds of the spawn points array
        if (playerIndex >= spawnPoints.Length)
        {
            Debug.LogWarning("Player index exceeds spawn points. Assigning to the last available spawn point.");
            playerIndex = spawnPoints.Length - 1;
        }

        // Select the spawn point based on player index
        

        Transform spawnPoint = spawnPoints[playerIndex];

        // Instantiate the player at the chosen spawn point
        GameObject player = PhotonNetwork.Instantiate(playerPrefab.name, spawnPoint.position, spawnPoint.rotation);

        // Call a method to mark the player, if applicable
        if (PhotonNetwork.LocalPlayer.ActorNumber == 1)
        {
            player.GetComponent<GotiPlayer>().ShowRedMark();
        }
        else
        {
            player.GetComponent<GotiPlayer>().ShowYellowMark();
        }

    }
    

}
