using System.Collections;
using System.Collections.Generic;
using Firebase.Extensions;
using Google;
using System.Threading.Tasks;
using UnityEngine;
using TMPro;
using Firebase.Auth;
using UnityEngine.UI;
using Firebase;

public class FirebaseManager : MonoBehaviour
{
    public string GoogleAPI = "98652851590-ni1vr825cbq1uj9ktoi6c6guloufvp9l.apps.googleusercontent.com";
    private GoogleSignInConfiguration configuration;

    //Firebase.DependencyStatus dependencyStatus = Firebase.DependencyStatus.UnavailableOther;
    FirebaseAuth auth;

    public TMP_Text Username, UserEmail;
    public TMP_InputField errInput; // Input field for room name

    //public GameObject LoginScreen, ProfileScreen;
    public Image UserProfilePic;
    private string imageUrl;

    private void Awake()
    {
        configuration = new GoogleSignInConfiguration
        {
            WebClientId = GoogleAPI,
            RequestIdToken = true,
        };
    }

    private void Start()
    {

        InitFirebase();
       
    }

    void InitFirebase()
    {
        auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
       
    }

    public void Login()
    {
        Username.text= "Login";
        GoogleSignIn.Configuration = new GoogleSignInConfiguration
        {
            // Copy this value from the google-service.json file.
            // oauth_client with type == 3
            WebClientId = GoogleAPI,
             RequestIdToken = true,
            RequestEmail = true,
            UseGameSignIn = false,
        };
        GoogleSignIn.Configuration.RequestEmail = true;

        try
        {
            GoogleSignIn.DefaultInstance.SignIn().ContinueWith(OnGoogleAuthComplete, TaskScheduler.Default);
        }
        catch (System.Exception ex)
        {
            errInput.text = "Exception during Sign-In: " + ex.Message;
        }



    }
    private void OnGoogleAuthComplete(Task<GoogleSignInUser> task)
    {
        TaskCompletionSource<FirebaseUser> signInCompleted = new TaskCompletionSource<FirebaseUser>();


        if (task.IsCanceled)
        {
            signInCompleted.SetCanceled();
            Debug.Log("Cancelled");
        }
        else if (task.IsFaulted)
        {
            signInCompleted.SetException(task.Exception);

            Debug.Log("Faulted " + task.Exception);

        }
        else
        {
            var googleUser = task.Result;
            Debug.Log("Google User ID Token: " + googleUser.IdToken);

            Credential credential = GoogleAuthProvider.GetCredential(googleUser.IdToken, null);
            auth.SignInWithCredentialAsync(credential).ContinueWith(authTask =>
            {
                if (authTask.IsCanceled)
                {
                    signInCompleted.SetCanceled();
                }
                else if (authTask.IsFaulted)
                {
                    signInCompleted.SetException(authTask.Exception);
                    Debug.Log("Faulted In Auth: " + authTask.Exception);
                }
                else
                {
                    signInCompleted.SetResult(((Task<FirebaseUser>)authTask).Result);
                    FirebaseUser user = auth.CurrentUser;
                    if (user != null)
                    {
                        Debug.Log($"User Signed In: {user.DisplayName}, {user.Email}");
                        UpdateUI(user.DisplayName, user.Email, user.PhotoUrl != null ? user.PhotoUrl.ToString() : null);
                    }
                    else
                    {
                        Username.text = "data is not fetch";
                    }
                    /*
                    Username.text = authTask.Result.DisplayName;
                    UserEmail.text = authTask.Result.Email;
                    Debug.Log("User is " + authTask.Result.DisplayName + " and Email is " + authTask.Result.Email);
                    if (authTask.Result.PhotoUrl != null)
                    {
                        StartCoroutine(LoadImage(authTask.Result.PhotoUrl.ToString()));
                    }
                    */
                }
            });
        }
    }


    private void UpdateUI(string username, string email, string imageUrl)
    {
        Username.text = username;
        UserEmail.text = email;

        if (!string.IsNullOrEmpty(imageUrl))
        {
            StartCoroutine(LoadImage(imageUrl));
        }
    }


    IEnumerator LoadImage(string imageUri)
    {
        WWW www = new WWW(imageUri);
        yield return www;

        UserProfilePic.sprite = Sprite.Create(www.texture, new Rect(0, 0, www.texture.width, www.texture.height), new Vector2(0, 0));
    }
    public void OnSignOut()
    {
        GoogleSignIn.DefaultInstance.SignOut();
    }

}// end of class
