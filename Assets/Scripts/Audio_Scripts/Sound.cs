using UnityEngine.Audio;
using UnityEngine;

[System.Serializable]
public class Sound
{
    //* Name of the audio clip assigned by us 
    public string name;

    public AudioClip clip;

    [Range(0f, 1f)]
    public float volume;

    [Range(0.1f, 3f)]
    public float pitch;

    public bool loop;

    public AudioMixerGroup mixerGroup;

    [HideInInspector]
    public AudioSource source;
}
