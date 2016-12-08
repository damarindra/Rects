using UnityEngine;
using System;
using System.Collections.Generic;

public class GameProgress {
    private const string PlayerPrefsKey = "Rects-Saved-Game";

    private int bestScoreCasual = 0;
    private int bestScoreTimed = 0;
    private int bestScoreMoves = 0;
    private int bestScoreL = 0;
    private int rectBank = 0;

    private int puExecuteBank = 0;
    private int puCrusherBank = 0;
    private int puRefreshBank = 0;

    //private bool[] themeOpened;// = new bool[3] { true, true, false};
    public List<bool> themeColorOpened = new List<bool>();
    public List<bool> themeSpriteOpened = new List<bool>();

    private TimeSpan timePlay;
    private DateTime loadedTime;

    // do we have modifications to write to disk/cloud?
    private bool mDirty;

    public GameProgress() {
        loadedTime = DateTime.Now;
        puCrusherBank = 2;
        puExecuteBank = 2;
        puRefreshBank = 2;
        themeColorOpened.Add(true);
        themeSpriteOpened.Add(false);
    }

    public static GameProgress LoadFromDisk()
    {
        string s = PlayerPrefs.GetString(PlayerPrefsKey, ""); //using 2 parameter, one is key, and one is default value
        //string s = KeyValueStorage.GetValue(PlayerPrefsKey);
        if (s == null || s.Trim().Length == 0) 
        {
            return new GameProgress();
        }
        Debug.Log("Saved File " +s);
        return GameProgress.FromString(s);
    }
    public TimeSpan TotalPlayTime
    {
        get { TimeSpan delta = DateTime.Now.Subtract(loadedTime);
            return timePlay.Add(delta);
        }
    }
    public byte[] ToBytes()
    {
        return System.Text.ASCIIEncoding.Default.GetBytes(ToString());
    }
    public static GameProgress FromBytes(byte[] b)
    {
        return GameProgress.FromString(System.Text.ASCIIEncoding.Default.GetString(b));
    }
    public override string ToString()
    {
        string s = "GP";            //using for check if player ever save the game
        s += ":BSC" + bestScoreCasual.ToString();       //':' for split from data one to another
        s += ":BST" + bestScoreTimed.ToString();        //BSC/BST/BSM is code for each data
        s += ":BSM" + bestScoreMoves.ToString();
        s += ":BSL" + bestScoreL.ToString();
        s += ":REB" + rectBank.ToString();
        s += ":PCB" + puCrusherBank.ToString();
        s += ":PEB" + puExecuteBank.ToString();
        s += ":PRB" + puRefreshBank.ToString();
        int i = 0;
        while (i < themeColorOpened.Count)
        {
            s += ":TCO" + System.Convert.ToString(themeColorOpened[i]);
            i++;
        }
        i = 0;
        while (i < themeSpriteOpened.Count)
        {
            s += ":TSO" + System.Convert.ToString(themeSpriteOpened[i]);
            Debug.Log(themeSpriteOpened[i]);
            i++;
        }
        s += ":TPT" + TotalPlayTime.TotalMilliseconds;
        return s;
    }
    public static GameProgress FromString(string s)             //Using to load saved game, convert from string to another type and put on variable
    {
        GameProgress gp = new GameProgress();
        gp.themeColorOpened.Clear();
        gp.themeSpriteOpened.Clear();
        string[] p = s.Split(new char[] { ':' });   //spliting (membelah, memisah) from parameter s
        if (!p[0].StartsWith("GP"))
        {
            Debug.LogError("Failed To parse Game Progress from : " + s);
            return gp;
        }
        foreach (string str in p)
        {
            if (str.Equals("GP"))
                continue;
            string st = str.Substring(3);
            Debug.Log(st);
            if (str.StartsWith("BSC"))
                gp.bestScoreCasual = System.Convert.ToInt32(st);
            else if (str.StartsWith("BST"))
                gp.bestScoreTimed = System.Convert.ToInt32(st);
            else if (str.StartsWith("BSM"))
                gp.bestScoreMoves = System.Convert.ToInt32(st);
            else if (str.StartsWith("BSL"))
                gp.bestScoreL = System.Convert.ToInt32(st);
            else if (str.StartsWith("REB"))
                gp.rectBank = System.Convert.ToInt32(st);
            else if (str.StartsWith("PCB"))
                gp.puCrusherBank = System.Convert.ToInt32(st);
            else if (str.StartsWith("PEB"))
                gp.puExecuteBank = System.Convert.ToInt32(st);
            else if (str.StartsWith("PRB"))
                gp.puRefreshBank = System.Convert.ToInt32(st);
            else if (str.StartsWith("TCO"))
            {
                if (System.Convert.ToBoolean(st))
                    gp.themeColorOpened.Add(true);
                else
                    gp.themeColorOpened.Add(false);
            }
            else if (str.StartsWith("TSO"))
            {
                if (System.Convert.ToBoolean(st))
                    gp.themeSpriteOpened.Add(true);
                else
                    gp.themeSpriteOpened.Add(false);
            }
            else if (str.StartsWith("TPT"))
            {
                if (p[0].Equals("GP"))
                {
                    double val = Double.Parse(st);
                    gp.timePlay = TimeSpan.FromMilliseconds(val > 0 ? val : 0f);
                }
                else
                {
                    gp.timePlay = new TimeSpan();
                }
            }
        }
        
        gp.loadedTime = DateTime.Now;
        return gp;
    }
    public void MergeWith(GameProgress other)
    {
        bestScoreCasual = other.bestScoreCasual;
        bestScoreTimed = other.bestScoreTimed;
        bestScoreMoves = other.bestScoreMoves;
        bestScoreL = other.bestScoreL;
        puCrusherBank = other.puCrusherBank;
        puExecuteBank = other.puExecuteBank;
        puRefreshBank = other.puRefreshBank;
        rectBank = other.rectBank;
        themeColorOpened = other.themeColorOpened;
        themeSpriteOpened = other.themeSpriteOpened;
        timePlay = other.timePlay;
        mDirty = true;
    }

    public void SaveToDisk()
    {
        PlayerPrefs.SetString(PlayerPrefsKey, ToString());
        mDirty = false;
    }

    public bool Dirty
    {
        get
        {
            return mDirty;
        }
        set
        {
            mDirty = value;
        }
    }
    public void SaveScore(Game.GameType type, int value)
    {
        if (type == Game.GameType.casual && bestScoreCasual < value)
        {
            bestScoreCasual = value;
            mDirty = true;
        }
        else if (type == Game.GameType.timed && bestScoreTimed < value)
        {
            bestScoreTimed = value;
            mDirty = true;
        }
        else if (type == Game.GameType.moves && bestScoreMoves < value)
        {
            bestScoreMoves = value;
            mDirty = true;
        }
        else if (type == Game.GameType.letterL && bestScoreL < value)
        {
            bestScoreL = value;
            mDirty = true;
        }
    }
    public int GetBestScore(Game.GameType type)
    {
        if (type == Game.GameType.casual)
            return bestScoreCasual;
        else if (type == Game.GameType.timed)
            return bestScoreTimed;
        else if (type == Game.GameType.moves)
            return bestScoreMoves;
        else if (type == Game.GameType.letterL)
            return bestScoreL;
        return 0;
    }
    public int Rects
    {
        get { return rectBank; }
        set {
            rectBank = value;
            mDirty = true;
        }
    }
    public bool GetColorTheme(int index)
    {
        return themeColorOpened[index];
    }
    public void SetColorTheme(int index, bool set)
    {
        themeColorOpened[index] = set;
    }
    public bool GetSpriteTheme(int index)
    {
        return themeSpriteOpened[index];
    }
    public void SetSpriteTheme(int index, bool set)
    {
        themeSpriteOpened[index] = set;
    }
    public void PowerUps(GameManager.PowerUpPick type, int plus)
    {
        if (type == GameManager.PowerUpPick.cross)
        {
            puCrusherBank += plus;
            mDirty = true;
        }
        else if (type == GameManager.PowerUpPick.colorPick)
        {
            puExecuteBank += plus;
            mDirty = true;
        }
        else if (type == GameManager.PowerUpPick.refresh)
        {
            puRefreshBank += plus;
            mDirty = true;
        }
    }
    public int GetPowerUpsNumber(GameManager.PowerUpPick type)
    {
        if (type == GameManager.PowerUpPick.cross)
            return puCrusherBank;
        else if (type == GameManager.PowerUpPick.colorPick)
            return puExecuteBank;
        else if (type == GameManager.PowerUpPick.refresh)
            return puRefreshBank;

        return 0;
    }
}
