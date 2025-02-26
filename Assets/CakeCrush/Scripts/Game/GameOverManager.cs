using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverManager : MonoBehaviour
{
    private static GameOverManager instance;
    public static GameOverManager Instance { get => instance; set => instance = value; }

    void Awake()
    {
        if (instance == null) instance = this; else Destroy(gameObject);
    }

    public void ResetGame()
    {
        if (GameStateController.Instance.CurrentGameState == GameState.GameOver
        || GameStateController.Instance.CurrentGameState == GameState.UI
        || GameStateController.Instance.CurrentGameState == GameState.WinGame)
        {
            GameStateController.Instance.CurrentGameState = GameState.Swipe;
            //Save data after restart
            PlayerPrefs.SetInt("CurrentSeconds", 10);
            PlayerPrefs.SetInt("MaxSeconds", 10);
            PlayerPrefs.SetInt("CurrentLevel", 1);
            PlayerPrefs.SetInt("CurrentExp", 0);
            PlayerPrefs.SetInt("MaxExp", 50);
            PlayerPrefs.SetInt("CurrentScore", 0);
            PlayerPrefs.SetFloat("TimeDelay", 1.5f);
            PlayerPrefs.Save();

            // Update all Score after restart
            ScoreController.Instance.CurrentScore = PlayerPrefs.GetInt("CurrentScore");
            ExperienceBarManager.Instance.CurrentExp = PlayerPrefs.GetInt("CurrentExp");
            ExperienceBarManager.Instance.MaxExp = PlayerPrefs.GetInt("MaxExp");
            CountDownTimeManager.Instance.Seconds = PlayerPrefs.GetInt("CurrentSeconds");
            CountDownTimeManager.Instance.MaxSeconds = PlayerPrefs.GetInt("MaxSeconds");
            CountDownTimeManager.Instance.TimeDelay = PlayerPrefs.GetFloat("TimeDelay");
            PlayerLevelManager.Instance.Level = PlayerPrefs.GetInt("CurrentLevel");
            AbilityManager.Instance.CountTimeCoolDown = 15f;

            // Update UI after restart
            ExperienceBarManager.Instance.UpdateText();
            CountDownTimeManager.Instance.UpdateTextSeconds();
            ScoreController.Instance.UpdateUIScore();
            Debug.Log("Vao day ne");
        }
    }
}
