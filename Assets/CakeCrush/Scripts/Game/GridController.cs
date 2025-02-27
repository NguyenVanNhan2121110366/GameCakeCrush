using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class GridController : MonoBehaviour
{
    private static GridController instance;
    [SerializeField] private int width, height;
    [SerializeField] private GameObject gridObj;
    [SerializeField] private Transform dotParent;
    [SerializeField] private GameObject[] dots = new GameObject[5];
    private GameObject[,] allGrid;
    private GameObject[,] allDots;

    public GameObject[,] AllDots { get => allDots; set => allDots = value; }
    public int Width { get => width; set => width = value; }
    public int Height { get => height; set => height = value; }
    public static GridController Instance { get => instance; set => instance = value; }
    public GameObject[] Dots { get => dots; set => dots = value; }

    private void Awake()
    {
        if (instance == null) instance = this; else Destroy(gameObject);
        if (dotParent == null) dotParent = GameObject.Find("Dots").transform; else return;
    }

    // Start is called before the first frame update
    void Start()
    {
        this.BrowserDots();
        allDots = new GameObject[width, height];
        allGrid = new GameObject[width, height];
        StartCoroutine(this.CreateGrid());
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    private void BrowserDots()
    {
        for (var i = 0; i < dotParent.childCount; i++)
        {
            dots[i] = dotParent.GetChild(i).gameObject;
        }
    }

    public IEnumerator CreateGrid()
    {
        for (var i = 0; i < width; i++)
        {
            yield return null;
            for (var j = 0; j < height; j++)
            {
                yield return null;
                var pos = new Vector2(i, j);
                var objGrid = Instantiate(gridObj, pos, Quaternion.identity);
                objGrid.SetActive(true);
                allGrid[i, j] = objGrid;
                objGrid.name = "(" + i + "," + j + ")";
                objGrid.transform.SetParent(transform);
            }
        }
        StartCoroutine(this.CreateDot());
    }

    public IEnumerator CreateDot()
    {
        for (var i = 0; i < width; i++)
        {
            yield return null;
            for (var j = 0; j < height; j++)
            {
                yield return null;
                this.HandleSpawnDotObj(i, j, 0);
            }
        }
        this.RandomDot();
        GameStateController.Instance.GameStateSwipe();
        StartCoroutine(CountDownTimeManager.Instance.MinusSecond());
    }

    private void HandleSpawnDotObj(int i, int j, int k)
    {
        for (; ; )
        {
            var dotToUse = this.DotToUse();
            var originDot = this.dots[dotToUse].tag;
            var leftDot = i - 1 >= 0 && this.allDots[i - 1, j] ? this.allDots[i - 1, j].tag : string.Empty;
            var downDot = j - 1 >= 0 && this.allDots[i, j - 1] ? this.allDots[i, j - 1].tag : string.Empty;
            if (originDot == leftDot || originDot == downDot)
                continue;
            var pos = new Vector2(i, j + k);
            var objDot = ObjectPoolCakes.Instance.GetCakes(originDot, pos);
            objDot.transform.rotation = Quaternion.identity;
            allDots[i, j] = objDot;
            objDot.GetComponent<DotInteraction>().Column = i;
            objDot.GetComponent<DotInteraction>().Row = j;
            break;
        }
    }


    public int DotToUse()
    {
        var index = 0;
        var randomValue = Random.Range(0, 100);
        if (randomValue >= 0 && randomValue < 20)
        {
            index = 0;
        }
        else if (randomValue >= 20 && randomValue < 40)
        {
            index = 1;
        }
        else if (randomValue >= 40 && randomValue < 60)
        {
            index = 2;
        }
        else if (randomValue >= 60 && randomValue < 80)
        {
            index = 3;
        }
        else if (randomValue >= 80 && randomValue < 98)
        {
            index = 4;
        }
        // else
        // {
        //     Debug.Log("Dang test");
        //     index = 0;
        // }
        return index;
    }

    #region shuff table

    private bool IsCheckCanMatched()
    {
        bool isCheck = false;
        for (var i = 0; i < width; i++)
        {
            for (var j = 0; j < height; j++)
            {
                // horizontal
                if (i + 2 < this.width && i - 1 >= 0)
                {
                    this.CheckMatched(i, j, 2, 0, -1, 0, out isCheck);
                    if (isCheck)
                        return true;
                }

                if (i - 2 >= 0 && i + 1 < this.width)
                {
                    this.CheckMatched(i, j, 1, 0, -2, 0, out isCheck);
                    if (isCheck)
                        return true;
                }

                // vertical
                if (j + 2 < this.height && j - 1 >= 0)
                {
                    this.CheckMatched(i, j, 0, 2, 0, -1, out isCheck);
                    if (isCheck)
                        return true;
                }

                if (j - 2 >= 0 && j + 1 < this.height)
                {
                    this.CheckMatched(i, j, 0, 1, 0, -2, out isCheck);
                    if (isCheck)
                        return true;
                }

                //left down
                if (j - 1 >= 0 && i - 1 >= 0 && i + 1 < width)
                {
                    this.CheckMatched(i, j, -1, 0, 1, -1, out isCheck);
                    if (isCheck)
                        return true;
                }

                //right down
                if (j - 1 >= 0 && i - 1 >= 0 && i + 1 < width)
                {
                    this.CheckMatched(i, j, 1, 0, -1, -1, out isCheck);
                    if (isCheck)
                        return true;
                }

                //right up
                if (j + 1 < height && i - 1 >= 0 && i + 1 < height)
                {
                    this.CheckMatched(i, j, -1, 0, 1, 1, out isCheck);
                    if (isCheck)
                        return true;
                }

                //left up
                if (j + 1 < height && i - 1 >= 0 && i + 1 < height)
                {
                    this.CheckMatched(i, j, -1, 1, 1, 0, out isCheck);
                    if (isCheck)
                        return true;
                }

                //up left
                if (j - 1 >= 0 && j + 1 < height && i - 1 >= 0)
                {
                    this.CheckMatched(i, j, 0, -1, -1, 1, out isCheck);
                    if (isCheck)
                        return true;
                }

                //up right
                if (j - 1 >= 0 && j + 1 < height && i + 1 < width)
                {
                    this.CheckMatched(i, j, 0, -1, 1, 1, out isCheck);
                    if (isCheck)
                        return true;
                }

                //down left
                if (j + 1 < height && j - 1 >= 0 && i - 1 >= 0)
                {
                    this.CheckMatched(i, j, 0, 1, -1, -1, out isCheck);
                    if (isCheck)
                        return true;
                }

                //down right
                if (j + 1 < height && j - 1 >= 0 && i + 1 < width)
                {
                    this.CheckMatched(i, j, 0, 1, 1, -1, out isCheck);
                    if (isCheck)
                        return true;
                }

                //left right down
                if (i - 1 >= 0 && i + 1 < width && j + 1 < height)
                {
                    this.CheckMatched(i, j, -1, 1, 1, 1, out isCheck);
                    if (isCheck)
                        return true;
                }

                //left right up
                if (i - 1 >= 0 && i + 1 < width && j - 1 >= 0)
                {
                    this.CheckMatched(i, j, -1, -1, 1, -1, out isCheck);
                    if (isCheck)
                        return true;
                }

                //up down right
                if (j + 1 < height && j - 1 >= 0 && i + 1 < width)
                {
                    this.CheckMatched(i, j, 1, 1, 1, -1, out isCheck);
                    if (isCheck)
                        return true;
                }

                //up down left
                if (j + 1 < height && j - 1 >= 0 && i - 1 >= 0)
                {
                    this.CheckMatched(i, j, -1, 1, -1, -1, out isCheck);
                    if (isCheck)
                        return true;
                }

            }
        }
        return isCheck;
    }

    private void CheckMatched(int i, int j, int valueDot1i, int valueDot1j, int valueDot2i, int valueDot2j, out bool isCheck)
    {
        var originDot = this.allDots[i, j];
        var dot1 = this.allDots[i + valueDot1i, j + valueDot1j];
        var dot2 = this.allDots[i + valueDot2i, j + valueDot2j];
        if (originDot && dot1 && dot2 && dot1.CompareTag(dot2.tag) && dot2.CompareTag(originDot.tag))
        {
            isCheck = true;
        }
        else
        {
            isCheck = false;
        }
    }

    public void ShuffTable()
    {
        //Add dot into list
        var listDot = new List<GameObject>();
        for (var i = 0; i < width; i++)
        {
            for (var j = 0; j < height; j++)
            {
                listDot.Add(this.allDots[i, j]);
            }
        }

        // RandomDot in list
        DotInteraction dotObj = null;
        for (var i = 0; i < width; i++)
        {
            for (var j = 0; j < height; j++)
            {
                var randomIndex = Random.Range(0, listDot.Count);
                this.allDots[i, j] = listDot[randomIndex];
                if (listDot[randomIndex])
                {
                    dotObj = listDot[randomIndex].GetComponent<DotInteraction>();
                    dotObj.Column = i;
                    dotObj.Row = j;
                }

                listDot.Remove(listDot[randomIndex]);
                if (i == width - 1 && j == height - 1)
                {

                    StartCoroutine(DestroyManager.Instance.DestroyAgain());
                    Debug.Log("Vao dau day");
                }
            }
        }
    }

    public void RandomDot()
    {
        if (!IsCheckCanMatched())
        {
            this.ShuffTable();
            Debug.Log("Vao day ne");
        }
        else
            Debug.Log("Khong vao");
    }
    #endregion
}
