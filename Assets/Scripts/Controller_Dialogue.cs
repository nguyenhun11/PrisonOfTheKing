using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Controller_Dialogue : MonoBehaviour
{
    #region singleton
    public static Controller_Dialogue Instance;
    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(this.gameObject);
    }
    #endregion

    [Header("UI Components")]
    public GameObject dialoguePanel;
    public TMP_Text dialogueText;
    public Image dialogueImage;

    [Header("Data Runtime")] 
    public bool IsDialogueActive { get; private set; }
    private DialogueData _currentData;
    private int _dialogueIndex;
    private bool _isTyping;

    private void Start()
    {
        EndDialogue();
    }
    private void Update()
    {
        if (IsDialogueActive && Input.GetKeyDown(KeyCode.Space))
        {
            NextLine();
        }
    }

    public void StartDialogue(DialogueData data)
    {
        if (data == null) return;

        _currentData = data;
        _dialogueIndex = 0;
        IsDialogueActive = true;
        
        // Setup UI
        dialoguePanel.SetActive(true);
        
        Controller_Pause.SetPause(true);

        DisplayCurrentLine();
    }


    // --- Logic xử lý (bê từ Character sang) ---
    private void DisplayCurrentLine()
    {
        StopAllCoroutines();
        dialogueImage.sprite = _currentData.lines[_dialogueIndex].image;
        StartCoroutine(TypeLine());
    }

    private IEnumerator TypeLine()
    {
        _isTyping = true;
        dialogueText.text = "";
        
        foreach (char letter in _currentData.lines[_dialogueIndex].text) // _currentData.dialogueLines[_dialogueIndex]
        {
            dialogueText.text += letter;
            // SoundEffectManager.PlayVoice(...);
            yield return new WaitForSecondsRealtime(_currentData.typingSpeed); // Dùng Realtime vì TimeScale đang = 0
        }
        
        _isTyping = false;

        // Tự động chuyển (logic cũ của bạn)
        if (_currentData.lines.Length > _dialogueIndex && _currentData.lines[_dialogueIndex].auto)
        {
            yield return new WaitForSecondsRealtime(_currentData.autoProgressDelay);
            NextLine();
        }
    }

    // Hàm này được gọi khi người chơi bấm nút (hoặc click màn hình)
    private void NextLine()
    {
        if (_currentData == null) return;

        if (_isTyping)
        {
            // Skip hiệu ứng gõ, hiện full text
            StopAllCoroutines();
            dialogueText.text = _currentData.lines[_dialogueIndex].text;
            _isTyping = false;
        }
        else if (++_dialogueIndex < _currentData.lines.Length)
        {
            // Sang câu tiếp theo
            DisplayCurrentLine();
        }
        else
        {
            // Hết thoại
            EndDialogue();
        }
    }

    public void EndDialogue()
    {
        StopAllCoroutines();
        dialoguePanel.SetActive(false);
        dialogueText.text = "";
        if (dialogueImage != null) dialogueImage.sprite = null;
        IsDialogueActive = false;
        _currentData = null;
        
        Controller_Pause.SetPause(false);
    }
}