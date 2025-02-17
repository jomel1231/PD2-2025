using UnityEngine;
using Firebase;
using Firebase.Auth;
using TMPro;

public class FirebaseLogin : MonoBehaviour
{
    private FirebaseAuth auth;

    public TMP_InputField emailInput;    // Input field for email
    public TMP_InputField passwordInput; // Input field for password
    public TextMeshProUGUI statusText;   // Text to display status messages

    public SceneManagerScript sceneManager; // Reference to SceneManagerScript

    void Start()
    {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task => {
            if (task.Result == DependencyStatus.Available)
            {
                FirebaseApp app = FirebaseApp.DefaultInstance;
                auth = FirebaseAuth.DefaultInstance;
                Debug.Log("Firebase Initialized Successfully!");
            }
            else
            {
                Debug.LogError("Could not resolve Firebase dependencies: " + task.Result);
            }
        });
    }

    public void Login()
    {
        string email = emailInput.text;
        string password = passwordInput.text;

        auth.SignInWithEmailAndPasswordAsync(email, password).ContinueWith(task => {
            if (task.IsCanceled || task.IsFaulted)
            {
                Debug.LogError("Login failed: " + task.Exception);
                UpdateStatus("Login failed. Please check your credentials.");
                return;
            }

            // Successful login
            Debug.Log("User logged in successfully!");
            UpdateStatus("Login successful! Redirecting...");

            // Call SceneManagerScript to load the DASHBOARD scene
            sceneManager.LoadScene("DASHBOARD");
            sceneManager.LoadScene("RegisterManager");
        });
    }

    private void UpdateStatus(string message)
    {
        if (statusText != null)
        {
            statusText.text = message;
        }
    }
}
