using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;
using UnityEngine.SceneManagement;
public abstract class ButtonUIManager : MonoBehaviour
{
    protected Button bntHome;
    protected Button bntAgain;
    protected virtual void OnClickResume() { }

    protected virtual void DestroyDotWinGameAndLose()
    {
        if (GameStateController.Instance.CurrentGameState == GameState.WinGame
        || GameStateController.Instance.CurrentGameState == GameState.GameOver)
        {
            for (var i = 0; i < GridController.Instance.Width; i++)
            {
                for (var j = 0; j < GridController.Instance.Height; j++)
                {
                    if (GridController.Instance.AllDots[i, j])
                    {
                        GridController.Instance.AllDots[i, j].GetComponent<DotInteraction>().IsMatched = true;
                        DestroyManager.Instance.DestroyMatchedAt(i, j);
                    }
                }
            }
        }
    }

    public virtual void TurnOnUIStateGame(RectTransform rect, TextMeshProUGUI txtScoreStateGame, GameState gameState)
    {
        ButtonSettingController.Instance.BntSetting.gameObject.SetActive(false);
        GameStateController.Instance.CurrentGameState = gameState;
        this.DestroyDotWinGameAndLose();
        rect.gameObject.SetActive(true);
        this.UpdateTextScoreStateGame(txtScoreStateGame, gameState);
        ButtonSettingController.Instance.FillUIGame.SetActive(true);
        rect.DOAnchorPosY(0, 0.5f);
    }

    public virtual void UpdateTextScoreStateGame(TextMeshProUGUI txtScoreStateGame, GameState gameState)
    {
        GameStateController.Instance.CurrentGameState = gameState;
        txtScoreStateGame.text = "Score : " + ScoreController.Instance.CurrentScore;
    }

    protected virtual void OnClickHome()
    {
        GameOverManager.Instance.ResetGame();
        SceneManager.LoadScene("LoadingScene");
    }

    protected abstract void OnclickRestart();

    protected virtual void Restart(RectTransform rect)
    {
        rect.DOAnchorPosY(-550, 0.5f).OnComplete(() =>
        {
            ButtonSettingController.Instance.BntSetting.gameObject.SetActive(true);
            rect.gameObject.SetActive(false);
            ButtonSettingController.Instance.FillUIGame.SetActive(false);
            GameOverManager.Instance.ResetGame();
            //StartCoroutine(CountDownTimeManager.Instance.MinusSecond());
            StartCoroutine(GridController.Instance.CreateDot());
            StartCoroutine(AbilityManager.Instance.UpdateCountTimeCoolDown());
        });
        Debug.Log("Co vao day khong");
    }
}
