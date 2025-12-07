using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_Level : MonoBehaviour
{
    [Header("UI References")]
    public Sprite lockedImage;
    public Sprite unlockedImage;
    public Sprite finishImage;
    
    public TextMeshProUGUI levelNameText;
    public Image levelImage;
    
    // Thêm tham chiếu đến Button để tắt/bật tương tác
    private Button btnComp;

    [Header("Data")]
    public LevelData levelData;

    private void Awake()
    {
        btnComp = GetComponent<Button>();
        
        if (btnComp != null)
        {
            btnComp.onClick.RemoveAllListeners();
            btnComp.onClick.AddListener(LoadLevel);
        }
    }
    
    public void Setup(LevelData data)
    {
        levelData = data;
        ResetLevelState();
    }

    public void ResetLevelState()
    {
        if (levelData != null)
        {
            if (levelNameText != null) 
                levelNameText.text = levelData.levelName;
            
            if (levelData.isLocked)
            {
                if (levelImage != null) levelImage.sprite = lockedImage;
                if (btnComp != null) btnComp.interactable = false; // Không cho bấm
            }
            else
            {
                // Trạng thái MỞ
                if (btnComp != null) btnComp.interactable = true; // Cho phép bấm

                if (levelData.isFinish)
                {
                    if (levelImage != null) levelImage.sprite = finishImage;
                }
                else
                {
                    if (levelImage != null) levelImage.sprite = unlockedImage;
                }
            }
        }
    }

    public void LoadLevel()
    {
        if (levelData != null)
        {
            levelData.LoadLevel();
        }
    }
}