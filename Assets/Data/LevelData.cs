using System.Collections.Generic;
using UnityEngine;
using System.Linq; 

[CreateAssetMenu(fileName = "LevelDialogueData", menuName = "Data/LevelDialogueData")]
public class LevelData : ScriptableObject
{
    [Header("Level Data")]
    public string sceneName;
    public bool isLocked;
    public bool isFinish;
    public string levelName;
    public List<LevelData> nextLevels;

    public void FinishLevel(bool finish = true)
    {
        isFinish = finish;
        foreach (LevelData nextLevel in nextLevels)
        {
            nextLevel.isLocked = false;
        }
    }

    public void LoadLevel()
    {
        if (isLocked) return;
        Controller_LoadLevel.Instance.currentLevel = this;
        Controller_Scene.Instance.LoadScene(sceneName);
    }
    
    [System.Serializable]
    public class EnemyCountTrigger
    {
        public int enemiesLeft;
        public DialogueData dialogueData;
        public bool isOpened;
    }
    
    [Header("Dialogue Data")]
    public List<EnemyCountTrigger> enemyDialogues;
    
    public DialogueData GetDialogueByCount(int currentCount)
    {
        foreach (EnemyCountTrigger item in enemyDialogues)
        {
            if (item.enemiesLeft == currentCount &&  !item.isOpened)
            {
                item.isOpened = true;
                return item.dialogueData;
            }
        }
        return null;
    }
}