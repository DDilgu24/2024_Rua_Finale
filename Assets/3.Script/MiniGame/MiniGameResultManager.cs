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

        GameManager.instance.MinigameResult(1, pass, rank);
        GameManager.instance.MinigameResult(2, pass, rank);
        GameManager.instance.MinigameResult(3, pass, rank);
        GameManager.instance.MinigameResult(4, pass, rank);

        GameManager.instance.LoadScene("4.Ingame");
    }
}
