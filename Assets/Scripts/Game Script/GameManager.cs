using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
public class GameManager : MonoBehaviourPunCallbacks
{
    public static GameManager Instance;
    [SerializeField] private GameObject matchPannel;
    public int currentPlayerIndex = 0; // Tracks the current player's turn
   public List<PhotonPlayer> players =new List<PhotonPlayer>();

    public List<GameObject> instanciatedPlayers = new List<GameObject>();
    public GameObject playerPrefab; // The prefab name in Resources folder
    public Transform[] spawnPoints;//ther point of red green yellow and blue house in game

    public PhotonPlayer currentPlayer;


    public float playerTime = 5.0f; // player time in seconds
    public bool startGame = false;
    public bool stopTimer = false;
    public bool autoPlay = false;
    [SerializeField] private TMP_Text numberOfPlayers;


    [Header("Winner Screen")]
    [SerializeField] private GameObject WininScreen; //refrence of wining Screen
    [SerializeField] private TMP_Text winneName;
    [SerializeField] private TMP_Text winnerAmount;
    [SerializeField] private TMP_Text loserName;


    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        PhotonNetwork.AutomaticallySyncScene = true;

        if (PhotonNetwork.IsConnected && PhotonNetwork.InRoom)
        {

            InstanciatePlayer();
        }
        numberOfPlayers.text= PhotonNetwork.CurrentRoom.PlayerCount.ToString() + " Players";
    }
    private void InstanciatePlayer()
    {
        GameObject player = PhotonNetwork.Instantiate(playerPrefab.name, Vector3.zero, Quaternion.identity);
        currentPlayer = player.GetComponent<PhotonPlayer>();

        if (PhotonNetwork.IsConnected)
        {
            if (PhotonNetwork.CurrentRoom.PlayerCount == 4)
            {
                GetListofPlayers();
            }
            else
            {
                Invoke("StartTurn", 2.0f);
                if(PhotonNetwork.LocalPlayer.ActorNumber==1)
                {
                  
                }
                else
                {
                   
                }
                //for (int i = 0; i < 2; i++)
                //{
                //    playersName[i].text = PhotonManager.instance.playerNames[i];

                //}

                //twoPlayers.SetActive(true);

            }

            //Invoke("StartTurn", 5.0f);
        }
    }
   
    public void StartTurn()
    {
        //GetListofPlayers();
        foreach (var item in instanciatedPlayers)
        {
            players.Add(item.GetComponent<PhotonPlayer>());
            
        }
        if (PhotonNetwork.CurrentRoom.PlayerCount <= 2)
        {
            for (int i = 0,j=0; i < instanciatedPlayers.Count; i++,j+=2)
            {
                instanciatedPlayers[i].transform.SetParent(spawnPoints[j].transform, false);
                players[i].GotiFace(j);
                UIManager.Instance.SetBoardPosition(i);
            }
        }
        if (PhotonNetwork.CurrentRoom.PlayerCount == 4)
        {
            for (int i = 0; i < instanciatedPlayers.Count; i++)
            {
                instanciatedPlayers[i].transform.SetParent(spawnPoints[i].transform, false);
                players[i].GotiFace(i);
                UIManager.Instance.SetBoardPosition(i);
            }
        }
        /*
        if (PhotonNetwork.IsMasterClient)
        {
            photonView.RPC("SetTurn", RpcTarget.All, currentPlayerIndex);
        }
        */
        TurnManager.instance.StartTurnSystem();
        matchPannel.SetActive(false);
    }
    public void ShowWinner()
    {
        WininScreen.SetActive(true);
        winneName.text = PhotonManager.instance.playerNames[currentPlayerIndex];
        winnerAmount.text = PhotonManager.instance.totalBet.ToString();
    }
    
    /*
    [PunRPC]
    public void SetTurn(int playerIndex)
    {
        matchPannel.SetActive(false);
        currentPlayerIndex = playerIndex;

        
        // Enable UI for the active player
        if (PhotonNetwork.LocalPlayer.ActorNumber - 1 == currentPlayerIndex)
        {
            UIManager.Instance.EnableDiceRoll();
            UpdatePlayerTimer.instance.restartTimer();
            Debug.Log($"Player {currentPlayerIndex}'s Turn");
            AutoPlayMode();
        }
        else
        {
            UIManager.Instance.DisableDiceRoll();
        }
    }
    */
    public void AutoPlayMode()
    {
        if (autoPlay)
        {
            Invoke("AutoTurn", 4f);
        }
    }
    private void AutoTurn()
   {
        DiceRoller.instance.RollDice();
        currentPlayer.MoveAPlayer();
   }
    /*
    public void EndTurn()
    {

        DiceRoller.instance.diceRolled = false;
        UIManager.Instance.DisableDiceRoll();
        UIManager.Instance.StopScaling();
        UpdatePlayerTimer.instance.Pause();
        Debug.Log("Curent Player Number === " + PhotonNetwork.LocalPlayer.ActorNumber);
        if (PhotonNetwork.LocalPlayer.ActorNumber - 1 == currentPlayerIndex)
        {
            currentPlayerIndex = (currentPlayerIndex + 1) % PhotonNetwork.CurrentRoom.PlayerCount; //Commennted to check
            Debug.Log("Next Player Number /// " + currentPlayerIndex);
            photonView.RPC("SetTurn", RpcTarget.All, currentPlayerIndex);
        }
      
    }
    */
    //get list of all players photon view id
    void GetListofPlayers()
    {

        Invoke("SortPlayersRelativeToLocal", 2f);


    }
    /*
    private void SwapTwoPlayers()
    {
        //if (PhotonNetwork.LocalPlayer.ActorNumber == 2)
        //{

        //    int firstPlayer = 1;
        //    Debug.Log("Swaping the Players: " + firstPlayer);
        //    GameObject tempPlayer = instanciatedPlayers[firstPlayer];
        //    instanciatedPlayers.RemoveAt(firstPlayer);
        //    instanciatedPlayers.Add(tempPlayer);

        //}
        //if (PhotonNetwork.LocalPlayer.ActorNumber == 3)
        //{
        //    for (int i = 0; i < PhotonNetwork.LocalPlayer.ActorNumber; i++)
        //    {
        //        int firstPlayer = i + 1;
        //        Debug.Log("Swaping the Players: " + firstPlayer);
        //        GameObject tempPlayer = instanciatedPlayers[firstPlayer];
        //        instanciatedPlayers.RemoveAt(firstPlayer);
        //        instanciatedPlayers.Add(tempPlayer);
        //    }
        //}
        //check the list of all instanciated players

       
       // SortPlayersRelativeToLocal();

        //foreach (var player in instanciatedPlayers)
        //{
        //    Debug.LogError(player.GetComponent<PhotonView>().ViewID);
        //}
        //.Invoke("StartTurn", 3.0f);

    }
    */

    //Sort the players according to the photonview id
    void SortPlayersRelativeToLocal()
    {
        // Get the local player's Photon View ID
        int localViewID = PhotonNetwork.LocalPlayer.ActorNumber; // Or assign based on PhotonView.OwnerActorNr

        // Sort players relative to the local player's ID
        instanciatedPlayers.Sort((a, b) =>
        {
            int relativeA = (a.GetComponent<PhotonView>().ViewID >= localViewID) ? a.GetComponent<PhotonView>().ViewID - localViewID : a.GetComponent<PhotonView>().ViewID - localViewID + PhotonNetwork.CurrentRoom.PlayerCount;
            int relativeB = (b.GetComponent<PhotonView>().ViewID >= localViewID) ? b.GetComponent<PhotonView>().ViewID - localViewID : b.GetComponent<PhotonView>().ViewID - localViewID + PhotonNetwork.CurrentRoom.PlayerCount;

            return relativeA.CompareTo(relativeB);
        });

        // Debug the sorted list
        Debug.Log("Sorted Player IDs relative to local player:");
        foreach (var player in instanciatedPlayers)
        {
            //Debug.LogError(player.GetComponent<PhotonView>().ViewID);
        }
        //Invoke("StartTurn", 5.0f);
        Invoke("SwapPlayers", 2f);

    }
    //swap the players acording to their view id 
    // like player 1 has sequence 1234
    //for player 2 has sequence 2341
    //for player 3 has 3412
    // and for player 4 has 4123
    private void SwapPlayers()
    {
        if (instanciatedPlayers != null)
        {
            for (int i = 0; i < PhotonNetwork.LocalPlayer.ActorNumber - 1; i++)
            {
                int firstPlayer = 0;
                Debug.Log("Swaping the Players: " + i);
                GameObject tempPlayer = instanciatedPlayers[firstPlayer];
                instanciatedPlayers.RemoveAt(firstPlayer);
                instanciatedPlayers.Add(tempPlayer);
            }
        }
        Debug.Log("Sorted Player IDs after swaping are:");
        foreach (var player in instanciatedPlayers)
        {
           // Debug.LogError(player.GetComponent<PhotonView>().ViewID);
        }
        Invoke("StartTurn", 5.0f);

    }

    /*
    //get list of all players photon view id
    void GetListofPlayers()
    {
        // Add all players' PhotonViews to the list
        foreach (GameObject player in instanciatedPlayers)
        {
            PhotonView view = player.GetComponent<PhotonView>();
            if (view != null)
                photonPlayers.Add(view);
        }

        SortPlayersRelativeToLocal();

        
    }

    void SortPlayersRelativeToLocal()
    {
        // Get the local player's Photon View ID
        int localViewID = PhotonNetwork.LocalPlayer.ActorNumber; // Or assign based on PhotonView.OwnerActorNr

        // Sort players relative to the local player's ID
        instanciatedPlayers.Sort((a, b) =>
        {
            int relativeA = (a.GetComponent<PhotonView>().ViewID >= localViewID) ? 
            a.GetComponent<PhotonView>().ViewID - localViewID : 
            a.GetComponent<PhotonView>().ViewID - localViewID + PhotonNetwork.CurrentRoom.PlayerCount;

            int relativeB = (b.GetComponent<PhotonView>().ViewID >= localViewID) ? 
            b.GetComponent<PhotonView>().ViewID - localViewID : 
            b.GetComponent<PhotonView>().ViewID - localViewID + PhotonNetwork.CurrentRoom.PlayerCount;

            return relativeA.CompareTo(relativeB);
        });

        // Debug the sorted list
        Debug.Log("Sorted Player IDs relative to local player:");
        foreach (var player in instanciatedPlayers)
        {
            Debug.Log(player.GetComponent<PhotonView>().ViewID);
        }
        //Invoke("SwapPlayers", 2f);

    }

    private void SwapPlayers()
    {
        for (int i = 0; i < PhotonNetwork.LocalPlayer.ActorNumber-1; i++)
        {
            int firstPlayer = 0;
            Debug.Log("Swaping the Players: " + i);
            GameObject tempPlayer = instanciatedPlayers[firstPlayer];
            instanciatedPlayers.RemoveAt(firstPlayer);
            instanciatedPlayers.Add(tempPlayer);
        }
        
        Debug.Log("Sorted Player IDs after swaping are:");
        foreach (var player in instanciatedPlayers)
        {
            Debug.Log(player.GetComponent<PhotonView>().ViewID);
        }
    }
    */
}//end of class

   
