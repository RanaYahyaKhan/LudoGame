using Photon.Pun;
using TMPro;
using UnityEngine;

public class PlayerInLobby : MonoBehaviour
{
    [SerializeField] private GameObject thisObj;
    public TMP_Text playerName;
    private void Start()
    {
        PhotonManager.instance.instanciatedPlayerInLobby.Add(thisObj);
    }
}
