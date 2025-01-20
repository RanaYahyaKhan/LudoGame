using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using TMPro;
using UnityEngine.UI;

public class APIManager : MonoBehaviour
{
    private string baseUrl = "http://192.168.10.30:8000/api/Login/"; // Replace with your server's IP and route

    [SerializeField] private TMP_InputField phoneNumber; // Assign in Inspector
    [SerializeField] private TMP_InputField otp; // Assign in Inspector

    public void GetData(string endpoint)
    {
        StartCoroutine(GetRequest($"{baseUrl}/{endpoint}"));
    }

    //public void PostData(string endpoint, string jsonData)
    //{
    //    StartCoroutine(PostRequest($"{baseUrl}/{endpoint}", jsonData));
    //}

    private IEnumerator GetRequest(string uri)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
        {
            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.Success)
            {
                Debug.Log($"Response: {webRequest.downloadHandler.text}");
            }
            else
            {
                Debug.LogError($"Error: {webRequest.error}");
            }
        }
    }

    public void PostData()
    {
        string value1 = phoneNumber.text;
        string value2 = otp.text;

        // Create JSON payload
        string jsonData = JsonUtility.ToJson(new Payload
        {
            field1 = value1,
            field2 = value2
        });

        // Post data
        StartCoroutine(PostRequest($"{baseUrl}/your-endpoint", jsonData)); // Replace "your-endpoint" with your actual route
    }

    private IEnumerator PostRequest(string uri, string jsonData)
    {
        using (UnityWebRequest webRequest = new UnityWebRequest(uri, "POST"))
        {
            byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(jsonData);
            webRequest.uploadHandler = new UploadHandlerRaw(jsonToSend);
            webRequest.downloadHandler = new DownloadHandlerBuffer();
            webRequest.SetRequestHeader("Content-Type", "application/json");

            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.Success)
            {
                Debug.Log($"Response: {webRequest.downloadHandler.text}");
            }
            else
            {
                Debug.LogError($"Error: {webRequest.error}");
            }
        }
    }

    // Define payload class
    [System.Serializable]
    private class Payload
    {
        public string field1;
        public string field2;
    }
}
