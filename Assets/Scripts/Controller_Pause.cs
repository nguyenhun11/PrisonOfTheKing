using UnityEngine;

public class Controller_Pause : MonoBehaviour
{
    public static bool IsGamePaused
    {
        get;
        private set;
    } = false;
    
    public static void SetPause(bool pause)
    {
        Debug.Log("pause set to" +  pause);
        IsGamePaused = pause;
        if (pause)
        {
            Time.timeScale = 0;
        }
        else Time.timeScale = 1;
    }
    
}
