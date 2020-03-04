using System.Collections;
using System.Collections.Generic;
using Firebase.Auth;
using Firebase;
using Firebase.Extensions;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class AuthManager : MonoBehaviour
{
    public bool IsFirebaseReady { get; private set; }
    public bool IsSignInOnProgress { get; private set; }
    public InputField emailField;
    public InputField passwordField;
    public Button signInButton;
    public Button signUpButton;
    public Button XButton;
    public GameObject signUpPanel;
    public Text SignUpResultText;

    public InputField SignUp_EmailField;
    public InputField SignUp_PasswordField;

    public static FirebaseApp firebaseApp;
    public static FirebaseAuth firebaseAuth;

    public static FirebaseUser User;

    private string Name;
    void Start()
    {
        signInButton.interactable = false;     
        passwordField.inputType = InputField.InputType.Password;
        SignUp_PasswordField.inputType = InputField.InputType.Password;

        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
        {
            var result = task.Result;
            if (result != DependencyStatus.Available)
            {
                Debug.LogError(result.ToString());
                IsFirebaseReady = false;
            }
            else
            {
                IsFirebaseReady = true;

                firebaseApp = FirebaseApp.DefaultInstance;
                firebaseAuth = FirebaseAuth.DefaultInstance;
            }
            signInButton.interactable = IsFirebaseReady;
        });
    }
   
    public void SignIn()
    {
        if (!IsFirebaseReady || IsSignInOnProgress || User != null)
            return;

        IsSignInOnProgress = true;
        signInButton.interactable = false;

        firebaseAuth.SignInWithEmailAndPasswordAsync(emailField.text, passwordField.text).ContinueWithOnMainThread((task) =>
        {
            Debug.Log(message: $"Sign in status : {task.Status}");

            IsSignInOnProgress = false;
            signInButton.interactable = true;

            if (task.IsFaulted)
            {
                Debug.LogError(task.Exception);
            }
            else if (task.IsCanceled)
            {
                Debug.LogError(message: "Sign-in canceled");
            }
            else
            {
                User = task.Result;
                Debug.Log(User.Email);
                SceneManager.LoadScene("Lobby");
            }
        });
    }

    public void SignUp()
    {
        string email = SignUp_EmailField.text;
        string password = SignUp_PasswordField.text;

        if (email.Length != 0 && password.Length != 0)
        {
            firebaseAuth.CreateUserWithEmailAndPasswordAsync(email, password).ContinueWith(
                task =>
                {
                    if (!task.IsCanceled && !task.IsFaulted)
                    {
                        SignUpResultText.text = "회원가입 성공!";
                    }
                    else
                    {
                        SignUpResultText.text = "회원가입 실패(공백을 입력하셨습니다.)!";
                    }
                });
        }
        signUpPanel.SetActive(false);
    }

    public void OnClick_ShowSignUpPanel()
    {
        signUpPanel.SetActive(true);
    }

    public void OnClick_Return()
    {
        signUpPanel.SetActive(false);
    }
}
