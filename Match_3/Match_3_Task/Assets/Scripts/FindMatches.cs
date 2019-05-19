using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindMatches : MonoBehaviour
{
    private Board board;
    public List<GameObject> currentMatches = new List<GameObject>();

    void Start()
    {
        board = FindObjectOfType<Board>(); 
    }
    
    public void FindAllMatches()
    {
        StartCoroutine(FindAllMatchesCo());
    }

    private IEnumerator FindAllMatchesCo()
    {
        yield return new WaitForSeconds(.1f);
        for (int i = 0; i < board.Width; i++)
        {
            for (int w = 0; w < board.Height; w++)
            {
                GameObject currentDot = board.allDots[i, w];
                if(currentDot != null)
                {
                    if(i > 0 && i < board.Width - 1)
                    {
                        GameObject leftdot = board.allDots[i - 1, w];
                        GameObject rightdot = board.allDots[i + 1, w];
                        if(leftdot != null && rightdot != null)
                        {
                            if(leftdot.tag == currentDot.tag && rightdot.tag == currentDot.tag)
                            {
                                if (!currentMatches.Contains(leftdot))
                                {
                                    currentMatches.Add(leftdot);
                                }
                                leftdot.GetComponent<Gems>().isMatched = true;
                                if (!currentMatches.Contains(rightdot))
                                {
                                    currentMatches.Add(rightdot);
                                }
                                rightdot.GetComponent<Gems>().isMatched = true;
                                if (!currentMatches.Contains(currentDot))
                                {
                                    currentMatches.Add(currentDot);
                                }
                                currentDot.GetComponent<Gems>().isMatched = true;
                            }
                        }
                    }

                    if (w > 0 && w < board.Height - 1)
                    {
                        GameObject updot = board.allDots[i, w + 1];
                        GameObject downdot = board.allDots[i, w - 1];
                        if (updot != null && downdot != null)
                        {
                            if (updot.tag == currentDot.tag && downdot.tag == currentDot.tag)
                            {
                                if (!currentMatches.Contains(updot))
                                {
                                    currentMatches.Add(updot);
                                }
                                updot.GetComponent<Gems>().isMatched = true;
                                if (!currentMatches.Contains(downdot))
                                {
                                    currentMatches.Add(downdot);
                                }
                                downdot.GetComponent<Gems>().isMatched = true;
                                if (!currentMatches.Contains(currentDot))
                                {
                                    currentMatches.Add(currentDot);
                                }
                                currentDot.GetComponent<Gems>().isMatched = true;
                            }
                        }
                    }
                }
            }
        }
    }

}
