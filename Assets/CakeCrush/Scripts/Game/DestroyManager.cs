using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyManager : MonoBehaviour
{
    private GameObject[] effects = new GameObject[5];
    private Transform parentEffects;
    private static DestroyManager instance;
    public static DestroyManager Instance { get => instance; set => instance = value; }

    void Awake()
    {
        if (parentEffects == null) parentEffects = GameObject.Find("Effects").transform;
        if (instance == null) instance = this; else Destroy(gameObject);
    }
    // Start is called before the first frame update
    void Start()
    {
        this.GetChildEffects();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void GetChildEffects()
    {
        for (var i = 0; i < parentEffects.childCount; i++)
        {
            effects[i] = parentEffects.GetChild(i).gameObject;
        }
    }

    public void DestroyMatchedAt(int column, int row)
    {
        if (GridController.Instance.AllDots[column, row].GetComponent<DotInteraction>().IsMatched)
        {
            var obj = GridController.Instance.AllDots[column, row];
            ObjectPoolEffects.Instance.HandleDestroyEffects(obj.tag, obj.transform.position);
            GridController.Instance.AllDots[column, row].GetComponent<Dot>().PlusScoreObj();
            obj.GetComponent<DotInteraction>().IsMatched = false;
            ObjectPoolCakes.Instance.ReturnCakes(obj, obj.tag);
            GridController.Instance.AllDots[column, row] = null;
        }
    }

    public IEnumerator DestroyMatched()
    {
        yield return new WaitForSeconds(0.5f);
        for (var i = 0; i < GridController.Instance.Width; i++)
        {
            for (var j = 0; j < GridController.Instance.Height; j++)
            {
                if (GridController.Instance.AllDots[i, j] != null)
                {
                    DestroyMatchedAt(i, j);
                }
            }
        }
        AudioManager.Instance.SoundDestroy();

        StartCoroutine(this.Falling());

    }

    private IEnumerator Falling()
    {
        yield return new WaitForSeconds(0.1f);
        var count = 0;
        for (var i = 0; i < GridController.Instance.Width; i++)
        {
            for (var j = 0; j < GridController.Instance.Height; j++)
            {
                if (!GridController.Instance.AllDots[i, j])
                {
                    count++;
                }
                else if (count > 0)
                {
                    GridController.Instance.AllDots[i, j].GetComponent<DotInteraction>().Row -= count;
                    GridController.Instance.AllDots[i, j] = null;
                }
            }
            count = 0;
        }
        StartCoroutine(this.DestroyAgain());
    }

    private IEnumerator SpawnAgain()
    {
        yield return new WaitForSeconds(0.2f);
        for (var i = 0; i < GridController.Instance.Width; i++)
        {
            for (var j = 0; j < GridController.Instance.Height; j++)
            {
                if (!GridController.Instance.AllDots[i, j])
                {
                    var pos = new Vector2(i, j + 1.5f);
                    var objDot = ObjectPoolCakes.Instance.GetCakes(GridController.Instance.Dots[GridController.Instance.DotToUse()].tag, pos);
                    objDot.transform.rotation = Quaternion.identity;
                    objDot.GetComponent<DotInteraction>().Column = i;
                    objDot.GetComponent<DotInteraction>().Row = j;
                    GridController.Instance.AllDots[i, j] = objDot;
                }
            }
        }
        AudioManager.Instance.SoundSpawnDot();
    }

    private bool CheckMatched()
    {
        for (var i = 0; i < GridController.Instance.Width; i++)
        {
            for (var j = 0; j < GridController.Instance.Height; j++)
            {
                if (GridController.Instance.AllDots[i, j])
                {
                    if (GridController.Instance.AllDots[i, j].GetComponent<DotInteraction>().IsMatched)
                    {
                        return true;
                    }
                }
            }
        }
        return false;
    }

    public IEnumerator DestroyAgain()
    {
        DotInteraction dot = null;

        StartCoroutine(this.SpawnAgain());
        yield return new WaitForSeconds(0.6f);
        if (CheckMatched())
        {
            StartCoroutine(this.DestroyMatched());
            yield return new WaitForSeconds(1f);
        }
        else
        {
            GameStateController.Instance.CurrentGameState = GameState.Finish;
            ScoreController.Instance.UpdateScore();
        }

        //Check column row dot
        for (var i = 0; i < GridController.Instance.Width; i++)
        {
            for (var j = 0; j < GridController.Instance.Height; j++)
            {
                if (GridController.Instance.AllDots[i, j])
                {
                    if (GridController.Instance.AllDots[i, j])
                    {
                        dot = GridController.Instance.AllDots[i, j].GetComponent<DotInteraction>();
                        dot.Column = i;
                        dot.Row = j;
                    }
                }
            }
        }
        if (dot)
        {
            if (!dot.IsMatched)
            {
                yield return new WaitForSeconds(0.5f);
                GridController.Instance.RandomDot();
            }
        }
    }

}
