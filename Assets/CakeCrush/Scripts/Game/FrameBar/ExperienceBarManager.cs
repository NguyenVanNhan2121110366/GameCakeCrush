using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ExperienceBarManager : MonoBehaviour
{
    private static ExperienceBarManager instance;
    [SerializeField] private int plusExp;
    [SerializeField] private int currentExp;
    private TextMeshProUGUI txtExp;
    private Image experienceBar;
    private Image frameBar;
    private int maxExp;
    private Animator animTxtLevel;
    private Animator animExperienceBar;
    private Animator animframeBar;
    private GameObject fillLevel;
    public static ExperienceBarManager Instance { get => instance; set => instance = value; }
    public int MaxExp { get => maxExp; set => maxExp = value; }
    public int PlusExp { get => plusExp; set => plusExp = value; }
    public int CurrentExp { get => currentExp; set => currentExp = value; }
    public GameObject FillLevel { get => fillLevel; set => fillLevel = value; }
    private void Awake()
    {
        if (instance == null) instance = this; else Destroy(gameObject);
        if (txtExp == null) txtExp = GameObject.Find("txtCurrentExp").GetComponent<TextMeshProUGUI>();
        else Debug.Log("txtExp was exist");
        if (experienceBar == null) experienceBar = GameObject.Find("experienceBar").GetComponent<Image>();
        else Debug.Log("experience was exist");
        if (frameBar == null) frameBar = GameObject.Find("bgrFrame").GetComponent<Image>();
        if (animExperienceBar == null) animExperienceBar = experienceBar.GetComponent<Animator>();
        if (animframeBar == null) animframeBar = frameBar.GetComponent<Animator>();
        if (fillLevel == null) fillLevel = GameObject.Find("fillLevelUp");
        else Debug.Log("fillLevel was exist");
        //if (PlayerLevelManager.Instance)
    }
    // Start is called before the first frame update
    void Start()
    {
        ObServerManager.AddObServer("UpdateScoreAfterRestart", GetValue);
        ObServerManager.AddObServer("UpdateUIAfterRestart", UpdateText);
        ObServerManager.AddObServer("UpdateExpAndSeconds", UpdateScoreExperience);
        if (animTxtLevel == null) animTxtLevel = PlayerLevelManager.Instance.TxtLevel.GetComponent<Animator>();
        this.GetValue();
        this.UpdateText();
        this.fillLevel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        this.UpdateExpBar();
    }

    private void GetValue()
    {
        currentExp = PlayerPrefs.GetInt("CurrentExp", 0);
        maxExp = PlayerPrefs.GetInt("MaxExp", CheckMaxLevelExp(PlayerLevelManager.Instance.Level));
    }

    private void UpdateExpBar()
    {
        experienceBar.fillAmount = Mathf.Lerp(experienceBar.fillAmount, (float)currentExp / (float)maxExp, 10 * Time.deltaTime);
    }

    private IEnumerator AnimateExperience(int plusExp)
    {
        var count = currentExp;
        for (var i = 0; i < plusExp; i += 5)
        {
            count += 5;
            txtExp.text = count + " / " + maxExp;
            yield return new WaitForSeconds(0.05f);
        }
        currentExp = count;
        StartCoroutine(CheckExperienceUpLevel());
        yield return new WaitForSeconds(1f);
        if (PlayerLevelManager.Instance.Level > 5)
        {
            CountDownTimeManager.Instance.Seconds = 0;
            UIWinGameManager.Instance.TurnOnUIStateGame(UIWinGameManager.Instance.RectWinGame, UIWinGameManager.Instance.TxtScoreGameOver, GameState.WinGame);
        }
    }
    private void UpdateScoreExperience()
    {
        StartCoroutine(AnimateExperience(plusExp));
    }

    private IEnumerator CheckExperienceUpLevel()
    {
        if (currentExp >= MaxExp)
        {
            var ischeck = true;
            var level = 0;
            fillLevel.SetActive(true);
            AudioManager.Instance.SoundUpLevel();
            while (ischeck)
            {
                var remainingExperience = currentExp - MaxExp;
                level = ++PlayerLevelManager.Instance.Level;
                Debug.Log(level);
                maxExp = this.CheckMaxLevelExp(level);
                currentExp = remainingExperience;
                this.UpdateText();
                animTxtLevel.SetTrigger("UpLevel");
                animExperienceBar.SetTrigger("UpLevel");
                animframeBar.SetTrigger("FrameUpLevel");
                CountDownTimeManager.Instance.UpdateTimeDelayByLevel(level);
                yield return new WaitForSeconds(0.5f);
                if (currentExp < maxExp)
                {
                    Debug.Log("Co vao day khong ");
                    ischeck = false;
                }
            }
            PlayerPrefs.SetInt("CurrentLevel", PlayerLevelManager.Instance.Level);
            PlayerPrefs.SetInt("CurrentExp", currentExp);
            PlayerPrefs.SetInt("MaxExp", maxExp);
            PlayerPrefs.Save();
            //this.UpdateText();
        }

    }

    private int CheckMaxLevelExp(int level)
    {
        return 50 * level;
    }

    private void UpdateText()
    {
        txtExp.text = currentExp + " / " + maxExp;
        PlayerLevelManager.Instance.TxtLevel.text = "Level " + PlayerLevelManager.Instance.Level;
    }
    void OnDestroy()
    {
        ObServerManager.RemoveObServer("UpdateUIAfterRestart", UpdateText);
        ObServerManager.RemoveObServer("UpdateScoreAfterRestart", GetValue);
        ObServerManager.RemoveObServer("UpdateExpAndSeconds", UpdateScoreExperience);
    }
}
