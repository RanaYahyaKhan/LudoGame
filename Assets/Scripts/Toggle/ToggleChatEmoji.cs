using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleChatEmoji : MonoBehaviour
{
    [SerializeField] private GameObject ChatBtn;
    [SerializeField] private GameObject ChatPannel;
    [SerializeField] private GameObject EmojiBtn;
    [SerializeField] private GameObject EmojiPannel;
    private void Start()
    {
        ActiveChat();
    }
    public void ActiveChat()
    {
        ChatBtn.SetActive(true);
        ChatPannel.SetActive(true);
        EmojiBtn.SetActive(false);
        EmojiPannel.SetActive(false);
    }
    public void ActiveEmoji()
    {
        ChatBtn.SetActive(false);
        ChatPannel.SetActive(false);
        EmojiBtn.SetActive(true);
        EmojiPannel.SetActive(true);
    }
}
