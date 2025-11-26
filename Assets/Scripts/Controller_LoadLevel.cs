using System.Collections.Generic;
using UnityEngine;

public class Controller_LoadLevel : MonoBehaviour
{
    #region singleton
    public static Controller_LoadLevel Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }
    #endregion
    public LevelData currentLevel;

    public void LoadLevel()
    {
        if (currentLevel != null) currentLevel.LoadLevel();
    }
}
