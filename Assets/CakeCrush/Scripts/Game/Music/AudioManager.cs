using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    private AudioSource audioSource;

    [SerializeField] AudioClip[] audioClips;
    public static AudioManager Instance;
    public AudioSource AudioSrc { get => audioSource; set => audioSource = value; }
    public AudioClip[] AudioClips { get => audioClips; set => audioClips = value; }

    void Awake()
    {
        if (Instance == null) Instance = this; else Destroy(gameObject);
        if (audioSource == null) audioSource = GetComponent<AudioSource>();
    }
    void Update()
    {
        //this.VolumeBgrMusic();
    }

    public void SoundDestroy()
    {
        audioSource.PlayOneShot(audioClips[0]);
    }

    public void SoundUpLevel()
    {
        audioSource.PlayOneShot(audioClips[1]);
    }

    public void SoundSpawnDot()
    {
        audioSource.PlayOneShot(audioClips[2]);
    }

    public void VolumeSound(float value)
    {
        audioSource.volume = value;
        PlayerPrefs.SetFloat("SoundVolume", value);
        PlayerPrefs.Save();
    }
}
