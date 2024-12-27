using UnityEngine;
using UnityEngine.InputSystem;

public class CursorController : MonoBehaviour
{
    public InputActionAsset inputActions; // Input Action Asset 연결
    private InputAction inputUp, inputLeft, inputDown, inputRight, inputNext, inputBack;
    public GameObject[] cursor;
    private RectTransform cursorRT;
    private int cursorPos;
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

        // Cursor 활성화
        cursor[0].SetActive(true);
        cursorRT = cursor[0].GetComponent<RectTransform>();
        cursorPos = 11;
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

    private void OnMoveUp(InputAction.CallbackContext context)
    {
        cursorMove(-10);
    }
    private void OnMoveLeft(InputAction.CallbackContext context)
    {
        cursorMove(-1);
    }
    private void OnMoveDown(InputAction.CallbackContext context)
    {
        cursorMove(10);
    }
    private void OnMoveRight(InputAction.CallbackContext context)
    {
        cursorMove(1);
    }
    private void OnNext(InputAction.CallbackContext context)
    {
        cursor[0].transform.GetChild(0).gameObject.SetActive(true);
    }
    private void OnBack(InputAction.CallbackContext context)
    {
        cursor[0].transform.GetChild(0).gameObject.SetActive(false);
    }
    private void cursorMove(int i)
    {
        cursorPos += i;
        if (cursorPos < 10) cursorPos += 30;
        else if (cursorPos % 10 == 0) cursorPos += 8;
        else if (cursorPos > 40) cursorPos -= 30;
        else if (cursorPos % 10 == 9) cursorPos -= 8;
        cursorRT.anchoredPosition = new Vector2((cursorPos % 10) * 200 - 900,
                                                (cursorPos / 10) * -150 + 50);
    }
}