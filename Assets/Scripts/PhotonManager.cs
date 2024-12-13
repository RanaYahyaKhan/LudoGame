using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
//using static UnityEditor.Timeline.TimelinePlaybackControls;
public class PhotonManager : MonoBehaviourPunCallbacks
{
    public static PhotonManager instance;
    public int numberOfPlayers=2;
    public int betAmount=10;
    public double totalBet;
    public TMP_InputField roomNameInputField; // Input field for room name

    //Room listing variables
    [Header("RoomListings")]
    [SerializeField]
    private Transform _content;
    [SerializeField]
    private RoomListing _roomListing;
    private List<RoomListing> _roomListingList = new List<RoomListing>();
    //input field for room create
    [Header("RoomCreate")]
    [SerializeField] private TMP_InputField roomNameInputFieldCreate;

    [Header("RoomJoinPannel")]
    [SerializeField] private GameObject lobbyPannel;
    [SerializeField] private TMP_Text waitingTex;

    [SerializeField] private GameObject gamePlayButtons;
   
    
    private const byte StartGameNow = 99;
    [SerializeField] private TMP_InputField playerName;
    [SerializeField] private GameObject signInPannel;

    private void Awake()
    {
        instance = this;
        betAmount = 10;
        totalBet = betAmount * numberOfPlayers * 0.70;
        PhotonNetwork.NetworkingClient.EventReceived += PhotonManager_OnPlayerEnterRoom;

    }



    private void Start()
    {
        // Ensure we're connected to Photon
        if (!PhotonNetwork.IsConnected)
        {
            PhotonNetwork.ConnectUsingSettings(); // Connect to Photon master server
            PhotonNetwork.AutomaticallySyncScene = true;
            
                Debug.Log("Connected");
        }
    }
    public void signInButton()
    {
        if (!string.IsNullOrEmpty(playerName.text))
        {
            PhotonNetwork.NickName = playerName.text;
            PhotonDataManager.Instance.onlinePlayer.Add(playerName.text);
        }
        signInPannel.SetActive(false);

    }
    #region PhotonRoom Creating and Joining;
    public void CreateRoom()
    {
        // Get the room name and max players from UI
        string roomName = playerName.text + Random.Range(1000, 9999).ToString();
        roomNameInputField.text=roomName;
        int maxPlayers = numberOfPlayers;
        Debug.Log(roomName + " and " + maxPlayers + " Bet Amount "+betAmount +"Toatl Bet"+totalBet);
        // Configure room options
        RoomOptions roomOptions = new RoomOptions
        {
            MaxPlayers = (byte)maxPlayers,
            CustomRoomProperties = new ExitGames.Client.Photon.Hashtable { { "BetAmount", betAmount },{"TotalBet" ,totalBet} },
            CustomRoomPropertiesForLobby = new string[] { "BetAmount" ,"TotalBet"}

        };

        // Create a room with the given name and options
        PhotonNetwork.CreateRoom(roomName, roomOptions, TypedLobby.Default);
    }
    private const string BetKey = "BetValue";
    private const string TotalBetKey = "TotalBetValue";
    public void CreateRoomWithCode()
    {
        string roomName = roomNameInputFieldCreate.text;
        int maxPlayers = numberOfPlayers;
        //betAmount = int.Parse(betAmountCreateRoomteRoom);
        Debug.Log(roomName + " and " + maxPlayers + " Bet Amount " + betAmount + "Toatl Bet" + totalBet);
        
        // Configure room options
        RoomOptions roomOptions = new RoomOptions
        {
            MaxPlayers = (byte)maxPlayers,
            CustomRoomProperties = new ExitGames.Client.Photon.Hashtable { { BetKey, betAmount }, { BetKey, totalBet } },
            CustomRoomPropertiesForLobby = new string[] { BetKey, BetKey }

        };

        // Create a room with the given name and options
        PhotonNetwork.CreateRoom(roomName, roomOptions, TypedLobby.Default);
    }
    public void JoinRoom(string roomName/*, int betA, double totlBetA*/)
    {
        PhotonNetwork.JoinRoom(roomName);
        roomNameInputField.text = roomName;
        //betAmount = betA;
        //totalBet = totlBetA;

    }
    public void JoinRoomWithCode()
    {
        // Attempt to join a room with the specified name
        string roomName = roomNameInputField.text;
        PhotonNetwork.JoinRoom(roomName);
    }
    //Update the room list on creating room
    public override void OnRoomListUpdate(List<RoomInfo> roomlist)
    {
        foreach (RoomInfo info in roomlist)
        {
            // Check if the room is removed or full
            if (info.RemovedFromList || info.PlayerCount >= info.MaxPlayers)
            {
                // Find the existing entry for this room
                int index = _roomListingList.FindIndex(x => x.RoomInfo.Name == info.Name);
                if (index != -1)
                {
                    // Destroy the UI entry and remove it from the list
                    Destroy(_roomListingList[index].gameObject);
                    _roomListingList.RemoveAt(index);
                }
            }
            else
            {
                // Check if this room already exists in the UI list
                RoomListing existingListing = _roomListingList.Find(x => x.RoomInfo.Name == info.Name);
                if (existingListing != null)
                {
                    // Update the existing room's information
                    existingListing.SetRoomInfo(info);
                }
                else
                {
                    // Create a new UI entry for the new room
                    RoomListing listing = Instantiate(_roomListing, _content);
                    if (listing != null)
                    {
                        listing.SetRoomInfo(info);
                        _roomListingList.Add(listing); // Add it to the tracking list
                    }
                }
            }
        }

    }
 
    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinLobby(); // Join the default lobby after connecting
        gamePlayButtons.SetActive(true);
        Debug.Log("Connected to Master and joined lobby.");

    }
    public override void OnJoinedLobby()
    {
        Debug.Log("Joined Lobby. Waiting for room list updates...");
    }

    // Called when the room creation is successful
    public override void OnCreatedRoom()
    {
        Debug.Log($"Room created successfully: {PhotonNetwork.CurrentRoom.Name}");
    }

    // Called when room creation fails
    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.LogWarning($"Room creation failed: {message}");
    }

    // Called when successfully joined a room
    public override void OnJoinedRoom()
    {
        if (PhotonNetwork.CurrentRoom.CustomProperties.TryGetValue(BetKey, out object betAmount))
        {
            Debug.Log(BetKey + " And " + betAmount);
        }
        lobbyPannel.SetActive(true); //lobby pannel set active
        StartCoroutine(AnimateDots("Waiting for the player to join"));
        
    }
    private IEnumerator AnimateDots(string str)
    {
        int dotCount = 0;

        while (true)
        {
            dotCount = (dotCount % 5) + 1; // Cycle through 1, 2, 3
            waitingTex.text =  str + new string('.', dotCount);
            yield return new WaitForSeconds(0.5f); // Adjust the speed as needed
        }
    }
    // Called when joining a room fails
    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        Debug.LogWarning($"Failed to join room: {message}");
    }
    [SerializeField] private int currentRoomPlayer;
    // Called when another player joins the room
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {

        Debug.Log($"Player joined the room: {newPlayer.NickName}");
        PhotonDataManager.Instance.onlinePlayer.Add(newPlayer.NickName);
        currentRoomPlayer = PhotonNetwork.CurrentRoom.PlayerCount;
        if (PhotonNetwork.CurrentRoom.PlayerCount == PhotonNetwork.CurrentRoom.MaxPlayers)
        {
            PhotonNetwork.CurrentRoom.IsOpen = false;
            PhotonNetwork.CurrentRoom.IsVisible = false;
            StartGame();
        }
    }
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        Debug.Log($"Player left the room: {otherPlayer.NickName}");
    }
    #endregion


    private void PhotonManager_OnPlayerEnterRoom(EventData obj)
    {
        if (obj.Code == StartGameNow)
        {
            
            StartGame();
        }
    }
    private void OnDestroy()
    {
        PhotonNetwork.NetworkingClient.EventReceived -= PhotonManager_OnPlayerEnterRoom;
    }
    
    private void CheckIfAllPlayersReady()
    {
        foreach (var player in PhotonNetwork.PlayerList)
        {
            if (!player.CustomProperties.ContainsKey("IsReady") || !(bool)player.CustomProperties["IsReady"])
            {
                Debug.Log($"Player {player.NickName} is not ready.");
                return;
            }
        }

        Debug.Log("All players are ready! Starting the game...");
        PhotonNetwork.LoadLevel("LudoGame");
        //PhotonNetwork.LoadLevel("LudoGamePlay");
    }
    private async void StartGame()
    {
        if (PhotonNetwork.IsMasterClient) // Only the Master Client can initiate scene loading
        {
            Debug.Log("Room is full. Starting the game...");
            await Task.Delay(4000);
            PhotonNetwork.LoadLevel("LudoGame");
            //PhotonNetwork.LoadLevel("LudoGamePlay");
        }
    }
    // Called when another player leaves the room
  


}
