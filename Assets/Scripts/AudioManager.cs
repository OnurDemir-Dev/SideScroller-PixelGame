using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    [SerializeField]
    private AudioSource _musicPlayer;

    [SerializeField]
    private AudioSource[] _soundChannels;

    [SerializeField]
    private AudioSource _uiChannel;

    [SerializeField]
    private float _musicVolume;

    [SerializeField]
    private float _soundVolume;

    [SerializeField]
    private float _uiVolume;

    [SerializeField]
    private bool _isMusicMuted;

    [SerializeField]
    private bool _isSoundMuted;

    [SerializeField]
    private bool _isUIMuted;

    public void PlayMusic(AudioClip clip)
    {
        _musicPlayer.clip = clip;
        _musicPlayer.Play();
    }

    public void PlaySound(AudioClip clip)
    {
        foreach (AudioSource channel in _soundChannels)
        {
            if (!channel.isPlaying)
            {
                channel.PlayOneShot(clip);
                break;
            }
        }
    }
    public void PlayUISound(AudioClip clip)
    {
        _uiChannel.clip = clip;
        _uiChannel.PlayOneShot(clip);
    }

    public void SetMusicVolume(float volume)
    {
        _musicVolume = volume;
        _musicPlayer.volume = _musicVolume;
    }

    public void SetSoundVolume(float volume)
    {
        _soundVolume = volume;
        foreach (AudioSource channel in _soundChannels)
        {
            channel.volume = _soundVolume;
        }
    }

    public void SetUIVolume(float volume)
    {
        _uiVolume = volume;
        _uiChannel.volume = _uiVolume;
    }

    public float GetMusicVolume()
    {
        return _musicVolume;
    }

    public float GetSoundVolume()
    {
        return _soundVolume;
    }

    public float GetUIVolume()
    {
        return _uiVolume;
    }

    public void MuteMusic(bool isMute)
    {
        _isMusicMuted = isMute;
        _musicPlayer.mute = _isMusicMuted;
    }

    public void ToggleMusic()
    {
        MuteMusic(!_isMusicMuted);
    }

    public void MuteSound(bool isMute)
    {
        _isSoundMuted = isMute;
        foreach (AudioSource channel in _soundChannels)
        {
            channel.mute = _isSoundMuted;
        }
    }

    public void ToggleSound()
    {
        MuteSound(!_isSoundMuted);
    }

    public void MuteUI(bool isMute)
    {
        _isUIMuted = isMute;
        _uiChannel.mute = _isUIMuted;
    }

}
