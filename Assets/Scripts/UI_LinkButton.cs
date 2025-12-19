using System;
using UnityEngine;
using UnityEngine.UI;

public class UI_LinkButton : MonoBehaviour
{
    [SerializeField] private Button buttonFB;
    [SerializeField] private Button buttonGH;
    [SerializeField] private Button buttonEM;

    private void Start()
    {
        buttonFB.onClick.AddListener(OpenFacebook);
        buttonGH.onClick.AddListener(OpenGitHub);
        buttonEM.onClick.AddListener(OpenEmail);
    }
    
    public void OpenFacebook()
    {
        Application.OpenURL("https://www.facebook.com/nguyen.hung.708327");
    }

    public void OpenGitHub()
    {
        Application.OpenURL("https://github.com/nguyenhun11/PrisonOfTheKing");
    }

    public void OpenEmail()
    {
        Application.OpenURL("mailto:giahungcantho11@gmail.com");
    }
}
