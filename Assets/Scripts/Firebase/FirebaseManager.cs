using Firebase;
using Firebase.Auth;
using Firebase.Extensions;
using Google;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class FirebaseManager : MonoBehaviour
{
    private string baseUrl = "https://api.canadianludo.com/api/SocialLogin/"; 

    private FirebaseAuth auth;
    private GoogleSignInConfiguration configuration;

    [SerializeField] private Button googleBtn;
    [SerializeField] private TMP_Text Username;

    private void Start()
    {
        InitFirebase();
        ConfigureGoogle();
        googleBtn.onClick.AddListener(SignIn);



    }
   
    void InitFirebase()
    {
        //auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
        {
            FirebaseApp app = FirebaseApp.DefaultInstance;
            auth = FirebaseAuth.DefaultInstance;
        });
    }
    void ConfigureGoogle()
    {
        configuration = new GoogleSignInConfiguration
        {
            WebClientId = "98652851590-ni1vr825cbq1uj9ktoi6c6guloufvp9l.apps.googleusercontent.com",
            RequestEmail = true,
            RequestIdToken = true
        };
    }

    private void SignIn()
    {
        SignInWithGoogle();
    }

    void SignInWithGoogle()
    {
        GoogleSignIn.Configuration = configuration;
        GoogleSignIn.DefaultInstance.SignIn().ContinueWith(task =>
        {
            if (task.IsFaulted || task.IsCanceled)
            {
                Debug.LogError("Google Sign-In failed.");
                foreach (var e in task.Exception.Flatten().InnerExceptions)
                {
                    Debug.LogError($"Sign-In error: {e.Message}");
                }
            }
            else
            {
                Debug.Log($"Google Sign-In succeeded. User: {task.Result.DisplayName}");
                Debug.Log($"ID Token: {task.Result.IdToken}");

                AuthenticateWithFirebase(task.Result.IdToken);
            }
        });
    }
    private void AuthenticateWithFirebase(string googleIdToken)
    {
        Credential credential = GoogleAuthProvider.GetCredential(googleIdToken, null);
        auth.SignInWithCredentialAsync(credential).ContinueWith(task =>
        {
            if (task.IsCompleted && !task.IsFaulted)
            {
                FirebaseUser user = task.Result;
                string deviceID = SystemInfo.deviceUniqueIdentifier;

                Username.text = user.DisplayName + user.Email + deviceID;
                
                PostData(user.Email, deviceID, user.DisplayName, "Google");
                PhotonManager.instance.SignInSocial(user.DisplayName);
                //UserEmail.text = user.Email;
                Debug.Log($"Firebase Authentication succeeded. User: {user.DisplayName}");
            }
            else
            {
                Debug.LogError("Firebase Authentication failed.");
                foreach (var e in task.Exception.Flatten().InnerExceptions)
                {
                    Debug.LogError($"Firebase Auth error: {e.Message}");
                }
            }
        });
    }



    public void OnSignOut()
    {
        GoogleSignIn.DefaultInstance.SignOut();
    }
    private void PostData(string _email,string _deviceId, string _userName, string _socialPlatform)
    {
        

        // Create JSON payload
        string jsonData = JsonUtility.ToJson(new SocialLogin
        {
            email= _email,
            device_id= _deviceId,
            full_name= _userName,
            social_platform= _socialPlatform
        });

        // Post data
        StartCoroutine(PostRequest(baseUrl, jsonData)); // Replace "your-endpoint" with your actual route
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
    [System.Serializable]
    public class SocialLogin
    {
        public string email;
        public string device_id;
        public string full_name;
        public string social_platform;
    }

}
