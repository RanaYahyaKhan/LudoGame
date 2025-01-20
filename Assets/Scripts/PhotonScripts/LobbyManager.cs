using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using TMPro;
using UnityEngine;

public class LobbyManager : MonoBehaviour
{
    
    
    [SerializeField] private GameObject player3;
    [SerializeField] private GameObject player4;


    [SerializeField] private TMP_Text myName;
    [SerializeField] private TMP_Text player2Name;
    [SerializeField] private TMP_Text player3Name;
    [SerializeField] private TMP_Text player4Name;
    private void Start()
    {
        if (PhotonManager.instance.numberOfPlayers == 2)
        {
            player4.SetActive(false);
            player3.SetActive(false);
            Show2Players();


        }
        if (PhotonManager.instance.numberOfPlayers == 4)
        {
            player4.SetActive(true);
            player3.SetActive(true);
            Show4Players();
        }

    }

    private void Show2Players()
    {
        if (PhotonNetwork.LocalPlayer.ActorNumber == 1)
        {
            myName.text = PhotonManager.instance.playerNames[0];
        }
        if (PhotonNetwork.LocalPlayer.ActorNumber == 2)
        {
            player2Name.text = PhotonManager.instance.playerNames[1];
        }
    }
    private void Show4Players()
    {
        if (PhotonNetwork.LocalPlayer.ActorNumber == 1)
        {
            myName.text = PhotonManager.instance.playerNames[0];
        }
        if (PhotonNetwork.LocalPlayer.ActorNumber == 2)
        {
            player2Name.text = PhotonManager.instance.playerNames[1];
        }
        if (PhotonNetwork.LocalPlayer.ActorNumber == 3)
        {
            player3Name.text = PhotonManager.instance.playerNames[2];
        }
        if (PhotonNetwork.LocalPlayer.ActorNumber == 4)
        {
            player4Name.text = PhotonManager.instance.playerNames[3];
        }
       
    }





}
