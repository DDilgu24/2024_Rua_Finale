using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    // 1-1. 캐릭터 sprite들 이름 담는 string
    public string[] spriteNames;
    // 1-2. 선택한 캐릭터 번호 담는 int
    public int[] selectCharNo = new int[4];

    // 2. 게임 매니져를 통해 모든 씬에서 페이드 관리
    public Image fadeImage; // 페이드 용 이미지
    public float fadeDuration = 0.9f; // 페이드 시간
    public bool IsFading { get; private set; } = true; // 페이드가 진행중인가?

    // 3. 미니게임의 결과를 저장할 클래스
    public class MiniGameResult
    {
        public bool isPass;
        public int rank;
    }
    MiniGameResult[] miniGameResult = new MiniGameResult[4];

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        spriteNames = Resources.LoadAll<Sprite>("Characters/Select").Select(s => s.name).ToArray();
    }

    // 페이드 관련 메소드
    private IEnumerator Fade(bool isIn, System.Action onComplete)
    {
        fadeImage.gameObject.SetActive(true);
        IsFading = true;

        float elapsedTime = 0f; // 페이드가 진행된 시간 
        Color color = fadeImage.color; // 페이드 이미지의 색상 캐싱
        float endAlpha = isIn ? 0 : 1; // 페이드용 이미지의 최종 투명도 값(0: 투명)

        while (elapsedTime < fadeDuration) // 투명도 조절
        {
            elapsedTime += Time.deltaTime;
            color.a = Mathf.Lerp(1 - endAlpha, endAlpha, elapsedTime / fadeDuration);
            fadeImage.color = color;
            yield return null;
        }

        color.a = endAlpha;
        fadeImage.color = color; // 확실히 정해진 알파 값에 도달하게 보정
        IsFading = false;
        fadeImage.gameObject.SetActive(false);
        onComplete?.Invoke(); // 동작 완료 후 전달 된 콜백 함수를 호출
    }
    public void FadeIn(System.Action onComplete = null) { StartCoroutine(Fade(true, onComplete)); }
    public void FadeOut(System.Action onComplete = null) { StartCoroutine(Fade(false, onComplete)); }
    public void LoadScene(string sceneName)
    {
        FadeOut(() =>
        {
            SceneManager.LoadScene(sceneName);
            FadeIn();
        });
    }

    // CursorPos(11~38)를 Index(0~23)으로 변경
    private int CursorPosToIndex(int CursorPos)
    {
        return (CursorPos % 3) * 8 + (CursorPos / 3);
    }
    public void MinigameResult(int playerNo, bool pass, int rank)
    {
        miniGameResult[playerNo - 1].isPass = pass;
        miniGameResult[playerNo - 1].rank = rank;
    }
}
