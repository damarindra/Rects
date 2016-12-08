using UnityEngine;
using System.Collections.Generic;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using UnityEngine.SocialPlatforms;
using GooglePlayGames.BasicApi.SavedGame;
using System;

public class GPGSController {

    public static GPGSController sInstance = new GPGSController();
    public static GPGSController Instance
    {
        get { return sInstance; }
    }

    Dictionary<string, int> pendingIncrement = new Dictionary<string, int>();
    public GameProgress mProgress = new GameProgress();
    
    private bool mSaving;

    private string mAutoSaveName = "AutoSaved";
    private Texture2D mScreenImage;

    public enum GPGSStatus {
        nope, authenticating, signIn, failed
    }
    public GPGSStatus status = GPGSStatus.nope;
    public void CaptureScreenshot()
    {
        mScreenImage = new Texture2D(Screen.width, Screen.height);
        mScreenImage.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
        mScreenImage.Apply();
        Debug.Log("Captured screen: " + mScreenImage);
    }
    public void InitializeGooglePlayGames()
    {
        mProgress = GameProgress.LoadFromDisk();
        Debug.Log("Load");
        mAutoSaveName = "AutoSaved";

        if (status == GPGSStatus.signIn || status == GPGSStatus.authenticating)
            return;
        PlayGamesClientConfiguration config = new PlayGamesClientConfiguration.Builder().EnableSavedGames()
            // enables saving game progress.
            .Build();
        PlayGamesPlatform.InitializeInstance(config);
        // recommended for debugging:
        PlayGamesPlatform.DebugLogEnabled = true;
        // Activate the Google Play Games platform
        PlayGamesPlatform.Activate();
        SignIn();
    }


    public void SaveProgress()
    {
        mProgress.SaveToDisk();
        //SaveToCloud(null);
        SaveToCloud(mAutoSaveName);
    }
    public void AutoSave()
    {
        if (mProgress.Dirty)
        {
            mProgress.SaveToDisk();
            SaveToCloud(mAutoSaveName);
        }
    }
    void SaveToCloud(string filename)
    {
        if (status == GPGSStatus.signIn)
        {
            mSaving = true;
            if (filename == null)
            {
                ((PlayGamesPlatform)Social.Active).SavedGame.ShowSelectSavedGameUI("Save game progress",
                                                                               1, true, true, SavedGameSelected);
            }
            else
            {
                // save to named file
                ((PlayGamesPlatform)Social.Active).SavedGame.OpenWithAutomaticConflictResolution(filename,
                                                                                         DataSource.ReadCacheOrNetwork,
                                                                                         ConflictResolutionStrategy.UseLongestPlaytime,
                                                                                         SavedGameOpened);
            }
        }
    }
    public void SavedGameSelected(SelectUIStatus status, ISavedGameMetadata game)
    {
        if (status == SelectUIStatus.SavedGameSelected)
        {
            string filename = game.Filename;
            Debug.Log("opening saved game:  " + game);
            if (filename == null || filename.Length == 0)
            {
                filename = "save" + DateTime.Now.ToBinary();
            }
            //open the data.
            ((PlayGamesPlatform)Social.Active).SavedGame.OpenWithAutomaticConflictResolution(filename,
                                                                                             DataSource.ReadCacheOrNetwork,
                                                                                             ConflictResolutionStrategy.UseLongestPlaytime,
                                                                                             SavedGameOpened);
        }
        else
        {
            Debug.LogWarning("Error selecting save game: " + status);
        }
    }
    public void SavedGameOpened(SavedGameRequestStatus status, ISavedGameMetadata game)
    {
        if (status == SavedGameRequestStatus.Success)
        {
            if (mSaving)
            {
                if (mScreenImage == null)
                {
                    CaptureScreenshot();
                }
                byte[] pngData = (mScreenImage != null) ? mScreenImage.EncodeToPNG() : null;
                Debug.Log("Saving to " + game);
                byte[] data = mProgress.ToBytes();
                TimeSpan playedTime = mProgress.TotalPlayTime;
                
                SavedGameMetadataUpdate.Builder builder = new
                    SavedGameMetadataUpdate.Builder()
                        .WithUpdatedPlayedTime(playedTime)
                        .WithUpdatedDescription("Saved Game at " + DateTime.Now);

                if (pngData != null)
                {
                    Debug.Log("Save image of len " + pngData.Length);
                    builder = builder.WithUpdatedPngCoverImage(pngData);
                }
                else
                {
                    Debug.Log("No image avail");
                }
                SavedGameMetadataUpdate updatedMetadata = builder.Build();
                ((PlayGamesPlatform)Social.Active).SavedGame.CommitUpdate(game, updatedMetadata, data, SavedGameWritten);
            }
            else
            {
                mAutoSaveName = game.Filename;
                ((PlayGamesPlatform)Social.Active).SavedGame.ReadBinaryData(game, SavedGameLoaded);
            }
        }
        else
        {
            Debug.LogWarning("Error opening game: " + status);
        }
    }
    public void SavedGameLoaded(SavedGameRequestStatus status, byte[] data)
    {
        if (status == SavedGameRequestStatus.Success)
        {
            Debug.Log("SaveGameLoaded, success=" + status);
            ProcessCloudData(data);
        }
        else
        {
            Debug.LogWarning("Error reading game: " + status);
        }
    }
    public void SavedGameWritten(SavedGameRequestStatus status, ISavedGameMetadata game)
    {
        if (status == SavedGameRequestStatus.Success)
        {
            Debug.Log("Game " + game.Description + " written");
        }
        else
        {
            Debug.LogWarning("Error saving game: " + status);
        }
    }
    void ProcessCloudData(byte[] cloudData)
    {
        if (cloudData == null)
        {
            Debug.Log("No data saved to the cloud yet...");
            return;
        }
        Debug.Log("Decoding cloud data from bytes.");
        GameProgress progress = GameProgress.FromBytes(cloudData);      //convert from byte to GameProgress
        Debug.Log("Merging with existing game progress.");
        mProgress.MergeWith(progress);      //For merging file save from local and cloud, take the right data for saving data
    }
    public void LoadFromCloud()
    {
        // Cloud save is not in ISocialPlatform, it's a Play Games extension,
        // so we have to break the abstraction and use PlayGamesPlatform:
        Debug.Log("Loading game progress from the cloud.");
        mSaving = false;
        ((PlayGamesPlatform)Social.Active).SavedGame.ShowSelectSavedGameUI("Select saved game to load",
                                                                           4, false, false, SavedGameSelected);
    }

    public void SignIn()
    {
        status = GPGSStatus.authenticating;
        Social.localUser.Authenticate((bool success) => 
        {
            if (success)
                status = GPGSStatus.signIn;
            else
                status = GPGSStatus.failed;
        });
    }
    public void ShowLeaderboard()
    {
        if (status == GPGSStatus.signIn)
            Social.ShowLeaderboardUI();
        else
            InitializeGooglePlayGames();
    }
    public void ShowLeaderboard(string id)
    {
        if (status == GPGSStatus.signIn)
            PlayGamesPlatform.Instance.ShowLeaderboardUI(id);
        else
            InitializeGooglePlayGames();
    }
    public void ReportScoreLeaderboard(string id, int score)
    {
        if (status == GPGSStatus.signIn)
        {
            Social.ReportScore(score, id, (bool success) => { });
        }
    }
    public void ShowAchievement()
    {
        if (status == GPGSStatus.signIn)
            Social.ShowAchievementsUI();
        else
            InitializeGooglePlayGames();
    }
    public void UnlockingAchievement(string id)
    {
        if (status == GPGSStatus.signIn)
        {
            if (PlayerPrefs.GetInt(id) != 1)
            {
                Social.ReportProgress(id, 100.0f, (bool success) => { });
                PlayerPrefs.SetInt(id, 1);
            }
        }
    }
    public void IncrementingAchievement(string id, int steps)
    {
        if (status == GPGSStatus.signIn)
        {
            if (pendingIncrement.ContainsKey(id))
            {
                steps += pendingIncrement[id];
            }
            pendingIncrement[id] = steps;
        }
    }
    public void FlushAchievement()
    {
        if (status == GPGSStatus.signIn)
        {
            foreach (string ach in pendingIncrement.Keys)
            {
                PlayGamesPlatform.Instance.IncrementAchievement(ach, pendingIncrement[ach], (bool success) => { });
            }
            pendingIncrement.Clear();
        }
    }
}
