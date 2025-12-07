using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller_Level : MonoBehaviour
{
    #region  singleton

    public static Controller_Level Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    #endregion
    [Header("Enemy")]
    public GameObject  enemies;
    private List<Enemy> _enemiesList;
    public int _enemiesCount;
    
    [Header("Player")]
    public GameObject  player;
    private PlayerState _playerState;
    private TravelTile _playerTravelTile;
    
    [Header("Game Logical")]
    public Key key;
    public Door door;
    public NPC npc;
    
    [Header("Dialogue Data")]
    public LevelData levelData;

    void Start()
    {
        _enemiesList = new List<Enemy>(enemies.GetComponentsInChildren<Enemy>());
        
        player = GameObject.FindGameObjectWithTag("Player");
        _playerState = player.GetComponent<PlayerState>();
        _playerTravelTile = player.GetComponent<TravelTile>();
        _playerTravelTile.OnTravelTileStop += CheckEnemiesLeft;
        _playerState.OnPlayerDeath += GameOver;

        Resume();
        ShowKey(false);
        
        Controller_Dialogue.Instance.EndDialogue();

        Invoke(nameof(CheckEnemiesLeft), 1f);
        levelData = Controller_LoadLevel.Instance.currentLevel; //Không xóa dòng này, bỏ comment lúc build game
        
        npc?.gameObject.SetActive(levelData.showNPC);
    }

    private void CheckEnemiesLeft()
    {
        _enemiesList.RemoveAll(e => e == null || e.IsDead);
        int count = _enemiesList.Count;
        bool isDifferent = _enemiesCount != count;
        _enemiesCount = count;
        if (isDifferent)
        {
            if (_enemiesCount == 0)
            {
                ShowKey(true);
            }

            DialogueData dialogueData = levelData.GetDialogueByCount(_enemiesCount, out float delayTime);
            if (dialogueData != null)
            {
                StartCoroutine(PlayDialogueWithDelay(dialogueData, delayTime));
            }
        }
    }
    
    private IEnumerator PlayDialogueWithDelay(DialogueData data, float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        Controller_Dialogue.Instance.StartDialogue(data);
    }

    public void RePlay()
    {
        Controller_LoadLevel.Instance.LoadLevel();
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

    public void ReturnToMenu()
    {
        if (Controller_LoadLevel.Instance != null)
        {
            Controller_LoadLevel.Instance.SaveProgress();
        }
        Controller_Pause.SetPause(false);
        Controller_Scene.Instance.LoadScene("LevelSelect");
    }

    private void ShowKey(bool on)
    {
        if (key != null)
        {
            key.gameObject.SetActive(on);
            if (on)
            {
                Controller_Sound.Play("Key", true);
                key.OnKeyClaimed += ClaimKey;
            }
        }
    }

    private void ClaimKey()
    {
        key.OnKeyClaimed -= ClaimKey;
        Controller_Sound.Play("Key", true);
        if (door != null)
        {
            door.CanOpenDoor(true);
            door.OnDoorOpen += GameWin;
        }
    }

    private void GameWin()
    {
        levelData.FinishLevel();
        levelData.showNPC = false;
        Controller_LoadLevel.Instance.CompleteCurrentLevel();
        StartCoroutine(WaitGameWin());
    }
    

    private IEnumerator WaitGameWin()
    {
        yield return new WaitForSecondsRealtime(1);
        Controller_Scene.Instance.LoadScene("Win");
    }

    private void GameOver()
    {
        Debug.Log("Game over");
        Controller_Scene.Instance.LoadScene("Lose");
    }
}
