using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStat : MonoBehaviour
{
    public int stock; // ���� ��ȸ ��
    public int coin; // ����(����)

    private void Start()
    {
        stock = 3;
        coin = 0;
    }
}
