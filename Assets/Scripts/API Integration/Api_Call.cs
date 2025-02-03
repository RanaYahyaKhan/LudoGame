using TMPro;
using UnityEngine.Networking;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Newtonsoft.Json;
public class Api_Call : MonoBehaviour
{
    public class Fact
    {
        public int id { get; set; }
        public string email { get; set; }
        public string username { get; set; }
        public string full_name { get; set; }
    }
    [SerializeField] private TMP_Text apiText;
    private string url = "https://api.canadianludo.com/api/SocialLogin/";
    [SerializeField] private Button refreshBtn;
    private void Start()
    {
        refreshBtn.onClick.AddListener(CallApi);
        CallApi();
    }

    private void CallApi()
    {
        StartCoroutine(GetRequest(url));
    }
    IEnumerator GetRequest(string uri)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
        {
            yield return webRequest.SendWebRequest();
            switch (webRequest.result)
            {
                case UnityWebRequest.Result.ConnectionError:
                case UnityWebRequest.Result.DataProcessingError:
                    Debug.LogError(string.Format("Some thing went wrong {0}", webRequest.error));
                    break;
                case UnityWebRequest.Result.Success:
                    Fact fact = JsonConvert.DeserializeObject<Fact>(webRequest.downloadHandler.text);
                    apiText.text = fact.username + "              Length      " + fact.email;
                    break;
            }
        }
    }
}
