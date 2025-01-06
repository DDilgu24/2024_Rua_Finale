using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    // 1-1. ĳ���� sprite�� �̸� ��� string
    public string[] spriteNames;
    // 1-2. ������ ĳ���� ��ȣ ��� int
    public int[] selectCharNo = new int[4];

    // 2. ���� �Ŵ����� ���� ��� ������ ���̵� ����
    public Image fadeImage; // ���̵� �� �̹���
    public float fadeDuration = 0.9f; // ���̵� �ð�
    public bool IsFading { get; private set; } = true; // ���̵尡 �������ΰ�?

    // 3. �̴ϰ����� ����� ������ Ŭ����
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

    // ���̵� ���� �޼ҵ�
    private IEnumerator Fade(bool isIn, System.Action onComplete)
    {
        fadeImage.gameObject.SetActive(true);
        IsFading = true;

        float elapsedTime = 0f; // ���̵尡 ����� �ð� 
        Color color = fadeImage.color; // ���̵� �̹����� ���� ĳ��
        float endAlpha = isIn ? 0 : 1; // ���̵�� �̹����� ���� ���� ��(0: ����)

        while (elapsedTime < fadeDuration) // ���� ����
        {
            elapsedTime += Time.deltaTime;
            color.a = Mathf.Lerp(1 - endAlpha, endAlpha, elapsedTime / fadeDuration);
            fadeImage.color = color;
            yield return null;
        }

        color.a = endAlpha;
        fadeImage.color = color; // Ȯ���� ������ ���� ���� �����ϰ� ����
        IsFading = false;
        fadeImage.gameObject.SetActive(false);
        onComplete?.Invoke(); // ���� �Ϸ� �� ���� �� �ݹ� �Լ��� ȣ��
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

    // CursorPos(11~38)�� Index(0~23)���� ����
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
