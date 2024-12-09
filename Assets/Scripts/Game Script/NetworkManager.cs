using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    //public static NetworkManager Instance;

    //private void Awake()
    //{
    //    if (Instance == null) Instance = this;
    //    else Destroy(gameObject);
    //}

    //private void Start()
    //{
    //    PhotonNetwork.ConnectUsingSettings(); // Connect to Photon
    //}

    //public override void OnConnectedToMaster()
    //{
    //    Debug.Log("Connected to Photon!");
    //    PhotonNetwork.JoinOrCreateRoom("LudoRoom", new RoomOptions { MaxPlayers = 4 }, null);
    //}

    //public override void OnJoinedRoom()
    //{
    //    Debug.Log("Joined Room!");
    //}
}
