using UnityEngine;
using UnityEngine.UI;

public class UI_OnGameSettingButton : MonoBehaviour
{
    public Button setting;
    public Button resume;
    public Button menu;

    private void Start()
    {
        setting.onClick.AddListener(Setting);
        resume.onClick.AddListener(Resume);
        menu.onClick.AddListener(Menu);
    }

    public void Setting()
    {
        Controller_Scene.Instance.LoadScene("Setting");
    }

    public void Resume()
    {
        Controller_Level.Instance.Resume();
    }

    public void Menu()
    {
        Controller_Scene.Instance.LoadScene("LevelSelect");
    }
}
