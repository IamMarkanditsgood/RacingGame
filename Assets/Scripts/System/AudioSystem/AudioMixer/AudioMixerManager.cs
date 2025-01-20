using System;
using UnityEngine;
using UnityEngine.Audio;

[Serializable]
public class AudioMixerManager
{
    [SerializeField] private AudioMixer _audioMixer;

    public AudioMixer AudioMixer => _audioMixer;

    #region public
    #region ChangeVolume Methods

    public void ChangeMasterVolume(float volume)
    {
        ChangeAllVolumes(MixerParameters.Master.ToString(), volume);
    }

    public void ChangeMusicVolume(float volume)
    {
        ChangeAllVolumes(MixerParameters.Music.ToString(), volume);
    }

    public void ChangeAllVoiceVolume(float volume)
    {
        ChangeAllVolumes(MixerParameters.Voice.ToString(), volume);
    }

    public void ChangeAllSoundVolume(float volume)
    {
        ChangeAllVolumes(MixerParameters.Sound.ToString(), volume);
    }
    #endregion
    #endregion

    #region private
    #region AudioVolumeChanging Methods

    private void ChangeAllVolumes(string mixerGroup, float volume)
    {
        if (_audioMixer != null)
        {
            _audioMixer.SetFloat(mixerGroup, volume);
        }
    }
    #endregion
    #endregion
}