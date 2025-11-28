using UnityEngine;
using UnityEngine.InputSystem;

public class MainMenuController : MonoBehaviour
{
    [Header("Menu Panels")]
    public GameObject MainButtons;        // BTN_Continue, BTN_NewGame, BTN_Exit
    public GameObject NewGameSubMenu;     // BTN_LocalCoop, BTN_OnlineCoop
    public GameObject OnlineSubMenu;      // BTN_CreateRoom, BTN_JoinRoom

    [Header("Quit Popup")]
    public GameObject QuitPopup;          // PanelBG + Text + Buttons

    void Start()
    {
        // Mặc định hiển thị menu chính, tắt các sub menu và popup
        MainButtons.SetActive(true);
        NewGameSubMenu.SetActive(false);
        OnlineSubMenu.SetActive(false);
        QuitPopup.SetActive(false);
    }

    void Update()
    {
        // ESC lùi menu hoặc đóng popup
        if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            HandleEscape();
        }
    }

    void HandleEscape()
    {
        // Nếu QuitPopup đang mở -> ESC = đóng popup
        if (QuitPopup.activeSelf)
        {
            CloseQuitPopup();
            return;
        }

        // ESC lùi từng bước menu
        if (OnlineSubMenu.activeSelf)
        {
            OnlineSubMenu.SetActive(false);
            NewGameSubMenu.SetActive(true);
            return;
        }

        if (NewGameSubMenu.activeSelf)
        {
            NewGameSubMenu.SetActive(false);
            MainButtons.SetActive(true);
            return;
        }

        // Nếu đang ở MainButtons → không làm gì
    }

    // ========================
    // MENU CHÍNH
    // ========================
    public void OpenNewGameMenu()
    {
        MainButtons.SetActive(false);
        NewGameSubMenu.SetActive(true);
        OnlineSubMenu.SetActive(false);
    }

    public void OpenOnlineMenu()
    {
        MainButtons.SetActive(false);
        NewGameSubMenu.SetActive(false);
        OnlineSubMenu.SetActive(true);
    }

    public void BackToMainButtons()
    {
        MainButtons.SetActive(true);
        NewGameSubMenu.SetActive(false);
        OnlineSubMenu.SetActive(false);
    }

    // ========================
    // QUIT POPUP
    // ========================
    public void OpenQuitPopup()
    {
        // Không tắt MainButtons, popup nổi trên menu chính
        QuitPopup.SetActive(true);
    }

    public void CloseQuitPopup()
    {
        QuitPopup.SetActive(false);
    }

    public void ConfirmQuit()
    {
        Application.Quit();

#if UNITY_EDITOR
        // Dừng play mode trong Editor
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
