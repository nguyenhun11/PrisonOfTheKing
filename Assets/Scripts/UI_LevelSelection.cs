using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class UI_LevelSelection : MonoBehaviour
{
    [Header("UI References")]
    [Tooltip("Kéo component Scroll Rect của UI vào đây")]
    public ScrollRect levelScrollView; 
    
    [Tooltip("Kéo object Content (cha của các nút Level) vào đây")]
    public Transform levelContentParent; 

    private void Start()
    {
        InitLevelButtons();
        
        RestoreScrollPosition();
    }

    private void InitLevelButtons()
    {
        List<LevelData> allLevelsData = Controller_LoadLevel.Instance.allLevels;
        
        for (int i = 0; i < allLevelsData.Count; i++)
        {
            if (i < levelContentParent.childCount)
            {
                Transform childButton = levelContentParent.GetChild(i);
                UI_Level uiLevelScript = childButton.GetComponent<UI_Level>();

                if (uiLevelScript != null)
                {
                    uiLevelScript.Setup(allLevelsData[i]);
                }
            }
        }
    }

    private void RestoreScrollPosition()
    {
        if (levelScrollView != null)
        {
            // Lấy vị trí đã lưu từ JSON (thông qua Controller)
            float savedPos = Controller_LoadLevel.Instance.GetSavedScrollPosition();
            levelScrollView.horizontalNormalizedPosition = savedPos;
        }
    }
    
    private void OnDisable()
    {
        SaveCurrentScrollPosition();
    }

    // Bạn cũng có thể gọi hàm này thủ công ở nút "Back" nếu muốn chắc chắn
    public void SaveCurrentScrollPosition()
    {
        if (levelScrollView != null && Controller_LoadLevel.Instance != null)
        {
            // Gửi vị trí hiện tại về Controller để lưu vào JSON
            Controller_LoadLevel.Instance.SaveScrollPosition(levelScrollView.horizontalNormalizedPosition);
        }
    }
}