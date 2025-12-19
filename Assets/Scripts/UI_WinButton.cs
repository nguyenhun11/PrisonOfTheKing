using UnityEngine;
using UnityEngine.UI;

public class UI_WinButton : MonoBehaviour
{
    public Button home;
    public Button replay;
    public Button next;

    void Start()
    {
        home.onClick.AddListener(Home);
        replay.onClick.AddListener(Replay);
        next.onClick.AddListener(LoadNextLevel);
    }

    private void Home()
    {
        Controller_Scene.Instance.LoadScene("LevelSelect");
    }

    private void Replay()
    {
        if (Controller_LoadLevel.Instance.currentLevel == null)
        {
            Home();
        }
        else
        {
            Controller_LoadLevel.Instance.LoadLevel();
        }
    }

    private void LoadNextLevel()
    {
        if (Controller_LoadLevel.Instance.currentLevel == null 
            || Controller_LoadLevel.Instance.currentLevel.nextLevels == null
            || Controller_LoadLevel.Instance.currentLevel.nextLevels.Count == 0)
        {
            Home();
        }
        else
        {
            Controller_LoadLevel.Instance.currentLevel = Controller_LoadLevel.Instance.currentLevel.nextLevels[0];
            Controller_LoadLevel.Instance.LoadLevel();
        }
    }
}
