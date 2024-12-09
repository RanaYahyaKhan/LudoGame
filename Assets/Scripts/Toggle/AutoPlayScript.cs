using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoPlayScript : MonoBehaviour
{
    [SerializeField] private GameObject autoPlayCheck;
    [SerializeField] private GameObject autoPlayUnCheck;
    [SerializeField] private GameObject noPannel;
    [SerializeField] private GameObject yesPannel;

    private void Start()
    {
        AutoPlay();
    }
    public void AutoPlay()
    {
        autoPlayCheck.SetActive(false);
        autoPlayUnCheck.SetActive(true);
        yesPannel.SetActive(false);
        noPannel.SetActive(true);
    }
    public void AutoPlayUnCheck()
    {
        autoPlayCheck.SetActive(true);
        autoPlayUnCheck.SetActive(false);
        yesPannel.SetActive(true);
        noPannel.SetActive(false);
    }
    public void YesBtn()
    {

    }
}
