using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Video;

public class Controller_Scene : MonoBehaviour
{
    public static Controller_Scene Instance;

    [Header("Settings")]
    [SerializeField] private RawImage rawImageScreen;
    [SerializeField] private VideoPlayer videoPlayer;
    [SerializeField] private CanvasGroup canvasGroup; // Vẫn cần để ẩn hiện cái RawImage

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

        // Tắt màn hình video đi
        canvasGroup.alpha = 0;
        canvasGroup.blocksRaycasts = false;
        rawImageScreen.gameObject.SetActive(false);
        
        // Đăng ký sự kiện để biết khi nào video đã chuẩn bị xong (để tránh bị giật lúc đầu)
        videoPlayer.prepareCompleted += (source) => { source.Play(); };
    }

    public void LoadSceneWithVideo(string sceneName)
    {
        StartCoroutine(VideoTransitionSequence(sceneName));
    }

    private IEnumerator VideoTransitionSequence(string sceneName)
    {
        // B1: Bật màn hình chứa video lên
        rawImageScreen.gameObject.SetActive(true);
        canvasGroup.blocksRaycasts = true;
        canvasGroup.alpha = 1; // Hiện lên ngay

        // B2: Chuẩn bị và Chạy Video
        videoPlayer.Prepare();
        
        // Đợi video load xong và bắt đầu chạy
        while (!videoPlayer.isPlaying)
        {
            yield return null; 
        }

        // B3: Chờ đến "điểm giữa" của video (lúc màn hình bị che hoàn toàn)
        // Ví dụ video dài 2s, thì giây thứ 1 là lúc đen xì.
        // Bạn có thể chờ 1 nửa thời gian:
        float waitTime = (float)videoPlayer.length / 2f;
        yield return new WaitForSeconds(waitTime);

        // B4: Load Scene ngầm
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);
        
        // Tắt chế độ tự động nhảy scene để mình kiểm soát (tuỳ chọn)
        operation.allowSceneActivation = false;

        // Chờ load xong 90%
        while (operation.progress < 0.9f)
        {
            yield return null;
        }

        // Cho phép nhảy scene
        operation.allowSceneActivation = true;

        // B5: Chờ nốt phần còn lại của video (hoặc chờ đến khi hết video)
        while (videoPlayer.isPlaying)
        {
            yield return null;
        }

        // B6: Tắt UI
        canvasGroup.alpha = 0;
        canvasGroup.blocksRaycasts = false;
        rawImageScreen.gameObject.SetActive(false);
    }

}