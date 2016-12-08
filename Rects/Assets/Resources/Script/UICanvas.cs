using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Advertisements;
using System;

public class UICanvas : MonoBehaviour {

    public static UICanvas instance = null;

    [HideInInspector]public Animator anim;
    public Text textScore;
    public Text textBestScore;
    public Text textTime;
    public Text textMove;

    public Animation PUColorAnimation;
    public Animation PUCrossAnimation;
    public Animation PURefreshAnimation;

    public Text textOver;

    public Text textScoreGameover;
    public Text textBestScoreGameover;
    public Text textRectsCollected;
    public Text textPUColorAtGameover;
    public Text textPUCrossAtGameover;
    public Text textPURefreshAtGameover;

    public Text textPUColorNumber;
    public Text textPUCrossNumber;
    public Text textPURefreshNumber;

    public Button[] soundButton;
    public Button[] vibrationButton;
    public Sprite[] soundOnOffSpr;
    public Sprite[] vibrationOnOffSpr;

    public Button colorPriceTag;
    public Button crossPriceTag;
    public Button refreshPriceTag;
    public Text textRectsAtShop;
    public Text textColorNumberAtShop;
    public Text textCrossNumberAtShop;
    public Text textRefreshNumberAtShop;

    public Image[] bgThemeShop = new Image[2];
    public Text[] allTextOnCanvas;

    public Button vidAds;

    enum ShopMenu {
        powerups, rects, theme
    }
    ShopMenu shopMenu = ShopMenu.powerups;

    //public void Play()
    public void Play(int type)
    {
        SoundManager.instance.ButtonSound();
        if (type == 1)
        {
            Game.instance.gameType = Game.GameType.casual;
            textMove.transform.parent.gameObject.SetActive(false);
            textTime.transform.parent.gameObject.SetActive(false);
        }
        else if (type == 2)
        {
            Game.instance.gameType = Game.GameType.timed;
            textMove.transform.parent.gameObject.SetActive(false);
            textTime.transform.parent.gameObject.SetActive(true);
        }
        else if (type == 3)
        {
            Game.instance.gameType = Game.GameType.moves;
            textMove.transform.parent.gameObject.SetActive(true);
            textTime.transform.parent.gameObject.SetActive(false);
        }
        else if (type == 4)
        {
            Game.instance.gameType = Game.GameType.letterL;
            textMove.transform.parent.gameObject.SetActive(false);
            textTime.transform.parent.gameObject.SetActive(false);
        }
        //OLD
        //SaveLoad.getBestScore(out Game.instance.bestScore, Game.instance.gameType);
        //NEW
        Game.instance.bestScore = GPGSController.Instance.mProgress.GetBestScore(Game.instance.gameType);

        GameManager.instance.SetupGame();
        //Transition
        anim.SetTrigger("MainmenuOut");
        anim.SetTrigger("GameplayIn");

        //GPGS
        GPGSController.Instance.UnlockingAchievement(KeyIds.instance.achievementFirstPlay);
        GPGSController.Instance.IncrementingAchievement(KeyIds.instance.achievementRectsAddict, 1);

        Game.instance.gameScreen = Game.GameScreen.gameplay;

        AdmobController.Instance.BannerShow();
    }

    public void PlayAgain()
    {
        SoundManager.instance.ButtonSound();
        //Transition
        if (Game.instance.gameScreen == Game.GameScreen.gameover)
        {
            anim.SetTrigger("GameoverOut");
            anim.SetTrigger("GameplayIn");
        }
        else if (Game.instance.gameScreen == Game.GameScreen.gameplay)
        {
            GameManager.instance.gameState = GameManager.GameState.onPlay;
            anim.SetTrigger("PauseOut");
        }
        GameManager.instance.SetupGame();
        Game.instance.gameScreen = Game.GameScreen.gameplay;

        //GPGS
        GPGSController.Instance.IncrementingAchievement(KeyIds.instance.achievementRectsAddict, 1);

        AdmobController.Instance.BannerShow();
    }

    Game.GameScreen gameScreenTemp = Game.GameScreen.mainmenu;

    public void Mainmenu()
    {
        SoundManager.instance.ButtonSound();
        if (Game.instance.gameScreen == Game.GameScreen.gameplay)
        {
            anim.SetTrigger("MainmenuIn");
            anim.SetTrigger("GameplayOut");
            anim.SetTrigger("PauseOut");
            GameManager.instance.gameState = GameManager.GameState.onPlay;
            GameManager.instance.DestroyColloredBoard();
        }
        else if (Game.instance.gameScreen == Game.GameScreen.gameover)
        {
            anim.SetTrigger("MainmenuIn");
            anim.SetTrigger("GameoverOut");
        }
        AdmobController.Instance.BannerHide();
        Game.instance.gameScreen = Game.GameScreen.mainmenu;
    }
    public void OptionButton()
    {
        anim.SetTrigger("MainmenuOut");
        anim.SetTrigger("OptionIn");
        Game.instance.gameScreen = Game.GameScreen.option;
    }

    public void Shopmenu()
    {
        SoundManager.instance.ButtonSound();
        Debug.Log(Game.instance.gameScreen);
        anim.SetTrigger("ShopIn");
        anim.SetTrigger("ShopPowerUpIn");
        if (Game.instance.gameScreen == Game.GameScreen.mainmenu)
        {
            anim.SetTrigger("MainmenuOut");
            gameScreenTemp = Game.GameScreen.mainmenu;
        }
        else if (Game.instance.gameScreen == Game.GameScreen.gameover)
        {
            anim.SetTrigger("GameoverOut");
            gameScreenTemp = Game.GameScreen.gameover;
        }
        crossPriceTag.transform.GetChild(0).GetComponent<Text>().text = Game.instance.pricePUCross.ToString();
        colorPriceTag.transform.GetChild(0).GetComponent<Text>().text = Game.instance.pricePUColorPick.ToString();
        refreshPriceTag.transform.GetChild(0).GetComponent<Text>().text = Game.instance.pricePURefresh.ToString();
        Game.instance.gameScreen = Game.GameScreen.shop;
        shopMenu = ShopMenu.powerups;
        //show banner
        AdmobController.Instance.BannerShow();
    }

    public void BackFromShop()
    {
        SoundManager.instance.ButtonSound();
        anim.SetTrigger("ShopOut");
        if (gameScreenTemp == Game.GameScreen.gameover)
        {
            anim.SetTrigger("GameoverIn");
            Game.instance.gameScreen = Game.GameScreen.gameover;
        }
        else
        {
            anim.SetTrigger("MainmenuIn");
            Game.instance.gameScreen = Game.GameScreen.mainmenu;
        }
        if (shopMenu == ShopMenu.powerups)
            anim.SetTrigger("ShopPowerUpOut");
        else if (shopMenu == ShopMenu.theme)
            anim.SetTrigger("ShopThemeOut");
        else if (shopMenu == ShopMenu.rects)
            anim.SetTrigger("ShopRectsOut");
        textRectsAtShop.text = GPGSController.Instance.mProgress.Rects.ToString(); //SaveLoad.getRects.ToString() + "  pieces"; OLD
        //Hide banner
        AdmobController.Instance.BannerHide();
    }

    public void AddRectsButton()
    {
        if (shopMenu == ShopMenu.powerups)
        {
            anim.SetTrigger("ShopPowerUpOut");
            anim.SetTrigger("ShopRectsIn");
        }
        else if (shopMenu == ShopMenu.theme)
        {
            anim.SetTrigger("ShopThemeOut");
            anim.SetTrigger("ShopRectsIn");
        }
        shopMenu = ShopMenu.rects;
    }

    public void ShowPauseLayer()
    {
        SoundManager.instance.ButtonSound();
        anim.SetTrigger("PauseIn");
    }

    public void ResumeButton()
    {
        SoundManager.instance.ButtonSound();
        anim.SetTrigger("PauseOut");
        Game.instance.gameScreen = Game.GameScreen.gameplay;
        StartCoroutine(GameManager.instance.ChangeStateToPlayInSeconds(0.5f));
    }
    public void PUColorButton()
    {
        SoundManager.instance.ButtonSound();
        //OLD
        //if (GameManager.instance.puPick == GameManager.PowerUpPick.colorPick || SaveLoad.GetPowerUp(GameManager.PowerUpPick.colorPick) <= 0 || GameManager.instance.shapePick != GameManager.ShapePick.nope)
        if (GameManager.instance.puPick == GameManager.PowerUpPick.colorPick || GPGSController.Instance.mProgress.GetPowerUpsNumber(GameManager.PowerUpPick.colorPick) <= 0 || GameManager.instance.shapePick != GameManager.ShapePick.nope)
        {
            GameManager.instance.puPick = GameManager.PowerUpPick.nope;
            PUColorAnimation.Play("Off");

        }
        else
        {
            GameManager.instance.puPick = GameManager.PowerUpPick.colorPick;
            PUCrossAnimation.Play("Off");
            PUColorAnimation.Play("On");
        }
    }
    public void PUCrossButton()
    {
        SoundManager.instance.ButtonSound();
        if (GameManager.instance.puPick == GameManager.PowerUpPick.cross || GPGSController.Instance.mProgress.GetPowerUpsNumber(GameManager.PowerUpPick.colorPick) <= 0 || GameManager.instance.shapePick != GameManager.ShapePick.nope)
        {
            GameManager.instance.puPick = GameManager.PowerUpPick.nope;
            PUCrossAnimation.Play("Off");
        }
        else
        {
            GameManager.instance.puPick = GameManager.PowerUpPick.cross;
            PUColorAnimation.Play("Off");
            PUCrossAnimation.Play("On");
        }
    }
    public void PURefreshButton()
    {
        SoundManager.instance.ButtonSound();
        if (GPGSController.Instance.mProgress.GetPowerUpsNumber(GameManager.PowerUpPick.refresh) <= 0 || GameManager.instance.shapePick != GameManager.ShapePick.nope)
            GameManager.instance.puPick = GameManager.PowerUpPick.nope;
        else
        {
            GameManager.instance.puPick = GameManager.PowerUpPick.refresh;
            PURefreshAnimation.Play("On");
            Invoke("StopAnimationRefresh", 0.7f);
            GameManager.instance.PowerUpRefresh();
        }
    }
    void StopAnimationRefresh()
    {
        PURefreshAnimation.Stop();
        PURefreshAnimation.transform.GetChild(0).localRotation = Quaternion.Euler(Vector3.zero);
    }

    public void SoundButton()
    {
        PlayerPrefs.SetInt("Sound", (PlayerPrefs.GetInt("Sound") == 1 ? 0 : 1));
        foreach(Button bt in soundButton)
            bt.GetComponent<Image>().sprite = soundOnOffSpr[PlayerPrefs.GetInt("Sound")];
        SoundManager.instance.ButtonSound();
    }
    public void VibrationButton()
    {
        PlayerPrefs.SetInt("Vibration", (PlayerPrefs.GetInt("Vibration") == 1 ? 0 : 1));
        foreach(Button bt in vibrationButton)
            bt.GetComponent<Image>().sprite = vibrationOnOffSpr[PlayerPrefs.GetInt("Vibration")];
        SoundManager.instance.ButtonSound();
    }

    private int rectCounterTemp;          //using for make an animation countdown on rectTotal
    private int rectCounterSpeed;
    public void BuyPowerUps(int id, int value)
    {
        SoundManager.instance.ButtonSound();
        rectCounterTemp = GPGSController.Instance.mProgress.Rects;
        if (id == 0)
        {
            //OLD
            //SaveLoad.BuyPowerUp(GameManager.PowerUpPick.cross);
            //SaveLoad.PlusRects(-Game.instance.pricePUCross);
            //NEW
            GPGSController.Instance.mProgress.PowerUps(GameManager.PowerUpPick.cross, value);
            GPGSController.Instance.mProgress.Rects = GPGSController.Instance.mProgress.Rects - Game.instance.pricePUCross;
            rectCounterSpeed = Game.instance.pricePUCross / 56;
        }
        else if (id == 1)
        {
            GPGSController.Instance.mProgress.PowerUps(GameManager.PowerUpPick.colorPick, value);
            GPGSController.Instance.mProgress.Rects = GPGSController.Instance.mProgress.Rects - Game.instance.pricePUColorPick;
            rectCounterSpeed = Game.instance.pricePUColorPick / 56;
        }
        else if (id == 2)
        {
            GPGSController.Instance.mProgress.PowerUps(GameManager.PowerUpPick.refresh, value);
            GPGSController.Instance.mProgress.Rects = GPGSController.Instance.mProgress.Rects - Game.instance.pricePURefresh;
            rectCounterSpeed = Game.instance.pricePURefresh / 56;
        }

        //GPGS
        GPGSController.Instance.UnlockingAchievement(KeyIds.instance.achievementFirstBuy);
        GPGSController.Instance.IncrementingAchievement(KeyIds.instance.achievementShopper, 1);
        GPGSController.Instance.FlushAchievement();
    }

    public void ShowLeaderboard()
    {
        GPGSController.Instance.ShowLeaderboard();
    }
    public void ShowAchievement()
    {
        GPGSController.Instance.ShowAchievement();
    }

    // Use this for initialization
    void Awake () {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);
        anim = GetComponent<Animator>();

        foreach(Button bt in soundButton)
            bt.GetComponent<Image>().sprite = soundOnOffSpr[PlayerPrefs.GetInt("Sound")];
        foreach (Button bt in vibrationButton)
            bt.GetComponent<Image>().sprite = vibrationOnOffSpr[PlayerPrefs.GetInt("Vibration")];
        allTextOnCanvas = GetComponentsInChildren<Text>();
    }

    public GameObject[] goColorThemeButton;
    public GameObject[] goSpriteThemeButton;
    void Start()
    {
        //Set the price! string will be convert to integer for decrease the rects after click buy
        //goColorThemeButton[0].GetComponent<Text>().text = "Price";
        goSpriteThemeButton[0].GetComponentInChildren<Text>().text = "1000";
    }

    // Update is called once per frame
    void Update () {
        if (Game.instance.gameScreen == Game.GameScreen.gameplay)
        {
            textScore.text = Game.instance.score.ToString();
            textBestScore.text = Game.instance.bestScore.ToString();
            textPUColorNumber.text = " :" + GPGSController.Instance.mProgress.GetPowerUpsNumber(GameManager.PowerUpPick.colorPick).ToString();
            textPUCrossNumber.text = " :" + GPGSController.Instance.mProgress.GetPowerUpsNumber(GameManager.PowerUpPick.cross).ToString();
            textPURefreshNumber.text = " :" + GPGSController.Instance.mProgress.GetPowerUpsNumber(GameManager.PowerUpPick.refresh).ToString();
            if (Game.instance.gameType == Game.GameType.casual) { }
            else if (Game.instance.gameType == Game.GameType.timed)
            {
                textTime.text = ((int)Game.instance.time / 60).ToString() + ":" + ((int)Game.instance.time % 60).ToString();
                //textTime.text = Game.instance.time.ToString("0:00");
            }
            else if (Game.instance.gameType == Game.GameType.moves)
            {
                textMove.text = Game.instance.move.ToString();
            }
            else if (Game.instance.gameType == Game.GameType.letterL)
            {
                textTime.text = Game.instance.time.ToString("0:00");
                textMove.text = Game.instance.move.ToString();
            }
        }
        else if (Game.instance.gameScreen == Game.GameScreen.gameover)
        {
            textScoreGameover.text = Game.instance.score.ToString();
            textBestScoreGameover.text = Game.instance.bestScore.ToString();
            textRectsCollected.text = GPGSController.Instance.mProgress.Rects.ToString() + "   pieces";
            textPUColorAtGameover.text = GPGSController.Instance.mProgress.GetPowerUpsNumber(GameManager.PowerUpPick.colorPick).ToString();
            textPUCrossAtGameover.text = GPGSController.Instance.mProgress.GetPowerUpsNumber(GameManager.PowerUpPick.cross).ToString();
            textPURefreshAtGameover.text = GPGSController.Instance.mProgress.GetPowerUpsNumber(GameManager.PowerUpPick.refresh).ToString();
            if (rectCounterTemp != GPGSController.Instance.mProgress.Rects)
            {
                rectCounterTemp -= rectCounterSpeed;
                if (rectCounterTemp <= GPGSController.Instance.mProgress.Rects + rectCounterSpeed)
                    rectCounterTemp = GPGSController.Instance.mProgress.Rects;
            }
            if (isShareScore)
                ShareScore();

            //ADS
            if (Advertisement.IsReady() && !vidHasSetup)
            {
                //Show Button
                RandomRewardAds();
                if (rewardAds == RewardAds.rects)
                {
                    vidAds.gameObject.SetActive(true);
                    rewardAmount = RandomRewardAmount();
                    vidAds.transform.GetChild(0).GetComponent<Text>().text = rewardAmount.ToString() + " Rects";
                }
                else if (rewardAds == RewardAds.nope)
                    vidAds.gameObject.SetActive(false);
                vidHasSetup = true;
            }
            else if (!Advertisement.IsReady())
                vidAds.gameObject.SetActive(false);
            }
        else if (Game.instance.gameScreen == Game.GameScreen.shop)
        {
            textRectsAtShop.text = rectCounterTemp.ToString() + "  pieces";

            //TouchHandler();

            bgThemeShop[0].color = GameManager.instance.backgroundColor;        //Mengeset color dari theme shop agar slide tidak terlihat
            bgThemeShop[1].color = GameManager.instance.backgroundColor;

            if (shopMenu == ShopMenu.powerups)
            {
                textColorNumberAtShop.text = GPGSController.Instance.mProgress.GetPowerUpsNumber(GameManager.PowerUpPick.colorPick).ToString();
                textCrossNumberAtShop.text = GPGSController.Instance.mProgress.GetPowerUpsNumber(GameManager.PowerUpPick.cross).ToString();
                textRefreshNumberAtShop.text = GPGSController.Instance.mProgress.GetPowerUpsNumber(GameManager.PowerUpPick.refresh).ToString();
                if (rectCounterTemp != GPGSController.Instance.mProgress.Rects)
                {
                    rectCounterTemp -= rectCounterSpeed;
                    if (rectCounterTemp <= GPGSController.Instance.mProgress.Rects + rectCounterSpeed)
                        rectCounterTemp = GPGSController.Instance.mProgress.Rects;
                }

                if (GPGSController.Instance.mProgress.Rects < Game.instance.pricePUCross)
                {
                    if (crossPriceTag.interactable)
                        crossPriceTag.interactable = false;
                }
                else if (GPGSController.Instance.mProgress.Rects > Game.instance.pricePUCross)
                {
                    if (!crossPriceTag.interactable)
                        crossPriceTag.interactable = true;
                }
                if (GPGSController.Instance.mProgress.Rects < Game.instance.pricePUColorPick)
                {
                    if (colorPriceTag.interactable)
                        colorPriceTag.interactable = false;
                }
                else if (GPGSController.Instance.mProgress.Rects > Game.instance.pricePUColorPick)
                {
                    if (!colorPriceTag.interactable)
                        colorPriceTag.interactable = true;
                }
                if (GPGSController.Instance.mProgress.Rects < Game.instance.pricePURefresh)
                {
                    if (refreshPriceTag.interactable)
                        refreshPriceTag.interactable = false;
                }
                else if (GPGSController.Instance.mProgress.Rects > Game.instance.pricePURefresh)
                {
                    if (!refreshPriceTag.interactable)
                        refreshPriceTag.interactable = true;
                }
            }
            else if (shopMenu == ShopMenu.theme)
            {
                if (rectCounterTemp != GPGSController.Instance.mProgress.Rects)
                {
                    rectCounterTemp -= rectCounterSpeed;
                    if (rectCounterTemp <= GPGSController.Instance.mProgress.Rects + rectCounterSpeed)
                        rectCounterTemp = GPGSController.Instance.mProgress.Rects;
                }

                int i = 0;
                while(i < goColorThemeButton.Length)
                {
                    if (i >= GPGSController.Instance.mProgress.themeColorOpened.Count)
                    {
                        if (GPGSController.Instance.mProgress.Rects < System.Convert.ToInt32(goColorThemeButton[i].GetComponentInChildren<Text>().text))
                            goSpriteThemeButton[i].GetComponent<Button>().interactable = false;
                        else
                            goSpriteThemeButton[i].GetComponent<Button>().interactable = true;
                        i++;
                        continue;
                    }
                    if (!GPGSController.Instance.mProgress.GetColorTheme(i))
                    {
                        if (GPGSController.Instance.mProgress.Rects < System.Convert.ToInt32(goColorThemeButton[i].GetComponentInChildren<Text>().text))
                            goColorThemeButton[i].GetComponent<Button>().interactable = false;
                        else
                            goColorThemeButton[i].GetComponent<Button>().interactable = true;
                    }
                    else
                    {
                        goColorThemeButton[i].GetComponentInChildren<Button>().interactable = true;
                        goColorThemeButton[i].GetComponentInChildren<Text>().text = "Choose";
                    }
                    i++;
                }

                i = 0;
                while (i < goSpriteThemeButton.Length)
                {
                    if (i >= GPGSController.Instance.mProgress.themeSpriteOpened.Count)
                    {
                        if (GPGSController.Instance.mProgress.Rects < System.Convert.ToInt32(goSpriteThemeButton[i].GetComponentInChildren<Text>().text))
                            goSpriteThemeButton[i].GetComponent<Button>().interactable = false;
                        else
                            goSpriteThemeButton[i].GetComponent<Button>().interactable = true;
                        i++;
                        continue;
                    }
                    if (!GPGSController.Instance.mProgress.GetSpriteTheme(i))
                    {
                        if (GPGSController.Instance.mProgress.Rects < System.Convert.ToInt32(goSpriteThemeButton[i].GetComponentInChildren<Text>().text))
                            goSpriteThemeButton[i].GetComponent<Button>().interactable = false;
                        else
                            goSpriteThemeButton[i].GetComponent<Button>().interactable = true;
                    }
                    else
                    {
                        goSpriteThemeButton[i].GetComponentInChildren<Button>().interactable = true;
                        goSpriteThemeButton[i].GetComponentInChildren<Text>().text = "Choose";
                    }
                    i++;
                }
            }
        }
        else if (Game.instance.gameScreen == Game.GameScreen.option)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                anim.SetTrigger("OptionOut");
                anim.SetTrigger("MainmenuIn");
                Game.instance.gameScreen = Game.GameScreen.mainmenu;
            }

        }

    }

    public void BackFromOption()
    {
        anim.SetTrigger("OptionOut");
        anim.SetTrigger("MainmenuIn");
        Game.instance.gameScreen = Game.GameScreen.mainmenu;
    }

	private bool vidHasSetup = false;
	[HideInInspector] public int rewardAmount = 0;
	enum RewardAds
	{
		nope, rects
	}
	RewardAds rewardAds = RewardAds.nope;
	void RandomRewardAds()
	{
		int random = UnityEngine.Random.Range(0, 5);            //Chance Vid Ads Button show 1:4
		rewardAds = (random == 0 ? RewardAds.rects : RewardAds.nope);
	}
	int RandomRewardAmount()
	{
		if (rewardAds == RewardAds.rects)
			return UnityEngine.Random.Range(400, 1201);     //how many rects I can get
		return 0;
	}


    public void ShopRightButton()
    {
        if (shopMenu == ShopMenu.powerups)
        {
            anim.SetTrigger("ShopPowerUpOut");
            anim.SetTrigger("ShopThemeIn");
            shopMenu = ShopMenu.theme;
        }
        else if (shopMenu == ShopMenu.theme)
        {
            anim.SetTrigger("ShopThemeOut");
            anim.SetTrigger("ShopPowerUpIn");
            shopMenu = ShopMenu.powerups;
        }
        else if (shopMenu == ShopMenu.rects)
        {
            anim.SetTrigger("ShopRectsOut");
            anim.SetTrigger("ShopPowerUpIn");
            shopMenu = ShopMenu.powerups;
        }
    }
    public void ShopLeftButton()
    {
        if (shopMenu == ShopMenu.powerups)
        {
            anim.SetTrigger("ShopPowerUpOutRight");
            anim.SetTrigger("ShopThemeInRight");
            shopMenu = ShopMenu.theme;
        }
        else if (shopMenu == ShopMenu.theme)
        {
            anim.SetTrigger("ShopThemeOutRight");
            anim.SetTrigger("ShopPowerUpInRight");
            shopMenu = ShopMenu.powerups;
        }
        else if (shopMenu == ShopMenu.rects)
        {
            anim.SetTrigger("ShopRectsOutRight");
            anim.SetTrigger("ShopPowerUpInRight");
            shopMenu = ShopMenu.powerups;
        }
    }

    void TouchHandler()
    {
        Vector2 touchPos = new Vector2();
        if (Input.GetMouseButtonDown(0))
            touchPos = Camera.main.WorldToScreenPoint(Input.mousePosition);
        if (Input.GetMouseButtonUp(0))
        {
            if (Input.mousePosition.x - touchPos.x < -0.5f)
            {
                //handle touch
                if (shopMenu == ShopMenu.powerups)
                {
                    anim.SetTrigger("ShopPowerUpOut");
                    anim.SetTrigger("ShopThemeIn");
                    shopMenu = ShopMenu.theme;
                }
                else if (shopMenu == ShopMenu.theme)
                {
                    anim.SetTrigger("ShopThemeOut");
                    anim.SetTrigger("ShopRectsIn");
                    shopMenu = ShopMenu.rects;
                }
                else if (shopMenu == ShopMenu.rects)
                {
                    anim.SetTrigger("ShopRectsOut");
                    anim.SetTrigger("ShopPowerUpIn");
                    shopMenu = ShopMenu.powerups;
                }
            }
            else if (Input.mousePosition.x - touchPos.x > 0.5f)
            {
                //handle touch
                if (shopMenu == ShopMenu.powerups)
                {
                    anim.SetTrigger("ShopPowerUpOut");
                    anim.SetTrigger("ShopThemeIn");
                    shopMenu = ShopMenu.theme;
                }
                else if (shopMenu == ShopMenu.theme)
                {
                    anim.SetTrigger("ShopThemeOut");
                    anim.SetTrigger("ShopRectsIn");
                    shopMenu = ShopMenu.rects;
                }
                else if (shopMenu == ShopMenu.rects)
                {
                    anim.SetTrigger("ShopRectsOut");
                    anim.SetTrigger("ShopPowerUpIn");
                    shopMenu = ShopMenu.powerups;
                }
            }
        }
    }

    public void AdsButton()
    {
        Advertisement.Show("rewardVideoPlace", new ShowOptions
        {
            resultCallback = result => {
                if (result == ShowResult.Finished)
                    GPGSController.Instance.mProgress.Rects = (GPGSController.Instance.mProgress.Rects+rewardAmount);
                    vidAds.gameObject.SetActive(false);
                    rectCounterTemp = GPGSController.Instance.mProgress.Rects;
            }
        });
    }

    public void LoadGameSavedFromCloud()
    {
        GPGSController.Instance.LoadFromCloud();
    }
    
    public void ChooseColorTheme(int index)
    {
        rectCounterTemp = GPGSController.Instance.mProgress.Rects;
        //handle from adding new Theme
        if (index >= GPGSController.Instance.mProgress.themeColorOpened.Count)
            GPGSController.Instance.mProgress.themeColorOpened.Add(false);
        if (!GPGSController.Instance.mProgress.GetColorTheme(index))
        {
            //Buy
            GPGSController.Instance.mProgress.Rects -= System.Convert.ToInt32(goColorThemeButton[index].GetComponentInChildren<Text>().text);
            GPGSController.Instance.mProgress.SetColorTheme(index, true);       //set to true, check the rects is enough to buy this theme on update method
            rectCounterSpeed = System.Convert.ToInt32(goColorThemeButton[index].GetComponentInChildren<Text>().text) / 56;

        }
        GameManager.instance.colorRectangle = GameManager.instance.colorRectArray[index].GetColor;
        GameManager.instance.rectType = ShapeCreator.RectType.color;
        PlayerPrefs.SetString("Theme", "C:" + index.ToString());
        GPGSController.Instance.SaveProgress();
    }
    public void ChooseSpriteBasedTheme(int index)
    {
        rectCounterTemp = GPGSController.Instance.mProgress.Rects;
        //handle from adding new Theme
        if (index >= GPGSController.Instance.mProgress.themeSpriteOpened.Count)
            GPGSController.Instance.mProgress.themeSpriteOpened.Add(false);

        if (!GPGSController.Instance.mProgress.GetSpriteTheme(index))
        {
            //Buy
            GPGSController.Instance.mProgress.Rects -= System.Convert.ToInt32(goSpriteThemeButton[index].GetComponentInChildren<Text>().text);
            GPGSController.Instance.mProgress.SetSpriteTheme(index, true);       //set to true, check the rects is enough to buy this theme on update method
            rectCounterSpeed = System.Convert.ToInt32(goSpriteThemeButton[index].GetComponentInChildren<Text>().text) / 56;
        }
        GameManager.instance.gameobjectRectangle = GameManager.instance.gameobjectRectangleArray[index].GetSprite;
        GameManager.instance.rectType = ShapeCreator.RectType.sprite;
        PlayerPrefs.SetString("Theme", "S:" + index.ToString());
        GPGSController.Instance.SaveProgress();
    }
    public void ChooseBackgroundTheme(int index)
    {
        GameManager.instance.backgroundColor = GameManager.instance.backgroundColorArray[index];
        Camera.main.backgroundColor = GameManager.instance.backgroundColor;
        GameManager.instance.textColor = GameManager.instance.textColorArray[index];
        if (PlayerPrefs.GetString("Theme") == null || PlayerPrefs.GetString("Theme") == "")
        {
            PlayerPrefs.SetString("Theme", "S:0:" + index.ToString());
        }
        else
            PlayerPrefs.SetString("Theme", PlayerPrefs.GetString("Theme") +":" + index.ToString());
        foreach (Text txt in allTextOnCanvas)
        {
            if(txt.gameObject.name.EndsWith("cc"))
                txt.color = GameManager.instance.textColor;
        }
    }

    private bool isShareScore = false;
    public void ShareScore()
    {
        FacebookController.Instance.ShareScore(Game.instance.score, out isShareScore);
    }
}
