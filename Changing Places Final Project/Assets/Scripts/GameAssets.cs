using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameAssets : MonoBehaviour
{
    #region Instance
    private static GameAssets instance;

    public static GameAssets GetInstance()
    {
        return instance;
    }
    #endregion

    private void Awake()
    {
        instance = this;
        InitialGameAssetsSetUp();
    }
    [Header("Count Down Text")]
    public Text txtCountdown;

    [Header("General")]
    public Transform pfBody;

    [Header("Easter Island")]
    public GameObject imgEasterIslandBackground;
    public Sprite imgMoaiHead;
    public GameObject windowEasterIslandInfo;

    [Header("Venice")]
    public GameObject imgVeniceBackground;
    public Sprite imgPole;
    public GameObject windowVeniceInfo;

    [Header("Mosque City of Bagerha")]
    public GameObject imgMosqueCityBackground;
    public Sprite imgDome;
    public GameObject windowMosqueCityInfo;

    [Header("Galapagos Islands")]
    public GameObject imgGalapagosBackground;
    public Sprite imgPenguin;
    public GameObject windowGalapagosInfo;

    [Header("Statue of Liberty")]
    public GameObject imgStatueOfLiberty;
    public Sprite imgBuilding;
    public GameObject windowStatueOfLibertyInfo;

    [Header("Sounds")]
    public SoundAudioClip[] soundAudioClips;

    [Serializable]
    public class SoundAudioClip
    {
        public SoundManager.Sound sound;
        public AudioClip audioClip;
    }

    public void InitialGameAssetsSetUp()
    {
        // Set all backgrounds false initially
        imgEasterIslandBackground.SetActive(false);
        imgVeniceBackground.SetActive(false);
        imgMosqueCityBackground.SetActive(false);
        imgGalapagosBackground.SetActive(false);
        imgStatueOfLiberty.SetActive(false);

        // Set info windows to alpha 0 and false so that they can fade in when it's time, and that void Start doesn't start until it's actually time to start.
        windowEasterIslandInfo.GetComponent<CanvasGroup>().alpha = 0;
        windowEasterIslandInfo.SetActive(false);

        windowVeniceInfo.GetComponent<CanvasGroup>().alpha = 0;
        windowVeniceInfo.SetActive(false);

        windowMosqueCityInfo.GetComponent<CanvasGroup>().alpha = 0;
        windowMosqueCityInfo.SetActive(false);

        windowGalapagosInfo.GetComponent<CanvasGroup>().alpha = 0;
        windowGalapagosInfo.SetActive(false);

        windowStatueOfLibertyInfo.GetComponent<CanvasGroup>().alpha = 0;
        windowStatueOfLibertyInfo.SetActive(false);

        // Set count down text active but enabled so it isn't shown until it's time
        txtCountdown.gameObject.SetActive(true);
    }
}
