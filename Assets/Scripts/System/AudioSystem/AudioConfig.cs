using UnityEngine;

[CreateAssetMenu(fileName = "Audio", menuName = "ScriptableObjects/AudioConfig", order = 0)]
public class AudioConfig : ScriptableObject
{
    [SerializeField] private string _audioSourceName;
    [SerializeField] private AudioClip _clip;
    [SerializeField] private Vector3 _position;
    [SerializeField] private float _volume = 1.0f;

    public string AudioSourceName => _audioSourceName;
    public AudioClip Clip => _clip;
    public Vector3 Position => _position;
    public float Volume => _volume;
}
