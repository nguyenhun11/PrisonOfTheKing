using System.Collections.Generic;
using UnityEngine;

public class UI_LevelList : MonoBehaviour
{
    public List<LevelData> levelData;
    public UI_Level levelPrefab;
    public GameObject nullPrefab;

    private void Start()
    {
        GenerateLevelItems();
        Controller_Sound.PlayMusic("Background");
    }
    
    void GenerateLevelItems()
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
        Instantiate(nullPrefab, transform);
        foreach (LevelData level in levelData)
        {
            UI_Level item = Instantiate(levelPrefab, transform);
            item.levelData = level;
            item.ResetLevelState();
        }
        Instantiate(nullPrefab, transform);
    }

    private void OnDisable()
    {
        Controller_Sound.StopMusic();
    }
    
}
