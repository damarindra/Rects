using UnityEngine;
using System.Collections;
using System;

public class SaveLoad {
    public static SaveLoad sInstance = new SaveLoad();
    public static SaveLoad Instance {
        get { return sInstance; }
    }

    /*public void OnLoadProgress()
    {
        GPGSController.Instance.LoadFromCloud();
    }
    public void OnSaveProgress()
    {
        GPGSController.Instance.SaveProgress();
    }


    public static void SaveScore(int score)
    {
        if (score > PlayerPrefs.GetInt("BestScore-" + Game.instance.gameType.ToString()))
        {
            PlayerPrefs.SetInt("BestScore-" + Game.instance.gameType.ToString(), score);
            //GPGS
            if (Game.instance.gameType == Game.GameType.casual)
            {
                GPGSController.Instance.ReportScoreLeaderboard(KeyIds.instance.leaderboardCasualIdKey,score);
            }
            else if (Game.instance.gameType == Game.GameType.timed)
            {
                GPGSController.Instance.ReportScoreLeaderboard(KeyIds.instance.leaderboardTimedIdKey, score);
            }
            else if (Game.instance.gameType == Game.GameType.moves)
            {
                GPGSController.Instance.ReportScoreLeaderboard(KeyIds.instance.leaderboardMovesIdKey, score);
            }
            else if (Game.instance.gameType == Game.GameType.letterL)
            {
                GPGSController.Instance.ReportScoreLeaderboard(KeyIds.instance.leaderboardLIdKey, score);
            }
        }

        //GPGS
        if (PlayerPrefs.GetInt("BestScore-" + Game.instance.gameType.ToString()) > 3000)
        {
            if (Game.instance.gameType == Game.GameType.casual)
            {
                GPGSController.Instance.UnlockingAchievement(KeyIds.instance.achievementCasualMaster);
            }
            else if (Game.instance.gameType == Game.GameType.timed)
            {
                GPGSController.Instance.UnlockingAchievement(KeyIds.instance.achievementTimeWalker);
            }
            else if (Game.instance.gameType == Game.GameType.moves)
            {
                GPGSController.Instance.UnlockingAchievement(KeyIds.instance.achievementEfficientMove);
            }
            else if (Game.instance.gameType == Game.GameType.letterL)
            {
                GPGSController.Instance.UnlockingAchievement(KeyIds.instance.achievementLCrusher);
            }
        }
    }

    public static void getBestScore(out int score, Game.GameType type)
    {
        score = PlayerPrefs.GetInt("BestScore-" + type.ToString());
    }

    public static void PlusRects(int rectsPlus)
    {
        PlayerPrefs.SetInt("RectCollected", getRects + rectsPlus);
    }

    public static int getRects
    {
        get { return PlayerPrefs.GetInt("RectCollected"); }
    }

    public static void BuyPowerUp(GameManager.PowerUpPick type)
    {
        PlayerPrefs.SetInt("PowerUp-" + type.ToString(), GetPowerUp(type) + 5);
    }

    public static void UsePowerUp(GameManager.PowerUpPick type)
    {
        PlayerPrefs.SetInt("PowerUp-" + type.ToString(), GetPowerUp(type) - 1);
    }

    public static int GetPowerUp(GameManager.PowerUpPick type)
    {
        return PlayerPrefs.GetInt("PowerUp-" + type.ToString());
    }

    public static void SetUpPowerUpAtFirstTime()
    {
        if (PlayerPrefs.GetInt("FirstInitialze") == 0)
        {
            PlayerPrefs.SetInt("PowerUp-" + GameManager.PowerUpPick.colorPick.ToString(), 5);
            PlayerPrefs.SetInt("PowerUp-" + GameManager.PowerUpPick.cross.ToString(), 5);
            PlayerPrefs.SetInt("PowerUp-" + GameManager.PowerUpPick.refresh.ToString(), 5);
            PlayerPrefs.SetInt("Sound", 1);
            PlayerPrefs.SetInt("Vibration", 1);
            PlayerPrefs.SetInt("FirstInitialze", 1);
        }
        else
            LoadSettings();
    }

    public static void LoadSettings()
    {
    }*/
}
