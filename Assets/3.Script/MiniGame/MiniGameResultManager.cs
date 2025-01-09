using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MiniGameResultManager : MonoBehaviour
{
    [SerializeField] private Toggle[] toggle;
    [SerializeField] private Slider[] slider;

    private void Start()
    {
        for (int i = 0; i < 4; i++)
        {
            int r = Random.Range(0, 60);
            toggle[i].isOn = (r >= 20);
            slider[i].value = r / 10;
        }
    }

    public void OnApplyButtonPressed()
    {

        for (int i = 1; i <= 4; i++)
        {
            bool pass = toggle[i-1].isOn;
            int rank = (int)slider[i-1].value;
            GameManager.instance.MinigameResult(i, pass, rank);
        }

        InGameManager.instance.AfterMiniGame();
        GameManager.instance.LoadScene("4.InGame", 0.15f);
    }
}
