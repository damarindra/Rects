using UnityEngine;
using System.Collections;

public class Game : MonoBehaviour {

    public static Game instance = null;
    public int pointPerRect;
    public int pointPerRectPerMatched;
    [HideInInspector]
    public int score;
    [HideInInspector]
    public int bestScore;
    [HideInInspector]
    public float time;
    [HideInInspector]
    public int move;
    [HideInInspector]
    public int rectCollectedTemp;
    public int pricePUColorPick = 6000;
    public int pricePUCross = 3000;
    public int pricePURefresh = 700;

    public enum GameScreen {
        mainmenu, gameplay, gameover, shop, option
    }
    public GameScreen gameScreen = GameScreen.mainmenu;

    public enum GameType {
        casual = 1, timed = 2, moves = 3, letterL = 4
    }
    public GameType gameType = GameType.casual;

	// Use this for initialization
	void Awake () {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);
        DontDestroyOnLoad(gameObject);

        if (PlayerPrefs.GetInt("FirstInitialize") != 1)
        {
            PlayerPrefs.SetInt("Sound", 1);
            PlayerPrefs.SetInt("Vibration", 1);
            PlayerPrefs.SetString("Theme", "C:0:0");
            
            PlayerPrefs.SetInt("FirstInitialize", 1);
        }
        
    }
    void Start()
    {
        string[] p = PlayerPrefs.GetString("Theme").Split(new char[] { ':' });
        if (p[0].Equals("C"))
        {
            GameManager.instance.rectType = ShapeCreator.RectType.color;
            GameManager.instance.colorRectangle = GameManager.instance.colorRectArray[System.Convert.ToInt32(p[1])].GetColor;
            GameManager.instance.backgroundColor = GameManager.instance.backgroundColorArray[System.Convert.ToInt32(p[2])];
        }
        else if (p[0].Equals("S"))
        {
            GameManager.instance.rectType = ShapeCreator.RectType.sprite;
            GameManager.instance.gameobjectRectangle = GameManager.instance.gameobjectRectangleArray[System.Convert.ToInt32(p[1])].GetSprite;
            GameManager.instance.backgroundColor = GameManager.instance.backgroundColorArray[System.Convert.ToInt32(p[2])];
        }
        else
        {
            GameManager.instance.rectType = ShapeCreator.RectType.color;
            GameManager.instance.colorRectangle = GameManager.instance.colorRectArray[0].GetColor;
            GameManager.instance.backgroundColor = GameManager.instance.backgroundColorArray[0];

        }
        Camera.main.backgroundColor = GameManager.instance.backgroundColor;
    }
	
	// Update is called once per frame
	void Update () {
        if (gameScreen == GameScreen.gameplay)
        {
            if (gameType == GameType.casual) { }
            else if (gameType == GameType.timed)
            {
                StartCoroutine(TimerStart(2));
                if (time <= 0)
                {
                    gameScreen = GameScreen.gameover;
                    UICanvas.instance.textOver.text = "Time Out";
                    GameManager.instance.GameOver();
                }
            }
            else if (gameType == GameType.moves)
            {
                if (move <= 0)
                {
                    gameScreen = GameScreen.gameover;
                    UICanvas.instance.textOver.text = "No Moves";
                    GameManager.instance.GameOver();
                }
            }
            else if (gameType == GameType.letterL)
            {

            }

        }
        else if (gameScreen == GameScreen.mainmenu)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
                Application.Quit();

            if (Input.GetKeyDown(KeyCode.M))
                GPGSController.Instance.mProgress.Rects += 1000;
        }
        if (Input.GetKeyDown(KeyCode.T))
            Debug.Log(GPGSController.Instance.mProgress.TotalPlayTime +" || "+GPGSController.Instance.mProgress.TotalPlayTime.Days);
    }

    IEnumerator TimerStart(float t)
    {
        yield return new WaitForSeconds(t);
        if(GameManager.instance.gameState == GameManager.GameState.onPlay)
            time -= Time.deltaTime;
    }
}
