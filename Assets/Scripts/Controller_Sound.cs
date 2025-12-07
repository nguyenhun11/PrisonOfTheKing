using UnityEngine;

public class Controller_Sound : MonoBehaviour
{
    private static AudioSource _audioSource;       // Dùng cho SFX
    private static AudioSource _randomPitchAudioSource; // Dùng cho SFX
    private static AudioSource _voiceAudioSource;  // Dùng cho SFX (hoặc tách riêng Voice)
    private static AudioSource _backgroundAudioSource; // Dùng cho Music

    private static SoundEffectLibrary _soundEffectLibrary;

    public static Controller_Sound Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            AudioSource[] audioSources = GetComponents<AudioSource>();
            // Giả định thứ tự AudioSource bạn setup trong Editor là đúng
            _audioSource = audioSources[0];
            _randomPitchAudioSource = audioSources[1];
            _voiceAudioSource = audioSources[2];
            _backgroundAudioSource = audioSources[3];

            _soundEffectLibrary = GetComponent<SoundEffectLibrary>();
        }
        else
        {
            Destroy(gameObject);
        }
    }
    

    // Chỉnh âm lượng nhạc nền
    public void SetMusicVolume(float volume)
    {
        if (_backgroundAudioSource != null)
        {
            _backgroundAudioSource.volume = volume;
        }
    }

    // Chỉnh âm lượng hiệu ứng (SFX)
    public void SetSFXVolume(float volume)
    {
        if (_audioSource != null) _audioSource.volume = volume;
        if (_randomPitchAudioSource != null) _randomPitchAudioSource.volume = volume;
        if (_voiceAudioSource != null) _voiceAudioSource.volume = volume;
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
        _backgroundAudioSource?.Stop();
    }
}