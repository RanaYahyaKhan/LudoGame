using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour
{
    [SerializeField] private AudioSource Source;
    [SerializeField] private AudioClip diceRoll;
    
    public static AudioController instance;
    private void Awake()
    {
        instance = this;
    }
  
    public void DiceRollSound()
    {
        Source.clip= diceRoll;
        if (Source != null && diceRoll != null)
        {
            Source.Play();
        }
    }
}
