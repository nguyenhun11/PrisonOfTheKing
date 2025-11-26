using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller_Level : MonoBehaviour
{
    [Header("Enemy")]
    public GameObject  enemies;
    private List<Enemy> _enemiesList;
    private int _enemiesCount;
    
    [Header("Player")]
    public GameObject  player;
    private PlayerState _playerState;
    private TravelTile _playerTravelTile;
    
    [Header("Game Logical")]
    public Key key;
    public Door door;
    
    [Header("Dialogue Data")]
    public LevelData levelData;
    //public DialogueData startLevelDialogue;

    void Start()
    {
        _enemiesList = new List<Enemy>(enemies.GetComponentsInChildren<Enemy>());
        
        player = GameObject.FindGameObjectWithTag("Player");
        _playerState = player.GetComponent<PlayerState>();
        _playerTravelTile = player.GetComponent<TravelTile>();
        _playerTravelTile.OnTravelTileStop += CheckEnemiesLeft;

        Resume();
        ShowKey(false);
        
        Controller_Dialogue.Instance.EndDialogue();

        Invoke(nameof(CheckEnemiesLeft), 1f);
    }

    private void CheckEnemiesLeft()
    {
        _enemiesList.RemoveAll(e => e == null);
        int count = _enemiesList.Count;
        bool isDifferent = _enemiesCount != count;
        _enemiesCount = count;
        if (isDifferent)
        {
            if (_enemiesCount == 0)
            {
                ShowKey(true);
            }

            DialogueData dialogueData = levelData.GetDialogueByCount(_enemiesCount);
            if (dialogueData != null)
            {
                Controller_Dialogue.Instance.StartDialogue(dialogueData);
                
            }
        }
    }

    public void RePlay()
    {
        levelData.LoadLevel();
    }
    
    
    public void Pause()
    {
        Controller_Dialogue.Instance.EndDialogue();
        Controller_Pause.SetPause(true);
        Controller_UI.Instance.ShowSettingPanel(true);
    }

    public void Resume()
    {
        Controller_Pause.SetPause(false);
        Controller_UI.Instance.ShowSettingPanel(false);
    }

    private void ShowKey(bool on)
    {
        if (key != null)
        {
            key.gameObject.SetActive(on);
            if (on)
            {
                key.OnKeyClaimed += ClaimKey;
            }
        }
    }

    private void ClaimKey()
    {
        key.OnKeyClaimed -= ClaimKey;
        if (door != null)
        {
            door.CanOpenDoor(true);
            door.OnDoorOpen += OpenDoor;
        }
    }

    private void OpenDoor()
    {
        GameWin();
    }

    private void GameWin()
    {
        levelData.isFinish = true;
        StartCoroutine(WaitGameWin());
    }

    private IEnumerator WaitGameWin()
    {
        yield return new WaitForSecondsRealtime(1);
        Controller_Scene.Instance.LoadScene("LevelSelect");
    }
}
