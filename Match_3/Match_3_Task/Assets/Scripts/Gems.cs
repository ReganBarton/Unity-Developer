using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gems : MonoBehaviour
{
    public int column;
    public int row;
    public int TargetX;
    public int TargetY;
    public bool isMatched = false;
    private GameObject otherDot;
    private Board board;
    private Vector2 firstTouchPosition; //On finger down
    private Vector2 lastTouchPosition;  //On finger up
    private Vector2 tempPostion;
    public float swipeAngle = 0;       //Angle finger moved

    void Start()
    {
        board = FindObjectOfType<Board>();
        TargetX = (int)transform.position.x;
        TargetY = (int)transform.position.y;
        row = TargetY;
        column = TargetX;
    }


    void Update()
    {
        FindMatch();
        if (isMatched)
        {
            SpriteRenderer mySprite = GetComponent<SpriteRenderer>();
            mySprite.color = new Color(1f, 1f, 1f, .2f);
        }
        TargetX = column;
        TargetY = row;
        if(Mathf.Abs(TargetX - transform.position.x) > .1)
        {
            //Move towards target
            tempPostion = new Vector2(TargetX, transform.position.y);
            transform.position = Vector2.Lerp(transform.position, tempPostion, .4f);
        }
        else
        {
            //Directly set position
            tempPostion = new Vector2(TargetX, transform.position.y);
            transform.position = tempPostion;
            board.allDots[column, row] = this.gameObject;
        }

        if (Mathf.Abs(TargetY - transform.position.y) > .1)
        {
            //Move towards target
            tempPostion = new Vector2(transform.position.x, TargetY);
            transform.position = Vector2.Lerp(transform.position, tempPostion, .4f);
        }
        else
        {
            //Directly set position
            tempPostion = new Vector2(transform.position.x, TargetY);
            transform.position = tempPostion;
            board.allDots[column, row] = this.gameObject;
        }
    }

    private void OnMouseDown()
    {
        firstTouchPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Debug.Log(firstTouchPosition);

    }

    private void OnMouseUp()
    {
        lastTouchPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Angle();
    }

    void Angle()
    {
        swipeAngle = Mathf.Atan2(lastTouchPosition.y - firstTouchPosition.y, lastTouchPosition.x - firstTouchPosition.x) * 180 / Mathf.PI;
        //Debug.Log(swipeAngle);
        MovePieces();
    }

    void MovePieces()
    {
        if(swipeAngle > -45 && swipeAngle <= 45 && column < board.Width)
        {
            //Right swipe
            otherDot = board.allDots[column + 1, row];
            otherDot.GetComponent<Gems>().column -= 1;
            column += 1;
        }
        else if (swipeAngle > 45 && swipeAngle <+ 135 && row < board.Height)
        {
            //Up swipe
            otherDot = board.allDots[column, row + 1];
            otherDot.GetComponent<Gems>().row -= 1;
            row += 1;
        }
        else if ((swipeAngle > 135 || swipeAngle <= -135) && column > 0)
        {
            //Left swipe
            otherDot = board.allDots[column - 1, row];
            otherDot.GetComponent<Gems>().column += 1;
            column -= 1;
        }
        else if (swipeAngle < -45 && swipeAngle >= -135 && row > 0)
        {
            //Down swipe
            otherDot = board.allDots[column, row - 1];
            otherDot.GetComponent<Gems>().row += 1;
            row -= 1;
        }
    }

    void FindMatch()
    {
        if(column > 0 && column < board.Width - 1)
        {
            GameObject leftGem1 = board.allDots[column - 1, row];
            GameObject rightGem1 = board.allDots[column + 1, row];
            if (leftGem1.tag == this.gameObject.tag && rightGem1.tag == this.gameObject.tag)
            {
                leftGem1.GetComponent<Gems>().isMatched = true;
                rightGem1.GetComponent<Gems>().isMatched = true;
                isMatched = true;
            }
        }

        if (row > 0 && row < board.Height - 1)
        {
            GameObject UpGem1 = board.allDots[column, row + 1];
            GameObject DownGem1 = board.allDots[column, row - 1];
            if (UpGem1.tag == this.gameObject.tag && DownGem1.tag == this.gameObject.tag)
            {
                UpGem1.GetComponent<Gems>().isMatched = true;
                DownGem1.GetComponent<Gems>().isMatched = true;
                isMatched = true;
            }
        }
    }

}
