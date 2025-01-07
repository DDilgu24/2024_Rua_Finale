using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.U2D;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;
using System.Collections.Generic;

public class CursorController : MonoBehaviour
{
    public InputActionAsset inputActions; // Input Action Asset 연결
    private InputAction inputUp, inputLeft, inputDown, inputRight, inputNext, inputBack;
    public GameObject[] cursor;
    private RectTransform cursorRT;

    private int selectTurn; // 0~3
    // 선택된 캐릭터 번호를 담는 해시셋
    HashSet<int> usedCharNo = new HashSet<int>();
    // 플레이어 별 선택한 캐릭터 번호(뒤로가기 할 때 용)
    private int[] selectCharNo = new int[4];

    private Tween blink;

    public SpriteAtlas profileAtlas;
    private Sprite[] profileSprites;
    public Image[] upperProfile;

    private int cursorPos;

    private void Start()
    {
        selectTurn = 0;
        // Cursor 활성화
        CursorActive();
    }



    void OnEnable()
    {
        // Action Map 가져오기
        var actionMap = inputActions.FindActionMap("CursorControls");

        // Action 가져오기
        inputUp = actionMap.FindAction("MoveUp");
        inputLeft = actionMap.FindAction("MoveLeft");
        inputDown = actionMap.FindAction("MoveDown");
        inputRight = actionMap.FindAction("MoveRight");
        inputNext = actionMap.FindAction("Next");
        inputBack = actionMap.FindAction("Back");

        // 이벤트 연결
        inputUp.performed += OnMoveUp;
        inputLeft.performed += OnMoveLeft;
        inputDown.performed += OnMoveDown;
        inputRight.performed += OnMoveRight;
        inputNext.performed += OnNext;
        inputBack.performed += OnBack;

        // Action 활성화
        inputUp.Enable();
        inputLeft.Enable();
        inputDown.Enable();
        inputRight.Enable();
        inputNext.Enable();
        inputBack.Enable();

    }
    void OnDisable()
    {
        // 이벤트 해제
        inputUp.performed -= OnMoveUp;
        inputLeft.performed -= OnMoveLeft;
        inputDown.performed -= OnMoveDown;
        inputRight.performed -= OnMoveRight;
        inputNext.performed -= OnNext;
        inputBack.performed -= OnBack;

        // Action 비활성화
        inputUp.Disable();
        inputLeft.Disable();
        inputDown.Disable();
        inputRight.Disable();
        inputNext.Disable();
        inputBack.Disable();
    }


    // 커서가 활성화될 때
    private void CursorActive()
    {
        // 1. 커서 오브젝트를 활성화
        cursor[selectTurn].SetActive(true);
        // 1.5 상단 캐릭터 alpha 1로
        upperProfile[selectTurn].color = new Color(1, 1, 1, 1);
        // 2. OK 로고는 비활성화
        cursor[selectTurn].transform.GetChild(0).gameObject.SetActive(false);
        // 3. 테두리 alpha를 1로
        cursor[selectTurn].transform.GetChild(2).GetComponent<Image>().DOFade(1f, 0.1f); 
        // 4. Tween 부여
        blink = cursor[selectTurn].transform.GetChild(2).GetComponent<Image>().DOFade(0.2f, 0.75f).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutSine);
        // 5. 커서의 RectTransform을 캐싱
        cursorRT = cursor[selectTurn].GetComponent<RectTransform>();
        // 6. 커서의 초기 위치를 설정하는 메소드
        CursorSet(selectCharNo[selectTurn], 0);
    }

    // 커서를 이동시킬 때
    private void CursorSet(int Pos, int addNo)
    {
        if (selectTurn >= 4) return;
        // 최초 등장한 커서는 11번으로 설정
        if (Pos == 0) Pos = 11;
        Pos += addNo; 
        if (Pos < 10) Pos += 30;
        else if (Pos > 40) Pos -= 30;
        else if (Pos % 10 == 0) Pos += 8;
        else if (Pos % 10 == 9) Pos -= 8;

        // 커서가 위치해야 할 곳이 중복이라면, 다른 곳으로
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
            // blink.Pause(); // Tween 일시정지
            cursor[selectTurn].transform.GetChild(2).GetComponent<Image>().DOFade(1f, 0.1f); // 초기 상태로 복원 (알파 값 1)
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