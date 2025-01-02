using Firebase;
using Firebase.Auth;
using Firebase.Extensions;
using Google;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.ConstrainedExecution;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

public class GoogleLoginScript : MonoBehaviour
{
    private GoogleSignInConfiguration configuration;
    public TMP_Text Username, UserEmail;
    private FirebaseAuth auth;

    void Start()
    {
        InitFirebase();
        configuration = new GoogleSignInConfiguration
        {
            WebClientId = "98652851590-ni1vr825cbq1uj9ktoi6c6guloufvp9l.apps.googleusercontent.com",
            RequestEmail = true,
            RequestIdToken = true
        };
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

    public void SignInWithGoogle()
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
                Username.text = user.DisplayName;
                UserEmail.text = user.Email;
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
}
