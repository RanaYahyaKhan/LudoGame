using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WhatsAppCopyAndOpen : MonoBehaviour
{
    [SerializeField] private TMP_InputField inputField; // InputField for user to type the message
    [SerializeField] private Button whatsappButton; // Button to open WhatsApp
    [SerializeField] private Button facebookButton; // Button to open Facebook Messenger
    [SerializeField] private Button otherAppButton; // Button to open another app

    void Start()
    {
        // Add listeners to buttons
        whatsappButton.onClick.AddListener(CopyAndOpenWhatsApp);
        facebookButton.onClick.AddListener(CopyText);
        otherAppButton.onClick.AddListener(CopyText);
    }

    void CopyAndOpenWhatsApp()
    {
        // Copy the input field text to the clipboard
        string textToCopy = inputField.text;

        if (!string.IsNullOrEmpty(textToCopy))
        {
            GUIUtility.systemCopyBuffer = textToCopy; // Copy to clipboard
            Debug.Log("Copied to clipboard: " + textToCopy);

            // Open WhatsApp
            string whatsappURL = "https://api.whatsapp.com/send";
            Application.OpenURL(whatsappURL);
        }
        else
        {
            Debug.LogWarning("Input field is empty. Please type a message.");
        }
    }
    void CopyText()
    {
        string textToShare = inputField.text;

        if (string.IsNullOrEmpty(textToShare))
        {
            Debug.LogWarning("Input field is empty. Please type a message.");
            return;
        }

        GUIUtility.systemCopyBuffer = textToShare; // Copy to clipboard
        Debug.Log("Copied to clipboard: " + textToShare);
    }
}
