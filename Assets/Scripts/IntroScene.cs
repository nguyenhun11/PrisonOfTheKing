using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Video;

public class IntroScene : MonoBehaviour
{
    public VideoPlayer videoPlayer;
    public Button languageButton;
    public TextMeshProUGUI startText;
    public Button settingButton;
    private bool _blocked;
    
    [Header("Exit")]
    public Button exitButton;
    public GameObject confirmPanel;
    public Button btnYes;
    public Button btnNo;
    public TextMeshProUGUI exitText;

    void Start()
    {
        if (videoPlayer == null) 
            videoPlayer = GetComponent<VideoPlayer>();
        Controller_Sound.PlayMusic("Background");
        CloseQuitConfirm();
        languageButton.onClick.AddListener(ChangeLanguage);
        InitText();
        exitButton.onClick.AddListener(OpenQuitConfirm);
        settingButton.onClick.AddListener(LoadSetting);
        btnYes.onClick.AddListener(Quit);
        btnNo.onClick.AddListener(CloseQuitConfirm);
    }

    void Update()
    {
        if (_blocked) return;
        
        if (Input.GetKeyDown(KeyCode.Space))
        {
            LoadMenu();
            return;
        }
        if (Input.GetMouseButtonDown(0))
        {
            if (EventSystem.current.IsPointerOverGameObject())
            {
                return;
            }
            LoadMenu();
        }
    }

    private void LoadMenu()
    {
        Controller_Scene.Instance.LoadScene("LevelSelect");
    }

    private void LoadSetting()
    {
        Controller_Scene.Instance.LoadScene("Setting");
    }

    private void ChangeLanguage()
    {
        Controller_GeneralSetting.Instance.ToggleLanguage();
        InitText();
    }

    private void InitText()
    {
        if (Controller_GeneralSetting.Instance.currentLanguage == Controller_GeneralSetting.Language.Vietnamese)
        {
            startText.text = "Chạm để vào tù";
            languageButton.GetComponentInChildren<TextMeshProUGUI>().text = "Tiếng Việt";
            btnNo.GetComponentInChildren<TextMeshProUGUI>().text = "Không";
            btnYes.GetComponentInChildren<TextMeshProUGUI>().text = "Có";
            exitText.text = "Bạn muốn thoát khỏi đây?";
        }
        else
        {
            startText.text = "Tap to enter the prison";
            languageButton.GetComponentInChildren<TextMeshProUGUI>().text = "English";
            btnNo.GetComponentInChildren<TextMeshProUGUI>().text = "No";
            btnYes.GetComponentInChildren<TextMeshProUGUI>().text = "Yes";
            exitText.text = "You want to get out?";
        }
    }
    
    public void OpenQuitConfirm()
    {
        confirmPanel.SetActive(true);
        exitButton.gameObject.SetActive(false);
        settingButton.gameObject.SetActive(false);
        languageButton.gameObject.SetActive(false);
        startText.gameObject.SetActive(false);
        _blocked = true;
    }
    
    private void CloseQuitConfirm()
    {
        confirmPanel.SetActive(false);
        exitButton.gameObject.SetActive(true);
        settingButton.gameObject.SetActive(true);
        languageButton.gameObject.SetActive(true);
        startText.gameObject.SetActive(true);
        _blocked = false;
    }

    private void Quit()
    {
        Application.Quit();
    }
    
}