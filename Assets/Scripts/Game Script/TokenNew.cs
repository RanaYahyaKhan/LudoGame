using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TokenNew : MonoBehaviourPunCallbacks
{
    public int currentPosition = -1; // Current position (-1 means start position)
    public Transform[] boardPositions; // Positions on the board

    public void Move(int steps)
    {
        int targetPosition = currentPosition + steps;
        if (targetPosition >= boardPositions.Length)
        {
            Debug.Log("Move exceeds board limit.");
            return;
        }

        StartCoroutine(MoveToPosition(targetPosition));
    }

    private IEnumerator MoveToPosition(int targetPosition)
    {
        while (currentPosition < targetPosition)
        {
            currentPosition++;
            transform.position = boardPositions[currentPosition].position;
            yield return new WaitForSeconds(0.3f);
        }

    }
}
