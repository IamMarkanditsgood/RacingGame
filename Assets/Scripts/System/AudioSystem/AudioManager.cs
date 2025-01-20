using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class AudioManager
{
    [SerializeField] private AudioMixerManager _audioMixerManager;

    private Dictionary<string, AudioSource> _soundSource = new Dictionary<string, AudioSource>();
    private Dictionary<string, AudioSource> _voiceSource = new Dictionary<string, AudioSource>();
    private AudioSource _musicSource;

    public AudioMixerManager AudioMixerManager
    {
        get
        {
            if (_audioMixerManager != null)
            {
                return _audioMixerManager;
            }
            Debug.LogError("You did not set AudioMixer!");
            return null;
        }
        set => _audioMixerManager = value;
    }

    #region Play Methods
    public void PlaySound(AudioConfig audioConfig, Transform parent = null, bool isLoop = false)
    {
        AudioSource audioSource = GetAudioSource(_soundSource, audioConfig, parent, isLoop);
        audioSource.clip = audioConfig.Clip;

        AddOutputAudioMixerGroup(audioSource, MixerParameters.Sound);

        audioSource.Play();
    }

    public void PlayVoice(AudioConfig audioConfig, Transform parent = null, bool isLoop = false)
    {

        AudioSource voiceSource = GetAudioSource(_voiceSource, audioConfig, parent, isLoop);
        voiceSource.clip = audioConfig.Clip;

        AddOutputAudioMixerGroup(voiceSource, MixerParameters.Voice);

        voiceSource.Play();
    }

    public void PlayOneShot(AudioConfig audioConfig, Transform parent = null, bool isLoop = false)
    {

        AudioSource audioSource = GetAudioSource(_soundSource, audioConfig, parent, isLoop);

        AddOutputAudioMixerGroup(audioSource, MixerParameters.Sound);

        audioSource.PlayOneShot(audioConfig.Clip);
    }

    public void PlayMusic(AudioConfig audioConfig, float volume = 1.0f)
    {
        if (_musicSource == null)
        {
            GameObject musicObject = new GameObject("MusicAudioSource");
            AudioSource musicSource = musicObject.AddComponent<AudioSource>();
            _musicSource = musicSource;
        }

        _musicSource.clip = audioConfig.Clip;
        _musicSource.volume = volume;
        AddOutputAudioMixerGroup(_musicSource, MixerParameters.Music);

        _musicSource.loop = true;
        _musicSource.Play();
    }

    private AudioSource GetAudioSource(Dictionary<string, AudioSource> audioList, AudioConfig audioConfig, Transform parent, bool isLoop)
    {
        if (!audioList.ContainsKey(audioConfig.AudioSourceName))
        {
            CreateNewAudioSource(audioList, audioConfig, parent);
        }

        AudioSource audioSource = audioList[audioConfig.AudioSourceName];
        audioSource.volume = audioConfig.Volume;
        audioSource.loop = isLoop;
        return audioSource;
    }

    private void CreateNewAudioSource(Dictionary<string, AudioSource> audioList, AudioConfig audioConfig, Transform parent)
    {
        GameObject newAudioSourceObject = new GameObject(audioConfig.AudioSourceName + "AudioSource");
        newAudioSourceObject.transform.position = audioConfig.Position;

        if (parent != null) // add to parent (Container)
        {
            newAudioSourceObject.transform.parent = parent;
        }

        AudioSource newAudioSource = newAudioSourceObject.AddComponent<AudioSource>();
        audioList.Add(audioConfig.AudioSourceName, newAudioSource);
    }

    private void AddOutputAudioMixerGroup(AudioSource audioSource, MixerParameters mixerParameter)
    {
        if (AudioMixerManager.AudioMixer != null)
        {
            audioSource.outputAudioMixerGroup = AudioMixerManager.AudioMixer.FindMatchingGroups(mixerParameter.ToString())[0];
        }
    }
    #endregion

    #region Mute/Unmute Methods
    public void SwitchMuteVoiceStatus(string voiceName)
    {
        SwitchMuteStatus(_voiceSource, voiceName);
    }

    public void SwitchMuteSoundStatus(string soundName)
    {
        SwitchMuteStatus(_soundSource, soundName);
    }

    public void SwitchMuteAllSoundsStatus()
    {
        SwitchMuteStatus(_soundSource);
    }

    public void SwitchMuteAllVoicesStatus()
    {
        SwitchMuteStatus(_voiceSource);
    }

    public void SwitchMuteMusicStatus()
    {
        if (_musicSource.volume == 0f)
        {
            _musicSource.volume = 1f;
        }
        else
        {
            _musicSource.volume = 0f;
        }
    }

    #endregion

    #region Stop Methods

    public void StopSound(string soundName)
    {
        StopAudio(_soundSource, soundName);
    }
    public void StopVoice(string voiceName)
    {
        StopAudio(_voiceSource, voiceName);
    }
    public void StopMusic()
    {
        AudioSource musicSource = GameObject.Find("MusicAudioSource")?.GetComponent<AudioSource>();
        if (musicSource != null)
        {
            musicSource.Stop();
        }
    }
    public void StopAllSounds()
    {
        StopAllAudio(_soundSource);
    }
    public void StopAllVoices()
    {
        StopAllAudio(_voiceSource);
    }

    #endregion

    #region VolumeChange
    public void ChangeVoiceVolume(string voiceName, float volume)
    {
        ChangeAudioVolume(_voiceSource, voiceName, volume);
    }
    public void ChangeSoundVolume(string soundName, float volume)
    {
        ChangeAudioVolume(_soundSource, soundName, volume);
    }


    #endregion


    #region MuteStatus Methods
    private void SwitchMuteStatus(Dictionary<string, AudioSource> audioSource, string audioName)
    {
        if (audioSource.ContainsKey(audioName))
        {

            if (audioSource[audioName].volume == 0f)
            {
                audioSource[audioName].volume = 1f;
            }
            else
            {
                audioSource[audioName].volume = 0f;
            }
        }
    }

    private void SwitchMuteStatus(Dictionary<string, AudioSource> audioSources)
    {
        foreach (var audioSource in audioSources.Values)
        {
            if (audioSource.volume == 0f)
            {
                audioSource.volume = 1f;
            }
            else
            {
                audioSource.volume = 0f;
            }
        }
    }

    #endregion

    #region Stop Methods
    private void StopAudio(Dictionary<string, AudioSource> audioSource, string audioName)
    {
        if (audioSource.ContainsKey(audioName))
        {
            AudioSource source = audioSource[audioName];
            source.Stop();
        }
    }
    private void StopAllAudio(Dictionary<string, AudioSource> audioSources)
    {
        foreach (var audioSource in audioSources.Values)
        {
            audioSource.Stop();
        }
    }

    #endregion

    #region VolumeChange
    public void ChangeAudioVolume(Dictionary<string, AudioSource> source, string soundName, float volume)
    {
        if (source.ContainsKey(soundName))
        {
            source[soundName].volume = volume;
        }
    }
    #endregion
}