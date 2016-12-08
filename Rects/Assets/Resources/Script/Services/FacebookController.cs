using UnityEngine;
using System.Collections;

/// <summary>
/// Facebook Controller for login and share score
/// </summary>
public class FacebookController {

    public static FacebookController sInstance = new FacebookController();
    public static FacebookController Instance
    {
        get { return sInstance; }
    }

    public void FBInit()
    {
        FB.Init(OnInitComplete, OnHideUnity);
    }
    public void OnInitComplete()
    {
        Debug.Log("FB.Init completed: Is user logged in? " + FB.IsLoggedIn);
    }

    public void OnHideUnity(bool isGameShown)
    {
        Debug.Log("Is game showing? " + isGameShown);
    }

    public void FBLogin()
    {
        if (SignIn)
            return;
        FB.Login();
    }

    int shareScoreAttemp = 0;
    public void ShareScore(int score, out bool shareScore)
    {
        if (SignIn)
        {
            FB.Feed(
                link: "http://play.google.com/publish/com.cgranule.rects",
                linkName: "Rects",
                linkCaption: "Can you beat it? My score " + score.ToString()
                );
            shareScore = false;
            shareScoreAttemp = 0;
        }
        else
        {
            FBLogin();
            if (shareScoreAttemp >= 1)
                shareScore = false;
            else
                shareScore = true;
        }
        shareScoreAttemp += 1;
    }

    public bool SignIn {
        get { return FB.IsLoggedIn; }
    }
}
