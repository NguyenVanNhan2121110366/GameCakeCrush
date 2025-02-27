using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class AbilityManager : MonoBehaviour
{
    private Button bntAbilityBomb;
    private GameObject fillAbility;
    private Image fillAbilityBomb;
    private float timeCoolDown;
    private float countTimeCoolDown;
    private bool isCheckClickAbility;
    public bool IsCheckClickAbility { get => isCheckClickAbility; set => isCheckClickAbility = value; }
    public static AbilityManager Instance;
    public float CountTimeCoolDown { get => countTimeCoolDown; set => countTimeCoolDown = value; }

    void Awake()
    {
        if (Instance == null) Instance = this; else Destroy(gameObject);
        if (fillAbility == null) fillAbility = GameObject.Find("FillAbility");
        else Debug.Log("fillAbility was exist");
        if (bntAbilityBomb == null) bntAbilityBomb = GameObject.Find("bntAbilityBom").GetComponent<Button>();
        else Debug.Log("bntAbilityBomb was exist");
        if (fillAbilityBomb == null) fillAbilityBomb = GameObject.Find("bgrFillAbilityBom").GetComponent<Image>();
        else Debug.Log("fillAbilityBomb was exist");
        bntAbilityBomb.onClick.AddListener(OnClickAbility);
    }
    // Start is called before the first frame update
    void Start()
    {
        ObServerManager.AddObServer("ClickResume", ObUpdateCountTimeCoolDown);
        ObServerManager.AddObServer("OnclickRestart", ObUpdateCountTimeCoolDown);
        ObServerManager.AddObServer("UpdateScoreAfterRestart", GetValueCountTimeCoolDown);
        isCheckClickAbility = false;
        timeCoolDown = 15f;
        this.GetValueCountTimeCoolDown();
        this.fillAbility.SetActive(false);
        StartCoroutine(this.UpdateCountTimeCoolDown());
    }

    private void GetValueCountTimeCoolDown()
    {
        countTimeCoolDown = timeCoolDown;
    }

    // Update is called once per frame
    void Update()
    {
        this.UpdateFill();
    }

    private void OnClickAbility()
    {
        GameStateController.Instance.CurrentGameState = GameState.ExcuteAbility;
        this.fillAbility.SetActive(true);
        isCheckClickAbility = true;
        GetOrderInLayer();
    }
    public void SkillWereUsed()
    {
        isCheckClickAbility = false;
        fillAbility.SetActive(false);
        countTimeCoolDown = timeCoolDown;
        fillAbilityBomb.gameObject.SetActive(true);
        StartCoroutine(this.UpdateCountTimeCoolDown());
        GetOrderInLayer();
    }

    private void GetOrderInLayer()
    {
        if (isCheckClickAbility)
        {
            this.SetOrderInLayer(6);
        }
        else
        {
            this.SetOrderInLayer(3);
        }
    }

    private void SetOrderInLayer(int layer)
    {
        for (var i = 0; i < GridController.Instance.Width; i++)
        {
            for (var j = 0; j < GridController.Instance.Height; j++)
            {
                GridController.Instance.AllDots[i, j].GetComponent<SpriteRenderer>().sortingOrder = layer;
            }
        }
    }

    private void UpdateFill()
    {
        fillAbilityBomb.fillAmount = Mathf.Lerp(fillAbilityBomb.fillAmount, countTimeCoolDown / timeCoolDown, 10 * Time.deltaTime);
    }

    private void ObUpdateCountTimeCoolDown()
    {
        StartCoroutine(UpdateCountTimeCoolDown());
    }


    public IEnumerator UpdateCountTimeCoolDown()
    {
        var isCheck = true;
        while (isCheck)
        {
            if (GameStateController.Instance.CurrentGameState != GameState.UI
            && GameStateController.Instance.CurrentGameState != GameState.GameOver
            && GameStateController.Instance.CurrentGameState != GameState.WinGame)
            {
                yield return new WaitForSeconds(1f);
                countTimeCoolDown--;
                if (countTimeCoolDown == 0)
                {
                    isCheck = false;
                    fillAbilityBomb.gameObject.SetActive(false);
                }
            }
            else
                isCheck = false;

        }
    }
    void OnDestroy()
    {
        ObServerManager.RemoveObServer("ClickResume", ObUpdateCountTimeCoolDown);
        ObServerManager.RemoveObServer("OnclickRestart", ObUpdateCountTimeCoolDown);
        ObServerManager.RemoveObServer("UpdateScoreAfterRestart", GetValueCountTimeCoolDown);
    }
}
