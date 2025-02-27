using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dot : MonoBehaviour
{
    [SerializeField] private int score;
    [SerializeField] private int exp;
    [SerializeField] private int countTime;
    // Start is called before the first frame update
    void Start()
    {
        this.PlusExpByLevel();
    }

    public void PlusScoreObj()
    {
        if (GameStateController.Instance.CurrentGameState != GameState.WinGame && GameStateController.Instance.CurrentGameState != GameState.GameOver)
        {
            this.PlusExpByLevel();
            ScoreController.Instance.PlusScore += score;
            ExperienceBarManager.Instance.PlusExp += exp;
            CountDownTimeManager.Instance.PlusSeconds += countTime;
        }
    }

    private void PlusExpByLevel()
    {
        var level = PlayerLevelManager.Instance.Level;
        if (level <= 5)
        {
            exp = 5;
        }
        else if (level > 5 && level <= 15)
        {
            exp = 10;
        }
        else if (level > 15 && level <= 20)
        {
            exp = 15;
        }
        else if (level > 20 && level <= 25)
        {
            exp = 20;
        }
        else
            exp = 25;
    }


    public void BomDestroy()
    {
        var dot = GetComponent<DotInteraction>();
        var column = dot.Column;
        var row = dot.Row;
        for (var i = -1; i < 2; i++)
        {
            for (var j = -1; j < 2; j++)
            {
                if (column + i >= 0 && column + i < GridController.Instance.Width && row + j >= 0
                && row + j < GridController.Instance.Height)
                {
                    if (GridController.Instance.AllDots[column + i, row + j] != null)
                    {
                        GridController.Instance.AllDots[column + i, row + j].GetComponent<DotInteraction>().IsMatched = true;
                        StartCoroutine(DestroyManager.Instance.DestroyMatched());
                    }
                }
            }
        }
        AbilityManager.Instance.SkillWereUsed();
    }

    void OnMouseDown()
    {
        if (AbilityManager.Instance.IsCheckClickAbility)
        {
            this.BomDestroy();
            
        }
    }

}
