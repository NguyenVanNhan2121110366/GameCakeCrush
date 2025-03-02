using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicBgr : MonoBehaviour
{
    [SerializeField] private AudioSource bgrMusic;
    public AudioSource BgrMusic { get => bgrMusic; set => bgrMusic = value; }
    public static MusicBgr Instance;
    private void Awake()
    {
        if (Instance == null) Instance = this; else Destroy(gameObject);
        if (bgrMusic == null) bgrMusic = GetComponent<AudioSource>();
        DontDestroyOnLoad(gameObject);
    }

    public void VolumeBgrMusic(float value)
    {
        bgrMusic.volume = value;
        PlayerPrefs.SetFloat("MusicVolume", value);
        PlayerPrefs.Save();
    }

}
