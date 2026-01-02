using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Controller_Scene : MonoBehaviour
{
    public static Controller_Scene Instance;

    [Header("Transition")]
    public Canvas transitionCanvas;
    public float transitionDuration = 1f;
    private Animator _transitionAnimator;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        if (transitionCanvas != null)
            _transitionAnimator = transitionCanvas.GetComponent<Animator>();
    }

    // Đăng ký sự kiện: Cứ hễ load xong scene là chạy hàm OnSceneLoaded
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    // Hàm này TỰ ĐỘNG CHẠY mỗi khi vào Scene mới
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Vừa vào Scene mới -> Màn hình đang đen -> Play Fade In để sáng dần
        _transitionAnimator.Play("StartScene"); 
        // Lưu ý: Trong animation FadeIn, frame cuối cùng bạn nhớ event tắt Canvas hoặc tắt Raycast
    }

    // Hàm này gọi khi bạn muốn chuyển scene (VD: chạm vào cửa)
    public void LoadScene(string sceneName)
    {
        StartCoroutine(Transition(sceneName));
    }

    private IEnumerator Transition(string sceneName)
    {
        transitionCanvas.gameObject.SetActive(true);
        
        _transitionAnimator.Play("OutScene");
        
        yield return new WaitForSecondsRealtime(transitionDuration);

        SceneManager.LoadScene(sceneName);
    }
}