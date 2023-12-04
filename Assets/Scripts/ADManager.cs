using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api;
using GoogleMobileAds.Common;
using System;
using UnityEngine.SceneManagement;

public class ADManager : MonoBehaviour
{
    RewardedAd rewardedAd;
    string adUnitId;
    GameManager gm;
    MainMenu main;
    void Start()
    {
        MobileAds.Initialize(initStatus =>
        {
            print("Ads Initialized!!");
        });
    }




    public void LoadRewardedAd()
    {
        if (SceneManager.GetActiveScene().buildIndex == 1)
        {
            adUnitId = "ca-app-pub-3940256099942544/5224354917";
        }
        else
        {
            adUnitId = "ca-app-pub-3940256099942544/5224354917";
        }
        if (rewardedAd != null)
        {
            rewardedAd.Destroy();
            rewardedAd = null;
        }
        var adRequest = new AdRequest();
        adRequest.Keywords.Add("unity-admob-sample");
        RewardedAd.Load(adUnitId, adRequest,(RewardedAd ad, LoadAdError error) =>
        {
            if (error != null || ad == null)
            {
                print("Error.");
                return;
            }
            print("Load Succesfully.");
            rewardedAd = ad;
            RewardedAdEvents(rewardedAd);
        });
    }
    public void ShowRewardedAd(string which)
    {
        if (rewardedAd != null && rewardedAd.CanShowAd())
        {
            rewardedAd.Show((Reward reward) =>
            {
                print("Give reward to player!!");
                GrantReward(which);
            });
        }
        else
        {
            print("Ad not ready");
        }
    }
    public void RewardedAdEvents(RewardedAd ad)
    {
        // Raised when the ad is estimated to have earned money.
        ad.OnAdPaid += (AdValue adValue) =>
        {
            Debug.Log(("Rewarded ad paid" +
                adValue.Value + "   " +
                adValue.CurrencyCode));
        };
        // Raised when an impression is recorded for an ad.
        ad.OnAdImpressionRecorded += () =>
        {
            Debug.Log("Rewarded ad recorded an impression.");
        };
        // Raised when a click is recorded for an ad.
        ad.OnAdClicked += () =>
        {
            Debug.Log("Rewarded ad was clicked.");
        };
        // Raised when an ad opened full screen content.
        ad.OnAdFullScreenContentOpened += () =>
        {
            Debug.Log("Rewarded ad full screen content opened.");
        };
        // Raised when the ad closed full screen content.
        ad.OnAdFullScreenContentClosed += () =>
        {
            Debug.Log("Rewarded ad full screen content closed.");
        };
        // Raised when the ad failed to open full screen content.
        ad.OnAdFullScreenContentFailed += (AdError error) =>
        {
            Debug.LogError("Rewarded ad failed to open full screen content " +
                           "with error : " + error);
        };
    }



    void GrantReward(string which)
    {
        if(which == "Continue")
        {
            if (gm == null)
            {
                gm = GetComponent<GameManager>();
            }
            gm.ContinueGame();
        }
        else if (which == "Health")
        {
            if (main == null)
            {
                main = GetComponent<MainMenu>();
            }
            main.HealthUpgrade();
        }
    }
}
