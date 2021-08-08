using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 싱ㅇ글턴 클래스로
// 스코어, 골드를 가지고 있을것임
// 스태이지가 시작되면 항상 시작되도록
// 씬이 부서지면 얘도 부서질거야
public class StageManager : SingletonMonoBehavior<StageManager>
{
    public SaveInt highScore; // 앱을 재시작 해도 남아있는다
    public int score;
    public SaveInt gold;

    new private void Awake()
    {
          base.Awake();
        highScore = new SaveInt("highScore");
        gold = new SaveInt("Gold");

        GoldUIRefresh();   // 앱이 켜졌을때 골드 유지
        ScoreUIRefresh();  // 앱이 켜졋을때 점수 유지
    }
   
    public void AddScore(int addScore)
    {
        
        score += addScore;

        if (highScore < score) // 만약 하이스코어보다 스코어가 크다면
            highScore.Value = score; // 하이스코어를 스코어로 바꾼다 

        ScoreUIRefresh();

    }
    internal void AddGold(int amount)
    {
        gold += amount;

        Debug.Log(amount);
        GoldUIRefresh();
    }




    void GoldUIRefresh()
    {
        GoldUI.Instance.UpdateUI(gold);
    }




    private void ScoreUIRefresh()
    {
        ScoreUI.Instance.UpdateUI(score, highScore);
    }
}
