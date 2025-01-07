using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.U2D;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class InGameManager : MonoBehaviour
{
    public static InGameManager instance;
    public SpriteAtlas iconSpriteAtlas;
    public Sprite[] NumberSprite;

    // UI, 캐싱 관련
    public GameObject roundUI;
    private Image[] roundNumber = new Image[2];

    public GameObject stateUI;
    private GameObject[] stateUI_Player = new GameObject[4];
    private Image[] stateUI_Icon_Player = new Image[4];
    private Image[] stateUI_Lives_Player = new Image[4];
    private Text[] stateUI_Coins_Player = new Text[4];

    private GameObject canvas;

    public GameObject UFO;
    private Transform[] UFO_Player = new Transform[4];
    private SpriteRenderer[] UFO_Icon_Player = new SpriteRenderer[4];


    // 인게임 정보
    private int[] lives = new int[4];
    private int[] coins = new int[4];
    private int round, coin_1st, coin_last;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            canvas = stateUI.transform.parent.gameObject;
            DontDestroyOnLoad(gameObject);
            DontDestroyOnLoad(UFO);
            DontDestroyOnLoad(canvas);
            // SceneManager.sceneLoaded += OnSceneLoaded;

            for (int i = 0; i < 4; i++)
            {
                if (i < 2) roundNumber[i] = roundUI.transform.GetChild(i + 1).GetComponent<Image>();

                // 변동될 것 캐싱
                stateUI_Player[i] = stateUI.transform.GetChild(i).gameObject;
                stateUI_Icon_Player[i] = stateUI_Player[i].transform.GetChild(0).GetComponent<Image>();
                stateUI_Lives_Player[i] = stateUI_Player[i].transform.GetChild(1).GetComponent<Image>();
                stateUI_Coins_Player[i] = stateUI_Player[i].transform.GetChild(3).GetChild(0).GetComponent<Text>();

                UFO_Player[i] = UFO.transform.GetChild(i).GetComponent<Transform>();
                UFO_Icon_Player[i] = UFO_Player[i].GetChild(1).GetComponent<SpriteRenderer>();

                // 캐릭터 아이콘 적용
                int CursorPos = GameManager.instance.selectCharNo[i];
                int selectCharNo = (CursorPos % 10) * 3 + (CursorPos / 10) - 4;
                string spriteName = GameManager.instance.spriteNames[selectCharNo];
                stateUI_Icon_Player[i].sprite = iconSpriteAtlas.GetSprite(spriteName);
                UFO_Icon_Player[i].sprite = iconSpriteAtlas.GetSprite(spriteName);
            }
        }
        else
        {
            canvas = stateUI.transform.parent.gameObject;
            Destroy(gameObject);
            Destroy(UFO);
            Destroy(canvas);
        }
    }

    private void Start()
    {
        NumberChange("r", 0, 0);
        coin_1st = 0; coin_last = 0;

        for (int i = 1; i <= 4; i++)
        {
            NumberChange("l", i, 3);
            NumberChange("c", i, 0);
            UFOAnimation("init", i);
        }

        StartCoroutine(BeforeMiniGame());
    }
    /*
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (round < 1 || scene.name != "4.Ingame") return;
        StartCoroutine(AfterMiniGame());
    }
    */
    private IEnumerator BeforeMiniGame()
    {
        yield return new WaitForSeconds(2.5f); // 대기: UFO 애니메이션 동작 시간(2초) + 0.5초
        NumberChange("r", 0, round + 1);
        yield return new WaitForSeconds(1.0f); // 대기: 라운드 숫자 애니메이션 동작 시간(0.5초) + 0.5초
        UFO.SetActive(false);
        canvas.SetActive(false);
        GameManager.instance.LoadScene("TestMinigame", 0.15f);
    }
    public void AfterMiniGame() { StartCoroutine(AfterMiniGame2()); }

    private IEnumerator AfterMiniGame2()
    {
        UFO.SetActive(true);
        canvas.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        var v = GameManager.instance.miniGameResult;
        for (int i = 1; i <= 4; i++)
        {
            NumberChange("coins", i, coins[i - 1] + v[i - 1].rank);
        }
        for (int i = 1; i <= 4; i++)
        {
            if (v[i - 1].isPass)
            {
                UFOAnimation("s", i);
            }
            else
            {
                NumberChange("lives", i, lives[i - 1] - 1);
                if (lives[i - 1] > 0)
                    UFOAnimation("f", i);
                else
                    UFOAnimation("o", i);
            }
        }
        yield return new WaitForSeconds(1.0f);

        StartCoroutine(BeforeMiniGame());
    }


    // 숫자(라운드, 라이프, 코인)를 바꿀땐 무조건 얘를 호출
    private void NumberChange(string type, int pNo, int num)
    {
        if(type == "round" || type == "r")
        {
            var RN = roundNumber;
            round = num;
            if(num < 10)
            {
                RN[0].rectTransform.anchoredPosition = new Vector3(96, 0, 0);
                RN[0].sprite = NumberSprite[num];
                RN[1].sprite = NumberSprite[10];
            }
            else
            {
                RN[0].rectTransform.anchoredPosition = new Vector3(80, 0, 0);
                RN[0].sprite = NumberSprite[num / 10];
                RN[1].sprite = NumberSprite[num % 10];
            }
            NumAppear(RN[0].transform);
            NumAppear(RN[1].transform);
        }
        else if(type == "lives" || type == "l")
        {
            var v = stateUI_Lives_Player[pNo - 1];
            lives[pNo - 1] = num;
            v.sprite = NumberSprite[num];
            if (num == 0)
                stateUI_Player[pNo - 1].GetComponent<CanvasGroup>().alpha = 0.3f;
            if (num <= 1) 
                v.color = new Color(0.8f, 0.4f, 0.4f);
            else
                v.color = new Color(1, 1, 1);
            NumAppear(v.transform);
        }
        else if(type == "coins" || type == "c")
        {
            coins[pNo - 1] = num;
            coin_1st = Math.Max(coin_1st, num);
            coin_last = coins.Min();
            stateUI_Coins_Player[pNo - 1].text = num.ToString();
        }
    }

    private void NumAppear(Transform t)
    {
        t.DOScale(2f, 0.25f).SetEase(Ease.OutQuad).SetLoops(2, LoopType.Yoyo);
    }

    // 미니게임 결과에 따른 UFO 애니메이션 메소드
    private void UFOAnimation(string type, int pNo)
    {
        var ufo = UFO_Player[pNo - 1];
        
        if (type == "init" || type == "i")
        {
            DOTween.Sequence()
                .Append(ufo.DOMoveX(-10, 0)) // 즉시 -10으로 이동
                .Append(ufo.DOMoveX(-4, 2f).SetEase(Ease.OutExpo)) // 1초 동안 -4로 이동
                ;// .Append(ufo.DORotate(new Vector3(0, 0, 0), 1f)).OnComplete(() => UFOAnimation("s", pNo));
        }
        else if (type == "success" || type == "s")
        {
            DOTween.Sequence()
                .Append(ufo.DOMove(ufo.position + Vector3.right * 5, 1f).SetEase(Ease.OutExpo))
                .Append(ufo.DOMove(UFO_TargetPos(pNo), 1f))
                ;// .Append(ufo.DORotate(new Vector3(0, 0, 0), 1f)).OnComplete(() => UFOAnimation("f", pNo));
        }
        else if (type == "fail" || type == "f")
        {
            ufo.rotation = Quaternion.Euler(0, 0, -30);
            DOTween.Sequence()
                .Append(ufo.DORotate(new Vector3(0, 0, 30), 0.25f).SetEase(Ease.InOutSine).SetLoops(4, LoopType.Yoyo))
                .Join(ufo.DOMove(UFO_TargetPos(pNo), 1f))
                .Append(ufo.DORotate(new Vector3(0, 0, 0), 0))
                ;// .Append(ufo.DORotate(new Vector3(0, 0, 0), 1f)).OnComplete(() => UFOAnimation("o", pNo));
        }
        else if (type == "out" || type == "o")
        {
            DOTween.Sequence()
                .Append(ufo.DORotate(new Vector3(0, 0, 180), 2f))
                .Join(ufo.DOJump(UFO_TargetPos(-pNo), 1f, 1, 1f))
                ;// .Append(ufo.DORotate(new Vector3(0, 0, 0), 1f)).OnComplete(() => UFOAnimation("c", pNo));
        }
        else if (type == "continue" || type == "c")
        {
            DOTween.Sequence()
                .Append(ufo.DORotate(new Vector3(0, 0, 0), 1f))
                .Append(ufo.DOMove(UFO_TargetPos(pNo), 1f).SetEase(Ease.OutExpo))
               ;//  .Append(ufo.DORotate(new Vector3(0, 0, 0), 1f)).OnComplete(() => UFOAnimation("s", pNo));
        }
    }

    private Vector3 UFO_TargetPos(int pNo)
    {
        float x, y;
        if (pNo < 0) x = -11;
        else if(coin_last == coin_1st) x = -4;
        else x = Mathf.InverseLerp(coin_last, coin_1st, coins[pNo - 1]) * 4 - 6;

        y = 4.5f - pNo * 2;
        if (pNo < 0) y += pNo * 4;
        return new Vector3(x, y, 0);
    }
}
