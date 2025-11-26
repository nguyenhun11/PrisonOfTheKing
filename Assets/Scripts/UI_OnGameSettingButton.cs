using UnityEngine;
using UnityEngine.UI;

public class UI_OnGameSettingButton : MonoBehaviour
{
    public Button home;
    public Button resume;
    public Button menu;

    private void Start()
    {
        home.onClick.AddListener(Home);
        resume.onClick.AddListener(Resume);
        menu.onClick.AddListener(Menu);
    }

    public void Home()
    {
        Controller_Scene.Instance.LoadScene("LevelSelect");
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
