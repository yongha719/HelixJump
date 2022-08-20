using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    //싱글톤 인스턴스 선언
    public static GameManager Instance;


    public int bestscore;
    public int BestScore
    {
        get
        {
            return bestscore;
        }
        set
        {
            bestscore = value;
        }
    }
    public int score;
    public int currentStage = 0;

    public Action ResetGame;

    private void Awake()
    {
        //싱글톤 인스턴스 초기화
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        //저장되어 있던 점수를 가져옴
        bestscore = PlayerPrefs.GetInt("Highscore");
    }


    public void NextLevel()
    {
        currentStage++;
        ResetGame();
    }

    public void RestartLevel()
    {
        Debug.Log("Restarting Level");
        Instance.score = 0;
        ResetGame();
    }

    public void AddScore(int scoreToAdd)
    {
        score += scoreToAdd;

        if (score > bestscore)
        {
            PlayerPrefs.SetInt("Highscore", score);
            bestscore = score;
        }
    }


}
