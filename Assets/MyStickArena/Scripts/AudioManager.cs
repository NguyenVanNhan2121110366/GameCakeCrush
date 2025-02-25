using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    private static AudioManager isntance;
    private AudioSource audioSource;
    private AudioSource bgrMusic;
    [SerializeField] AudioClip[] audioClips;
    public static AudioManager Instance { get => isntance; set => isntance = value; }
    public AudioSource AudioSrc { get => audioSource; set => audioSource = value; }
    public AudioClip[] AudioClips { get => audioClips; set => audioClips = value; }
    public AudioSource BgrMusic { get => bgrMusic; set => bgrMusic = value; }

    void Awake()
    {
        if (isntance == null) isntance = this; else Destroy(gameObject);
        if (audioSource == null) audioSource = GetComponent<AudioSource>();
        if (bgrMusic == null) bgrMusic = GameObject.Find("Main Camera").GetComponent<AudioSource>();
    }
    void Update()
    {
        //this.VolumeBgrMusic();
    }

    public void VolumeBgrMusic(float value)
    {
        bgrMusic.volume = value;
        PlayerPrefs.SetFloat("MusicVolume", value);
        PlayerPrefs.Save();
    }

    public void VolumeSound(float value)
    {
        audioSource.volume = value;
        PlayerPrefs.SetFloat("SoundVolume", value);
        PlayerPrefs.Save();
    }
}
