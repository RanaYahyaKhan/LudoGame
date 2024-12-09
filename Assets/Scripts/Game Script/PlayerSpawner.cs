using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    public Transform[] spawnPoints;
    public GameObject playerHolder;
    private void Start()
    {
        // Spawn the local player when joining
        SpawnPlayer();
    }

    public void SpawnPlayer()
    {
        int actorNumber = PhotonNetwork.LocalPlayer.ActorNumber; // Unique player ID in the room
        int playerIndex = actorNumber - 1; // Convert to 0-based index

        if (playerIndex < spawnPoints.Length)
        {
            // Spawn the player object at the assigned spawn point
            GameObject spawnedObject =PhotonNetwork.Instantiate("Player1", spawnPoints[playerIndex].position, Quaternion.identity);
            spawnedObject.transform.SetParent(playerHolder.transform, false);
            // Assign properties or player script as needed
            //PlayerScript playerScript = player.GetComponent<PlayerScript>();
            //playerScript.Initialize(playerIndex);

            Debug.Log($"Spawned Player {playerIndex + 1} at position {spawnPoints[playerIndex].position}");
        }
        else
        {
            Debug.LogError("Player index exceeds spawn points available.");
        }
    }
}
