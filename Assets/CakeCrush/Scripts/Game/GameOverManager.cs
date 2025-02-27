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
    void Start()
    {
        ObServerManager.AddObServer("OnclickRestart", ResetGame);
    }

    public void ResetGame()
    {
        if (GameStateController.Instance.CurrentGameState == GameState.GameOver
        || GameStateController.Instance.CurrentGameState == GameState.UI
        || GameStateController.Instance.CurrentGameState == GameState.WinGame)
        {
            GameStateController.Instance.GameStateSwipe();
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
            ObServerManager.Notifine("UpdateScoreAfterRestart");

            // Update UI after restart
            ObServerManager.Notifine("UpdateUIAfterRestart");
        }
    }

    void OnDestroy()
    {
        ObServerManager.RemoveObServer("OnclickRestart", ResetGame);
    }
}
