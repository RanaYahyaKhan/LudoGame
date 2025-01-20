using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public enum GameMode
{
    Manual,
    Auto
}
public class AutoPlayScript : MonoBehaviour
{
    [SerializeField] private Image autoPlayImg;
    [SerializeField] private Image autoPlayInfo;
    [SerializeField] private GameObject autoPlayCheck;
    [SerializeField] private GameObject autoPlayUnCheck;
    [SerializeField] private GameObject noPannel;
    [SerializeField] private GameObject yesPannel;
    [SerializeField] private Sprite autPlayCheck;
    [SerializeField] private Sprite autPlayUnCheck;
    [SerializeField] private GameObject autoPlayPannel;

    [SerializeField] private GameMode mode = GameMode.Manual;
    [SerializeField] private GameMode newMode; 
    private void Start()
    {
        if(mode == GameMode.Manual)
        {
            autoPlayImg.sprite = autPlayUnCheck;
            autoPlayInfo.sprite = autPlayUnCheck;
        }
    }
    public void ChangeMode()
    {
        autoPlayPannel.SetActive(true);
        if(mode == GameMode.Manual)
        {
            autoPlayCheck.SetActive(true);
            autoPlayUnCheck.SetActive(false);
            newMode = GameMode.Auto;
        }
        else
        {
            autoPlayCheck.SetActive(false);
            autoPlayUnCheck.SetActive(true);
            newMode = GameMode.Manual;
        }
        confirmPannel();
    }
    public void AutoPlay()
    {
        autoPlayCheck.SetActive(true);
        autoPlayUnCheck.SetActive(false);
        newMode = GameMode.Auto;
        if (newMode != mode)
        {
            confirmPannel();
            autoPlayImg.sprite = autPlayCheck;
            autoPlayInfo.sprite = autPlayCheck;
        }
        else
        {
            CancelPannel(); 
            autoPlayImg.sprite = autPlayUnCheck;
            autoPlayInfo.sprite = autPlayUnCheck;
        }
        GameManager.Instance.autoPlay = true;
    }
    public void AutoPlayUnCheck()
    {
        autoPlayCheck.SetActive(false);
        autoPlayUnCheck.SetActive(true);
        newMode = GameMode.Manual;
        if (newMode != mode)
        {
            confirmPannel();
            autoPlayImg.sprite = autPlayUnCheck;
        }
        else
        {
            CancelPannel();
            autoPlayImg.sprite = autPlayCheck;
        }
        GameManager.Instance.autoPlay = false;
     

    }
    public void YesBtn()
    {
        mode = newMode;
        autoPlayPannel.SetActive(false);
        if (mode == GameMode.Manual)
        {
            GameManager.Instance.autoPlay = false;
            autoPlayImg.sprite = autPlayUnCheck;
            autoPlayInfo.sprite = autPlayUnCheck;
        }
        else
        {
            GameManager.Instance.autoPlay = true;
            autoPlayImg.sprite = autPlayCheck;
            autoPlayInfo.sprite = autPlayCheck;
        }

    }
    public void NoBtn()
    {
        autoPlayPannel.SetActive(false);
        if (mode == GameMode.Manual)
        {
            GameManager.Instance.autoPlay = false;
            autoPlayImg.sprite = autPlayUnCheck;
            autoPlayInfo.sprite = autPlayUnCheck;
        }
        else
        {
            GameManager.Instance.autoPlay = true;
            autoPlayImg.sprite = autPlayCheck;
            autoPlayInfo.sprite = autPlayCheck;
        }

    }
    private void confirmPannel()
    {
        yesPannel.SetActive(true);
        noPannel.SetActive(false);
    }
    private void CancelPannel()
    {
        yesPannel.SetActive(false);
        noPannel.SetActive(true);
    }
}
