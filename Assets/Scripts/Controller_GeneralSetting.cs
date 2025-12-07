using UnityEngine;
using System;

public class Controller_GeneralSetting : MonoBehaviour
{
    public static Controller_GeneralSetting Instance;
    public Action<Language> OnLanguageChanged;

    public enum Language { Vietnamese, English };
    
    public float musicVolume
    {
        get { return SaveSystem.data != null ? SaveSystem.data.musicVolume : 1f; }
        set { SetMusicVolume(value); }
    }

    public float sfxVolume
    {
        get { return SaveSystem.data != null ? SaveSystem.data.sfxVolume : 1f; }
        set { SetSfxVolume(value); }
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            
            // 1. Load Data từ JSON lên RAM
            SaveSystem.Load();
            
            // 2. Áp dụng setting ngay lập tức (Volume, Language)
            ApplySettings();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void ApplySettings()
    {
        // Set âm lượng tổng của game
        AudioListener.volume = musicVolume; 
        
        // Debug
        Debug.Log($"Loaded Settings: Music={musicVolume}, SFX={sfxVolume}");
    }

    // --- CÁC HÀM XỬ LÝ ---

    public void SetMusicVolume(float value)
    {
        if (SaveSystem.data != null)
        {
            SaveSystem.data.musicVolume = value; // Lưu vào data
            AudioListener.volume = value;        // Chỉnh âm thanh thật
            SaveSystem.Save();                   // Ghi xuống ổ cứng
        }
    }

    public void SetSfxVolume(float value)
    {
        if (SaveSystem.data != null)
        {
            SaveSystem.data.sfxVolume = value;
            SaveSystem.Save();
            // Nếu bạn có Controller_Sound riêng quản lý SFX, có thể gọi event ở đây
        }
    }

    public void ToggleLanguage()
    {
        Language currentLang = (Language)SaveSystem.data.languageIndex;
        SetLanguage(currentLang == Language.Vietnamese ? Language.English : Language.Vietnamese);
    }

    public void SetLanguage(Language lang)
    {
        SaveSystem.data.languageIndex = (int)lang;
        SaveSystem.Save();
        OnLanguageChanged?.Invoke(lang);
    }
    
    // Getter hỗ trợ UI lấy ngôn ngữ hiện tại
    public Language currentLanguage => (Language)SaveSystem.data.languageIndex;
}