using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AuthenticationService
{
    public delegate void AuthenticationServiceDelegate();
    public static AuthenticationServiceDelegate afterAuthentication;
    public static AuthenticationServiceDelegate afterSignOut;

    static string EMAIL = "email";
    static string AUTHORIZED = "authorized";

    public static bool CurrentEmail(out string email)
    {
        email = PlayerPrefs.GetString(EMAIL, null);
        return !string.IsNullOrEmpty(email);
    }

    public static bool IsAuthorized()
    {
        return !string.IsNullOrEmpty(PlayerPrefs.GetString(EMAIL, null));
    }

    public static void SignIn(string email)
    {
       
        if (email != PlayerPrefs.GetString(EMAIL, null))
        {
            SetEmail(email);
            Debug.Log("---- SignIn ----");
            afterAuthentication?.Invoke();
        }
    }

    /// <summary>
    /// Can now trigger SignOut via Hamburger Menu.
    /// Can also trigger SignOut from the Unity Editor via Edit->Clear Player Prefs.
    /// </summary>
    public static void SignOut()
    {
        Debug.Log("SignOut");
        SetEmail(null);
        afterSignOut?.Invoke();
    }

    static void SetEmail(string email)
    {
        Debug.Log("Setting email: " + email);
        PlayerPrefs.SetString(EMAIL, email);
    }
}
