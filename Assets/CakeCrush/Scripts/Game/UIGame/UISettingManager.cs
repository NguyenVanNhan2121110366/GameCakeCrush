using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UISettingManager : ButtonUIManager
{
    private Slider sliderMusic;
    private Slider sliderSound;
    private Button bntResume;
    public static UISettingManager Instance;
    public Slider SliderMusic { get => sliderMusic; set => sliderMusic = value; }
    public Slider SliderSound { get => sliderSound; set => sliderSound = value; }
    private void Awake()
    {
        if (Instance == null) Instance = this; else Destroy(gameObject);
        if (bntResume == null) bntResume = GameObject.Find("Resume").GetComponent<Button>();
        else Debug.Log("bntResume was exist");
        if (bntHome == null) bntHome = GameObject.Find("HomeSetting").GetComponent<Button>();
        else Debug.Log("bntHome was exist");
        if (bntAgain == null) bntAgain = GameObject.Find("BntRestartSetting").GetComponent<Button>();
        else Debug.Log("bntRestart was exist");
        if (sliderMusic == null) sliderMusic = GameObject.Find("SliderMusic").GetComponent<Slider>();
        else Debug.Log("sliderMusic was exist");
        if (sliderSound == null) sliderSound = GameObject.Find("SliderSound").GetComponent<Slider>();
        else Debug.Log("sliderSound was exist");
        bntHome.onClick.AddListener(OnClickHome);
        bntResume.onClick.AddListener(OnClickResume);
        bntAgain.onClick.AddListener(OnclickRestart);
    }
    // Start is called before the first frame update
    void Start()
    {
        sliderMusic.onValueChanged.AddListener(MusicBgr.Instance.VolumeBgrMusic);
        sliderSound.onValueChanged.AddListener(AudioManager.Instance.VolumeSound);
        sliderMusic.value = PlayerPrefs.GetFloat("MusicVolume", 0.5f);
        sliderSound.value = PlayerPrefs.GetFloat("SoundVolume", 0.5f);
        PlayerPrefs.SetString("SceneName", "HomeScene");
        PlayerPrefs.Save();
    }

    protected override void OnClickHome()
    {
        SceneManager.LoadScene("LoadingScene");
    }

    protected override void OnClickResume()
    {
        base.OnClickResume();
        ObServerManager.Notifine("ClickResume");
    }

    protected override void OnclickRestart()
    {
        ObServerManager.Notifine("OnclickRestart");
    }



}
