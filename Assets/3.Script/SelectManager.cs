using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UI;

public class SelectManager : MonoBehaviour
{
    public SpriteAtlas spriteAtlas;  // Sprite Atlas�� Inspector���� �Ҵ�
    public Image[] image;            // UI Image ������Ʈ
    public GameObject[] cursor;
    private string[] spriteNames;
    private void Start()
    {
        spriteNames = Resources.LoadAll<Sprite>("Characters/Icon").Select(s => s.name).ToArray();

        for (int i = 0; i < 24; i++)
        {
            // Sprite Atlas���� Ư�� ��������Ʈ�� ��������
            Sprite targetSprite = spriteAtlas.GetSprite(spriteNames[i]);  // "SpriteName"�� ��������Ʈ �̸�
            int x = i / 3;
            int y = i % 3;
            image[y * 8 + x].sprite = targetSprite;
        }

        // cursor[0].SetActive(true);
    }
}