using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    private Board board;
    public Text scoreText;
    public int score;
    public Image scoreBar;
    private EndGameManager endGame;


    void Start()
    {
        endGame = FindObjectOfType<EndGameManager>();
        board = FindObjectOfType<Board>();
        
    }


    void Update()
    {
        scoreText.text = "" + score;
    }

    public void IncreaseScore(int amountToIncrease)
    {
        score += amountToIncrease;
        UpdateBar();
        
    }


    private void UpdateBar()
    {
        if (board != null && scoreBar != null)
        {
            int length = board.scoreGoals.Length;
            scoreBar.fillAmount = (float)score / (float)board.scoreGoals[length - 1];
        }
        if(score >= board.scoreGoals[1])
        {
            endGame.WinGame();
        }
    }

}
