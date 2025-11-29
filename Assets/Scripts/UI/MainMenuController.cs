using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using TMPro;

public class MainMenuController : MonoBehaviour
{
    [Header("Menu Panels")]
    public GameObject MainButtons;        // BTN_Continue, BTN_NewGame, BTN_Options, BTN_Exit
    public GameObject NewGameSubMenu;
    public GameObject OnlineSubMenu;
    public GameObject OptionsPanel;

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

        // Hiển thị menu chính
        MainButtons.SetActive(true);
        NewGameSubMenu.SetActive(false);
        OnlineSubMenu.SetActive(false);
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
    // ESCAPE / BACK LOGIC
    // =============================================
    void HandleEscape()
    {
        if (OptionsPanel.activeSelf)
        {
            CloseOptions(false); // cancel
            return;
        }

        if (QuitPopup.activeSelf)
        {
            CloseQuitPopup();
            return;
        }

        if (OnlineSubMenu.activeSelf)
        {
            BackFromOnlineMenu();
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
        OnlineSubMenu.SetActive(false);
    }

    public void OpenOnlineMenu()
    {
        MainButtons.SetActive(false);
        NewGameSubMenu.SetActive(false);
        OnlineSubMenu.SetActive(true);
    }

    // BACK từ NewGame → MainButtons
    public void BackFromNewGame()
    {
        NewGameSubMenu.SetActive(false);
        MainButtons.SetActive(true);
    }

    // BACK từ Online → NewGame
    public void BackFromOnlineMenu()
    {
        OnlineSubMenu.SetActive(false);
        NewGameSubMenu.SetActive(true);
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
            // rollback
            AudioListener.volume = savedVolume;
            Slider_Volume.value = savedVolume;
            Text_Value.text = Mathf.RoundToInt(savedVolume * 100).ToString();
        }

        OptionsPanel.SetActive(false);
        MainButtons.SetActive(true);
    }
}
