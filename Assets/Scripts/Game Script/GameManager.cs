using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
public class GameManager : MonoBehaviourPunCallbacks
{
    public static GameManager Instance;
    [SerializeField] private GameObject matchPannel;
    public int currentPlayerIndex = 0; // Tracks the current player's turn
   public List<PhotonPlayer> players =new List<PhotonPlayer>();


    public List<GameObject> instanciatedPlayers = new List<GameObject>();
    public GameObject playerPrefab; // The prefab name in Resources folder
    public Transform[] spawnPoints;

    public PhotonPlayer currentPlayer;




    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        PhotonNetwork.AutomaticallySyncScene = true;

        //if (PhotonNetwork.IsMasterClient)
        //{
        //    StartTurn();
        //}
        if (PhotonNetwork.IsConnected && PhotonNetwork.InRoom)
        {

            InstanciatePlayer();
        }
    }
    private void InstanciatePlayer()
    {
        GameObject player = PhotonNetwork.Instantiate(playerPrefab.name, Vector3.zero, Quaternion.identity);
        currentPlayer = player.GetComponent<PhotonPlayer>();

        if (PhotonNetwork.IsConnected)
        {
            Invoke("StartTurn",3.0f);
        }
    }
    /*
    private void SpawnPlayer()
    {


        // Get the actor number of the player
        int playerIndex = PhotonNetwork.LocalPlayer.ActorNumber - 1; // ActorNumber starts from 1, array index starts from 0

        // Instantiate the player at the chosen spawn point

        //GameObject player = PhotonNetwork.Instantiate(playerPrefab.name, Vector3.zero, Quaternion.identity);
        //players = player.GetComponent<PhotonPlayer>();
        //instanciatedPlayers.Add(player);
        GameObject gm = InstantiateObject(playerPrefab.name, Vector3.zero, Quaternion.identity);
        players = gm.GetComponent<PhotonPlayer>();
        // Call a method to mark the player, if applicable
        Invoke("StartTurn", 3.0f);

    }

    public static GameObject InstantiateObject(string prefabName, Vector3 position, Quaternion rotation)
    {
        GameObject newObject = PhotonNetwork.Instantiate(prefabName, position, rotation);
        instanciatedPlayers.Add(newObject);
        PhotonView photonView = newObject.GetPhotonView();
        photonView.RPC("RPC_AddToObjectList", RpcTarget.Others, photonView.ViewID);
        return newObject;
    }
    [PunRPC]
    public void RPC_AddToObjectList(int viewID)
    {
        PhotonView view = PhotonView.Find(viewID);
        if (view != null)
        {
            instanciatedPlayers.Add(view.gameObject);
        }
    }
    */
    public void StartTurn()
    {
        foreach (var item in instanciatedPlayers)
        {
            players.Add(item.GetComponent<PhotonPlayer>());
            
        }
        for (int i = 0; i < instanciatedPlayers.Count; i++)
        {
            instanciatedPlayers[i].transform.SetParent(spawnPoints[i].transform, false);
            players[i].GotiFace(i);
            UIManager.Instance.SetBoardPosition(i);
        }
        if (PhotonNetwork.IsMasterClient)
        {
            photonView.RPC("SetTurn", RpcTarget.All, currentPlayerIndex);
        }
    }

    
    
    [PunRPC]
    public void SetTurn(int playerIndex)
    {
        matchPannel.SetActive(false);
        currentPlayerIndex = playerIndex;

        Debug.Log("Curent Player Number === " + PhotonNetwork.LocalPlayer.ActorNumber);
        // Enable UI for the active player
        if (PhotonNetwork.LocalPlayer.ActorNumber - 1 == currentPlayerIndex)
        {
            UIManager.Instance.EnableDiceRoll();
            Debug.Log($"Player {currentPlayerIndex}'s Turn");
        }
        else
        {
            UIManager.Instance.DisableDiceRoll();
        }
    }

    public void EndTurn()
    {
        DiceRoller.instance.diceRolled = false;
        UIManager.Instance.DisableDiceRoll();
       
        if (PhotonNetwork.IsConnected)
        {
            // Update turn index and synchronize it
            currentPlayerIndex = (currentPlayerIndex + 1) % 2; //Commennted to check
            photonView.RPC("SetTurn", RpcTarget.All, currentPlayerIndex);
        }
    }
    

}

   
