using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Unity.VisualScripting;
public class ButtonSettingController : MonoBehaviour
{
    private Button bntSetting;
    private Image uISetting;
    private RectTransform recUISetting;
    private GameObject fillUIgame;
    public static ButtonSettingController Instance;
    public GameObject FillUIGame { get => fillUIgame; set => fillUIgame = value; }
    public Button BntSetting { get => bntSetting; set => bntSetting = value; }

    void Awake()
    {
        if (Instance == null) Instance = this; else Destroy(gameObject);
        if (bntSetting == null) bntSetting = GameObject.Find("bntSetting").GetComponent<Button>();
        else Debug.Log("bntSetting was exist");
        if (uISetting == null) uISetting = GameObject.Find("BgrSetting").GetComponent<Image>();
        else Debug.Log("bntUISetting was exist");
        if (recUISetting == null) recUISetting = GameObject.Find("BgrSetting").GetComponent<RectTransform>();
        else Debug.Log("recUISetting was exist");
        if (fillUIgame == null) fillUIgame = GameObject.Find("fillUIGame");
        bntSetting.onClick.AddListener(OnClickBntSetting);
    }
    // Start is called before the first frame update
    void Start()
    {
        ObServerManager.AddObServer("ClickResume", AnimTurnOffUI);
        ObServerManager.AddObServer("OnclickRestart", AnimTurnOffUI);
        fillUIgame.SetActive(false);
        uISetting.gameObject.SetActive(false);
    }
    private void OnClickBntSetting()
    {
        GameStateController.Instance.CurrentGameState = GameState.UI;
        fillUIgame.SetActive(true);
        uISetting.gameObject.SetActive(true);
        recUISetting.DOAnchorPosY(0, 0.5f);
        bntSetting.gameObject.SetActive(false);
    }

    private void AnimTurnOffUI()
    {
        recUISetting.DOAnchorPosY(-550, 0.5f).OnComplete(() =>
            {
                uISetting.gameObject.SetActive(false);
                fillUIgame.SetActive(false);
                bntSetting.gameObject.SetActive(true);
                if (CountDownTimeManager.Instance.Seconds == 0)
                {
                    UIGameOverManager.Instance.TurnOnUIStateGame(UIGameOverManager.Instance.RectGameOver, UIGameOverManager.Instance.TxtScoreGameOver, GameState.GameOver);
                }
            });
    }
    void OnDestroy()
    {
        ObServerManager.RemoveObServer("ClickResume", AnimTurnOffUI);
        ObServerManager.RemoveObServer("OnclickRestart", AnimTurnOffUI);
    }
}
