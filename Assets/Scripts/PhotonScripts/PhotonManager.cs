using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Pun.Demo.PunBasics;
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
    public int numberOfPlayers = 2;
    public int betAmount = 10;
    public double totalBet;
    public TMP_InputField roomNameInputField; // Input field for room name
    //Room listing variables
    [Header("RoomListings")]
    [SerializeField] private Transform _content;
    [SerializeField] private RoomListing _roomListing;
    private List<RoomListing> _roomListingList = new List<RoomListing>();
    //input field for room create
    [Header("RoomCreate")]
    [SerializeField] private TMP_InputField roomNameInputFieldCreate;

    [Header("RoomJoinPannel")]
    [SerializeField] private TMP_Text waitingTex;

    [SerializeField] private GameObject gamePlayButtons;
    [SerializeField] private GameObject loadingScreen;


    private const byte StartGameNow = 99;
    [SerializeField] private TMP_InputField playerName;
    [SerializeField] private GameObject signInPannel;
    [Header("Lobby Screen")]
    public List<string> playerNames = new List<string>();
    [SerializeField] private List<TMP_Text> playersName = new List<TMP_Text>();

    [SerializeField] private GameObject lobbyPannel;

 

  


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
        }
        signInPannel.SetActive(false);

    }
    public void SignInSocial(string name)
    {
        PhotonNetwork.NickName = name;

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
            CustomRoomProperties = new ExitGames.Client.Photon.Hashtable { { "MaxPlayers", maxPlayers },{ "BetAmount", betAmount },{"TotalBet" ,totalBet} },
            CustomRoomPropertiesForLobby = new string[] { "MaxPlayers", "BetAmount" ,"TotalBet"}

        };

        // Create a room with the given name and options
        PhotonNetwork.CreateRoom(roomName, roomOptions, TypedLobby.Default);
    }
    [Header("Friends Room Name")]
    [SerializeField] private TMP_InputField roomNameForFriends;
    [SerializeField] private GameObject friendsPannel;
    public void CreateRoomForFriends()
    {
        friendsPannel.SetActive(true);
        string roomName = playerName.text + Random.Range(1000, 9999).ToString();
        roomNameForFriends.text = roomName;
        int maxPlayers = numberOfPlayers;
        Debug.Log(roomName + " and " + maxPlayers + " Bet Amount " + betAmount + "Toatl Bet" + totalBet);
        // Configure room options
        RoomOptions roomOptions = new RoomOptions
        {
            MaxPlayers = (byte)maxPlayers,
            CustomRoomProperties = new ExitGames.Client.Photon.Hashtable { { "MaxPlayers", maxPlayers }, { "BetAmount", betAmount }, { "TotalBet", totalBet } },
            CustomRoomPropertiesForLobby = new string[] { "MaxPlayers", "BetAmount", "TotalBet" }

        };

        // Create a room with the given name and options
        PhotonNetwork.CreateRoom(roomName, roomOptions, TypedLobby.Default);
    }
    private const string BetKey = "BetAmount";
    private const string TotalBetKey = "TotalBet";
    private const string maxPlayer = "MaxPlayers";
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
            CustomRoomProperties = new ExitGames.Client.Photon.Hashtable { { maxPlayer, maxPlayers },{ BetKey, betAmount }, { TotalBetKey, totalBet } },
            CustomRoomPropertiesForLobby = new string[] { maxPlayer,BetKey, TotalBetKey }

        };

        // Create a room with the given name and options
        PhotonNetwork.CreateRoom(roomName, roomOptions, TypedLobby.Default);
    }
    public void JoinRoom(string roomName,int betA, double totlBetA,int Maxplayers)
    {
        PhotonNetwork.JoinRoom(roomName);
        roomNameInputField.text = roomName;
        betAmount = betA;
        totalBet = totlBetA;
        numberOfPlayers = Maxplayers;
      

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
        loadingScreen.SetActive(false);
        

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
    [SerializeField]
    private GameObject playerPrefab;

    [SerializeField]
    private Transform container; // Assign this in the Inspector (e.g., a UI Panel in the Canvas)

    //public List<GameObject> instanciatedPlayers = new List<GameObject>();

    public List<GameObject> instanciatedPlayerInLobby = new List<GameObject>();
    private void InstantiatePlayer()
    {
        if (playerPrefab == null || container == null)
        {
            Debug.LogError("PlayerPrefab or Container is not set.");
            return;
        }

        // Instantiate the player as a child of the container
        GameObject playerUI = PhotonNetwork.Instantiate(playerPrefab.name, Vector3.zero, Quaternion.identity);

        // Set the instantiated UI object's parent to the container
        playerUI.transform.SetParent(container, false); // 'false' keeps the RectTransform values consistent

        // Optionally, initialize the UI
       // InitializePlayerUI(playerUI);
    }

    private void InitializePlayerUI(GameObject playerUI)
    {
        // Example: Set player name
        var playerNameText = playerUI.GetComponentInChildren<TMPro.TMP_Text>();
        if (playerNameText != null)
        {
            playerNameText.text = PhotonNetwork.NickName;
        }
    }
    private void SetParent()
    {
        for (int i = 0; i < instanciatedPlayerInLobby.Count; i++)
        {
            instanciatedPlayerInLobby[i].transform.SetParent(container, false);
            instanciatedPlayerInLobby[i].GetComponent<PlayerInLobby>().playerName.text = playerNames[i];
        }
    }
    // Called when successfully joined a room
    public override void OnJoinedRoom()
    {
        if (PhotonNetwork.CurrentRoom.CustomProperties.TryGetValue(BetKey, out object betAmount))
        {
            Debug.Log(BetKey + " And " + betAmount);
        }

        playerNames.Clear();
        InstantiatePlayer();
        foreach (Player player in PhotonNetwork.PlayerList)
        {
            playerNames.Add(player.NickName);
        }
        if (!friendsPannel.activeInHierarchy)
        {
            lobbyPannel.SetActive(true); //lobby pannel set active
            StartCoroutine(AnimateDots("Waiting for the player to join"));
        }
        
    }
   

    private IEnumerator AnimateDots(string str)
    {
        int dotCount = 0;

        while (true)
        {
            dotCount = (dotCount % 5) + 1; // Cycle through 1, 2, 3
            waitingTex.text =  str + new string('.', dotCount);
            SetParent();//set the parent of instanciated obj
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
        playerNames.Add(newPlayer.NickName);
        UpdatePlayerName(newPlayer.NickName);
        currentRoomPlayer = PhotonNetwork.CurrentRoom.PlayerCount;
        if (PhotonNetwork.CurrentRoom.PlayerCount == PhotonNetwork.CurrentRoom.MaxPlayers)
        {
            PhotonNetwork.CurrentRoom.IsOpen = false;
            PhotonNetwork.CurrentRoom.IsVisible = false;
            StartGame();
        }
    }
    public void UpdatePlayerName(string playerName)
    {
        foreach (var player in playersName)
        {
            player.text = playerName; // Update with the player's name
        }
    }
    public void LeaveRoom()
    {
        if (PhotonNetwork.InRoom)
        {
            PhotonNetwork.LeaveRoom();

            Debug.Log("Player is leaving the room...");
        }
        else
        {
            Debug.LogWarning("Player is not in any room to leave.");
        }
    }
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        Debug.Log($"Player left the room: {otherPlayer.NickName}");
        playerNames.Remove(otherPlayer.NickName);

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
            await Task.Delay(8000);
            PhotonNetwork.LoadLevel("LudoGame");
            //PhotonNetwork.LoadLevel("LudoGamePlay");
        }
    }
    // Called when another player leaves the room
  


}
