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
    
    // Biến lưu nội dung text hiện tại (sau khi đã chọn ngôn ngữ)
    private string _currentLocalizedText; 

    private void Start()
    {
        EndDialogue();
    }
    
    private void Update()
    {
        if (!IsDialogueActive)  return;
        if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space))
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
        Controller_Pause.SetPause(true); // Pause game khi thoại

        DisplayCurrentLine();
    }

    // --- HÀM MỚI: Lấy text theo ngôn ngữ ---
    private string GetLocalizedContent(int index)
    {
        if (_currentData == null || index >= _currentData.lines.Length) return "";

        // Kiểm tra ngôn ngữ từ Controller_GeneralSetting
        if (Controller_GeneralSetting.Instance != null && 
            Controller_GeneralSetting.Instance.currentLanguage == Controller_GeneralSetting.Language.English)
        {
            return _currentData.lines[index].textEN;
        }
        
        // Mặc định trả về tiếng Việt
        return _currentData.lines[index].textVI;
    }

    private void DisplayCurrentLine()
    {
        StopAllCoroutines();
        
        // 1. Cập nhật hình ảnh
        dialogueImage.sprite = _currentData.lines[_dialogueIndex].image;
        
        // 2. Lấy nội dung chữ đúng ngôn ngữ
        _currentLocalizedText = GetLocalizedContent(_dialogueIndex);

        // 3. Bắt đầu gõ
        StartCoroutine(TypeLine());
    }

    private IEnumerator TypeLine()
    {
        _isTyping = true;
        dialogueText.text = "";
        
        // Duyệt qua chuỗi text đã được chọn ngôn ngữ (_currentLocalizedText)
        foreach (char letter in _currentLocalizedText) 
        {
            dialogueText.text += letter;
            
            // Âm thanh gõ (nếu có sound library)
            Controller_Sound.Play("Type"); 
            
            yield return new WaitForSecondsRealtime(_currentData.typingSpeed);
        }
        
        _isTyping = false;

        // Logic tự động chuyển trang
        if (_currentData.lines.Length > _dialogueIndex && _currentData.lines[_dialogueIndex].auto)
        {
            yield return new WaitForSecondsRealtime(_currentData.autoProgressDelay);
            NextLine();
        }
    }

    private void NextLine()
    {
        if (_currentData == null) return;

        if (_isTyping)
        {
            // Skip hiệu ứng gõ: Hiển thị ngay lập tức đoạn text đầy đủ
            StopAllCoroutines();
            dialogueText.text = _currentLocalizedText;
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
        _currentLocalizedText = "";
        
        Controller_Pause.SetPause(false); // Unpause game
    }
}