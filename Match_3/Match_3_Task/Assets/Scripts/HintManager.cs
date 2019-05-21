using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HintManager : MonoBehaviour
{

    private Board board;
    public float hintDelay;
    private float hintDelaySeconds;
    public GameObject hintParticle;
    public GameObject currentHint;

    // Use this for initialization
    void Start()
    {
        board = FindObjectOfType<Board>();
        hintDelaySeconds = hintDelay;
    }

    // Update is called once per frame
    void Update()
    {
        hintDelaySeconds -= Time.deltaTime;
        if (hintDelaySeconds <= 0 && currentHint == null)
        {
            MarkHint();
            hintDelaySeconds = hintDelay;
        }

    }

    //First, I want to find all possible matches on the board
    List<GameObject> FindAllMatches()
    {
        List<GameObject> possibleMoves = new List<GameObject>();
        for (int i = 0; i < board.Width; i++)
        {
            for (int w = 0; w < board.Height; w++)
            {
                if (board.allDots[i, w] != null)
                {
                    if (i < board.Width - 1)
                    {
                        if (board.SwitchCheck(i, w, Vector2.right))
                        {
                            possibleMoves.Add(board.allDots[i, w]);
                        }
                    }
                    if (w < board.Height - 1)
                    {
                        if (board.SwitchCheck(i, w, Vector2.up))
                        {
                            possibleMoves.Add(board.allDots[i, w]);

                        }
                    }
                }
            }
        }
        return possibleMoves;
    }
    //Pick one of those matches randomly
    GameObject PickOneRandomly()
    {
        List<GameObject> possibleMoves = new List<GameObject>();
        possibleMoves = FindAllMatches();
        if (possibleMoves.Count > 0)
        {
            int pieceToUse = Random.Range(0, possibleMoves.Count);
            return possibleMoves[pieceToUse];
        }
        return null;
    }
    //Create the hint behind the chosen match
    private void MarkHint()
    {
        GameObject move = PickOneRandomly();
        if (move != null)
        {
            currentHint = Instantiate(hintParticle, move.transform.position, Quaternion.identity);
        }
    }
    //Destroy the hint.
    public void DestroyHint()
    {
        if (currentHint != null)
        {
            Destroy(currentHint);
            currentHint = null;
            hintDelaySeconds = hintDelay;
        }
    }
}
