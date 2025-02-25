using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CountDownTimeManager : MonoBehaviour
{
    private static CountDownTimeManager instance;

    private TextMeshProUGUI txtSeconds;
    private int plusSeconds;
    private Image timeBar;
    public int seconds;
    public int maxSeconds;
    private float timeDelay;
    public static CountDownTimeManager Instance { get => instance; set => instance = value; }
    public int PlusSeconds { get => plusSeconds; set => plusSeconds = value; }
    public int MaxSeconds { get => maxSeconds; set => maxSeconds = value; }
    public float TimeDelay { get => timeDelay; set => timeDelay = value; }
    public int Seconds { get => seconds; set => seconds = value; }
    private void Awake()
    {
        if (instance == null) instance = this; else Destroy(gameObject);
        if (txtSeconds == null) txtSeconds = GameObject.Find("txtSeconds").GetComponent<TextMeshProUGUI>();
        if (timeBar == null) timeBar = GameObject.Find("timeBar").GetComponent<Image>();

    }
    // Start is called before the first frame update
    void Start()
    {
        this.UpdateTextSeconds();
        timeDelay = PlayerPrefs.GetFloat("TimeDelay", 1.5f);
        this.UpdateTimeDelayByLevel(PlayerLevelManager.Instance.Level);
    }

    // Update is called once per frame
    void Update()
    {
        this.UpdateCountDownTime();
    }

    public void UpdateTextSeconds()
    {
        maxSeconds = PlayerPrefs.GetInt("MaxSeconds", 10);
        seconds = PlayerPrefs.GetInt("CurrentSeconds", maxSeconds);

        txtSeconds.text = seconds.ToString();
        Debug.Log("UpdateTextSeconds");
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
        Debug.Log("GameOver");
    }

    public IEnumerator AnimatePlusSeconds(int plusSeconds)
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
        // var levelPlayer = PlayerLevelManager.Instance.Level;
        // if (timeDelay >= 1f)
        //     timeDelay = 3 - (level - 1) * 0.3f;
        // else
        // {
        //     if (levelPlayer > 7 && levelPlayer <= 21)
        //     {
        //         timeDelay = 0.9f;
        //     }
        //     else if (levelPlayer > 21 && levelPlayer <= 31)
        //     {
        //         timeDelay = 0.8f;
        //     }
        //     else if (levelPlayer > 31 && levelPlayer <= 41)
        //     {
        //         timeDelay = 0.6f;
        //     }
        //     else if (levelPlayer > 41 && levelPlayer <= 51)
        //     {
        //         timeDelay = 0.5f;
        //     }
        //     else if (levelPlayer > 51 && levelPlayer <= 61)
        //     {
        //         timeDelay = 0.4f;
        //     }
        //     else
        //         timeDelay = 0.3f;
        // }
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
}
