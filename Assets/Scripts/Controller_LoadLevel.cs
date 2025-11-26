using System.Collections.Generic;
using UnityEngine;

public class Controller_LoadLevel : MonoBehaviour
{
    #region singleton
    public static Controller_LoadLevel Instance;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }
    #endregion
    public LevelData currentLevel;

    public void LoadLevel()
    {
        currentLevel.LoadLevel();
    }
}
