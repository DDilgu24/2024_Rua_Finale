using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class UFOs_Controller : MonoBehaviour
{
    Tween tween;
    // �÷��̾���� ���� Ŭ���� �迭
    public PlayerStat[] playerStats;
    // �÷��̾���� ������ �迭(x��ǥ ���� ��)
    private int[] playerCoins;

    private void Start()
    {
        
    }

    // 1. ���� ����
    private void Success()
    {/*
        tween = gameObject.transform
            .DOMove(GetPos() + Vector3.right * 5, 1f)
            .SetEase(Ease.OutQuart)
            .OnComplete(() => gameObject.transform.DOMove);*/
    }

    // 2. ���� ����
    private void Fail()
    {

    }

    // 3. �÷��̾� Ż��
    private void GameOver()
    {

    }

    // 4. ��Ƽ��
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
        // 1�� �÷��� ����� ������ ��
        int score_1st = playerStats[0].coin;
        int score_last = playerStats[0].coin;
        for (int i = 1; i < 4; i++)
        {
            score_1st = playerStats.Max(p => p.coin);
        }
        int x = 
    }*/
}
