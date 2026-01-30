using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

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

    public Button skipDialogueButton;
    public GameObject dialogueChoicesPanel;
    public TMP_Text dialogueChoicesPanelText;
    public Button skipAllButton;
    public Button skipThisButton;
    public Button cancelButton;

    private void Start()
    {
        // Đảm bảo tắt UI khi bắt đầu
        EndDialogue();
        dialogueChoicesPanel.SetActive(false);

        // --- SETUP SỰ KIỆN CHO CÁC NÚT (Quan trọng) ---

        // 1. Nút mở bảng chọn Skip
        skipDialogueButton.onClick.AddListener(OnOpenSkipOptions);

        // 2. Nút Skip All (Bỏ qua tất cả level về sau)
        skipAllButton.onClick.AddListener(() => {
            // Gọi sang Controller_LoadLevel để xử lý dữ liệu toàn cục
            Controller_LoadLevel.Instance.SkipAllFutureDialogues();
            // Tắt thoại ngay lập tức
            CloseDialoguePanel(resume: false);
        });

        // 3. Nút Skip This (Chỉ bỏ qua level này)
        skipThisButton.onClick.AddListener(() => {
            // Gọi sang Controller_LoadLevel hoặc trực tiếp vào LevelData
            Controller_LoadLevel.Instance.SkipCurrentLevelDialogues();
            // Tắt thoại ngay lập tức
            CloseDialoguePanel(resume: false);
        });

        // 4. Nút Hủy (Đóng bảng chọn, tiếp tục đọc thoại)
        cancelButton.onClick.AddListener(() => CloseDialoguePanel(resume: true));
        if (Controller_GeneralSetting.Instance.currentLanguage == Controller_GeneralSetting.Language.Vietnamese)
        {
            dialogueChoicesPanelText.text = "Bỏ qua lời thoại?";
            skipAllButton.GetComponentInChildren<TMP_Text>().text = "Tất cả";
            skipThisButton.GetComponentInChildren<TMP_Text>().text = "Chỉ màn này";
            cancelButton.GetComponentInChildren<TMP_Text>().text = "Không";
        }
        else
        {
            dialogueChoicesPanelText.text = "Skip all dialogue?";
            skipAllButton.GetComponentInChildren<TMP_Text>().text = "All level";
            skipThisButton.GetComponentInChildren<TMP_Text>().text = "Only this level";
            cancelButton.GetComponentInChildren<TMP_Text>().text = "Cancel";
        }
    }

    private void Update()
    {
        if (!IsDialogueActive) return;

        if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space))
        {
            // --- KIỂM TRA VÙNG CẤM CLICK ---

            // 1. Nếu chuột đang đè lên nút Skip -> Return ngay, để nút Skip tự xử lý việc của nó
            if (IsMouseOverUI(skipDialogueButton.gameObject)) return;

            // 2. Nếu bảng chọn Skip đang bật và chuột đè lên bảng đó -> Return luôn
            if (IsMouseOverUI(dialogueChoicesPanel)) return;

            // -------------------------------

            // Nếu không dính các nút trên thì NextLine vô tư (kể cả click vào mặt nhân vật hay khung thoại)
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
        dialogueChoicesPanel.SetActive(false);
        dialogueText.text = "";
        if (dialogueImage != null) dialogueImage.sprite = null;
        
        IsDialogueActive = false;
        _currentData = null;
        _currentLocalizedText = "";
        
        Controller_Pause.SetPause(false); // Unpause game
    }
    private void OnOpenSkipOptions()
    {
        // Khi mở bảng chọn skip, ta chỉ hiện UI lên
        // Game vẫn đang Pause sẵn do StartDialogue đã gọi SetPause(true)
        dialogueChoicesPanel.SetActive(true);
        dialoguePanel.SetActive(false);
    }


    private void CloseDialoguePanel(bool resume = true)
    {
        dialogueChoicesPanel.SetActive(false);
        if (resume)
        {
            dialoguePanel.SetActive(true);
        }
        else
        {
            EndDialogue();
        }
    }

    // Hàm kiểm tra xem chuột có đang nằm trong phạm vi của một RectTransform cụ thể không
    private bool IsMouseOverUI(GameObject targetObj)
    {
        if (targetObj == null || !targetObj.activeInHierarchy) return false;

        // Lấy RectTransform của đối tượng
        RectTransform rect = targetObj.GetComponent<RectTransform>();

        // Kiểm tra toạ độ chuột có nằm trong hình chữ nhật của đối tượng đó không
        return RectTransformUtility.RectangleContainsScreenPoint(rect, Input.mousePosition);
    }
}