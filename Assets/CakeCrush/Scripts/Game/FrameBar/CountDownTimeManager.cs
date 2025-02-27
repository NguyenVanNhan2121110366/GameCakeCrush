using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CountDownTimeManager : MonoBehaviour
{
    private TextMeshProUGUI txtSeconds;
    private int plusSeconds;
    private Image timeBar;
    public int seconds;
    public int maxSeconds;
    private float timeDelay;
    public static CountDownTimeManager Instance;
    public int PlusSeconds { get => plusSeconds; set => plusSeconds = value; }
    public int MaxSeconds { get => maxSeconds; set => maxSeconds = value; }
    public float TimeDelay { get => timeDelay; set => timeDelay = value; }
    public int Seconds { get => seconds; set => seconds = value; }
    private void Awake()
    {
        if (Instance == null) Instance = this; else Destroy(gameObject);
        if (txtSeconds == null) txtSeconds = GameObject.Find("txtSeconds").GetComponent<TextMeshProUGUI>();
        if (timeBar == null) timeBar = GameObject.Find("timeBar").GetComponent<Image>();

    }
    // Start is called before the first frame update
    void Start()
    {
        ObServerManager.AddObServer("UpdateScoreAfterRestart", UpdateSeconds);
        ObServerManager.AddObServer("UpdateUIAfterRestart", UpdateTextSeconds);
        ObServerManager.AddObServer("UpdateExpAndSeconds", UpdateScoreSeconds);
        this.UpdateSeconds();
        this.UpdateTextSeconds();
        this.UpdateTimeDelayByLevel(PlayerLevelManager.Instance.Level);
    }

    // Update is called once per frame
    void Update()
    {
        this.UpdateCountDownTime();
    }

    private void UpdateSeconds()
    {
        maxSeconds = PlayerPrefs.GetInt("MaxSeconds", 10);
        seconds = PlayerPrefs.GetInt("CurrentSeconds", maxSeconds);
        timeDelay = PlayerPrefs.GetFloat("TimeDelay", 1.5f);
    }

    public void UpdateTextSeconds()
    {
        txtSeconds.text = seconds.ToString();
    }

    public IEnumerator MinusSecond()
    {

        for (var i = 0; i < maxSeconds; i++)
        {
            //if()
            if (seconds > 0 && GameStateController.Instance.CurrentGameState == GameState.Swipe)
            {
                yield return new WaitForSeconds(timeDelay);
                seconds--;
                seconds = seconds >= 0 ? seconds : 0;
                txtSeconds.text = seconds.ToString();
                PlayerPrefs.SetInt("CurrentSeconds", seconds);
                PlayerPrefs.Save();
            }
            if (i == maxSeconds - 1 && seconds > 0)
            {
                yield return null;
                StartCoroutine(MinusSecond());
            }
            if (seconds == 0 && GameStateController.Instance.CurrentGameState != GameState.UI)
            {
                if (GameStateController.Instance.CurrentGameState != GameState.WinGame)
                {
                    UIGameOverManager.Instance.TurnOnUIStateGame(UIGameOverManager.Instance.RectGameOver, UIGameOverManager.Instance.TxtScoreGameOver, GameState.GameOver);
                }
            }
        }
    }

    private IEnumerator AnimatePlusSeconds(int plusSeconds)
    {
        var count = seconds;
        for (var i = 0; i < plusSeconds; i++)
        {
            count++;
            txtSeconds.text = count.ToString();
            yield return new WaitForSeconds(0.01f);
        }
        seconds = count;
        if (seconds >= maxSeconds)
        {
            maxSeconds = seconds;
            seconds = maxSeconds;
            PlayerPrefs.SetInt("MaxSeconds", maxSeconds);
            PlayerPrefs.SetInt("CurrentSeconds", seconds);
            PlayerPrefs.Save();
            this.UpdateTextSeconds();
        }
    }

    public void UpdateTimeDelayByLevel(int level)
    {
        timeDelay = 1.5f - (level - 1) * 0.2f;
        if (timeDelay < 0.5f)
        {
            timeDelay = 0.5f;
        }

        PlayerPrefs.SetFloat("TimeDelay", timeDelay);
        PlayerPrefs.Save();
    }

    private void UpdateCountDownTime()
    {
        timeBar.fillAmount = Mathf.Lerp(timeBar.fillAmount, (float)seconds / (float)maxSeconds, 5 * Time.deltaTime);
    }

    private void UpdateScoreSeconds()
    {
        StartCoroutine(this.AnimatePlusSeconds(plusSeconds));
    }

    void OnDestroy()
    {
        ObServerManager.RemoveObServer("UpdateScoreAfterRestart", UpdateSeconds);
        ObServerManager.RemoveObServer("UpdateUIAfterRestart", UpdateTextSeconds);
        ObServerManager.RemoveObServer("UpdateExpAndSeconds", UpdateScoreSeconds);
    }
}
