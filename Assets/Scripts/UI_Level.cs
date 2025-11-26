using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_Level : MonoBehaviour
{
    public Sprite lockedImage;
    public Sprite unlockedImage;
    public Sprite finishImage;
    [Header("UI Elements")]
    public TextMeshProUGUI levelNameText;
    public Image levelImage;
    [Header("Level data")]
    public LevelData levelData;

    private void Start()
    {
        ResetLevelState();
    }

    public void ResetLevelState()
    {
        if (levelData != null)
        {
            levelNameText.text = levelData.levelName;
            if (levelData.isLocked)
            {
                levelImage.sprite = lockedImage;
            }
            else
            {
                if (levelData.isFinish)
                {
                    levelImage.sprite = finishImage;
                }
                else
                {
                    levelImage.sprite = unlockedImage;
                }
            }
        }
    }

    public void LoadLevel()
    {
        levelData.LoadLevel();
    }
    
}
