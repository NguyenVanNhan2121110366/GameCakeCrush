using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;
public class UIHome : MonoBehaviour
{
    private RectTransform uISetting;
    private Button bntSettingGame;
    private Button bntTurnOffSetting;
    private Button bntStartGame;
    private Button bntExitGame;
    private GameObject fillSetting;
    private Slider sliderMusic;

    void Awake()
    {
        if (bntSettingGame == null) bntSettingGame = GameObject.Find("bntSetting").GetComponent<Button>();
        else Debug.Log("bntSettingGame was exist");
        if (bntStartGame == null) bntStartGame = GameObject.Find("bntStart").GetComponent<Button>();
        else Debug.Log("bntStartGame was exist");
        if (bntExitGame == null) bntExitGame = GameObject.Find("bntExit").GetComponent<Button>();
        else Debug.Log("bntExitGame was exist");
        if (uISetting == null) uISetting = GameObject.Find("UIVolume").GetComponent<RectTransform>();
        else Debug.Log("uISetting was exist");
        if (fillSetting == null) fillSetting = GameObject.Find("fillSetting");
        else Debug.Log("fillSetting was exist");
        if (sliderMusic == null) sliderMusic = GameObject.Find("SliderMusic").GetComponent<Slider>();
        else Debug.Log("sliderMusic was exist");
        if (bntTurnOffSetting == null) bntTurnOffSetting = GameObject.Find("bntTurnOffUI").GetComponent<Button>();
        else Debug.Log("bntTurnOffSetting was exist");
        bntStartGame.onClick.AddListener(OnClickStartGame);
        bntExitGame.onClick.AddListener(OnClickExit);
        bntSettingGame.onClick.AddListener(OnClickSetting);
        bntTurnOffSetting.onClick.AddListener(TurnOffSetting);
    }

    void Start()
    {
        fillSetting.SetActive(false);
        uISetting.gameObject.SetActive(false);
        sliderMusic.onValueChanged.AddListener(AudioManager.Instance.VolumeBgrMusic);
        sliderMusic.value = PlayerPrefs.GetFloat("MusicVolume", 0.5f);
    }

    private void OnClickStartGame()
    {
        PlayerPrefs.SetString("SceneName", "GameScene");
        PlayerPrefs.Save();
        SceneManager.LoadScene("LoadingScene");
    }

    private void OnClickExit()
    {
        Application.Quit();
    }

    private void OnClickSetting()
    {
        fillSetting.SetActive(true);
        uISetting.gameObject.SetActive(true);
        uISetting.DOAnchorPosX(0, 1f);
    }
    private void TurnOffSetting()
    {
        uISetting.DOAnchorPosX(-830, 1f).OnComplete(() =>
        {
            uISetting.gameObject.SetActive(false);
            fillSetting.SetActive(false);
        });
    }


}
