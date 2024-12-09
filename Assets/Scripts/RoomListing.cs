using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class RoomListing : MonoBehaviour
{
    [SerializeField]
    private TMP_Text playrerText;
    [SerializeField]
    private TMP_Text betAmountText;
    [SerializeField]
    private TMP_Text totalBetText;
    [SerializeField]
    private Button joinButton;

    public RoomInfo RoomInfo {  get; private set; }

    public void SetRoomInfo(RoomInfo roomInfo)
    {
        RoomInfo=roomInfo;
        playrerText.text = roomInfo.MaxPlayers + " Players";
        betAmountText.text = roomInfo.CustomProperties["BetAmount"].ToString();
        totalBetText.text = roomInfo.CustomProperties["TotalBet"].ToString();
    }
    public void JoinRoomClick()
    {
        PhotonManager.instance.JoinRoom(RoomInfo.Name
      //      ,int.Parse(betAmountText.text),
      //double.Parse(totalBetText.text)
      );
    }
}
