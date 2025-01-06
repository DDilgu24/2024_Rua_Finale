using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class UFOs_Controller : MonoBehaviour
{
    Tween tween;
    // 플레이어들의 스텟 클래스 배열
    public PlayerStat[] playerStats;
    // 플레이어들의 점수값 배열(x좌표 설정 용)
    private int[] playerCoins;

    private void Start()
    {
        
    }

    // 1. 라운드 성공
    private void Success()
    {/*
        tween = gameObject.transform
            .DOMove(GetPos() + Vector3.right * 5, 1f)
            .SetEase(Ease.OutQuart)
            .OnComplete(() => gameObject.transform.DOMove);*/
    }

    // 2. 라운드 실패
    private void Fail()
    {

    }

    // 3. 플레이어 탈락
    private void GameOver()
    {

    }

    // 4. 컨티뉴
    private void Continue()
    {

    }

    private Vector3 GetPos() { return gameObject.transform.position;}
    /*private Vector3 TargetPos()
    {
        for (int i = 0; i < playerStats.Length; i++)
        {
            playerCoins[i] = playerStats[i].coin;
        }
        // 1인 플레이 만들면 조정할 것
        int score_1st = playerStats[0].coin;
        int score_last = playerStats[0].coin;
        for (int i = 1; i < 4; i++)
        {
            score_1st = playerStats.Max(p => p.coin);
        }
        int x = 
    }*/
}
