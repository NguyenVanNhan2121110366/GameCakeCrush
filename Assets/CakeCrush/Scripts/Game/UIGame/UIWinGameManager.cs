using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class UIWinGameManager : ButtonUIManager
{
    private RectTransform rectWinGame;
    private TextMeshProUGUI txtScoreGameOver;
    public static UIWinGameManager Instance;
    public RectTransform RectWinGame { get => rectWinGame; set => rectWinGame = value; }
    public TextMeshProUGUI TxtScoreGameOver { get => txtScoreGameOver; set => txtScoreGameOver = value; }

    void Awake()
    {
        if (Instance == null) Instance = this; else Destroy(gameObject);
        if (rectWinGame == null) rectWinGame = GameObject.Find("BgrWinGame").GetComponent<RectTransform>();
        else Debug.Log("rectGameOver was exist");
        if (txtScoreGameOver == null) txtScoreGameOver = GameObject.Find("txtScoreWinGame").GetComponent<TextMeshProUGUI>();
        else Debug.Log("txtScoreGameOver was exist");
        if (bntAgain == null) bntAgain = GameObject.Find("RestartWinGame").GetComponent<Button>();
        else Debug.Log("bntRestartGame was exist");
        if (bntHome == null) bntHome = GameObject.Find("HomeWinGame").GetComponent<Button>();
        else Debug.Log("bntHome was exist");
        bntAgain.onClick.AddListener(OnclickRestart);
        bntHome.onClick.AddListener(OnClickHome);
    }
    // Start is called before the first frame update
    void Start()
    {
        rectWinGame.gameObject.SetActive(false);
    }

    protected override void OnclickRestart()
    {
        this.Restart(rectWinGame);
    }

}
