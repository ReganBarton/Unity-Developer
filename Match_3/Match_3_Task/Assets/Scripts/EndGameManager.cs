﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum GameType
{
    Moves,
    Time
}

[System.Serializable]
public class EndGameRequirements
{
    public GameType gameType;
    public int counterValue;
}

public class EndGameManager : MonoBehaviour
{
    public GameObject movesLabel;
    public GameObject timeLabel;
    public GameObject youWinPanel;
    public GameObject tryAgainPanel;
    public GameObject pausePanel;
    public Text counter;
    public EndGameRequirements requirements;
    public int currentCounterValue;
    private Board board;
    private float timerSeconds;


    // Start is called before the first frame update
    void Start()
    {
        SetupGame();
        board = FindObjectOfType<Board>();
    }

    void SetupGame()
    {
        currentCounterValue = requirements.counterValue;
        if(requirements.gameType == GameType.Moves)
        {
            movesLabel.SetActive(true);
            timeLabel.SetActive(false);
        }
        else
        {
            timerSeconds = 1;
            movesLabel.SetActive(false);
            timeLabel.SetActive(true);
        }
        counter.text = "" + currentCounterValue;
    }

    public void DecreaseCounterValue()
    {
        if(board.currentState != GameState.pause)
        {

        currentCounterValue--;
        counter.text = "" + currentCounterValue;
        if(currentCounterValue <= 0)
        {
           LoseGame();
        }
        }

    }

    public void WinGame()
    {
        youWinPanel.SetActive(true);
        board.currentState = GameState.paused;
        currentCounterValue = 0;
        counter.text = "" + currentCounterValue;
        FadePanelController fade = FindObjectOfType<FadePanelController>();
        fade.GameOver();
    }


    public void LoseGame()
    {
        tryAgainPanel.SetActive(true);
        board.currentState = GameState.paused;
        currentCounterValue = 0;
        counter.text = "" + currentCounterValue;
        FadePanelController fade = FindObjectOfType<FadePanelController>();
        fade.GameOver();

    }

    public void PauseGame()
    {
        pausePanel.SetActive(true);
        
        Debug.Log("You Lose");
        FadePanelController fade = FindObjectOfType<FadePanelController>();
        fade.GameOver();
        board.currentState = GameState.paused;

    }

    // Update is called once per frame
    void Update()
    {
        if(requirements.gameType == GameType.Time && currentCounterValue > 0)
        {
            timerSeconds -= Time.deltaTime;
            if(timerSeconds <= 0)
            {

                DecreaseCounterValue();
                timerSeconds = 1;
            }
        }
    }
}
