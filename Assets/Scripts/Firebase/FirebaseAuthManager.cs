using Firebase;
using Firebase.Auth;
using Firebase.Extensions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FirebaseAuthManager : MonoBehaviour
{
    // Firebase variables
    private FirebaseAuth auth;

    // UI Elements for Register and Login
    public TMP_InputField usernameInput;
    public TMP_InputField emailInput;
    public TMP_InputField passwordInput;
    public TMP_InputField confirmPasswordInput;
    public Button registerButton;
    public Button loginButton;
    public TMP_Text statusText;

    // Start is called before the first frame update
    void Start()
    {
        // Initialize Firebase
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
        {
            FirebaseApp app = FirebaseApp.DefaultInstance;
            auth = FirebaseAuth.DefaultInstance;
        });

        // Register button click listener
        registerButton.onClick.AddListener(OnRegisterButtonClicked);
        loginButton.onClick.AddListener(OnLoginButtonClicked);
    }

    // Register button clicked
    public void OnRegisterButtonClicked()
    {
        string username = usernameInput.text;
        string email = emailInput.text;
        string password = passwordInput.text;
        string confirmPassword = confirmPasswordInput.text;

        if (password != confirmPassword)
        {
            statusText.text = "Passwords do not match!";
            return;
        }

        // Create user with email and password
        auth.CreateUserWithEmailAndPasswordAsync(email, password).ContinueWithOnMainThread(task =>
        {
            if (task.IsCanceled)
            {
                statusText.text = "Registration canceled.";
                return;
            }
            if (task.IsFaulted)
            {
                statusText.text = "Error: " + task.Exception.Flatten().Message;
                return;
            }

            FirebaseUser newUser = task.Result.User;

            // Optionally, set the username as part of the user's profile
            UserProfile profile = new UserProfile { DisplayName = username };
            newUser.UpdateUserProfileAsync(profile).ContinueWithOnMainThread(profileTask =>
            {
                if (profileTask.IsCanceled)
                {
                    statusText.text = "Profile update canceled.";
                    return;
                }
                if (profileTask.IsFaulted)
                {
                    statusText.text = "Profile update error: " + profileTask.Exception.Flatten().Message;
                    return;
                }
                statusText.text = "Registration successful!";
            });
        });
    }

    // Login button clicked
    public void OnLoginButtonClicked()
    {
        string email = emailInput.text;
        string password = passwordInput.text;

        // Sign in the user
        auth.SignInWithEmailAndPasswordAsync(email, password).ContinueWithOnMainThread(task =>
        {
            if (task.IsCanceled)
            {
                statusText.text = "Login canceled.";
                return;
            }
            if (task.IsFaulted)
            {
                statusText.text = "Error: " + task.Exception.Flatten().Message;
                return;
            }

            FirebaseUser user = task.Result.User;
            statusText.text = "Login successful! Welcome " + user.DisplayName;
        });
    }
}
