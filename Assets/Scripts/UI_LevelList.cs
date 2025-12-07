using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // Cần thiết cho ScrollRect

public class UI_LevelList : MonoBehaviour
{
    [Header("Data References")]
    public UI_Level levelPrefab; // Prefab nút Level
    private List<LevelData> levelData;

    [Header("UI References")]
    [Tooltip("Kéo Scroll View (cái chứa ScrollRect) vào đây")]
    public ScrollRect levelScrollView; 

    private void Start()
    {
        if (Controller_LoadLevel.Instance != null)
        {
            levelData = Controller_LoadLevel.Instance.allLevels;
        }

        // 2. Sinh ra các nút Level (Logic cũ của bạn)
        GenerateLevelItems();

        // 3. Khôi phục vị trí cuộn (Logic mới)
        RestoreScrollPosition();

        // 4. Phát nhạc
        Controller_Sound.PlayMusic("Background");
    }

    void GenerateLevelItems()
    {
        // Xóa các nút cũ (nếu có)
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }

        // Tạo nút mới từ Prefab
        if (levelData != null)
        {
            foreach (LevelData level in levelData)
            {
                // Instantiate con vào transform hiện tại (Content)
                UI_Level item = Instantiate(levelPrefab, transform);
                
                // Gọi hàm Setup (như chúng ta đã sửa ở UI_Level trước đó)
                // Hoặc dùng code cũ của bạn: item.levelData = level; item.ResetLevelState();
                item.Setup(level); 
            }
        }
    }

    // --- TÍNH NĂNG MỚI: SCROLL ---

    private void RestoreScrollPosition()
    {
        // Vì Instantiate cần 1 khung hình để cập nhật kích thước Content, 
        // ta dùng Coroutine hoặc ForceUpdate để Scroll hoạt động đúng.
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

    // --- CÁC SỰ KIỆN ---

    private void OnDisable()
    {
        // 1. Lưu vị trí Scroll trước khi tắt/hủy
        SaveCurrentScrollPosition();

        // 2. Tắt nhạc (Logic cũ)
        Controller_Sound.StopMusic();
    }
    
    public void LoadSettingScene()
    {
        // Lưu vị trí scroll trước khi chuyển cảnh cho chắc chắn
        SaveCurrentScrollPosition();
        
        Controller_Scene.Instance.LoadScene("Setting");
    }
}