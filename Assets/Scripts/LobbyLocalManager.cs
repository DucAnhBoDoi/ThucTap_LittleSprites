using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

public class LobbyLocalManager : MonoBehaviour
{
    [Header("Player Slots")]
    public GameObject Player1Slot;
    public GameObject Player2Slot;

    [Header("Player Icons")]
    public Image Icon_Player1Input;
    public Image Icon_Player2Input;

    [Header("Device Sprites")]
    public Sprite keyboardSprite;
    public Sprite gamepadSprite;

    [Header("Colors")]
    public Color normalColor = new Color(1, 1, 1, 0.35f);
    public Color activeColor = Color.white;

    private enum DeviceType { None, Keyboard, Gamepad }
    private enum PlayerSlot { None, Player1, Player2 }

    // Trạng thái thiết bị đã xác nhận
    private DeviceType p1ConfirmedDevice = DeviceType.None;
    private DeviceType p2ConfirmedDevice = DeviceType.None;
    private Gamepad p1Gamepad = null;
    private Gamepad p2Gamepad = null;

    // Trạng thái chọn slot (chưa xác nhận)
    private bool isKeyboardSelecting = false;
    private bool isGamepadSelecting = false;
    private PlayerSlot currentKeyboardSelection = PlayerSlot.Player1;
    private PlayerSlot currentGamepadSelection = PlayerSlot.Player1;
    private Gamepad selectingGamepad = null;

    void OnEnable()
    {
        ResetIcons();
    }

    void Update()
    {
        if (isKeyboardSelecting)
        {
            HandleKeyboardSlotSelection();
        }
        else if (!isGamepadSelecting)
        {
            DetectInitialKeyboardInput();
        }

        if (isGamepadSelecting)
        {
            HandleGamepadSlotSelection();
        }
        else if (!isKeyboardSelecting)
        {
            DetectInitialGamepadInput();
        }

        KeepConfirmedIconsBright();
    }

    void ResetIcons()
    {
        p1ConfirmedDevice = DeviceType.None;
        p2ConfirmedDevice = DeviceType.None;
        p1Gamepad = null;
        p2Gamepad = null;
        isKeyboardSelecting = false;
        isGamepadSelecting = false;
        currentKeyboardSelection = PlayerSlot.Player1;
        currentGamepadSelection = PlayerSlot.Player1;
        selectingGamepad = null;

        Icon_Player1Input.gameObject.SetActive(false);
        Icon_Player2Input.gameObject.SetActive(false);
    }

    // =======================================
    // KEYBOARD
    // =======================================

    void DetectInitialKeyboardInput()
    {
        if (p1ConfirmedDevice != DeviceType.Keyboard && p2ConfirmedDevice != DeviceType.Keyboard)
        {
            if (Keyboard.current != null && Keyboard.current.anyKey.wasPressedThisFrame)
            {
                isKeyboardSelecting = true;

                if (p1ConfirmedDevice == DeviceType.None)
                    currentKeyboardSelection = PlayerSlot.Player1;
                else
                    currentKeyboardSelection = PlayerSlot.Player2;

                ShowKeyboardSelectionPreview();
            }
        }
    }

    void HandleKeyboardSlotSelection()
    {
        if (Keyboard.current == null) return;

        if (Keyboard.current.aKey.wasPressedThisFrame || Keyboard.current.leftArrowKey.wasPressedThisFrame)
        {
            if (p1ConfirmedDevice == DeviceType.None)
            {
                currentKeyboardSelection = PlayerSlot.Player1;
                ShowKeyboardSelectionPreview();
            }
        }
        else if (Keyboard.current.dKey.wasPressedThisFrame || Keyboard.current.rightArrowKey.wasPressedThisFrame)
        {
            if (p2ConfirmedDevice == DeviceType.None)
            {
                currentKeyboardSelection = PlayerSlot.Player2;
                ShowKeyboardSelectionPreview();
            }
        }

        if (Keyboard.current.enterKey.wasPressedThisFrame ||
            Keyboard.current.spaceKey.wasPressedThisFrame ||
            Keyboard.current.numpadEnterKey.wasPressedThisFrame)
        {
            ConfirmKeyboardSelection();
        }

        if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            CancelKeyboardSelection();
        }
    }

    void ShowKeyboardSelectionPreview()
    {
        // Ẩn cả 2 trước
        Icon_Player1Input.gameObject.SetActive(false);
        Icon_Player2Input.gameObject.SetActive(false);

        if (currentKeyboardSelection == PlayerSlot.Player1 && p1ConfirmedDevice == DeviceType.None)
        {
            Icon_Player1Input.gameObject.SetActive(true);
            Icon_Player1Input.sprite = keyboardSprite;
            Icon_Player1Input.color = normalColor;
        }
        else if (currentKeyboardSelection == PlayerSlot.Player2 && p2ConfirmedDevice == DeviceType.None)
        {
            Icon_Player2Input.gameObject.SetActive(true);
            Icon_Player2Input.sprite = keyboardSprite;
            Icon_Player2Input.color = normalColor;
        }

        // Hiện lại icon đã confirm
        if (p1ConfirmedDevice != DeviceType.None)
            Icon_Player1Input.gameObject.SetActive(true);
        if (p2ConfirmedDevice != DeviceType.None)
            Icon_Player2Input.gameObject.SetActive(true);
    }

    void ConfirmKeyboardSelection()
    {
        isKeyboardSelecting = false;

        if (currentKeyboardSelection == PlayerSlot.Player1 && p1ConfirmedDevice == DeviceType.None)
        {
            p1ConfirmedDevice = DeviceType.Keyboard;
            Icon_Player1Input.gameObject.SetActive(true);
            Icon_Player1Input.sprite = keyboardSprite;
            Icon_Player1Input.color = activeColor;
        }
        else if (currentKeyboardSelection == PlayerSlot.Player2 && p2ConfirmedDevice == DeviceType.None)
        {
            p2ConfirmedDevice = DeviceType.Keyboard;
            Icon_Player2Input.gameObject.SetActive(true);
            Icon_Player2Input.sprite = keyboardSprite;
            Icon_Player2Input.color = activeColor;
        }
    }

    void CancelKeyboardSelection()
    {
        isKeyboardSelecting = false;

        Icon_Player1Input.gameObject.SetActive(false);
        Icon_Player2Input.gameObject.SetActive(false);

        // Hiện lại icon đã confirm
        if (p1ConfirmedDevice != DeviceType.None)
            Icon_Player1Input.gameObject.SetActive(true);
        if (p2ConfirmedDevice != DeviceType.None)
            Icon_Player2Input.gameObject.SetActive(true);
    }

    // =======================================
    // GAMEPAD
    // =======================================

    void DetectInitialGamepadInput()
    {
        foreach (var gp in Gamepad.all)
        {
            if (p1Gamepad == gp || p2Gamepad == gp) continue;

            if (PressedAnyButton(gp))
            {
                isGamepadSelecting = true;
                selectingGamepad = gp;

                if (p1ConfirmedDevice == DeviceType.None)
                    currentGamepadSelection = PlayerSlot.Player1;
                else
                    currentGamepadSelection = PlayerSlot.Player2;

                ShowGamepadSelectionPreview();
                break;
            }
        }
    }

    void HandleGamepadSlotSelection()
    {
        if (selectingGamepad == null) return;

        try
        {
            // Chuyển slot bằng D-Pad
            if (selectingGamepad.dpad.left.wasPressedThisFrame)
            {
                if (p1ConfirmedDevice == DeviceType.None)
                {
                    currentGamepadSelection = PlayerSlot.Player1;
                    ShowGamepadSelectionPreview();
                }
            }
            else if (selectingGamepad.dpad.right.wasPressedThisFrame)
            {
                if (p2ConfirmedDevice == DeviceType.None)
                {
                    currentGamepadSelection = PlayerSlot.Player2;
                    ShowGamepadSelectionPreview();
                }
            }

            // Xác nhận bằng nút X (buttonSouth)
            if (selectingGamepad.buttonSouth.wasPressedThisFrame)
            {
                ConfirmGamepadSelection();
            }

            // Hủy bằng nút B (buttonEast)
            if (selectingGamepad.buttonEast.wasPressedThisFrame)
            {
                CancelGamepadSelection();
            }
        }
        catch
        {
            // Nếu có lỗi, hủy selection
            CancelGamepadSelection();
        }
    }
    void ShowGamepadSelectionPreview()
    {
        // Ẩn cả 2 trước
        Icon_Player1Input.gameObject.SetActive(false);
        Icon_Player2Input.gameObject.SetActive(false);

        if (currentGamepadSelection == PlayerSlot.Player1 && p1ConfirmedDevice == DeviceType.None)
        {
            Icon_Player1Input.gameObject.SetActive(true);
            Icon_Player1Input.sprite = gamepadSprite;
            Icon_Player1Input.color = normalColor;
        }
        else if (currentGamepadSelection == PlayerSlot.Player2 && p2ConfirmedDevice == DeviceType.None)
        {
            Icon_Player2Input.gameObject.SetActive(true);
            Icon_Player2Input.sprite = gamepadSprite;
            Icon_Player2Input.color = normalColor;
        }

        // Hiện lại icon đã confirm
        if (p1ConfirmedDevice != DeviceType.None)
            Icon_Player1Input.gameObject.SetActive(true);
        if (p2ConfirmedDevice != DeviceType.None)
            Icon_Player2Input.gameObject.SetActive(true);
    }

    void ConfirmGamepadSelection()
    {
        if (currentGamepadSelection == PlayerSlot.Player1 && p1ConfirmedDevice == DeviceType.None)
        {
            p1ConfirmedDevice = DeviceType.Gamepad;
            p1Gamepad = selectingGamepad;
            Icon_Player1Input.gameObject.SetActive(true);
            Icon_Player1Input.sprite = gamepadSprite;
            Icon_Player1Input.color = activeColor;
        }
        else if (currentGamepadSelection == PlayerSlot.Player2 && p2ConfirmedDevice == DeviceType.None)
        {
            p2ConfirmedDevice = DeviceType.Gamepad;
            p2Gamepad = selectingGamepad;
            Icon_Player2Input.gameObject.SetActive(true);
            Icon_Player2Input.sprite = gamepadSprite;
            Icon_Player2Input.color = activeColor;
        }

        isGamepadSelecting = false;
        selectingGamepad = null;
    }

    void CancelGamepadSelection()
    {
        isGamepadSelecting = false;
        selectingGamepad = null;

        Icon_Player1Input.gameObject.SetActive(false);
        Icon_Player2Input.gameObject.SetActive(false);

        // Hiện lại icon đã confirm
        if (p1ConfirmedDevice != DeviceType.None)
            Icon_Player1Input.gameObject.SetActive(true);
        if (p2ConfirmedDevice != DeviceType.None)
            Icon_Player2Input.gameObject.SetActive(true);
    }

    // =======================================
    // HELPER
    // =======================================

    bool PressedAnyButton(Gamepad gp)
    {
        foreach (var control in gp.allControls)
        {
            if (control is ButtonControl b && b.wasPressedThisFrame)
                return true;
        }
        return false;
    }

    void KeepConfirmedIconsBright()
    {
        if (p1ConfirmedDevice != DeviceType.None)
        {
            Icon_Player1Input.color = activeColor;
        }

        if (p2ConfirmedDevice != DeviceType.None)
        {
            Icon_Player2Input.color = activeColor;
        }
    }

    // =======================================
    // PUBLIC METHODS
    // =======================================

    public void ResetPlayer(int playerNumber)
    {
        if (playerNumber == 1)
        {
            p1ConfirmedDevice = DeviceType.None;
            p1Gamepad = null;
            Icon_Player1Input.gameObject.SetActive(false);
        }
        else if (playerNumber == 2)
        {
            p2ConfirmedDevice = DeviceType.None;
            p2Gamepad = null;
            Icon_Player2Input.gameObject.SetActive(false);
        }
    }

    public void ResetAll()
    {
        ResetIcons();
    }
}