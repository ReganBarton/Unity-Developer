using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Gems : MonoBehaviour
{
    public int column;
    public int row;
    public int previousColumn;
    public int previousRow;
    public int TargetX;
    public int TargetY;
    public bool isMatched = false;

    private EndGameManager endGameManager;
    private HintManager hintManager;
    private FindMatches findMatches; 
    private GameObject otherDot;
    private Board board;
    private Vector2 firstTouchPosition; 
    private Vector2 lastTouchPosition;  
    private Vector2 tempPostion;
    public float swipeAngle = 0;       
    public float swipeResist = 1f;

    void Start()
    {


        endGameManager = FindObjectOfType<EndGameManager>();
        hintManager = FindObjectOfType<HintManager>();
        board = FindObjectOfType<Board>();
        findMatches = FindObjectOfType<FindMatches>();
    }


    void Update()
    {
        
        if (isMatched)
        {
            SpriteRenderer mySprite = GetComponent<SpriteRenderer>();
            mySprite.color = new Color(1f, 1f, 1f, .2f);
        }
        TargetX = column;
        TargetY = row;
        if(Mathf.Abs(TargetX - transform.position.x) > .1)
        {
          
            tempPostion = new Vector2(TargetX, transform.position.y);
            transform.position = Vector2.Lerp(transform.position, tempPostion, .6f);
            if (board.allDots[column, row] != this.gameObject)
            {
                board.allDots[column, row] = this.gameObject;
            }
            findMatches.FindAllMatches();
        }
        else
        {
          
            tempPostion = new Vector2(TargetX, transform.position.y);
            transform.position = tempPostion;
            board.allDots[column, row] = this.gameObject;
        }

        if (Mathf.Abs(TargetY - transform.position.y) > .1)
        {
          
            tempPostion = new Vector2(transform.position.x, TargetY);
            transform.position = Vector2.Lerp(transform.position, tempPostion, .6f);
            if(board.allDots[column, row] != this.gameObject)
            {
                board.allDots[column, row] = this.gameObject;
            }

            findMatches.FindAllMatches();

        }
        else
        {
      
            tempPostion = new Vector2(transform.position.x, TargetY);
            transform.position = tempPostion;
            board.allDots[column, row] = this.gameObject;
        }
    }

    public IEnumerator CheckMoveCo()
    {
        yield return new WaitForSeconds(.3f);
        if(otherDot != null)
        {
            if (!isMatched && !otherDot.GetComponent<Gems>().isMatched)
            {
                otherDot.GetComponent<Gems>().row = row;
                otherDot.GetComponent<Gems>().column = column;
                row = previousRow;
                column = previousColumn;
                yield return new WaitForSeconds(.3f);
                board.currentState = GameState.move;
            }
            else
            {
                if(endGameManager != null)
                {
                    if (endGameManager.requirements.gameType == GameType.Moves)
                    {
                        endGameManager.DecreaseCounterValue();
                    }
                }
                board.DestroyMatches();

            }
            otherDot = null;
        }
        

    }

    private void OnMouseDown()
    {

        if(hintManager != null)
        {
            hintManager.DestroyHint();
        }

        if (board.currentState == GameState.move)
        { 
        firstTouchPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }



    }

    private void OnMouseUp()
    {
        if (board.currentState == GameState.move)
        {
            lastTouchPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Angle();
        }
    }

    void Angle()
    {
        if(Mathf.Abs(lastTouchPosition.y -firstTouchPosition.y) > swipeResist || Mathf.Abs(lastTouchPosition.x - firstTouchPosition.x) > swipeResist)
        {
        swipeAngle = Mathf.Atan2(lastTouchPosition.y - firstTouchPosition.y, lastTouchPosition.x - firstTouchPosition.x) * 180 / Mathf.PI;
        MovePieces();
            board.currentState = GameState.wait;
        }
        else
        {
            board.currentState = GameState.move;
        }
    }

    void MovePieces()
    {
        if(swipeAngle > -45 && swipeAngle <= 45 && column < board.Width - 1)
        {
    
            otherDot = board.allDots[column + 1, row];
            previousRow = row;
            previousColumn = column;
            otherDot.GetComponent<Gems>().column -= 1;
            column += 1;
        }
        else if (swipeAngle > 45 && swipeAngle <+ 135 && row < board.Height - 1)
        {
      
            otherDot = board.allDots[column, row + 1];
            previousRow = row;
            previousColumn = column;
            otherDot.GetComponent<Gems>().row -= 1;
            row += 1;
        }
        else if ((swipeAngle > 135 || swipeAngle <= -135) && column > 0)
        {
         
            otherDot = board.allDots[column - 1, row];
            previousRow = row;
            previousColumn = column;
            otherDot.GetComponent<Gems>().column += 1;
            column -= 1;
        }
        else if (swipeAngle < -45 && swipeAngle >= -135 && row > 0)
        {

            otherDot = board.allDots[column, row - 1];
            previousRow = row;
            previousColumn = column;
            otherDot.GetComponent<Gems>().row += 1;
            row -= 1;
        }
        StartCoroutine(CheckMoveCo());
    }

    void FindMatch()
    {
        if(column > 0 && column < board.Width - 1)
        {
            GameObject leftGem1 = board.allDots[column - 1, row];
            GameObject rightGem1 = board.allDots[column + 1, row];

            if(leftGem1 != null && rightGem1 != null)
        {
            
            if (leftGem1.tag == this.gameObject.tag && rightGem1.tag == this.gameObject.tag)
            {
                leftGem1.GetComponent<Gems>().isMatched = true;
                rightGem1.GetComponent<Gems>().isMatched = true;
                isMatched = true;
            }
        }
    }

        if (row > 0 && row < board.Height - 1)
        {
            GameObject UpGem1 = board.allDots[column, row + 1];
            GameObject DownGem1 = board.allDots[column, row - 1];
            if(UpGem1 != null && DownGem1 != null)
            { 
            if (UpGem1.tag == this.gameObject.tag && DownGem1.tag == this.gameObject.tag)
                {
                    UpGem1.GetComponent<Gems>().isMatched = true;
                 DownGem1.GetComponent<Gems>().isMatched = true;
                 isMatched = true;
                }
            }
        }
    }

}
