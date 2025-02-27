using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateController : MonoBehaviour
{
    private static GameStateController instance;
    [SerializeField] private GameState currentGameState;
    public GameState CurrentGameState { get => currentGameState; set => currentGameState = value; }
    public static GameStateController Instance { get => instance; set => instance = value; }

    private void Awake()
    {
        if (instance == null) instance = this; else Destroy(gameObject);

    }
    // Start is called before the first frame update
    void Start()
    {
        ObServerManager.AddObServer("ClickResume", GameStateSwipe);
        currentGameState = GameState.None;
    }

    // Update is called once per frame
    void Update()
    {
        if (currentGameState == GameState.Finish)
        {
            currentGameState = GameState.Swipe;
        }
    }
    public void GameStateSwipe()
    {
        currentGameState = GameState.Swipe;
    }
    void OnDestroy()
    {
        ObServerManager.RemoveObServer("ClickResume", GameStateSwipe);
    }
}

public enum GameState
{
    None,
    Swipe,
    CheckingDot,
    DestroyDot,
    UI,
    Finish,
    GameOver,
    WinGame,
    ExcuteAbility,
}
