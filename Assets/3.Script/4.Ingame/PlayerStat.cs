using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStat : MonoBehaviour
{
    public int stock; // 남은 기회 수
    public int coin; // 점수(코인)

    private void Start()
    {
        stock = 3;
        coin = 0;
    }
}
