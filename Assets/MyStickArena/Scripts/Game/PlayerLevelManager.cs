using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerLevelManager : MonoBehaviour
{
    private static PlayerLevelManager instance;
    [SerializeField] private TextMeshProUGUI txtLevel;
    private int level;
    public static PlayerLevelManager Instance { get => instance; set => instance = value; }
    public int Level { get => level; set => level = value; }
    public TextMeshProUGUI TxtLevel { get => txtLevel; set => txtLevel = value; }

    private void Awake()
    {
        if (instance == null) instance = this; else Destroy(gameObject);
        if (txtLevel == null) txtLevel = GameObject.Find("txtLevelPlayer").GetComponent<TextMeshProUGUI>();
        level = PlayerPrefs.GetInt("CurrentLevel", 1);
    }

    // Start is called before the first frame update
    void Start()
    {
        this.TextLevel();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void TextLevel()
    {
        txtLevel.text = "Level " + level;
    }


}
