using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Firebase;
using Firebase.Auth;
using Firebase.Firestore;
using UnityEngine.SceneManagement;

public class FirebaseAuthManager : MonoBehaviour
{

    // Write summaries for everything

    #region Variables

    //Firebasevariable

    [Header("Firebase")]
    public DependencyStatus dependencyStatus;
    public FirebaseAuth auth;
    public FirebaseUser user;

    // Login Variables
    [Space]
    [Header("Login")]
    public InputField emailLoginField;
    public InputField passwordLoginField;


    // Registration Variables

    [Space]
    [Header("Registartion")]
    public InputField nameRegisterField;
    public InputField addressRegisterField;
    public InputField emailRegisterField;
    public InputField phoneNumberRegisterField;
    public InputField yearOfBirthRegisterField;
    public InputField dateOfBirthRegisterField;
    public InputField passwordRegisterField;
    public InputField confirmPasswordRegisterField;

    private string userId;

    #endregion

    #region initialyze
    private void Awake()
    {
        // Check that all of the necessary dependencies for firebase are present on the system
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
        {
            dependencyStatus = task.Result;
            if (dependencyStatus == DependencyStatus.Available)
            {
               /* InitializeFirebase();*/

                if (auth.CurrentUser != null)
                    auth.SignOut();
            }
            else
            {
                Debug.LogError("Could not resolve all firebase dependencies: " + dependencyStatus);
            }
        });
    }

    void InitializeFirebase()
    {
        //Set the default instance object
        auth = FirebaseAuth.DefaultInstance;
        auth.StateChanged += AuthStateChanged;
        AuthStateChanged(this, null);
    }


    void AuthStateChanged(object sender, System.EventArgs eventArgs)
    {
        if (auth.CurrentUser != user)
        {
            bool signedIn = user != auth.CurrentUser && auth.CurrentUser != null;
            if (!signedIn && user != null)
            {
                Debug.Log("Signed out " + user.UserId);
            }
            user = auth.CurrentUser;
            if (signedIn)
            {
                Debug.Log("Signed in " + user.UserId);
            }
        }
    }

    #endregion


    #region Login method
    public void Login()
    {
        StartCoroutine(LoginAsync(emailLoginField.text, passwordLoginField.text));
    }
    private IEnumerator LoginAsync(string email, string password)
    {
        var loginTask = auth.SignInWithEmailAndPasswordAsync(email, password);
        yield return new WaitUntil(() => loginTask.IsCompleted);
        if (loginTask.Exception != null)
        {
            Debug.LogError(loginTask.Exception);
            FirebaseException firebaseException = loginTask.Exception.GetBaseException() as FirebaseException;
            AuthError authError = (AuthError)firebaseException.ErrorCode;
            string failedMessage = "Login Failed! Because ";
            switch (authError)
            {
                case AuthError.InvalidEmail:
                    failedMessage += "Email is invalid";
                    break;
                case AuthError.WrongPassword:
                    failedMessage += "Wrong Password";
                    break;
                case AuthError.MissingEmail:
                    failedMessage += "Email is missing";
                    break;
                case AuthError.MissingPassword:
                    failedMessage += "Password is missing";
                    break;
                default:
                    failedMessage = "Login Failed";
                    break;
            }
            Debug.Log(failedMessage);
        }
        else
        {
            //string interpulation  
            user = loginTask.Result;

            Debug.LogFormat("{0} You Are Successfully Logged In", user.DisplayName);

            SceneManager.LoadScene("GameScene");


        }
    }
    #endregion


    #region Register Method
    public void Register()
    {
        StartCoroutine(RegisterAsync(nameRegisterField.text, emailRegisterField.text, passwordRegisterField.text, confirmPasswordRegisterField.text, addressRegisterField.text, int.Parse(phoneNumberRegisterField.text), dateOfBirthRegisterField.text, int.Parse(yearOfBirthRegisterField.text)));
    }

    private void AddUserToDb(string userID, UserInfo userInfo)
    {
        var firestore = FirebaseFirestore.DefaultInstance;
        firestore.Document($"users/{userID}").SetAsync(userInfo);
    }


    private IEnumerator RegisterAsync(string name, string email, string password, string confirmPassword, string address, int phonenumber, string dateOfBirth, int yearOfBirth)
    {
        if (name == "")
        {
            Debug.LogError("User Name is empty");
        }
        else if (email == "")
        {
            Debug.LogError("email field is empty");
        }
        else if (passwordRegisterField.text != confirmPasswordRegisterField.text)
        {
            Debug.LogError("Password does not match");
        }
        else
        {
            var registerTask = auth.CreateUserWithEmailAndPasswordAsync(email, password);
            yield return new WaitUntil(() => registerTask.IsCompleted);
            if (registerTask.Exception != null)
            {
                Debug.LogError(registerTask.Exception);
                FirebaseException firebaseException = registerTask.Exception.GetBaseException() as FirebaseException;
                AuthError authError = (AuthError)firebaseException.ErrorCode;
                string failedMessage = "Registration Failed! Becuase ";
                switch (authError)
                {
                    case AuthError.InvalidEmail:
                        failedMessage += "Email is invalid";
                        break;
                    case AuthError.WrongPassword:
                        failedMessage += "Wrong Password";
                        break;
                    case AuthError.MissingEmail:
                        failedMessage += "Email is missing";
                        break;
                    case AuthError.MissingPassword:
                        failedMessage += "Password is missing";
                        break;
                    default:
                        failedMessage = "Registration Failed";
                        break;
                }
                Debug.Log(failedMessage);
            }
            else
            {
                // Get The User After Registration Success
                user = registerTask.Result;

                UserProfile userProfile = new UserProfile { DisplayName = name };

                var updateProfileTask = user.UpdateUserProfileAsync(userProfile);
                yield return new WaitUntil(() => updateProfileTask.IsCompleted);
                if (updateProfileTask.Exception != null)
                {
                    // Delete the user if user update failed
                    user.DeleteAsync();
                    Debug.LogError(updateProfileTask.Exception);
                    FirebaseException firebaseException = updateProfileTask.Exception.GetBaseException() as FirebaseException;
                    AuthError authError = (AuthError)firebaseException.ErrorCode;
                    string failedMessage = "Profile update Failed! Becuase ";
                    switch (authError)
                    {
                        case AuthError.InvalidEmail:
                            failedMessage += "Email is invalid";
                            break;
                        case AuthError.WrongPassword:
                            failedMessage += "Wrong Password";
                            break;
                        case AuthError.MissingEmail:
                            failedMessage += "Email is missing";
                            break;
                        case AuthError.MissingPassword:
                            failedMessage += "Password is missing";
                            break;
                        default:
                            failedMessage = "Profile update Failed";
                            break;
                    }
                    Debug.Log(failedMessage);
                }
                else
                {

                    try
                    {

                        userId = user.UserId;
                        Debug.Log("Registration Sucessful Welcome " + user.DisplayName);

                        //var Firestore = FirebaseFirestore.DefaultInstance;


                        UserInfo newUser = new UserInfo()
                        {
                            Name = name,
                            Address = address,
                            Email = email,
                            PhoneNumber = phonenumber,
                            DateOfBirth = dateOfBirth,
                            YearOfBirth = yearOfBirth
                        };

                        var firestore = FirebaseFirestore.DefaultInstance;
                        firestore.Document($"users/{userId}").SetAsync(newUser);
                        Debug.Log($"Added user: {userId}");
                        AddUserToDb(userId, newUser);
                       /* UIManager.Instance.OpenLoginPanel();*/
                    }
                    catch (UnityException e)
                    {
                        Debug.Log(e.Message);
                    }



                }
            }
        }
    }

    #endregion



}
