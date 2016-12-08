using UnityEngine;
using System.Collections;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using UnityEngine.SocialPlatforms;

/// <summary>
/// store Leaderboard and Achievements ID Key
/// </summary>
public class KeyIds : MonoBehaviour {

    public static KeyIds instance = null;

    public string leaderboardCasualIdKey;
    public string leaderboardTimedIdKey;
    public string leaderboardMovesIdKey;
    public string leaderboardLIdKey;

    public string achievementFirstPlay;
    public string achievementRectsAddict;
    public string achievementRectsWallet;
    public string achievementRectsPiggyBank;
    public string achievementRectsBanker;
    public string achievementCasualMaster;
    public string achievementTimeWalker;
    public string achievementEfficientMove;
    public string achievementLCrusher;
    public string achievementFirstBuy;
    public string achievementShopper;

    public string unityAdsKey;

    public string admobBannerKey;
    public string admobInterstitialKey;

    // Use this for initialization
    void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }
}
