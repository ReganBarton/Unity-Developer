using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum GameState
{
    wait,
    move
}


public class Board : MonoBehaviour
{
    public GameState currentState = GameState.move;
    public int Width;
    public int Height;
    public int offSet;
    public GameObject TilePrefab;
    public GameObject[] dots;
    private Background[,] Tiles;
    public GameObject[,] allDots;
    private FindMatches findMatches;

    
    void Start()
    {
        findMatches = FindObjectOfType<FindMatches>();
        Tiles = new Background[Width, Height];  /*Initilizing array for background game tiles. */
        allDots = new GameObject[Width, Height]; /*Initilizing array for interactive tiles. */
        SetUp();

    }

    private void SetUp()
    {
        for ( int i = 0; i < Width; i++)
        {
           for (int w = 0; w < Height; w++)
            {
                Vector2 tempPosition = new Vector2(i, w + offSet);   /*Creates game tiles and alignes them in rows and columns. */
                GameObject backgroundTile =  Instantiate(TilePrefab, tempPosition,Quaternion.identity) as GameObject;
                backgroundTile.transform.parent = this.transform;
                backgroundTile.name = "( " + i + ", " + w + " )";
                int dotToUse = Random.Range(0, dots.Length);  /*Creates random colored interactive dots on the generated tiles.*/
                int maxIterations = 0;
                while(MatchesAt(i, w, dots[dotToUse]) && maxIterations < 100)
                {
                    dotToUse = Random.Range(0, dots.Length);
                    maxIterations++;
                    Debug.Log(maxIterations);
                }

                maxIterations = 0;

                GameObject dot = Instantiate(dots[dotToUse], tempPosition, Quaternion.identity);
                dot.GetComponent<Gems>().row = w;
                dot.GetComponent<Gems>().column = i;
                dot.transform.parent = this.transform;
                dot.name = "( " + i + ", " + w + " )";
                allDots[i, w] = dot;
            }
        }
    }

    private bool MatchesAt(int column, int row, GameObject piece)
    {
      if(column > 1 && row > 1)
        {
            if (allDots[column - 1, row].tag == piece.tag && allDots[column - 2, row].tag == piece.tag)
            {
                return true;
            }
            if (allDots[column, row -1].tag == piece.tag && allDots[column, row -2].tag == piece.tag)
            {
                return true;
            }
        } else if(column <= 1 || row <= 1)
        {
            if(row > 1)
            {
                if(allDots[column, row - 1].tag == piece.tag && allDots[column, row - 2].tag == piece.tag)
                {
                    return true;
                }
            }
            if (column > 1)
            {
                if (allDots[column - 1, row].tag == piece.tag && allDots[column - 2, row].tag == piece.tag)
                {
                    return true;
                }
            }
        }

        return false;
    }

    private void DestroyMatchesAt(int column, int row)
    {
        if (allDots[column, row].GetComponent<Gems>().isMatched)
        {
            findMatches.currentMatches.Remove(allDots[column, row]);
            Destroy(allDots[column, row]);
            allDots[column, row] = null;
        }
    }

    public void DestroyMatches()
    {
        for(int i = 0; i < Width; i++)
        {
            for(int w = 0; w < Height; w++)
            {
                if(allDots[i, w] != null)
                {
                    DestroyMatchesAt(i, w);
                }
            }
        }
        StartCoroutine(DecreaseRowCo());
    }

    private IEnumerator DecreaseRowCo()
    {
        int nullCount = 0;
        for(int i = 0; i < Width; i++)
        {
            for(int w = 0; w < Height; w++)
            {
                if(allDots[i, w] == null)
                {
                    nullCount++;
                }else if(nullCount > 0)
                {
                    allDots[i, w].GetComponent<Gems>().row -= nullCount;
                    allDots[i, w] = null;
                }
            }
            nullCount = 0;
        }
        yield return new WaitForSeconds(.3f);
        StartCoroutine(FillBoardCo());
    }

    private void RefillBoard()
    {
        for(int i = 0; i< Width; i++)
        {
            for (int w = 0; w < Height; w++)
            {
                if (allDots[i, w] == null)
                {
                    Vector2 tempPosition = new Vector2(i, w + offSet);
                    int dotToUse = Random.Range(0, dots.Length);
                    GameObject piece = Instantiate(dots[dotToUse], tempPosition, Quaternion.identity);
                    allDots[i, w] = piece;
                    piece.GetComponent<Gems>().row = w;
                    piece.GetComponent<Gems>().column = i;
                }
            }
        }
    }

    private bool MatchesOnBoard()
    {
        for (int i = 0; i < Width; i++)
        {
            for (int w = 0; w < Height; w++)
            {
                if(allDots[i, w] != null)
                {
                    if(allDots[i, w].GetComponent<Gems>().isMatched)
                    {
                        return true;
                    }
                }
            }
        }
        return false;
    }

    private IEnumerator FillBoardCo()
    {
        RefillBoard();
        yield return new WaitForSeconds(.3f);
        int Maxiterations = 0; 
        while (MatchesOnBoard() && Maxiterations < 100)
        {
            yield return new WaitForSeconds(.3f);
            DestroyMatches();
            Maxiterations++;
        }
        yield return new WaitForSeconds(.3f);

        if(NoMatches())
        {
            Debug.Log("No More Matches");
        }
        yield return new WaitForSeconds(.3f);


        currentState = GameState.move;
    }

    private void SwitchPieces(int column, int row, Vector2 direction)
    {
        GameObject holder = allDots[column + (int)direction.x, row + (int)direction.y] as GameObject;
        allDots[column + (int)direction.x, row + (int)direction.y] = allDots[column, row];
        allDots[column, row] = holder;

    }

    private bool CheckForMatches()
    {
        for(int i = 0; i < Width; i++)
        {
            for(int w = 0; w < Width; w++)
            {
                if(allDots[i, w] != null)
                {
                     if(i < Width - 2)
                    {
                     if(allDots[i + 1, w] != null && allDots[i + 2, w] != null)
                      {
                         if(allDots[i + 1, w].tag == allDots[i, w].tag && allDots[i + 2, w].tag == allDots[i, w].tag)
                          {
                               return true;
                          }
                      }
                    }

                     if(w < Height - 2)
                  {

                    

                    if (allDots[i, w + 1] != null && allDots[i, w + 2] != null)
                    {
                        if(allDots[i, w + 1].tag == allDots[i, w].tag && allDots[i, w + 2].tag == allDots[i, w].tag)
                        {
                            return true;
                        }
                    }
                  }
                }
            }
        }
        return false;
    }

    private bool SwitchCheck(int column, int row, Vector2 direction)
    {
        SwitchPieces(column, row, direction);
        if (CheckForMatches())
        {
            SwitchPieces(column, row, direction);
            return true;
        }
        SwitchPieces(column, row, direction);
        return false;
    }

    private bool NoMatches()
    {
        for (int i = 0; i < Width; i++)
        {
            for (int w = 0; w < Height; w++)
            {
                if (allDots[i, w] != null)
                {
                    if (i < Width - 1)
                    {
                        if (SwitchCheck(i, w, Vector2.right))
                        {
                            return false;
                        }
                    }
                    if (w < Height - 1)
                    {
                        if (SwitchCheck(i, w, Vector2.up))
                        {
                            return false;
                        }
                    }
                }
            }
        }
        return true;
    }
}
