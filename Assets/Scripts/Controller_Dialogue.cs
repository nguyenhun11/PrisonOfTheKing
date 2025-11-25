using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Controller_Dialogue : MonoBehaviour
{
    #region  singleton

    public static Controller_Dialogue Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    #endregion
    
    public GameObject dialoguePanel;
    public TMP_Text dialogueText;
    public Image avt;
    
    public void ShowDialogue(bool on)
    {
        dialoguePanel.SetActive(on);
    }

    public void SetImage(Sprite sprite)
    {
        avt.sprite = sprite;
    }
    
    public void SetDialogueText(string text)
    {
        dialogueText.text = text;
    }

    public void EndDialogue()
    {
        dialoguePanel.SetActive(false);
        avt.sprite = null;
        dialogueText.text = "";
    }
}
