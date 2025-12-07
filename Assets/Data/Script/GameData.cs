using System.Collections.Generic;

[System.Serializable]
public class GameData
{
    // --- SETTINGS ---
    public float musicVolume = 1f;
    public float sfxVolume = 1f;
    public int languageIndex = 0;
    
    // --- SCROLL ---
    public float lastScrollPosition = 0f;

    // --- PROGRESS ---
    public List<LevelState> levelStates = new List<LevelState>();
}

[System.Serializable]
public class LevelState
{
    public int levelID;
    public bool isLocked;
    public bool isFinish;
    public bool showNPC;
    public bool showNPCDialogue;
    
    public List<bool> enemyStates = new List<bool>();
}