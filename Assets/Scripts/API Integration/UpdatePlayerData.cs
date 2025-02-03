using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;

public class UpdatePlayerData : MonoBehaviour
{
    private string apiUrl = "https://api.canadianludo.com/api/UpdateProfile/";
    [SerializeField] private TMP_InputField userNameInput;
    [SerializeField] private TMP_InputField emailInput;
    [SerializeField] private TMP_InputField phoneNumberInput;
    public void OnUpdatePlayerDataButtonClicked()
    {
        if(userNameInput != null && emailInput!=null && phoneNumberInput != null)
        {
            string username = userNameInput.text;
            string email = emailInput.text;
            string phoneNumber = phoneNumberInput.text;

            UpdatePlayer(username, email, phoneNumber);
        }
        
    }
    private void UpdatePlayer(string username, string email, string phoneNumber)
    {
        PlayerData playerData = new PlayerData
        {
            username = username,
            email = email,
            phoneNumber = phoneNumber
        };

        string jsonData = JsonUtility.ToJson(playerData);

        StartCoroutine(PostRequest(apiUrl, jsonData));
    }

    private IEnumerator PostRequest(string url, string jsonData)
    {
        UnityWebRequest request = new UnityWebRequest(url, "POST");
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonData);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.LogError("Error: " + request.error);
        }
        else
        {
            Debug.Log("Player data updated successfully!");
            Debug.Log("Response: " + request.downloadHandler.text);
        }
    }
    [System.Serializable]
    public class PlayerData
    {
        public string username;
        public string email;
        public string phoneNumber;
    }
}
