using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class UIGameOverManager : ButtonUIManager
{
    private RectTransform rectGameOver;
    private TextMeshProUGUI txtScoreGameOver;
    public static UIGameOverManager Instance;
    public RectTransform RectGameOver { get => rectGameOver; set => rectGameOver = value; }
    public TextMeshProUGUI TxtScoreGameOver { get => txtScoreGameOver; set => txtScoreGameOver = value; }

    void Awake()
    {
        if (Instance == null) Instance = this; else Destroy(gameObject);
        if (rectGameOver == null) rectGameOver = GameObject.Find("BgrLoseGame").GetComponent<RectTransform>();
        else Debug.Log("rectGameOver was exist");
        if (txtScoreGameOver == null) txtScoreGameOver = GameObject.Find("txtScoreGameOver").GetComponent<TextMeshProUGUI>();
        else Debug.Log("txtScoreGameOver was exist");
        if (bntAgain == null) bntAgain = GameObject.Find("RestartGameOver").GetComponent<Button>();
        else Debug.Log("bntRestartGame was exist");
        if (bntHome == null) bntHome = GameObject.Find("HomeGameOver").GetComponent<Button>();
        else Debug.Log("bntHome was exist");
        bntAgain.onClick.AddListener(OnclickRestart);
        bntHome.onClick.AddListener(OnClickHome);
    }
    // Start is called before the first frame update
    void Start()
    {
        rectGameOver.gameObject.SetActive(false);
    }

    protected override void OnclickRestart()
    {
        this.Restart(rectGameOver);
    }


}
