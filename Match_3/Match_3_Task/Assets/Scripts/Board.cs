using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    public int Width;
    public int Height;
    public GameObject TilePrefab;
    public GameObject[] dots;
    private Background[,] Tiles;
    public GameObject[,] allDots;

    
    void Start()
    {
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
                Vector2 tempPosition = new Vector2(i, w);   /*Creates game tiles and alignes them in rows and columns. */
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
            Destroy(allDots[column, row]);
            allDots[column, row] = null;
        }
    }

    public void DestroyMatches()
    {
        for(int i = 0; i < Width; i++)
        {
            for(int w = 0; w < Width; w++)
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
        yield return new WaitForSeconds(.4f);
    }
}
