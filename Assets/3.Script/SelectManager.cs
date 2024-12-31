using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UI;

public class SelectManager : MonoBehaviour
{
    public SpriteAtlas spriteAtlas;  // Sprite Atlas를 Inspector에서 할당
    public Image[] image;            // UI Image 컴포넌트
    public GameObject[] cursor;
    private string[] spriteNames;
    private void Start()
    {
        spriteNames = Resources.LoadAll<Sprite>("Characters/Icon").Select(s => s.name).ToArray();

        for (int i = 0; i < 24; i++)
        {
            // Sprite Atlas에서 특정 스프라이트를 가져오기
            Sprite targetSprite = spriteAtlas.GetSprite(spriteNames[i]);  // "SpriteName"은 스프라이트 이름
            int x = i / 3;
            int y = i % 3;
            image[y * 8 + x].sprite = targetSprite;
        }

        // cursor[0].SetActive(true);
    }
}