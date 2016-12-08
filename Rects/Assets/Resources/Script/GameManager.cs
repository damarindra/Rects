using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// GameManager is use when Game is live
/// </summary>
public class GameManager : ShapeCreator {

    public static GameManager instance = null;      //singleton
    
    private GameObject[,] board = new GameObject[10, 10];       //board size 
    private GameObject[,] rectangleOnBoard = new GameObject[10, 10];    //rectangle board for saving the board is available or not
    public GameObject boardRectangle;       //board rectangle, to instantiate
    public Color boardRectangleColor;       //color board rectangle back

    private Vector2 shapeSpawnPoint = new Vector2(20, -6.5f);       //spawn shape position
    private List<GameObject> shapeArray = new List<GameObject>(); //at gameplay, only 3 shape will spawn
    private Vector2[] shapePlace = new Vector2[3] { new Vector2(0, -5.5f), new Vector2(4.5f, -5.5f), new Vector2(9f, -5.5f) }; //shape position

    private Transform boardParent;      //board parent
    private Transform coloredBoardParent;   //rectangle board parent
    private Transform shapePicked;      //store shape which picked

    private bool instantiateNewShape = false;       //handling for checking isgameover

    private Vector2 lastPos;        //saving for last pos


    public enum ShapePick           //enum for saving which shape picked
    {
        left = 0, middle = 1, right = 2, nope
    }
    [HideInInspector]public ShapePick shapePick = ShapePick.nope;

    public enum PowerUpPick {       //enum for which power up picked
        colorPick, cross, refresh, nope
    }
    [HideInInspector]public PowerUpPick puPick = PowerUpPick.nope;

    public enum GameState {         //enum for game state, is on play, or pause
        onPlay, Paused
    }
    public GameState gameState = GameState.onPlay;

    //setup the game
    public void SetupGame()
    {
        if (Game.instance.gameType == Game.GameType.casual)
        {
        }
        else if (Game.instance.gameType == Game.GameType.timed)
        {
            Game.instance.time = 160;
        }
        else if (Game.instance.gameType == Game.GameType.moves)
        {
            Game.instance.move = 60;
        }
        else if (Game.instance.gameType == Game.GameType.letterL)
        {
        }
        Game.instance.gameScreen = Game.GameScreen.gameplay;    //when setup complete, set the game screen at gameplay
        Game.instance.score = 0;            //reset the score
        if (boardParent != null)            //when in world there is still remaining boardparent
            DestroyAndSetupNewGame();       //destroy board and setup again board
        else
            Invoke("InitializeBoard", .6f); //if not, only initialize the board
        hintPlace.Clear();
    }

    /// <summary>
    /// method for initializing board
    /// </summary>
    void InitializeBoard()
    {
        boardParent = new GameObject("Board").transform;
        boardParent.position = new Vector3(0, 0, 2);
        coloredBoardParent = new GameObject("Color").transform;
        coloredBoardParent.position = new Vector3(0, 0, 0);
        coloredBoardParent.transform.SetParent(boardParent);
        int x = 0;
        while (x < 10)
        {
            int y = 0;
            while (y < 10)
            {
                //isAvailable[x, y] = true;
                rectangleOnBoard[x, y] = null;
                GameObject instance = Instantiate(boardRectangle, new Vector3(x, y, 2), Quaternion.identity) as GameObject;
                instance.name = "Board : " + x + "," + y;
                instance.GetComponent<SpriteRenderer>().color = boardRectangleColor;
                instance.transform.SetParent(boardParent);
                instance.transform.localScale = Vector2.zero;
                board[x, y] = instance;
                iTween.ScaleTo(instance, Vector2.one, .5f);
                y++;
            }
            x++;
        }
        RandomShape();
    }

    /// <summary>
    /// method for randomize the shape
    /// </summary>
    void RandomShape()
    {
        if (shapeArray.Count != 0)
            shapeArray.Clear();
        int index = 0;
        while (index < 3)
        {
            int random;
            if (Game.instance.gameType != Game.GameType.letterL)
                random = Random.Range(0, 3); //0 box, 1 stick, 2 letterL
            else
                random = Random.Range(2, 3);
            if (random == 0)
            {
                shapeArray.Add(Box(Random.Range(1, 4), shapeSpawnPoint));
                shapeArray[index].transform.SetParent(coloredBoardParent);
            }
            else if (random == 1)
            {
                shapeArray.Add(Stick(Random.Range(2, 7), Random.Range(0, 2) == 0 ? Direction.horizontal : Direction.vertical, shapeSpawnPoint));
                shapeArray[index].transform.SetParent(coloredBoardParent);
            }
            else if (random == 2)
            {
                shapeArray.Add(LetterL(Random.Range(2, 4), Random.Range(0, 2) == 0 ? (Random.Range(0, 2) == 0 ? LetterType.type1 : LetterType.type2) : (Random.Range(0, 2) == 0 ? LetterType.type3 : LetterType.type4), shapeSpawnPoint));
                shapeArray[index].transform.SetParent(coloredBoardParent);
            }
            shapeArray[index].transform.localScale = new Vector3(.5f, .5f, 1);
            iTween.MoveTo(shapeArray[index], shapePlace[index], .8f);
            index++;
        }
        instantiateNewShape = false;
        CheckIfGameover();
    }

    /// <summary>
    /// method for destroy and setup a new game
    /// </summary>
    public void DestroyAndSetupNewGame()
    {
        DestroyColloredBoard();
        Invoke("RemoveAndSetup", 1.2f);
    }

    /// <summary>
    /// mehod for destroy the rectangle on board
    /// </summary>
    public void DestroyColloredBoard()
    {
        Transform[] gos = coloredBoardParent.Cast<Transform>().ToArray();
        foreach (Transform go in gos)
        {
            iTween.ScaleTo(go.gameObject, Vector3.zero, 0.5f);
        }
        Invoke("DestroyBoard", .3f);
    }

    /// <summary>
    /// method for destroying the board
    /// </summary>
    public void DestroyBoard()
    {
        Transform[] gos = boardParent.Cast<Transform>().ToArray();
        foreach (Transform go in gos)
        {
            iTween.ScaleTo(go.gameObject, Vector3.zero, 0.5f);
        }
        shapeArray.Clear();
        board = new GameObject[10, 10];
        rectangleOnBoard = new GameObject[10, 10];
    }

    /// <summary>
    /// method for removing and setup boardParent and colored board parent
    /// </summary>
    void RemoveAndSetup()
    {
        shapeArray.Clear();
        Destroy(boardParent.gameObject);
        Destroy(coloredBoardParent.gameObject);
        InitializeBoard();
    }

    // Use this for initialization
    protected override void Awake() {
        //singleton
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);
        DontDestroyOnLoad(gameObject);
        base.Awake();
    }

    private bool hasbeenCheckIfGameover = false;
    float timeCheckGameover;
    bool hintShown = false;
    float timeShowHint;
    private bool dontShowHintAtFirstTime = false;
    // Update is called once per frame
    void Update() {
        if (Game.instance.gameScreen == Game.GameScreen.gameplay)
        {
            if (Input.GetKeyDown(KeyCode.V))
                GameOver();
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (gameState == GameState.onPlay)
                {
                    UICanvas.instance.anim.SetTrigger("PauseIn");
                    if (hintPlace.Count != 0 && hintShown)
                    {
                        StopHint();
                        hintPlace.Clear();
                    }
                    hasbeenCheckIfGameover = false;
                    timeShowHint = 0;
                    gameState = GameState.Paused;
                }
                else
                {
                    UICanvas.instance.anim.SetTrigger("PauseOut");
                    StartCoroutine(ChangeStateToPlayInSeconds(.5f));
                }
            }
            else if (gameState == GameState.onPlay)
            {
                Debug.Log(shapeArray.Count);
                TouchHandler();
                timeShowHint += Time.deltaTime;
                if (timeShowHint >= 7 && !hintShown && hintPlace.Count != 0 && dontShowHintAtFirstTime)
                    ShowHint();
            }

        }
    }
    public IEnumerator ChangeStateToPlayInSeconds(float time)
    {
        yield return new WaitForSeconds(time);
        gameState = GameState.onPlay;
    }

    

    void TouchHandler()
    {
        if (Input.GetMouseButtonDown(0))
        {
            CheckIfGameover();

            multiplier = 1;                 //Reset multiplier after input down / break rect
            if (puPick == PowerUpPick.nope)
            {
                Vector2 position = Camera.main.ScreenToViewportPoint(Input.mousePosition);
                if (position.y <= 0.3f && position.y >= 0.1f)
                {
                    SoundManager.instance.ButtonSound();
                    if (position.x < 0.33f)
                    {
                        if (shapeArray[0] != null)
                        {
                            shapePick = ShapePick.left;
                            shapePicked = shapeArray[0].transform;
                        }
                        else
                            shapePick = ShapePick.nope;
                    }
                    else if (position.x > 0.66f)
                    {
                        if (shapeArray[2] != null)
                        {
                            shapePick = ShapePick.right;
                            shapePicked = shapeArray[2].transform;
                        }
                        else
                            shapePick = ShapePick.nope;
                    }
                    else if (position.x <= 0.66f && position.x >= 0.33f)
                    {
                        if (shapeArray[1] != null)
                        {
                            shapePick = ShapePick.middle;
                            shapePicked = shapeArray[1].transform;
                        }
                        else
                            shapePick = ShapePick.nope;
                    }
                    else
                    {
                        shapePick = ShapePick.nope;
                        shapePicked = null;
                    }
                }
                else
                {
                    shapePick = ShapePick.nope;
                    shapePicked = null;
                }
            }
            else if (puPick == PowerUpPick.cross)
            {
                if (Camera.main.ScreenToViewportPoint(Input.mousePosition).y >= 0.4f)
                {
                    Vector2 position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    PowerUpCross(Mathf.RoundToInt(position.x), Mathf.RoundToInt(position.y));
                }
            }
            else if (puPick == PowerUpPick.colorPick)
            {
                if (Camera.main.ScreenToViewportPoint(Input.mousePosition).y >= 0.3f)
                {
                    Vector2 position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    PowerUpColor(Mathf.RoundToInt(position.x), Mathf.RoundToInt(position.y));
                }
            }
            else if (puPick == PowerUpPick.refresh)
            {

            }
        }
        if (Input.GetMouseButton(0) && shapePick != ShapePick.nope && puPick == PowerUpPick.nope)
        {
            lastPos = shapePicked.position;
            Vector2 position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            shapePicked.position = new Vector3(position.x, position.y + 4, -2);
            iTween.ScaleTo(shapePicked.gameObject, new Vector3(1, 1, 1), .35f);
        }
        if (Input.GetMouseButtonUp(0) && shapePick != ShapePick.nope && puPick == PowerUpPick.nope)
        {
            SoundManager.instance.ButtonSound();
            if (hintPlace.Count != 0 && hintShown)
                StopHint();
            List<Transform> positionToMatching = shapePicked.Cast<Transform>().ToList();        //Get Component on childern without parent. if using GetComponentsInChildren, parent is include!!!

            if ((lastPos - (Vector2)shapePicked.position).sqrMagnitude >= 0.08f)
            {
                iTween.MoveTo(shapePicked.gameObject, shapePlace[(int)shapePick], .8f);
                iTween.ScaleTo(shapePicked.gameObject, new Vector3(.5f, .5f, .5f), .8f);
                shapePicked = null;
                return;
            }
            
            List<Vector2> posMatchingTemp = new List<Vector2>();        //untuk mengecek posisi vector2[i] apakah sama dengan vector2[i-1], jika iya, shapenya kembali ke posisi awal
            int i = 0;
            while (i < positionToMatching.Count)
            {
                posMatchingTemp.Add(new Vector2(Mathf.RoundToInt(positionToMatching[i].position.x), Mathf.RoundToInt(positionToMatching[i].position.y)));
                if (posMatchingTemp[i].x <= -0.45f || posMatchingTemp[i].x >= 9.45f || posMatchingTemp[i].y <= -0.45f || posMatchingTemp[i].y >= 9.45f)
                {
                    iTween.MoveTo(shapePicked.gameObject, shapePlace[(int)shapePick], .8f);
                    iTween.ScaleTo(shapePicked.gameObject, new Vector3(.5f, .5f, .5f), .8f);
                    shapePicked = null;
                    break;
                }
                if (rectangleOnBoard[(int)posMatchingTemp[i].x, (int)posMatchingTemp[i].y] != null)
                {
                    iTween.MoveTo(shapePicked.gameObject, shapePlace[(int)shapePick], .8f);
                    iTween.ScaleTo(shapePicked.gameObject, new Vector3(.5f, .5f, .5f), .8f);
                    shapePicked = null;
                    break;
                }
                i++;
                if (i == positionToMatching.Count)
                {
                    int j = 0;
                    while (j < posMatchingTemp.Count)
                    {
                        int k = j + 1;
                        while (k < posMatchingTemp.Count)
                        {
                            if (posMatchingTemp[j] == posMatchingTemp[k])
                                goto endOfChecking;
                            k++;
                        }
                        j++;
                    }
                    //PUT ON BOARD
                    if (j == posMatchingTemp.Count)
                    {
                        //time to matched
                        PutOnBoard(positionToMatching.ToArray());

                        //For Game Type Moves
                        Game.instance.move -= 1;
                        shapeArray[(int)shapePick] = null;
                        shapePicked = null;
                        //if (shapeArray.Count == 0)
                        if (shapeArray[0] == null && shapeArray[1] == null && shapeArray[2] == null)
                        {
                            shapeArray.Clear();
                            instantiateNewShape = true;
                            RandomShape();
                            //Invoke("RandomShape", .5f);
                        }
                        CheckIfGameover();
                    }
                }
            }
        endOfChecking:
            shapePick = ShapePick.nope;
        }
    }
    void PutOnBoard(Transform[] shape)
    {
        timeShowHint = 0;
        List<Vector2> pos = new List<Vector2>();
        int i = 0;
        while (i < shape.Length)
        {
            int x = Mathf.RoundToInt(shape[i].position.x);
            int y = Mathf.RoundToInt(shape[i].position.y);
            if (i > 0)
                pos.Add(new Vector2(x, y));
            else
                pos.Add(new Vector2(x,y));
            iTween.ScaleTo(shapePicked.gameObject, Vector3.one, .3f);
            iTween.MoveTo(shape[i].gameObject, new Vector2(x, y), .3f);
            rectangleOnBoard[x, y] = shape[i].gameObject;
            //isAvailable[x, y] = false;
            Game.instance.score += Game.instance.pointPerRect;
            i++;
        }
        hasbeenCheckIfGameover = false;
        StartCoroutine(CheckMatchesInSecond(pos, .2f));
    }

    float multiplier = 1;
    
    IEnumerator CheckMatchesInSecond(List<Vector2> pos, float time)
    {
        yield return new WaitForSeconds(time);
        CheckMatches(pos);
        CheckIfGameover();                      //Check Again
    }

    
    void CheckMatches(List<Vector2> posToCheck)
    {
        List<Transform> rectTemp = new List<Transform>();

        foreach (Vector2 p in posToCheck)
        {
            int i = 0;
            while (i < 2)
            {
                int j = 0;
                while (j < 10)
                {
                    if (i == 0)
                    {
                        if (rectangleOnBoard[(int)p.x, j] == null)
                            break;
                        if (j == 9)
                        {
                            int z = 0;
                            while (z < 10)
                            {
                                rectTemp.Add(rectangleOnBoard[(int)p.x, z].transform);
                                z++;
                            }
                        }
                    }
                    else
                    {
                        if (rectangleOnBoard[j, (int)p.y] == null)
                            break;
                        if (j == 9)
                        {
                            int z = 0;
                            while (z < 10)
                            {
                                rectTemp.Add(rectangleOnBoard[z, (int)p.y].transform);
                                z++;
                            }
                        }
                    }
                    j++;
                    if (j == 10)
                        multiplier += .2f;
                }
                i++;
            }
        }
        BreakRect(rectTemp);
    }
    
    void BreakRect(List<Transform> tr)
    {
        foreach (Transform t in tr)
        {
            iTween.ScaleTo(t.gameObject, new Vector3(0, 0, 0), .6f);
            rectangleOnBoard[Mathf.RoundToInt(t.position.x), Mathf.RoundToInt(t.position.y)] = null;
            Game.instance.rectCollectedTemp += 1;
            StartCoroutine(DestroyInSeconds(t.gameObject, 1f));
            Game.instance.score += (int)(Game.instance.pointPerRectPerMatched * multiplier);
        }
        hasbeenCheckIfGameover = true;              //Handle on checking game over updater (void Update), using invoke for checking game over, see above
        if (multiplier > 1)
        {
            if(PlayerPrefs.GetInt("Vibration") == 1)
                Handheld.Vibrate();
            SoundManager.instance.PlaySrink();
        }
    }

    void PowerUpColor(int x, int y)
    {
        Color color = new Color();
        if (x < 0 || x > 9 || y > 9 || y < 0)
        {
            return;
        }
        if (rectangleOnBoard[x, y] != null)
        {
            color = new Color(rectangleOnBoard[x, y].GetComponent<SpriteRenderer>().color.r,
                rectangleOnBoard[x, y].GetComponent<SpriteRenderer>().color.g,
                rectangleOnBoard[x, y].GetComponent<SpriteRenderer>().color.b,
                1);
            DestroyColor(color);
            GPGSController.Instance.mProgress.PowerUps(puPick, -1);

            puPick = PowerUpPick.nope;
            UICanvas.instance.PUColorAnimation.Play("Off");
        }
    }

    void DestroyColor(Color clr)
    {
        List<Transform> rectTemp = new List<Transform>();
        SpriteRenderer[] srenderer = coloredBoardParent.GetComponentsInChildren<SpriteRenderer>();
        foreach (SpriteRenderer sr in srenderer)
        {
            if (sr.transform.position.x < 0 || sr.transform.position.x > 9 || sr.transform.position.y < 0 || sr.transform.position.y > 9)
                continue;
            if (sr.color == clr)
                rectTemp.Add(sr.transform);
        }
        BreakRectForPowerUps(rectTemp.ToArray());
    }

    void PowerUpCross(int x, int y)
    {
        List<Transform> rectTemp = new List<Transform>();
        if (x < 0 || x > 9 || y > 9 || y < 0)
        {
            return;
        }
        if (rectangleOnBoard[x, y] != null)
        {
            for (int i = 0; i < 10; i++)     //horizon
            {
                if (rectangleOnBoard[i, y] != null)
                    rectTemp.Add(rectangleOnBoard[i, y].transform);
            }
            for (int i = 0; i < 10; i++)        //vertical
            {
                if (i == y)
                    continue;
                if (rectangleOnBoard[x, i] != null)
                    rectTemp.Add(rectangleOnBoard[x, i].transform);
            }
            GPGSController.Instance.mProgress.PowerUps(puPick, -1);
            puPick = PowerUpPick.nope;
            UICanvas.instance.PUCrossAnimation.Play("Off");
            BreakRectForPowerUps(rectTemp.ToArray());
        }
    }

    [HideInInspector]public bool PURefreshEnter = false;
    public void PowerUpRefresh()
    {
        PURefreshEnter = true;
        foreach (GameObject go in shapeArray)
        {
            if (go == null)
                continue;
            iTween.MoveTo(go, new Vector3(go.transform.position.x - 20, go.transform.position.y, 0), 1f);
        }
        //SaveLoad.UsePowerUp(puPick);
        GPGSController.Instance.mProgress.PowerUps(puPick, -1);
        puPick = PowerUpPick.nope;
        Invoke("PURefreshEnterToggle", 1.7f);
        Invoke("RandomShape", .6f);
    }
    private void PURefreshEnterToggle()
    {
        PURefreshEnter = !PURefreshEnter;
    }
    void BreakRectForPowerUps(Transform[] tr)
    {
        foreach (Transform t in tr)
        {
            iTween.ScaleTo(t.gameObject, new Vector3(0, 0, 0), .6f);
            rectangleOnBoard[Mathf.RoundToInt(t.position.x), Mathf.RoundToInt(t.position.y)] = null;
            Game.instance.rectCollectedTemp += 1;
            StartCoroutine(DestroyInSeconds(t.gameObject, 1f));
            Game.instance.score += (int)(Game.instance.pointPerRectPerMatched * multiplier);
        }
        hasbeenCheckIfGameover = true;                  //Handle on checking game over updater (void Update), using invoke for checking game over, see above
        if (PlayerPrefs.GetInt("Vibration") == 1)
            Handheld.Vibrate();
        SoundManager.instance.PlaySrink();
    }

    IEnumerator DestroyInSeconds(GameObject obj, float time)
    {
        yield return new WaitForSeconds(time);
        Destroy(obj);
    }

    IEnumerator CheckGameoverInSeconds(float time)
    {
        yield return new WaitForSeconds(time);
        //CheckIfGameover(go);
        CheckIfGameover();
    }

    List<GameObject> hintPlace = new List<GameObject>();
    
    void CheckIfGameover()
    {
        if (shapePick == ShapePick.nope)
        {
            if(hintPlace.Count != 0 && hintShown)
                StopHint();
            hintPlace.Clear();
            bool[] provingOver = new bool[3] {false, false, false };
            //foreach (GameObject go in gameObj)
            //Debug.Log(gameObj.Count);
            //if (gameObj[0] == null && gameObj[1] == null && gameObj[2] == null)
            if ((shapeArray[0] == null && shapeArray[1] == null && shapeArray[2] == null) || shapeArray.Count == 0)
                return;
            for(int g = 0; g < shapeArray.Count; g++)
            {
                if (shapeArray[g] == null)
                {
                    provingOver[g] = true;
                    continue;
                }
                Transform[] tr = shapeArray[g].transform.Cast<Transform>().ToArray();
                Vector2 origin = new Vector2(tr[0].position.x*-1, tr[0].position.y*-1);
                Vector2[] posToCheck = new Vector2[tr.Length];
                int i = 0;
                while (i < tr.Length)
                {
                    posToCheck[i] = new Vector2((tr[i].position.x + origin.x) * 2, ((tr[i].position.y + origin.y) * 2));
                    i++;
                }
                int x = 0;
                while (x < 10)
                {
                    int y = 0;
                    while (y < 10)
                    {
                        if (rectangleOnBoard[x, y] == null)
                        {
                            for (int z = 0; z < posToCheck.Length; z++)
                            {
                                if (x + (int)posToCheck[z].x < 0 || x + (int)posToCheck[z].x > 9 || y + (int)posToCheck[z].y < 0 || y + (int)posToCheck[z].y > 9
                                    || rectangleOnBoard[x + (int)posToCheck[z].x, y + (int)posToCheck[z].y] != null)
                                {
                                    hintPlace.Clear();
                                    break;
                                }
                                hintPlace.Add(board[x + (int)posToCheck[z].x, y + (int)posToCheck[z].y]);

                                if (z == posToCheck.Length - 1)
                                {
                                    provingOver[g] = false;
                                    goto searchAnother;
                                }
                            }
                        }
                        y++;
                    }
                    x++;
                    if (x == 10)
                        provingOver[g] = true;
                }
            }
            if (provingOver[0] == true && provingOver[1] == true && provingOver[2] == true)
            {
                UICanvas.instance.textOver.text = "Stucked";
                Invoke("GameOver", 1.5f);
            }
        searchAnother:
            Debug.Log("Ends");
            dontShowHintAtFirstTime = true;
            hasbeenCheckIfGameover = true;
        }
    }

    void Show()
    {
        foreach (GameObject g in hintPlace)
            Debug.Log(g.name);
        CheckIfGameover();
        int i = 0;
        while (i < 10)
        {
            int j = 0;
            while (j < 10)
            {
                if (rectangleOnBoard[i, j] == null)
                    Debug.Log("FREE : " + i + "," + j);
                j++;
            }
            i++;
        }
    }

    private int admobInterstitialCounter = 0;
    public void GameOver()
    {
        //Reset All
        hintPlace.Clear();
        hasbeenCheckIfGameover = false;
        
        DestroyColloredBoard();
        GPGSController.Instance.mProgress.Rects = GPGSController.Instance.mProgress.Rects + Game.instance.rectCollectedTemp;
        GPGSController.Instance.mProgress.SaveScore(Game.instance.gameType, Game.instance.score);
        GPGSController.Instance.AutoSave();
        Game.instance.bestScore = GPGSController.Instance.mProgress.GetBestScore(Game.instance.gameType);

        UICanvas.instance.anim.SetTrigger("GameplayOut");
        UICanvas.instance.anim.SetTrigger("GameoverIn");
        Game.instance.gameScreen = Game.GameScreen.gameover;

        //GPGS
        GPGSController.Instance.IncrementingAchievement(KeyIds.instance.achievementRectsWallet, Game.instance.rectCollectedTemp);
        GPGSController.Instance.IncrementingAchievement(KeyIds.instance.achievementRectsPiggyBank, Game.instance.rectCollectedTemp);
        GPGSController.Instance.IncrementingAchievement(KeyIds.instance.achievementRectsBanker, Game.instance.rectCollectedTemp);
        GPGSController.Instance.FlushAchievement();

        Game.instance.rectCollectedTemp = 0;

        admobInterstitialCounter += 1;
        if (admobInterstitialCounter >= 3)
        {
            AdmobController.Instance.InterstitialShow();
            admobInterstitialCounter = 0;
        }

        //hide the banner
        AdmobController.Instance.BannerHide();
    }

    void ShowHint()
    {
        foreach (GameObject h in hintPlace)
        {
            iTween.ScaleBy(h, iTween.Hash("x", 0.6f, "y", 0.6f, "easeType", "easeInOutExpo", "loopType", "pingPong"));
            Debug.Log(h.name);
        }
        timeShowHint = 0;
        hintShown = true;
    }
    void StopHint()
    {
        if (hintPlace.Count != 0)
        {
            foreach (GameObject h in hintPlace)
            {
                iTween.Stop(h);
                h.transform.localScale = Vector2.one;
            }
        }
        hintShown = false;
    }

}
