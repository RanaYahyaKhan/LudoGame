using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleScript : MonoBehaviour
{
    [SerializeField] private GameObject AutoCheck;
    [SerializeField] private GameObject ManualCheck;
    [SerializeField] private GameObject AutoUnCheck;
    [SerializeField] private GameObject ManualUnCheck;
   
    public static ToggleScript Instance { get; private set; }
    private void Awake()
    {
       Instance = this;
    }
    public void AutoChecked()
    {
        AutoCheck.SetActive(true); AutoUnCheck.SetActive(false);
        ManualCheck.SetActive(false); ManualUnCheck.SetActive(true);
    }
    public void ManualChecked()
    {
        AutoCheck.SetActive(false); AutoUnCheck.SetActive(true);
        ManualCheck.SetActive(true); ManualUnCheck.SetActive(false);
    }
}
