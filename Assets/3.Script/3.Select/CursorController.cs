using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.U2D;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;
using System.Collections.Generic;

public class CursorController : MonoBehaviour
{
    public InputActionAsset inputActions; // Input Action Asset ����
    private InputAction inputUp, inputLeft, inputDown, inputRight, inputNext, inputBack;
    public GameObject[] cursor;
    private RectTransform cursorRT;

    private int selectTurn; // 0~3
    // ���õ� ĳ���� ��ȣ�� ��� �ؽü�
    HashSet<int> usedCharNo = new HashSet<int>();
    // �÷��̾� �� ������ ĳ���� ��ȣ(�ڷΰ��� �� �� ��)
    private int[] selectCharNo = new int[4];

    private Tween blink;

    public SpriteAtlas profileAtlas;
    private Sprite[] profileSprites;
    public Image[] upperProfile;

    private int cursorPos;

    private void Start()
    {
        selectTurn = 0;
        // Cursor Ȱ��ȭ
        CursorActive();
    }



    void OnEnable()
    {
        // Action Map ��������
        var actionMap = inputActions.FindActionMap("CursorControls");

        // Action ��������
        inputUp = actionMap.FindAction("MoveUp");
        inputLeft = actionMap.FindAction("MoveLeft");
        inputDown = actionMap.FindAction("MoveDown");
        inputRight = actionMap.FindAction("MoveRight");
        inputNext = actionMap.FindAction("Next");
        inputBack = actionMap.FindAction("Back");

        // �̺�Ʈ ����
        inputUp.performed += OnMoveUp;
        inputLeft.performed += OnMoveLeft;
        inputDown.performed += OnMoveDown;
        inputRight.performed += OnMoveRight;
        inputNext.performed += OnNext;
        inputBack.performed += OnBack;

        // Action Ȱ��ȭ
        inputUp.Enable();
        inputLeft.Enable();
        inputDown.Enable();
        inputRight.Enable();
        inputNext.Enable();
        inputBack.Enable();

    }
    void OnDisable()
    {
        // �̺�Ʈ ����
        inputUp.performed -= OnMoveUp;
        inputLeft.performed -= OnMoveLeft;
        inputDown.performed -= OnMoveDown;
        inputRight.performed -= OnMoveRight;
        inputNext.performed -= OnNext;
        inputBack.performed -= OnBack;

        // Action ��Ȱ��ȭ
        inputUp.Disable();
        inputLeft.Disable();
        inputDown.Disable();
        inputRight.Disable();
        inputNext.Disable();
        inputBack.Disable();
    }


    // Ŀ���� Ȱ��ȭ�� ��
    private void CursorActive()
    {
        // 1. Ŀ�� ������Ʈ�� Ȱ��ȭ
        cursor[selectTurn].SetActive(true);
        // 1.5 ��� ĳ���� alpha 1��
        upperProfile[selectTurn].color = new Color(1, 1, 1, 1);
        // 2. OK �ΰ�� ��Ȱ��ȭ
        cursor[selectTurn].transform.GetChild(0).gameObject.SetActive(false);
        // 3. �׵θ� alpha�� 1��
        cursor[selectTurn].transform.GetChild(2).GetComponent<Image>().DOFade(1f, 0.1f); 
        // 4. Tween �ο�
        blink = cursor[selectTurn].transform.GetChild(2).GetComponent<Image>().DOFade(0.2f, 0.75f).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutSine);
        // 5. Ŀ���� RectTransform�� ĳ��
        cursorRT = cursor[selectTurn].GetComponent<RectTransform>();
        // 6. Ŀ���� �ʱ� ��ġ�� �����ϴ� �޼ҵ�
        CursorSet(selectCharNo[selectTurn], 0);
    }

    // Ŀ���� �̵���ų ��
    private void CursorSet(int Pos, int addNo)
    {
        if (selectTurn >= 4) return;
        // ���� ������ Ŀ���� 11������ ����
        if (Pos == 0) Pos = 11;
        Pos += addNo; 
        if (Pos < 10) Pos += 30;
        else if (Pos > 40) Pos -= 30;
        else if (Pos % 10 == 0) Pos += 8;
        else if (Pos % 10 == 9) Pos -= 8;

        // Ŀ���� ��ġ�ؾ� �� ���� �ߺ��̶��, �ٸ� ������
        if (usedCharNo.Contains(Pos))
        {
            if (addNo == 0) addNo = 1;
            CursorSet(Pos, addNo);
        }
        else
        {
            cursorPos = Pos;
            cursorRT.anchoredPosition = new Vector2((cursorPos % 10) * 200 - 900,
                                                    (cursorPos / 10) * -150);
            upperProfile[selectTurn].sprite = profileAtlas.GetSprite(GameManager.instance.spriteNames[(cursorPos % 10 - 1) * 3 + (cursorPos / 10 - 1)]);
            upperProfile[selectTurn].gameObject.GetComponent<RectTransform>().sizeDelta = upperProfile[selectTurn].sprite.bounds.size * 100 * 345 / 320;
        }
    }

    private void OnMoveUp(InputAction.CallbackContext context)
    {
        CursorSet(cursorPos, -10);
    }
    private void OnMoveLeft(InputAction.CallbackContext context)
    {
        CursorSet(cursorPos, -1);
    }
    private void OnMoveDown(InputAction.CallbackContext context)
    {
        CursorSet(cursorPos, 10);
    }
    private void OnMoveRight(InputAction.CallbackContext context)
    {
        CursorSet(cursorPos, 1);
    }

    private void OnNext(InputAction.CallbackContext context)
    {
        if (selectTurn >= 4) return;
        cursor[selectTurn].transform.GetChild(0).gameObject.SetActive(true);
        if (blink != null && blink.IsActive())
        {
            blink.Kill();
            // blink.Pause(); // Tween �Ͻ�����
            cursor[selectTurn].transform.GetChild(2).GetComponent<Image>().DOFade(1f, 0.1f); // �ʱ� ���·� ���� (���� �� 1)
        }
        selectTurn++;
        selectCharNo[selectTurn - 1] = cursorPos;
        usedCharNo.Add(cursorPos);
        if (selectTurn < 4)
        {
            CursorActive();
        }
        else
        {
            GameManager.instance.selectCharNo = selectCharNo;
            GameManager.instance.LoadScene("4.InGame");
        }
    }

    private void OnBack(InputAction.CallbackContext context)
    {
        if(selectTurn == 0)
        {
            Debug.Log("Back");
        }
        else
        {
            usedCharNo.Remove(selectCharNo[selectTurn - 1]);
            if (selectTurn < 4)
            {
                blink.Kill();
                upperProfile[selectTurn].color = new Color(1, 1, 1, 0);
                cursor[selectTurn].transform.GetChild(2).GetComponent<Image>().DOFade(1f, 0.1f); 
                cursor[selectTurn].SetActive(false);
            }
            selectTurn--;
            CursorActive();
        }
    }

}