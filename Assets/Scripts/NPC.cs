using System;
using UnityEngine;

public class NPC : MonoBehaviour
{
    [SerializeField] private LevelData _levelData;
    [SerializeField] private Tile _tile;

    private void Start()
    {
        _levelData = Controller_Level.Instance.levelData;
        _tile = GetComponent<Tile>();

        _tile.isStopTile = _levelData.showNPCDialogue;
        gameObject.SetActive(_levelData.showNPC);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent(out PlayerAnimator player))
        {
            if (_levelData.showNPCDialogue)
            {
                ShowDialogue(player);
            }
        }
    }
    
    private void ShowDialogue(PlayerAnimator player)
    {
        DialogueData dialogueData = _levelData.npcDialogue;
        if (_levelData.showNPCDialogue && dialogueData != null)
        {
            Controller_Dialogue.Instance.StartDialogue(dialogueData);
            _levelData.showNPCDialogue = false;
            _tile.isStopTile = false;
            player.StandOnGround();
        }
    }
}
