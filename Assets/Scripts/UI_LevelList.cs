using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // Cần thiết cho ScrollRect

public class UI_LevelList : MonoBehaviour
{
    [Header("Data References")]
    public UI_Level levelPrefab; // Prefab nút Level
    [SerializeField] private List<LevelData> _levelDatas;

    [Header("UI References")]
    [Tooltip("Kéo Scroll View (cái chứa ScrollRect) vào đây")]
    public ScrollRect levelScrollView; 

    private void Start()
    {
        if (Controller_LoadLevel.Instance != null)
        {
            _levelDatas = Controller_LoadLevel.Instance.allLevels;
        }
        
        GenerateLevelItems();
        RestoreScrollPosition();
        
        Controller_Sound.PlayMusic("Background");
    }

    void GenerateLevelItems()
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
        
        if (_levelDatas != null)
        {
            foreach (LevelData level in _levelDatas)
            {
                UI_Level item = Instantiate(levelPrefab, transform);
                item.Setup(level); 
            }
        }
    }
    

    private void RestoreScrollPosition()
    {
        Canvas.ForceUpdateCanvases(); 

        if (levelScrollView != null && Controller_LoadLevel.Instance != null)
        {
            float savedPos = Controller_LoadLevel.Instance.GetSavedScrollPosition();
            levelScrollView.horizontalNormalizedPosition = savedPos;
        }
    }

    public void SaveCurrentScrollPosition()
    {
        if (levelScrollView != null && Controller_LoadLevel.Instance != null)
        {
            Controller_LoadLevel.Instance.SaveScrollPosition(levelScrollView.horizontalNormalizedPosition);
        }
    }
    

    private void OnDisable()
    {
        SaveCurrentScrollPosition();
        Controller_Sound.StopMusic();
    }
    
    public void LoadSettingScene()
    {
        Controller_Scene.Instance.LoadScene("Setting");
    }
}