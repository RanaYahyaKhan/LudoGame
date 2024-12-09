using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GotiPlayer : MonoBehaviourPun
{
    public GameObject[] RedMark;
    public GameObject[] GreenMark;
    public GameObject[] YellowMark;
    public GameObject[] BlueMark;
    public void ShowRedMark()
    {
        if (photonView.IsMine)
        {
            if (RedMark != null)
            {
                foreach (var item in RedMark)
                {
                    item.SetActive(true);
                }
            }
        }
    }
    public void ShowYellowMark()
    {
        if (photonView.IsMine)
        {
            if (YellowMark != null)
            {
                foreach (var item in YellowMark)
                {
                    item.SetActive(true);
                }
            }
        }
    }
    private void Start()
    {
        if (PhotonNetwork.LocalPlayer.ActorNumber - 1 == 1)
        {
            Invoke("InvokeSwap", 2.0f);
            //Debug.Log("Call on seonc player");

        }
    }
    private void InvokeSwap()
    {
        //SwapSpawnPoints(0, 1); // Pass parameters here
        GameObject targetObject = GameObject.FindWithTag("LudoBoard");
        if (targetObject != null)
        {
            // Rotate the object 180 degrees around the Z-axis
            targetObject.transform.Rotate(0, 0, 180);
            Debug.Log($"Rotated {targetObject.name} by 180 degrees on the Z-axis.");
        }
        else
        {
            Debug.LogError($"No GameObject found with tag: ");
        }
    }
    private void SwapSpawnPoints(int indexA, int indexB)
    {
        if (indexA < 0 || indexA >=LudoManager.Instance.spawnPoints.Length || indexB < 0 || indexB >= LudoManager.Instance.spawnPoints.Length)
        {
            Debug.LogError("Invalid indices for swapping spawn points.");
            return;
        }

        // Swap the spawn points
        Transform temp = LudoManager.Instance.spawnPoints[indexA];
        LudoManager.Instance.spawnPoints[indexA] = LudoManager.Instance.spawnPoints[indexB];
        LudoManager.Instance.spawnPoints[indexB] = temp;

        Debug.Log($"Swapped spawn points: {indexA} with {indexB}");
    }
}
