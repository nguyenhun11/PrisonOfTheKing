using UnityEngine;
using UnityEngine.UI;

public class Controller_Sound : MonoBehaviour
{
    private static AudioSource _audioSource;
    private static AudioSource _randomPitchAudioSource;
    private static AudioSource _voiceAudioSource;
    private static AudioSource _backgroundAudioSource;
    private static SoundEffectLibrary _soundEffectLibrary;
    [SerializeField] private Slider soundEffectSlider;
   
    #region singleton
    public static Controller_Sound Instance;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            AudioSource[] audioSources = GetComponents<AudioSource>(); 
            _audioSource = audioSources[0];
            _randomPitchAudioSource = audioSources[1];
            _voiceAudioSource = audioSources[2];
            _backgroundAudioSource = audioSources[3];

            _soundEffectLibrary = GetComponent<SoundEffectLibrary>();
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    #endregion

    private void Start()
    {
        soundEffectSlider?.onValueChanged.AddListener(delegate
        {
            OnValueChange();
        });
    }

    public static void Play(string soundName, bool randomPitch = false)
    {
        AudioClip clip = _soundEffectLibrary.GetRandomClip(soundName);
        if (clip != null)
        {
            if (randomPitch)
            {
                _randomPitchAudioSource.pitch = Random.Range(0.5f, 1.5f);
                _randomPitchAudioSource.PlayOneShot(clip);
                _randomPitchAudioSource.pitch = 1;
            }
            else
            {
                _audioSource.PlayOneShot(clip);
            }
        }
    }

    public void SetVolume(float volume)
    {
        _audioSource.volume = volume;
        _randomPitchAudioSource.volume = volume;
        _voiceAudioSource.volume = volume;
    }

    public void OnValueChange()
    {
        SetVolume(soundEffectSlider.value);
    }

    public static void PlayVoice(AudioClip clip, float pitch = 1f)
    {
        _voiceAudioSource.pitch = pitch;
        _voiceAudioSource.PlayOneShot(clip);
    }
    
    public static void PlayMusic(string soundName)
    {
        AudioClip clip = _soundEffectLibrary.GetRandomClip(soundName);
    
        if (clip != null)
        {
            if (_backgroundAudioSource.clip == clip && _backgroundAudioSource.isPlaying) return;

            _backgroundAudioSource.clip = clip;  
            _backgroundAudioSource.loop = true;  
            _backgroundAudioSource.Play();     
        }
    }
    
    public static void StopMusic()
    {
        _backgroundAudioSource.Stop();
    }
}
