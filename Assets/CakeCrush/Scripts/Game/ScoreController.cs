using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreController : MonoBehaviour
{
    private static ScoreController instance;
    [SerializeField] private int currentScore;

    [SerializeField] private int plusScore;


    private TextMeshProUGUI txtScore;
    public static ScoreController Instance { get => instance; set => instance = value; }
    public int PlusScore { get => plusScore; set => plusScore = value; }
    public TextMeshProUGUI TxtScore { get => txtScore; set => txtScore = value; }
    public int CurrentScore { get => currentScore; set => currentScore = value; }



    private void Awake()
    {
        if (instance == null) instance = this; else Destroy(gameObject);

        if (txtScore == null) txtScore = GameObject.Find("txtScore").GetComponent<TextMeshProUGUI>(); else Debug.Log("txtScore was exist");
    }
    private void Start()
    {
        ObServerManager.AddObServer("UpdateScoreAfterRestart", GetValueCurrentScore);
        ObServerManager.AddObServer("UpdateUIAfterRestart", UpdateUIScore);
        this.GetValueCurrentScore();
        this.UpdateUIScore();
    }

    private void GetValueCurrentScore()
    {
        currentScore = PlayerPrefs.GetInt("CurrentScore", 0);
    }

    public void UpdateScore()
    {
        StartCoroutine(this.AnimateScore(plusScore));
        ObServerManager.Notifine("UpdateExpAndSeconds");
        this.ResetScore();
    }

    private IEnumerator AnimateScore(int plusScore)
    {
        var count = currentScore;
        for (var i = 0; i < plusScore; i += 5)
        {
            count += 5;
            txtScore.text = count.ToString();
            yield return new WaitForSeconds(0.05f);
        }
        currentScore = count;
        PlayerPrefs.SetInt("CurrentScore", currentScore);
    }

    private void UpdateUIScore()
    {
        txtScore.text = currentScore.ToString();
    }

    private void ResetScore()
    {
        plusScore = 0;
        ExperienceBarManager.Instance.PlusExp = 0;
        CountDownTimeManager.Instance.PlusSeconds = 0;
    }

    void OnDestroy()
    {
        ObServerManager.RemoveObServer("UpdateScoreAfterRestart", GetValueCurrentScore);
        ObServerManager.RemoveObServer("UpdateUIAfterRestart", UpdateUIScore);
    }
}
