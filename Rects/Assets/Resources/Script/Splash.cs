using UnityEngine;
using System.Collections;
using UnityEngine.Advertisements;

public class Splash : MonoBehaviour {

    Animator anim;                          //Animator for animation in splash screen
    public bool deleteSavedGame;            //set true if you want to delete the saved game

    // Use this for initialization
    void Awake() {
        anim = GetComponent<Animator>();    //get Animator component
        if (deleteSavedGame)                //if deleteSavedGame,
            PlayerPrefs.DeleteAll();        //Delete save game
        GPGSController.Instance.InitializeGooglePlayGames();        //Setup Google Play Games Services
        FacebookController.Instance.FBInit();           //Setup Facebook SDK
 
    }
    
    void Start()
    {
        if (Advertisement.isSupported)          //Check if device support Video Ads Unity
            Advertisement.Initialize(KeyIds.instance.unityAdsKey);  //fill with your ID key to monetiz
        else
            Debug.Log("Not Supported");

        //Admob
        AdmobController.Instance.BannerSetup();
        AdmobController.Instance.InterstitialSetup();
    }

    // Update is called once per frame
    void Update() {
        if (GPGSController.Instance.status == GPGSController.GPGSStatus.authenticating || GPGSController.Instance.status == GPGSController.GPGSStatus.nope)
            anim.SetBool("isLoading", true);            // when login google still authenticating, animation still play
        else if (GPGSController.Instance.status == GPGSController.GPGSStatus.signIn || GPGSController.Instance.status == GPGSController.GPGSStatus.failed)
        {
            anim.SetBool("isLoading", false);       //when login google success or fail, loading is over
            StartCoroutine(goToMainScreen(1f));     //and open main scene
        }
    }
    IEnumerator goToMainScreen(float time)
    {
        yield return new WaitForSeconds(time);  //wait time
        Application.LoadLevel("Main");          //open main scene
    }
}
