using System.Collections.Generic;
using UnityEngine;
using System.Linq; 

[CreateAssetMenu(fileName = "LevelDialogueData", menuName = "Data/LevelDialogueData")]
public class LevelDialogueData : ScriptableObject
{
    [System.Serializable]
    public struct EnemyCountTrigger
    {
        public int enemiesLeft;
        public DialogueData dialogueData;
    }
    
    public List<EnemyCountTrigger> enemyDialogues;
    
    public DialogueData GetDialogueByCount(int currentCount)
    {
        foreach (var item in enemyDialogues)
        {
            if (item.enemiesLeft == currentCount)
            {
                return item.dialogueData;
            }
        }
        return null;
    }
}