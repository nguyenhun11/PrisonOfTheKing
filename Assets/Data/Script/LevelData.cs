using System.Collections.Generic;
using UnityEngine;
using System.Linq; 

[CreateAssetMenu(fileName = "LevelData", menuName = "Data/LevelData")]
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
        public float delayTime;
    }
    
    [Header("Dialogue Data")]
    public List<EnemyCountTrigger> enemyDialogues;
    
    public DialogueData GetDialogueByCount(int currentCount, out float delayTime)
    {
        foreach (EnemyCountTrigger item in enemyDialogues)
        {
            if (item.enemiesLeft == currentCount &&  !item.isOpened)
            {
                item.isOpened = true;
                delayTime = item.delayTime;
                return item.dialogueData;
            }
        }
        delayTime = 0f;
        return null;
    }
}