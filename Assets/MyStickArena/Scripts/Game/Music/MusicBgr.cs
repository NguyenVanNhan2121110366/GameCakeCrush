using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicBgr : MonoBehaviour
{
    private static MusicBgr isntance;
    private AudioSource bgrMusic;
    public AudioSource BgrMusic { get => bgrMusic; set => bgrMusic = value; }
    public static MusicBgr Instance { get => isntance; set => isntance = value; }
    private void Awake()
    {
        if (isntance == null) isntance = this; else Destroy(gameObject);
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
