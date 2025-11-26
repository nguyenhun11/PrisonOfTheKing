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
            //DontDestroyOnLoad(gameObject);
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
        
        videoPlayer.Prepare();
        
        while (!videoPlayer.isPlaying)
        {
            yield return null; 
        }
        
        float waitTime = (float)videoPlayer.length / 2f;
        yield return new WaitForSeconds(waitTime);
        
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);
        
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
    
    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

}