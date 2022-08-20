using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    //싱글톤 인스턴스 선언
    public static UIManager Instance;

    public Text txtScore;
    public Text txtBest;

    private void Awake()
    {
        //싱글톤 인스턴스 초기화
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    void Update()
    {
        txtBest.text = "Best: " + GameManager.Instance.bestscore;
        txtScore.text = "Score: " + GameManager.Instance.score;
    }
}
