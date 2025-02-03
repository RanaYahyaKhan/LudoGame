using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using TMPro;
using UnityEngine.UI;
using System;
using Firebase.Auth;
using Firebase.Extensions;

public class APIManager : MonoBehaviour
{
    private string baseUrl = "https://api.canadianludo.com/api/Login/"; // Replace with your server's IP and route


    public TMP_InputField phoneNumber, EnterCode_Inp;
    public TextMeshProUGUI logTxt;
  
    uint phoneAuthTimeoutMs = 3 * 60000;//minutes to milisec
    FirebaseAuth auth;
    PhoneAuthProvider provider;
    private void Start()
    {
        auth = FirebaseAuth.DefaultInstance;
        provider = PhoneAuthProvider.GetInstance(auth);
    }

    public void sendSMS()
    {
        print("send sms pressed");
        showLogMsg("send sms pressed");

        string userNumber = "+92" + phoneNumber.text;

        PhoneAuthProvider provider = PhoneAuthProvider.GetInstance(auth);
        provider.VerifyPhoneNumber(
          new PhoneAuthOptions
          {
              PhoneNumber = userNumber,
              TimeoutInMilliseconds = 60,
              ForceResendingToken = null
          },
          verificationCompleted: (credential) => {
              print("Verification completed");
              showLogMsg("Verification completed");

              // Auto-sms-retrieval or instant validation has succeeded (Android only).
              // There is no need to input the verification code.
              // `credential` can be used instead of calling GetCredential().
          },
          verificationFailed: (error) => {
              print("Error: " + error);
              showLogMsg(error);

              // The verification code was not sent.
              // `error` contains a human readable explanation of the problem.
          },
          codeSent: (id, token) => {
              showLogMsg("Code send success!!");
              // Verification code was successfully sent via SMS.
              // `id` contains the verification id that will need to passed in with
              PlayerPrefs.SetString("verificationId", id);
              // the code from the user when calling GetCredential().
              // `token` can be used if the user requests the code be sent again, to
              // tie the two requests together.
          },
          codeAutoRetrievalTimeOut: (id) => {
              // Called when the auto-sms-retrieval has timed out, based on the given
              // timeout parameter.
              // `id` contains the verification id of the request that timed out.
          });
    }
    public void VerifyOtp()
    {

        print("verify otp pressed");
        string verificationId = PlayerPrefs.GetString("verificationId");
        string verificationCode = EnterCode_Inp.text;

        PhoneAuthCredential credential = provider.GetCredential(verificationId, verificationCode);

        auth.SignInAndRetrieveDataWithCredentialAsync(credential).ContinueWithOnMainThread(task => {
            if (task.IsFaulted)
            {
                Debug.LogError("SignInAndRetrieveDataWithCredentialAsync encountered an error: " +
                               task.Exception);
                return;
            }

            FirebaseUser newUser = task.Result.User;
            Debug.Log("User signed in successfully");
            showLogMsg("Success");
            // This should display the phone number.
            Debug.Log("Phone number: " + newUser.PhoneNumber);
            // The phone number providerID is 'phone'.
            Debug.Log("Phone provider ID: " + newUser.ProviderId);
            PostData();
            PhotonManager.instance.SignInSocial(newUser.PhoneNumber);


        });
    }

    #region extra
    void showLogMsg(string msg)
    {
        logTxt.text = msg;
        //logTxt.GetComponent<Animation>().Play("textFadeout");
    }
    #endregion


    public void PostData()
    {
        string value1 = phoneNumber.text;

        // Create JSON payload
        string jsonData = JsonUtility.ToJson(new PhoneAuth
        {
            phone_number = value1,
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
   
    // Define payload class
    [System.Serializable]
    private class PhoneAuth
    {
        public string phone_number;
    }
}
