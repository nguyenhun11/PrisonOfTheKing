using UnityEngine;
using UnityEngine.UI;

public class UI_WinButton : MonoBehaviour
{
    public Button home;
    public Button replay;
    public Button menu;

    void Start()
    {
        home.onClick.AddListener(Home);
        replay.onClick.AddListener(Replay);
        menu.onClick.AddListener(Menu);
    }

    private void Home()
    {
        Controller_Scene.Instance.LoadScene("LevelSelect");
    }

    private void Replay()
    {
        Controller_LoadLevel.Instance.LoadLevel();
    }

    private void Menu()
    {
        Controller_Scene.Instance.LoadScene("LevelSelect");
    }
}
