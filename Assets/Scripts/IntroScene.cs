using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class IntroScene : MonoBehaviour
{
    public VideoPlayer videoPlayer;
    public string menuSceneName = "LevelSelect"; // Tên scene menu của bạn

    void Start()
    {
        if (videoPlayer == null) 
            videoPlayer = GetComponent<VideoPlayer>();
        Controller_Sound.StopMusic();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space))
        {
            LoadMenu();
        }
    }
    

    IEnumerator LoadScene()
    {
        yield return new WaitForSeconds(2);
        LoadMenu();
    }

    void LoadMenu()
    {
        Controller_Scene.Instance.LoadScene("LevelSelect");
    }
}