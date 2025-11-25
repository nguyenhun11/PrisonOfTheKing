using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using UnityEngine;
using UnityEngine.Rendering;

public class Controller_Level : MonoBehaviour
{
    [Header("Enemy")]
    public GameObject  enemies;
    public List<Enemy> enemiesList;
    public int enemiesCount;
    
    [Header("Player")]
    public GameObject  player;
    public PlayerState playerState;
    public TravelTile playerTravelTile;
    
    [Header("Game Logical")]
    public Key key;
    public Door door;
   
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        enemiesList = new List<Enemy>(enemies.GetComponentsInChildren<Enemy>());
        CheckEnemiesLeft();
        
        player = GameObject.FindGameObjectWithTag("Player");
        playerState = player.GetComponent<PlayerState>();
        playerTravelTile = player.GetComponent<TravelTile>();
        playerTravelTile.OnTravelTileStop += CheckEnemiesLeft;

        Resume();
        ShowKey(false);
        if (door != null) door.CloseDoor();
    }

    private void CheckEnemiesLeft()
    {
        enemiesList.RemoveAll(e => e == null);
        enemiesCount = enemiesList.Count;
        if (enemiesCount == 0)
        {
            ShowKey(true);
        }
    }

    public void RePlay()
    {
        
    }

    public void GameWin()
    {
        
    }

    public void GameLose()
    {
        
    }
    
    public void Pause()
    {
        Time.timeScale = 0;
        Controller_UI.Instance.ShowSettingPanel(true);
    }

    public void Resume()
    {
        Time.timeScale = 1;
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
        Debug.Log("OpenDoor");
    }
}
