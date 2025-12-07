gitusing System.Collections.Generic;
using UnityEngine;

public class Controller_LoadLevel : MonoBehaviour
{
    public static Controller_LoadLevel Instance;

    [Header("Runtime Data")]
    public LevelData currentLevel;
    public List<LevelData> allLevels;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            
            SaveSystem.Load();
            SyncDataToScriptableObjects();
        }
        else
        {
            Destroy(gameObject);
        }
    }
    

    public void LoadLevel()
    {
        if (currentLevel != null)
        {
            currentLevel.LoadLevel();
        }
    }

    public float GetSavedScrollPosition()
    {
        return SaveSystem.data.lastScrollPosition;
    }

    public void SaveScrollPosition(float pos)
    {
        SaveSystem.data.lastScrollPosition = pos;
        SaveSystem.Save();
    }
    
    public void ResetToLevel(int levelNumber)
    {
        int targetIndex = levelNumber - 1;
        
        for (int i = 0; i < allLevels.Count; i++)
        {
            if (i < targetIndex)
            {
                allLevels[i].UnlockLevel(true);
            }
            else if (i == targetIndex)
            {
                allLevels[i].ResetLevel(false);
            }
            else
            {
                allLevels[i].ResetLevel(true);
            }
        }
        
        SaveProgress(); 
    }

    // --- LOGIC LOAD (JSON -> SCRIPTABLE OBJECT) ---
    public void SyncDataToScriptableObjects()
    {
        if (SaveSystem.data.levelStates == null || SaveSystem.data.levelStates.Count == 0)
        {
            InitNewSaveData();
        }

        foreach (var savedState in SaveSystem.data.levelStates)
        {
            if (savedState.levelID < allLevels.Count)
            {
                LevelData lv = allLevels[savedState.levelID];
                
                lv.isLocked = savedState.isLocked;
                lv.isFinish = savedState.isFinish;
                lv.showNPC = savedState.showNPC;
                lv.showNPCDialogue = savedState.showNPCDialogue;
                
                if (savedState.enemyStates != null && lv.enemyDialogues != null)
                {
                    for (int j = 0; j < lv.enemyDialogues.Count; j++)
                    {
                        if (j < savedState.enemyStates.Count)
                        {
                            lv.enemyDialogues[j].isOpened = savedState.enemyStates[j];
                        }
                    }
                }
                
                if (lv.isFinish)
                {
                    lv.UnlockLevel(true); 
                    
                    if (savedState.levelID + 1 < allLevels.Count)
                    {
                        allLevels[savedState.levelID + 1].isLocked = false;
                    }
                }
            }
        }
    }

    // --- LOGIC SAVE (SCRIPTABLE OBJECT -> JSON) ---
    public void SaveProgress()
    {
        for (int i = 0; i < allLevels.Count; i++)
        {
            LevelData currentLvData = allLevels[i];
            
            if (i >= SaveSystem.data.levelStates.Count)
            {
                SaveSystem.data.levelStates.Add(new LevelState { levelID = i, enemyStates = new List<bool>() });
            }
            LevelState savedState = SaveSystem.data.levelStates[i];
            
            savedState.isLocked = currentLvData.isLocked;
            savedState.isFinish = currentLvData.isFinish;
            savedState.showNPC = currentLvData.showNPC;
            savedState.showNPCDialogue = currentLvData.showNPCDialogue;
            
            savedState.enemyStates.Clear(); 
            if (currentLvData.enemyDialogues != null)
            {
                foreach (var enemyTrigger in currentLvData.enemyDialogues)
                {
                    savedState.enemyStates.Add(enemyTrigger.isOpened);
                }
            }
        }

        SaveSystem.Save();
    }
    
    private void InitNewSaveData()
    {
        SaveSystem.data.levelStates = new List<LevelState>();

        for (int i = 0; i < allLevels.Count; i++)
        {
            List<bool> defaultEnemyStates = new List<bool>();
            
            if (allLevels[i].enemyDialogues != null)
            {
                foreach (var enemyTrigger in allLevels[i].enemyDialogues)
                {
                    defaultEnemyStates.Add(false);
                }
            }
            
            SaveSystem.data.levelStates.Add(new LevelState 
            { 
                levelID = i, 
                isLocked = (i != 0),
                isFinish = false,
                showNPC = true,
                showNPCDialogue = true,
                enemyStates = defaultEnemyStates
            });
            
            if (i == 0) allLevels[i].ResetLevel(false);
            else allLevels[i].ResetLevel(true);
        }

        SaveSystem.Save();

    }

    // Gọi khi thắng Level
    public void CompleteCurrentLevel()
    {
        if (currentLevel == null) return;
        
        currentLevel.FinishLevel(true);
        
        SaveProgress();
    }
}