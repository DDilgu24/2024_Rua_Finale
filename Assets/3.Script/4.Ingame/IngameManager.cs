using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.U2D;

public class IngameManager : MonoBehaviour
{
    public SpriteAtlas iconSpriteAtlas; 
    public SpriteRenderer[] charIconInUFO;
    public Image[] charIconInState;

    private void Awake()
    {
        for (int i = 0; i < 4; i++)
        {
            int selectCharNo = GameManager.instance.selectCharNo[i];
            string spriteName = GameManager.instance.spriteNames[CursorPosToIndex(selectCharNo)];
            charIconInUFO[i].sprite = iconSpriteAtlas.GetSprite(spriteName);
            charIconInState[i].sprite = iconSpriteAtlas.GetSprite(spriteName);
        }
    }

    // CursorPos(11~38)를 Index(0~23)으로 변경
    private int CursorPosToIndex(int CursorPos)
    {
        return (CursorPos % 10) * 3 + (CursorPos / 10) - 4;
    }
}
