using Photon.Pun;
using System.Collections;
using UnityEngine;

public class PlayerPiece : MonoBehaviourPun
{
    //public int position = 0; // Position on the board
    //public Transform[] path; // Reference to the board path tiles
    //public DiceController dice;
    //public void MovePiece()
    //{
    //    int steps = dice.diceResult;
    //    if (LudoManager.Instance.IsMyTurn() && photonView.IsMine)
    //    {
    //        StartCoroutine(MoveCoroutine(steps));
    //    }
    //}

    //private IEnumerator MoveCoroutine(int steps)
    //{
    //    for (int i = 0; i < steps; i++)
    //    {
    //        if (position + 1 < path.Length)
    //        {
    //            position++;
    //            transform.position = path[position].position;
    //            yield return new WaitForSeconds(0.2f);
    //        }
    //    }

    //    // End turn after movement
    //    LudoManager.Instance.EndTurn();
    //}
}
