using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpdatePlayerTimer : MonoBehaviour
{
    public static UpdatePlayerTimer instance;
    private float playerTime;
    public GameObject timerObject;
    private Image timer;
    private bool timeSoundsStarted;

    public bool myTimer;
    public bool paused = false;
    private void Awake()
    {
        instance=this;
    }
    void Start()
    {
        timer = gameObject.GetComponent<Image>();
    }

    void OnEnable()
    {
        timer = gameObject.GetComponent<Image>();
    }
    public void Pause()
    {
        paused = true;
    }
    void Update()
    {
        if (!DiceRoller.instance.diceRolled && PhotonNetwork.LocalPlayer.ActorNumber-1 == GameManager.Instance.currentPlayerIndex)
            updateClock();
    }

    public void restartTimer()
    {
        paused = false;
        timer.fillAmount = 1.0f;
    }


    void OnDisable()
    {
        if (timer != null)
        {
            timer.fillAmount = 1.0f;
            paused = false;
        }
    }

    private void updateClock()
    {
        float minus;

        playerTime = GameManager.Instance.playerTime;
        if (UIManager.Instance.enableUi)
            playerTime = GameManager.Instance.playerTime;
        minus = 1.0f / playerTime * Time.deltaTime;

        timer.fillAmount -= minus;

        if (timer.fillAmount < 0.25f && !DiceRoller.instance.diceRolled)
        {
            if (!DiceRoller.instance.diceRolled)
            {
                DiceRoller.instance.RollDice();
                restartTimer();
            }
            else
            {

            }
            
        }

        if (timer.fillAmount == 0)
        {
            Debug.Log("TIME 0");
        }


    }

}//end of class

