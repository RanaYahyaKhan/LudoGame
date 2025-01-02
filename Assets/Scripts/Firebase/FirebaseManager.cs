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
    Firebase.Auth.FirebaseAuth auth;
    Firebase.Auth.FirebaseUser user;

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
        //auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
        {
            FirebaseApp app = FirebaseApp.DefaultInstance;
            auth = FirebaseAuth.DefaultInstance;
        });
    }

    public void Login()
    {
       
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
        Username.text = "Login";
        if (!task.IsCanceled && !task.IsFaulted || task.IsCompleted) 
        {
            Credential credential = GoogleAuthProvider.GetCredential(((Task<GoogleSignInUser>)task).Result.IdToken, null);
            auth.SignInWithCredentialAsync(credential).ContinueWith(authTask =>
            {
                if (!authTask.IsCanceled && !authTask.IsFaulted || authTask.IsCompleted)
                {
                    signInCompleted.SetResult(((Task<FirebaseUser>)authTask).Result);
                    Debug.Log("Success");
                    Username.text = "Success";
                    user = FirebaseAuth.DefaultInstance.CurrentUser;
                    //user = auth.CurrentUser;
                    Username.text = user.DisplayName;
                    UserEmail.text = user.Email;

                    StartCoroutine(LoadImage(CheckImageUrl(user.PhotoUrl.ToString())));
                }
                else
                {
                    Debug.LogError("some this is wrong in credential");
                }
             
            });
        }
        else
        {
            Debug.LogError("some this is wrong in sigin");
        }

        /*
        if (task.IsCanceled )
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
            Credential credential = Firebase.Auth.GoogleAuthProvider.GetCredential(((Task<GoogleSignInUser>)task).Result.IdToken, null);
            auth.SignInWithCredentialAsync(credential).ContinueWith(authTask =>
            {
                if (authTask.IsCanceled)
                {
                    signInCompleted.SetCanceled();
                }
                else if (authTask.IsFaulted)
                {
                    signInCompleted.SetException(authTask.Exception);
                    Debug.Log("Faulted In Auth " + task.Exception);
                    Username.text = "Faulted In Auth " + task.Exception;
                }
                else
                {
                    signInCompleted.SetResult(((Task<FirebaseUser>)authTask).Result);
                    Debug.Log("Success");
                    Username.text = "Success";
                    user = auth.CurrentUser;
                    Username.text = user.DisplayName;
                    UserEmail.text = user.Email;

                    StartCoroutine(LoadImage(CheckImageUrl(user.PhotoUrl.ToString())));
                }
            });
        }
        */
    }


    private string CheckImageUrl(string url)
    {
        if (!string.IsNullOrEmpty(url))
        {
            return url;
        }
        return imageUrl;
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

}// end f cass
