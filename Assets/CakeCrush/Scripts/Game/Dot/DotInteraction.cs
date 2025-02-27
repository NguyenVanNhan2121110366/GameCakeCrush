using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DotInteraction : MonoBehaviour
{
    private int column, row;
    private Vector2 mouseUp, mouseDown;
    private int originCol, originRow;
    private int saveCol, saveRow;
    private bool isMatched;
    Dot dot;
    [SerializeField] private GameObject targetDot;
    public int Column { get => column; set => column = value; }
    public int Row { get => row; set => row = value; }
    public bool IsMatched { get => isMatched; set => isMatched = value; }

    private void Start()
    {
        isMatched = false;
    }

    private void Update()
    {
        originRow = row;
        originCol = column;
        this.MoveObject();
        Find3Matched();
    }

    #region Find Matched
    private void Find3Matched()
    {
        if (column - 1 >= 0 && column + 1 < GridController.Instance.Width)
        {
            var leftDot = GridController.Instance.AllDots[column - 1, row];
            var rightDot = GridController.Instance.AllDots[column + 1, row];
            if (leftDot && rightDot && leftDot.CompareTag(rightDot.tag) && rightDot.CompareTag(gameObject.tag))
            {
                if (leftDot != gameObject && rightDot != gameObject)
                {
                    leftDot.GetComponent<DotInteraction>().IsMatched = true;
                    rightDot.GetComponent<DotInteraction>().IsMatched = true;
                    isMatched = true;
                }

            }
        }

        if (row - 1 >= 0 && row + 1 < GridController.Instance.Width)
        {
            var upDot = GridController.Instance.AllDots[column, row + 1];
            var downDot = GridController.Instance.AllDots[column, row - 1];
            if (upDot && downDot && upDot.CompareTag(downDot.tag) && downDot.CompareTag(gameObject.tag))
            {
                if (upDot != gameObject && downDot != gameObject)
                {
                    upDot.GetComponent<DotInteraction>().IsMatched = true;
                    downDot.GetComponent<DotInteraction>().IsMatched = true;
                    isMatched = true;
                }
            }
        }
    }

    #endregion

    #region Input

    private Vector2 PosValue()
    {
        var inputPosition = Input.mousePosition;
        return inputPosition;
    }

    private string GetValue()
    {
        var inputString = "";
        if (Vector2.Distance(mouseUp, mouseDown) < 10)
        {
            GameStateController.Instance.GameStateSwipe();
            return inputString = "Invalid";
        }

        var posX = mouseUp.x - mouseDown.x;
        var posY = mouseUp.y - mouseDown.y;
        if (Mathf.Abs(posX) > Mathf.Abs(posY))
        {
            if (posX > 0)
            {
                inputString = "Right";
            }
            else
            {
                inputString = "Left";
            }
        }
        else
        {
            if (posY > 0)
            {
                inputString = "Up";
            }
            else
            {
                inputString = "Down";
            }
        }
        return inputString;
    }

    private void GetValueInput(string inputValue)
    {
        if (inputValue == "Right" && column + 1 < GridController.Instance.Width)
        {
            targetDot = GridController.Instance.AllDots[column + 1, row];
            targetDot.GetComponent<DotInteraction>().Column -= 1;
            this.SetValue();
            this.column += 1;
            GameStateController.Instance.CurrentGameState = GameState.CheckingDot;
        }
        if (inputValue == "Left" && column - 1 >= 0)
        {
            targetDot = GridController.Instance.AllDots[column - 1, row];
            targetDot.GetComponent<DotInteraction>().Column += 1;
            this.SetValue();
            column -= 1;
            GameStateController.Instance.CurrentGameState = GameState.CheckingDot;
        }
        if (inputValue == "Up" && row + 1 < GridController.Instance.Height)
        {
            targetDot = GridController.Instance.AllDots[column, row + 1];
            targetDot.GetComponent<DotInteraction>().Row -= 1;
            this.SetValue();
            row += 1;
            GameStateController.Instance.CurrentGameState = GameState.CheckingDot;
        }
        if (inputValue == "Down" && row - 1 >= 0)
        {
            targetDot = GridController.Instance.AllDots[column, row - 1];
            targetDot.GetComponent<DotInteraction>().Row += 1;
            this.SetValue();
            row -= 1;
            GameStateController.Instance.CurrentGameState = GameState.CheckingDot;
        }

        StartCoroutine(this.CheckTargetMatched());
    }
    private void SetValue()
    {
        saveCol = column;
        saveRow = row;
    }
    private void MoveObject()
    {
        // move 
        if (Mathf.Abs(originCol - transform.position.x) > 0.01f)
        {
            var pos = new Vector2(originCol, transform.position.y);
            transform.position = Vector2.Lerp(transform.position, pos, 5 * Time.deltaTime);
            if (GridController.Instance.AllDots[column, row] != gameObject)
            {
                GridController.Instance.AllDots[column, row] = gameObject;
            }
        }
        else
        {
            var pos = new Vector2(originCol, transform.position.y);
            transform.position = pos;
        }

        if (Mathf.Abs(originRow - transform.position.y) > 0.01f)
        {
            var pos = new Vector2(transform.position.x, originRow);
            transform.position = Vector2.Lerp(transform.position, pos, 5 * Time.deltaTime);
            if (GridController.Instance.AllDots[column, row] != gameObject)
            {
                GridController.Instance.AllDots[column, row] = gameObject;
            }
        }
        else
        {
            var pos = new Vector2(transform.position.x, originRow);
            transform.position = pos;
        }
    }


    void OnMouseDown()
    {
        if (GameStateController.Instance.CurrentGameState != GameState.Swipe)
            return;
        mouseDown = PosValue();
    }

    void OnMouseUp()
    {
        if (GameStateController.Instance.CurrentGameState != GameState.Swipe)
            return;
        mouseUp = PosValue();
        this.GetValueInput(this.GetValue());
    }
    #endregion

    private IEnumerator CheckTargetMatched()
    {
        if (!targetDot)
        {
            Debug.Log("No target");
        }
        else
        {
            yield return null;
            if (!isMatched && !targetDot.GetComponent<DotInteraction>().IsMatched)
            {
                yield return new WaitForSeconds(0.5f);
                targetDot.GetComponent<DotInteraction>().Column = column;
                targetDot.GetComponent<DotInteraction>().Row = row;
                column = saveCol;
                row = saveRow;
                GameStateController.Instance.GameStateSwipe();
                Debug.Log("No chay vao day");
            }
            else
            {
                GameStateController.Instance.CurrentGameState = GameState.DestroyDot;
                StartCoroutine(DestroyManager.Instance.DestroyMatched());
            }
            targetDot = null;
        }
    }
}
