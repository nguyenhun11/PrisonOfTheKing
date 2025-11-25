using UnityEngine;

[CreateAssetMenu(fileName = "DialogueData", menuName = "Data/DialogueData")]
public class DialogueData : ScriptableObject
{
    public DialogueLine[] lines;
    public float autoProgressDelay = 1.5f;
    public float typingSpeed = 0.05f;
    public AudioClip voiceSound;
    public float voicePitch = 1f;

    [System.Serializable]
    public struct DialogueLine
    {
        public Sprite image;
        [TextArea(3, 5)] public string text;
        public bool auto;
    }
}
