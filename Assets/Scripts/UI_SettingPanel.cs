using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro; // Nhớ dùng TMP

public class UI_SettingsPanel : MonoBehaviour
{
    [Header("--- TEXT OBJECTS (Kéo Text vào đây) ---")]
    [SerializeField] private TextMeshProUGUI txtMusicLabel;
    [SerializeField] private TextMeshProUGUI txtSFXLabel;
    [SerializeField] private TextMeshProUGUI txtResetZoneLabel;
    [SerializeField] private TextMeshProUGUI txtResetBtnLabel;
    [SerializeField] private TextMeshProUGUI txtLanguageLabel;
    [SerializeField] private TextMeshProUGUI txtLanguageBtnLabel;

    [Header("--- CONTROLS ---")]
    [SerializeField] private Slider sliderMusic;
    [SerializeField] private Slider sliderSFX;
    [SerializeField] private TMP_Dropdown dropdownLevel; 
    [SerializeField] private Button btnExecuteReset;
    [SerializeField] private Button btnToggleLanguage;

    [Header("--- CONFIRM RESET LEVEL ---")]
    [SerializeField] private GameObject confirmPanel;
    [SerializeField] private TextMeshProUGUI txtConfirmTitle;
    [SerializeField] private TextMeshProUGUI txtConfirmMessage;
    [SerializeField] private Button btnYesButton;
    [SerializeField] private Button btnNoButton;
        
    private void Start()
    {
        // 1. Setup Audio (Lấy giá trị từ Controller -> JSON)
        if (Controller_GeneralSetting.Instance != null)
        {
            sliderMusic.value = Controller_GeneralSetting.Instance.musicVolume;
            sliderSFX.value = Controller_GeneralSetting.Instance.sfxVolume;
            
            // Cập nhật text ngôn ngữ lần đầu
            UpdateAllText(Controller_GeneralSetting.Instance.currentLanguage);
            
            // Đăng ký sự kiện thay đổi ngôn ngữ
            Controller_GeneralSetting.Instance.OnLanguageChanged += UpdateAllText;
        }
        
        sliderMusic.onValueChanged.AddListener(OnMusicVolumeChange);
        sliderSFX.onValueChanged.AddListener(OnSFXVolumeChange);

        // 2. Setup Language Button
        btnToggleLanguage.onClick.AddListener(OnLanguageToggleClicked);

        // 3. Setup Reset Level
        confirmPanel.SetActive(false);
        InitLevelDropdown();
        btnExecuteReset.onClick.AddListener(OpenConfirmPanel);
    }

    private void OnDestroy()
    {
        // Luôn hủy đăng ký sự kiện khi UI bị tắt để tránh lỗi null reference
        if (Controller_GeneralSetting.Instance != null)
        {
            Controller_GeneralSetting.Instance.OnLanguageChanged -= UpdateAllText;
        }
    }

    // --- CÁC HÀM EVENT ---

    private void OnMusicVolumeChange(float value)
    {
        if (Controller_GeneralSetting.Instance != null)
        {
            // Gọi vào Controller, Controller sẽ tự lưu JSON và chỉnh AudioListener
            Controller_GeneralSetting.Instance.SetMusicVolume(value);
            
            // Nếu bạn vẫn muốn dùng Controller_Sound cũ:
            if (Controller_Sound.Instance != null)
            {
                Controller_Sound.Instance.SetMusicVolume(value);
            }
        }
    }

    private void OnSFXVolumeChange(float value)
    {
        if (Controller_GeneralSetting.Instance != null)
        {
            Controller_GeneralSetting.Instance.SetSfxVolume(value);

            // Nếu bạn vẫn muốn dùng Controller_Sound cũ:
            if (Controller_Sound.Instance != null)
            {
                Controller_Sound.Instance.SetSFXVolume(value);
            }
        }
    }

    private void OnLanguageToggleClicked()
    {
        if (Controller_GeneralSetting.Instance != null)
        {
            Controller_GeneralSetting.Instance.ToggleLanguage();
        }
    }

    private void UpdateAllText(Controller_GeneralSetting.Language lang)
    {
        if (lang == Controller_GeneralSetting.Language.Vietnamese)
        {
            if(txtMusicLabel) txtMusicLabel.text = "Nhạc nền:";
            if(txtSFXLabel) txtSFXLabel.text = "Hiệu ứng:";
            if(txtResetZoneLabel) txtResetZoneLabel.text = "Chọn màn:";
            if(txtResetBtnLabel) txtResetBtnLabel.text = "Khóa";
            if(txtLanguageLabel) txtLanguageLabel.text = "Ngôn ngữ:";
            if(txtLanguageBtnLabel) txtLanguageBtnLabel.text = "Tiếng Việt";
        }
        else
        {
            if(txtMusicLabel) txtMusicLabel.text = "Music:";
            if(txtSFXLabel) txtSFXLabel.text = "Sound FX:";
            if(txtResetZoneLabel) txtResetZoneLabel.text = "Select Level:";
            if(txtResetBtnLabel) txtResetBtnLabel.text = "Lock";
            if(txtLanguageLabel) txtLanguageLabel.text = "Language:";
            if(txtLanguageBtnLabel) txtLanguageBtnLabel.text = "English";
        }
    }

    private void InitLevelDropdown()
    {
        dropdownLevel.ClearOptions();
        
        if (Controller_LoadLevel.Instance != null && Controller_LoadLevel.Instance.allLevels != null)
        {
            List<string> options = new List<string>();
            var allLevels = Controller_LoadLevel.Instance.allLevels;
            int totalLevels = allLevels.Count;

            for (int i = 0; i < totalLevels; i++)
            {
                if (!allLevels[i].isLocked)
                {
                    options.Add(allLevels[i].levelName); 
                }
            }

            dropdownLevel.AddOptions(options);

            // (Tùy chọn) Nếu danh sách trống (chưa xong màn nào), 
            // bạn có thể thêm text mặc định hoặc disable nút Reset để tránh lỗi
            if (options.Count == 0)
            {
                dropdownLevel.options.Add(new TMP_Dropdown.OptionData("Chưa có màn nào"));
                dropdownLevel.interactable = false; // Khóa dropdown
            }
            else
            {
                dropdownLevel.interactable = true;
                // Chọn mặc định phần tử cuối cùng (level cao nhất đã finish)
                dropdownLevel.value = options.Count - 1; 
            }
        }
    }

    private void OpenConfirmPanel()
    {
        confirmPanel.SetActive(true);
        GenerateConfirmMessage(Controller_GeneralSetting.Instance.currentLanguage,
            dropdownLevel.options[dropdownLevel.value].text);
    }
    
    private void CloseConfirmPanel()
    {
        confirmPanel.SetActive(false);
    }

    private void GenerateConfirmMessage(Controller_GeneralSetting.Language lang, string level)
    {
        if (lang == Controller_GeneralSetting.Language.Vietnamese)
        {
            txtConfirmTitle.text = "Bạn chắc chắn muốn khóa màn?";
            txtConfirmMessage.text = $"Màn {level} sẽ được khôi phục lời thoại, các màn sau đó sẽ được khóa lại";
            btnYesButton.GetComponentInChildren<TextMeshProUGUI>().text = "Có";
            btnNoButton.GetComponentInChildren<TextMeshProUGUI>().text = "Không";
        }
        else
        {
            txtConfirmTitle.text = "Are you sure to lock this level";
            txtConfirmMessage.text = $"Level {level} will have its dialogues restored, and the subsequent stages will be locked again";
            btnYesButton.GetComponentInChildren<TextMeshProUGUI>().text = "Yes";
            btnNoButton.GetComponentInChildren<TextMeshProUGUI>().text = "No";
        }
        btnYesButton.onClick.RemoveAllListeners();
        btnYesButton.onClick.AddListener(ResetLevel);
        btnNoButton.onClick.RemoveAllListeners();
        btnNoButton.onClick.AddListener(CloseConfirmPanel);
        
    }

    private void ResetLevel()
    {
        BackToMenuScene();
        if (Controller_LoadLevel.Instance != null)
        {
            string selectedLevel = dropdownLevel.options[dropdownLevel.value].text;
            Controller_LoadLevel.Instance.ResetToLevel(selectedLevel);
        }
    }

    public void BackToMenuScene()
    {
        Controller_Scene.Instance.LoadScene("LevelSelect");
    }
}