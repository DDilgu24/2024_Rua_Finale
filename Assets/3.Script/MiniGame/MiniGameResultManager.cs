using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MiniGameResultManager : MonoBehaviour
{
    [SerializeField] private Toggle toggle;
    [SerializeField] private Slider slider;
    public void OnApplyButtonPressed()
    {
        bool pass = toggle.isOn;
        int rank = (int)slider.value;

        for (int i = 1; i <= 4; i++)
        {
            GameManager.instance.MinigameResult(i, pass, rank);
        }

        InGameManager.instance.AfterMiniGame();
        GameManager.instance.LoadScene("4.InGame", 0.15f);
    }
}
