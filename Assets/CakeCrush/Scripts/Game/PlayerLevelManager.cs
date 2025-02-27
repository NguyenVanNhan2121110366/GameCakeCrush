using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerLevelManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI txtLevel;
    private int level;
    public static PlayerLevelManager Instance;
    public int Level { get => level; set => level = value; }
    public TextMeshProUGUI TxtLevel { get => txtLevel; set => txtLevel = value; }

    private void Awake()
    {
        if (Instance == null) Instance = this; else Destroy(gameObject);
        if (txtLevel == null) txtLevel = GameObject.Find("txtLevelPlayer").GetComponent<TextMeshProUGUI>();
        this.GetValueLevel();
    }

    // Start is called before the first frame update
    void Start()
    {
        ObServerManager.AddObServer("UpdateScoreAfterRestart", GetValueLevel);
        this.TextLevel();
    }

    private void GetValueLevel()
    {
        level = PlayerPrefs.GetInt("CurrentLevel", 1);
    }

    private void TextLevel()
    {
        txtLevel.text = "Level " + level;
    }

    void OnDestroy()
    {
        ObServerManager.RemoveObServer("UpdateScoreAfterRestart", GetValueLevel);
    }


}
