using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UI;

public class SelectManager : MonoBehaviour
{
    public SpriteAtlas spriteAtlas;  // Sprite Atlas를 Inspector에서 할당
    public Image[] image;            // UI Image 컴포넌트
    private string[] spriteName = {"11_Mario","12_Luigi","13_Yoshi","14_Peach","15_Daisy","16_Rosalina",
                                   "21_Amitie","22_Raffina","23_Sig","24_Lemres","25_Feli","26_Klug",
                                   "31_Pinkbean","32_Evan","33_Phantom","34_Angelic","35_Hoyoung","36_Lara",
                                   "41_HelloKitty","42_MyMelody","43_Kuromi","44_Pocha","45_Purin","46_Cinnamoroll"};
    private void Start()
    {
        for (int i = 0; i < 24; i++)
        {
            // Sprite Atlas에서 특정 스프라이트를 가져오기
            Sprite targetSprite = spriteAtlas.GetSprite(spriteName[i]);  // "SpriteName"은 스프라이트 이름
            int x = (i / 6) + (i % 2);
            int y = (i % 6) / 2;
            // image[i]
        }
    }
}