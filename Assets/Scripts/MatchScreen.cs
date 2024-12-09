using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MatchScreen : MonoBehaviour
{
    [SerializeField] private TMP_Text matchText;
    private void Start()
    {
        StartCoroutine(AnimateDots("Match Starting"));
    }
    private IEnumerator AnimateDots(string str)
    {
        int dotCount = 0;

        while (true)
        {
            dotCount = (dotCount % 5) + 1; // Cycle through 1, 2, 3
            matchText.text = str + new string('.', dotCount);
            yield return new WaitForSeconds(0.5f); // Adjust the speed as needed
        }
    }
}
