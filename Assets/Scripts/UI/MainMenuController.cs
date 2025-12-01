using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using TMPro;

public class MainMenuController : MonoBehaviour
{
    [Header("Menu Panels")]
    public GameObject MainButtons;        // Continue, NewGame, Options, Exit
    public GameObject NewGameSubMenu;     // Local Coop + Online Coop
    public GameObject Text_GameTitle;     // Tiêu đề game
    public GameObject OptionsPanel;

    [Header("Lobby Panels")]
    public GameObject LobbyOnlinePanel;
    public GameObject LobbyLocalPanel;

    [Header("Quit Popup")]
    public GameObject QuitPopup;

    [Header("Options Controls")]
    public Slider Slider_Volume;
    public TMP_Text Text_Value;

    private float currentVolume = 0.5f;
    private float savedVolume = 0.5f;

    void Start()
    {
        // Load âm lượng đã lưu
        savedVolume = PlayerPrefs.GetFloat("MasterVolume", 0.5f);
        currentVolume = savedVolume;
        AudioListener.volume = savedVolume;

        // Khởi tạo UI Options
        Slider_Volume.value = savedVolume;
        Text_Value.text = Mathf.RoundToInt(savedVolume * 100).ToString();

        // Hiển thị menu chính, ẩn các menu khác
        MainButtons.SetActive(true);
        Text_GameTitle.SetActive(true);
        NewGameSubMenu.SetActive(false);
        LobbyOnlinePanel.SetActive(false);
        LobbyLocalPanel.SetActive(false);
        OptionsPanel.SetActive(false);
        QuitPopup.SetActive(false);
    }

    void Update()
    {
        if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            HandleEscape();
        }
    }

    // =============================================
    // ESC / BACK LOGIC
    // =============================================
    void HandleEscape()
    {
        if (OptionsPanel.activeSelf)
        {
            CloseOptions(false);
            return;
        }

        if (QuitPopup.activeSelf)
        {
            CloseQuitPopup();
            return;
        }

        if (LobbyOnlinePanel.activeSelf)
        {
            CloseLobbyOnline();
            return;
        }

        if (LobbyLocalPanel.activeSelf)
        {
            CloseLobbyLocal();
            return;
        }

        if (NewGameSubMenu.activeSelf)
        {
            BackFromNewGame();
            return;
        }
    }

    // =============================================
    // MENU CHÍNH
    // =============================================
    public void OpenNewGameMenu()
    {
        MainButtons.SetActive(false);
        NewGameSubMenu.SetActive(true);
    }

    public void BackFromNewGame()
    {
        NewGameSubMenu.SetActive(false);
        MainButtons.SetActive(true);
        Text_GameTitle.SetActive(true);
    }

    // =============================================
    // LOBBY ONLINE
    // =============================================
    public void OpenLobbyOnline()
    {
        LobbyOnlinePanel.SetActive(true);
        NewGameSubMenu.SetActive(false);
        MainButtons.SetActive(false);
        Text_GameTitle.SetActive(false);
    }

    public void CloseLobbyOnline()
    {
        LobbyOnlinePanel.SetActive(false);
        // Quay về menu con NewGame
        NewGameSubMenu.SetActive(true);
        Text_GameTitle.SetActive(true);
    }

    // =============================================
    // LOBBY LOCAL
    // =============================================
    public void OpenLobbyLocal()
    {
        LobbyLocalPanel.SetActive(true);
        NewGameSubMenu.SetActive(false);
        MainButtons.SetActive(false);
        Text_GameTitle.SetActive(false);
    }

    public void CloseLobbyLocal()
    {
        LobbyLocalPanel.SetActive(false);
        // Quay về menu con NewGame
        NewGameSubMenu.SetActive(true);
        Text_GameTitle.SetActive(true);
    }

    // =============================================
    // QUIT POPUP
    // =============================================
    public void OpenQuitPopup()
    {
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
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }

    // =============================================
    // OPTIONS
    // =============================================
    public void OpenOptions()
    {
        OptionsPanel.SetActive(true);
        Slider_Volume.value = savedVolume;
        Text_Value.text = Mathf.RoundToInt(savedVolume * 100).ToString();
    }

    public void OnVolumeSliderChanged()
    {
        currentVolume = Slider_Volume.value;
        Text_Value.text = Mathf.RoundToInt(currentVolume * 100).ToString();
        AudioListener.volume = currentVolume;
    }

    public void SaveOptions()
    {
        savedVolume = currentVolume;
        AudioListener.volume = savedVolume;
        PlayerPrefs.SetFloat("MasterVolume", savedVolume);
        PlayerPrefs.Save();
        CloseOptions(true);
    }

    public void CancelOptions()
    {
        CloseOptions(false);
    }

    void CloseOptions(bool saved)
    {
        if (!saved)
        {
            AudioListener.volume = savedVolume;
            Slider_Volume.value = savedVolume;
            Text_Value.text = Mathf.RoundToInt(savedVolume * 100).ToString();
        }

        OptionsPanel.SetActive(false);
        MainButtons.SetActive(true);
        Text_GameTitle.SetActive(true);
    }
}
