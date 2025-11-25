using NUnit.Framework;
using UnityEngine;

public class Controller_UI : MonoBehaviour
{
    #region singleton
    public static Controller_UI Instance;
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    #endregion

    public GameObject settingPanel;
    
    public void ShowSettingPanel(bool show)
    {
        settingPanel.SetActive(show);
    }
}
