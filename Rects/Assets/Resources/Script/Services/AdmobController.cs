using UnityEngine;
using GoogleMobileAds.Api;

public class AdmobController : MonoBehaviour {

    public static AdmobController mInstance = new AdmobController();

    public static AdmobController Instance {
        get { return mInstance; }
    }

    private BannerView bannerView;
    private InterstitialAd interstitial;

    //Ads
    public void BannerSetup()
    {
        if (KeyIds.instance.admobBannerKey.Trim().Length != 0)
        {
            if (bannerView == null)
            {
                bannerView = new BannerView(KeyIds.instance.admobBannerKey, AdSize.Banner, AdPosition.Bottom);
                AdRequest adRequest = new AdRequest.Builder().Build();
                bannerView.LoadAd(adRequest);
            }
            bannerView.Hide();
        }
    }

    public void BannerShow()
    {
        if(bannerView != null)
            bannerView.Show();
    }

    public void BannerHide()
    {
        bannerView.Hide();
    }

    public void InterstitialSetup()
    {
        if(KeyIds.instance.admobInterstitialKey.Trim().Length != 0)
        {
            // Initialize an InterstitialAd.
            interstitial = new InterstitialAd(KeyIds.instance.admobInterstitialKey);
            // Create an empty ad request.
            AdRequest request = new AdRequest.Builder().Build();
            // Load the interstitial with the request.
            interstitial.LoadAd(request);
        }
    }

    public void InterstitialShow()
    {
        if (interstitial.IsLoaded())
            interstitial.Show();
    }
}
