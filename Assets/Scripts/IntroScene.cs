using System.Collections;
<<<<<<< HEAD
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
=======
using UnityEngine;
using UnityEngine.SceneManagement;
>>>>>>> f8e8b9a9d2c6bfb990909d2158650045c34747fb
using UnityEngine.Video;

public class IntroScene : MonoBehaviour
{
    public VideoPlayer videoPlayer;
<<<<<<< HEAD
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
=======
    public string menuSceneName = "LevelSelect"; // Tên scene menu của bạn
>>>>>>> f8e8b9a9d2c6bfb990909d2158650045c34747fb

    void Start()
    {
        if (videoPlayer == null) 
            videoPlayer = GetComponent<VideoPlayer>();
<<<<<<< HEAD
        Controller_Sound.PlayMusic("Background");
        CloseQuitConfirm();
        languageButton.onClick.AddListener(ChangeLanguage);
        InitText();
        exitButton.onClick.AddListener(OpenQuitConfirm);
        settingButton.onClick.AddListener(LoadSetting);
        btnYes.onClick.AddListener(Quit);
        btnNo.onClick.AddListener(CloseQuitConfirm);
=======
        Controller_Sound.StopMusic();
>>>>>>> f8e8b9a9d2c6bfb990909d2158650045c34747fb
    }

    void Update()
    {
<<<<<<< HEAD
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
    
=======
        if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space))
        {
            LoadMenu();
        }
    }
    

    IEnumerator LoadScene()
    {
        yield return new WaitForSeconds(2);
        LoadMenu();
    }

    void LoadMenu()
    {
        Controller_Scene.Instance.LoadScene("LevelSelect");
    }
>>>>>>> f8e8b9a9d2c6bfb990909d2158650045c34747fb
}