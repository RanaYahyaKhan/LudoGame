using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class FriendsManager : MonoBehaviour
{
    [SerializeField] private GameObject friendBtn;
    [SerializeField] private GameObject buddyBtn;
    [SerializeField] private GameObject friendReqestBtn;
    [SerializeField] private GameObject friendActive;
    [SerializeField] private GameObject buddyActive;
    [SerializeField] private GameObject friendsPannel;
    [SerializeField] private GameObject buddyPannel;
    [SerializeField] private GameObject friendRequestPannel;
    [SerializeField] private GameObject friendSearchPannel;
    [SerializeField] private TMP_InputField SearchInput;
    [SerializeField] private GameObject backBtn;
    [SerializeField] private GameObject player;

    public Vector2 targetSize = new Vector2(300, 100); // New size
    public Vector2 SmallSize = new Vector2(500, 120); // New size
    public float duration = 0.5f; // Time for the animation

    private RectTransform rectTransform;
    private void Start()
    {
        ShowFriends();
        if(SearchInput != null)
        {
            rectTransform = SearchInput.GetComponent<RectTransform>();
            SearchInput.onSelect.AddListener(SearchInputClick);
        }
    }
    public void ShowFriends()
    {
        if (friendBtn != null) 
        {
            friendBtn.SetActive(false);
            buddyActive.SetActive(false);
            buddyPannel.SetActive(false);
            backBtn.SetActive(false);

            friendRequestPannel.SetActive(false);
            friendSearchPannel.SetActive(false);

            friendsPannel.SetActive(true);
            friendActive.SetActive(true);
            buddyBtn.SetActive(true);
            player.SetActive(true);
            SearchInput.gameObject.SetActive(true);
            if (rectTransform != null)
            {
                StartCoroutine(ScaleOverTime(SmallSize, duration));
            }

        }
    }
    public void ShowBuddy()
    {
        if (buddyBtn != null)
        {
            buddyBtn.SetActive(false);
            friendActive.SetActive(false);
            backBtn.SetActive(false);

            friendsPannel.SetActive(false);
            friendRequestPannel.SetActive(false);
            friendSearchPannel.SetActive(false);

            buddyPannel.SetActive(true);
            buddyActive.SetActive(true);
            friendBtn.SetActive(true);

        }
    }
    public void ShowFriendsRequest()
    {
        if (friendReqestBtn != null)
        {
            buddyBtn.SetActive(false);
            friendActive.SetActive(false);

            friendsPannel.SetActive(false);
            friendSearchPannel.SetActive(false);

            buddyPannel.SetActive(false);
            buddyActive.SetActive(false);
            friendBtn.SetActive(false);
            SearchInput.gameObject.SetActive(false);
            player.SetActive(false);

            friendRequestPannel.SetActive(true);
            backBtn.SetActive(true);
            


        }
    }
    public void SearchInputClick(string text)
    {
        if (SearchInput != null)
        {
            buddyBtn.SetActive(false);
            friendActive.SetActive(false);

            friendsPannel.SetActive(false);
            friendSearchPannel.SetActive(false);

            buddyPannel.SetActive(false);
            buddyActive.SetActive(false);
            friendBtn.SetActive(false);
            player.SetActive(false);
            friendRequestPannel.SetActive(false);

            friendSearchPannel.SetActive(true);
            backBtn.SetActive(true);
        }
        if(rectTransform != null)
        {
            StartCoroutine(ScaleOverTime(targetSize, duration));
        }
    }
    private System.Collections.IEnumerator ScaleOverTime(Vector2 target, float duration)
    {
        Vector2 initialSize = rectTransform.sizeDelta;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            rectTransform.sizeDelta = Vector2.Lerp(initialSize, target, elapsed / duration);
            yield return null;
        }

        rectTransform.sizeDelta = target;
    }

    private void OnDestroy()
    {
        if (SearchInput != null)
        {
            SearchInput.onSelect.RemoveListener(SearchInputClick);
        }
    }
}
