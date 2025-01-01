using Firebase;
using Firebase.Auth;
using TMPro;
using UnityEngine;

public class FirebaseTest : MonoBehaviour
{
    FirebaseAuth auth;
    public TMP_Text errInput;

    void Start()
    {
        InitFirebase();


    }
    void InitFirebase()
    {
        auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
    }
    public void CheckDependency()
    {
        auth.SignInAnonymouslyAsync().ContinueWith(task =>
        {
            if (task.IsCanceled)
            {
                Debug.LogError("SignInAnonymouslyAsync was canceled.");
                errInput.text = "SignInAnonymouslyAsync was canceled.";
                return;
            }
            if (task.IsFaulted)
            {
                var exception = task.Exception.Flatten().InnerExceptions[0];
                Debug.LogError("SignInAnonymouslyAsync encountered an error: " + exception.Message);
                errInput.text = "SignInAnonymouslyAsync encountered an error: " + exception.Message;
                return;
            }

            Debug.Log($"Firebase user created successfully: {task.Result}");
            errInput.text = "Firebase user created successfully:" + task.Result;
        });
    }

    private void SignInAnonymously()
    {
       
    }
}
