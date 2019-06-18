using Firebase.Auth;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FirebaseLogin : MonoBehaviour
{

    #region Variables

    private FirebaseAuth auth;
    private FirebaseUser user;
    public InputField Email, Mdp;

    #endregion

    #region Monobehaviour

    void Start()
    {
        InitializeFirebase();
    }

    #endregion

    #region Firebase

    void InitializeFirebase()
    {
        auth = FirebaseAuth.DefaultInstance;
        auth.SignOut();
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
                SceneManager.LoadScene("SceneAR");
            }
        }
    }

    #endregion

    #region Buttons Functions

    public void ButtonConnexion()
    {
        auth.SignInWithEmailAndPasswordAsync(Email.text, Mdp.text).ContinueWith(task =>
        {
            if (task.IsCanceled)
            {
                Debug.Log("SignInWithEmailAndPasswordAsync was canceled.");
                return;
            }
            if (task.IsFaulted)
            {
                Debug.Log("SignInWithEmailAndPasswordAsync encountered an error: " + task.Exception);
                return;
            }

            FirebaseUser newUser = task.Result;
            Debug.LogFormat("User signed in successfully: {0} ({1})",
                newUser.DisplayName, newUser.UserId);
        });
    }

    public void BoutonInscription()
    {
        auth.CreateUserWithEmailAndPasswordAsync(Email.text, Mdp.text).ContinueWith(task => {
            if (task.IsCanceled)
            {
                Debug.Log("CreateUserWithEmailAndPasswordAsync was canceled.");
                return;
            }
            if (task.IsFaulted)
            {
                Debug.Log("CreateUserWithEmailAndPasswordAsync encountered an error: " + task.Exception);
                return;
            }

            FirebaseUser newUser = task.Result;
            Debug.LogFormat("Firebase user created successfully: {0} ({1})",
                newUser.DisplayName, newUser.UserId);
        });
    }

    #endregion

}
