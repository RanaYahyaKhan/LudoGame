using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    [Header("Player Names")]
    [SerializeField] private List<TMP_Text> playersName =new List<TMP_Text>();
    private List<string> playersNames = new List<string>();
    [Header("Player")]
    [SerializeField] private GameObject player3;
    [SerializeField] private GameObject player4;

    [SerializeField] private List<GameObject> fourPlayers = new List<GameObject>();

    [SerializeField] private TMP_Text[] playersInMatch;
    [SerializeField] private TMP_Text[] playersInMatchfor2;


    private void Start()
    {
        
        if(PhotonNetwork.CurrentRoom.PlayerCount == 2)
        {
            player3.SetActive(false);
            player4.SetActive(false);
            fourPlayers[0].SetActive(true);
            if (PhotonNetwork.LocalPlayer.ActorNumber == 1)
            {
                playersName[0].text= PhotonManager.instance.playerNames[0];
                playersName[1].text= PhotonManager.instance.playerNames[1];
                playersInMatchfor2[0].text = PhotonManager.instance.playerNames[0];
                playersInMatchfor2[1].text = PhotonManager.instance.playerNames[1];
            }
            else
            {
                playersName[0].text = PhotonManager.instance.playerNames[1];
                playersName[1].text = PhotonManager.instance.playerNames[0];
                playersInMatchfor2[0].text = PhotonManager.instance.playerNames[1];
                playersInMatchfor2[1].text = PhotonManager.instance.playerNames[0];
            }
        }
        if (PhotonNetwork.CurrentRoom.PlayerCount == 4)
        {
            player3.SetActive(true);
            player4.SetActive(true);
            fourPlayers[0].SetActive(true);
            fourPlayers[1].SetActive(true);
            InitializeList();
            UpdatePlayerList();
            
        }
    }
    // Initialzie for 4 Players
    public void InitializeList()
    {
        // Ensure the lists have the same length
        //int count = Mathf.Min(playersName.Count, PhotonManager.instance.playerNames.Count);
        //Debug.Log(count + playersName.Count + PhotonManager.instance.playerNames.Count);
        foreach (var item in PhotonManager.instance.playerNames)
        {
            playersNames.Add(item);
        }

        for (int i = 0; i < PhotonManager.instance.numberOfPlayers; i++)
        {
            if (playersName[i] != null) // Check if TextMesh exists
            {
                playersName[i].text = PhotonManager.instance.playerNames[i];
                playersInMatch[i].text= PhotonManager.instance.playerNames[i];
            }
        }
    }
    private void UpdatePlayerList()
    {
        if (PhotonManager.instance.playerNames != null)
        {
            for (int i = 0; i < PhotonNetwork.LocalPlayer.ActorNumber - 1; i++)
            {
                int firstPlayer = 0;
                Debug.Log("Swaping the Players: " + i);
                string tempPlayer = playersNames[firstPlayer];
                playersNames.RemoveAt(firstPlayer);
                playersNames.Add(tempPlayer);
            }
        }
        for (int i = 0; i < playersNames.Count; i++)
        {
            playersName[i].text = playersNames[i];
            playersInMatch[i].text = playersNames[i];
        }
      
    }


}
