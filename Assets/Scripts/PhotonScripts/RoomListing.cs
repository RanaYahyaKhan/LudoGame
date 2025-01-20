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
    
        //if (RoomInfo.CustomProperties.TryGetValue("BetAmount", out object betAmount) &&
        //RoomInfo.CustomProperties.TryGetValue("TotalBet", out object totalBet))
        //{
        //    PhotonManager.instance.JoinRoom(RoomInfo.Name, int.Parse(betAmount.ToString()), double.Parse(totalBet.ToString()));
        //}
        //else
        //{
        //    Debug.LogError("BetAmount or TotalBet properties are missing in the room info.");
        //}
        if (RoomInfo.CustomProperties.TryGetValue("BetAmount", out object betAmount) &&
        RoomInfo.CustomProperties.TryGetValue("TotalBet", out object totalBet))
        {
            // Pass the max players along with other room details
            if (RoomInfo.CustomProperties.TryGetValue("MaxPlayers", out object maxPlayers))
            {
                PhotonManager.instance.JoinRoom(RoomInfo.Name, int.Parse(betAmount.ToString()), double.Parse(totalBet.ToString()), int.Parse(maxPlayers.ToString()));
            }
            else
            {
                Debug.LogError("MaxPlayers property is missing in the room info.");
            }
        }
        else
        {
            Debug.LogError("BetAmount or TotalBet properties are missing in the room info.");
        }
    }
}
