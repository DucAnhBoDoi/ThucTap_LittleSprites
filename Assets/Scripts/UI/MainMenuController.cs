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
    public GameObject OptionsPanel;       // Panel Options

    [Header("Quit Popup")]
    public GameObject QuitPopup;          

    [Header("Options Controls")]
    public Slider Slider_Volume;
    public TMP_Text Text_Value;
    public CanvasGroup MainButtonsCanvasGroup; // để vô hiệu hóa click khi Options mở

    private float currentVolume = 0.5f;    // giá trị tạm khi kéo slider
    private float savedVolume = 0.5f;      // giá trị đã lưu

    void Start()
    {
        // Load âm lượng đã lưu
        savedVolume = PlayerPrefs.GetFloat("MasterVolume", 0.5f);
        currentVolume = savedVolume;
        AudioListener.volume = savedVolume;

        // Khởi tạo UI
        Slider_Volume.value = savedVolume;
        Text_Value.text = Mathf.RoundToInt(savedVolume * 100).ToString();

        // Hiển thị menu chính
        MainButtons.SetActive(true);
        NewGameSubMenu.SetActive(false);
        OnlineSubMenu.SetActive(false);
        QuitPopup.SetActive(false);
        OptionsPanel.SetActive(false);

        // MainButtons tương tác
        if (MainButtonsCanvasGroup != null)
        {
            MainButtonsCanvasGroup.interactable = true;
            MainButtonsCanvasGroup.blocksRaycasts = true;
        }
    }

    void Update()
    {
        if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            HandleEscape();
        }
    }

    void HandleEscape()
    {
        if (OptionsPanel.activeSelf)
        {
            CloseOptions(false); // Cancel
            return;
        }

        if (QuitPopup.activeSelf)
        {
            CloseQuitPopup();
            return;
        }

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

    // ========================
    // OPTIONS
    // ========================
    public void OpenOptions()
    {
        OptionsPanel.SetActive(true);

        // Vô hiệu hóa click MainButtons khi Options mở
        if (MainButtonsCanvasGroup != null)
        {
            MainButtonsCanvasGroup.interactable = false;
            MainButtonsCanvasGroup.blocksRaycasts = false;
        }

        // Khởi tạo slider và text
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

        // Lưu vào PlayerPrefs
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
            // Rollback âm lượng nếu Cancel
            AudioListener.volume = savedVolume;
            Slider_Volume.value = savedVolume;
            Text_Value.text = Mathf.RoundToInt(savedVolume * 100).ToString();
        }

        OptionsPanel.SetActive(false);

        // Cho MainButtons tương tác lại
        if (MainButtonsCanvasGroup != null)
        {
            MainButtonsCanvasGroup.interactable = true;
            MainButtonsCanvasGroup.blocksRaycasts = true;
        }
    }
}
