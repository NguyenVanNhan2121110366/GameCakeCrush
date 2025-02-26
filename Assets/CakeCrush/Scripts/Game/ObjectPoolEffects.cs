using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolEffects : MonoBehaviour
{
    private static ObjectPoolEffects instance;
    [SerializeField] private GameObject[] allEffects = new GameObject[5];
    private Dictionary<string, Queue<GameObject>> effects = new();
    private Transform parentEffects;
    public static ObjectPoolEffects Instance { get => instance; set => instance = value; }
    public Dictionary<string, Queue<GameObject>> Effects { get => effects; set => effects = value; }

    void Awake()
    {
        if (instance == null) instance = this; else Destroy(gameObject);
        if (parentEffects == null) parentEffects = GameObject.Find("Effects").transform;
    }
    // Start is called before the first frame update
    void Start()
    {
        this.AddEffectIntoArray();
        this.SpawnPoolEffects();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void AddEffectIntoArray()
    {
        for (var i = 0; i < parentEffects.childCount; i++)
        {
            allEffects[i] = parentEffects.GetChild(i).gameObject;
        }
    }

    private void SpawnPoolEffects()
    {
        for (var i = 0; i < allEffects.Length; i++)
        {
            var nameKey = "Cake" + (i + 1);
            effects[nameKey] = new Queue<GameObject>();
            for (var j = 0; j < 10; j++)
            {
                var obj = Instantiate(allEffects[i]);
                effects[nameKey].Enqueue(obj);
                obj.SetActive(false);
                obj.transform.parent = transform;
            }
        }
    }

    private GameObject GetEffects(string nameTag, Vector2 pos)
    {
        if (effects.ContainsKey(nameTag) && effects[nameTag].Count > 0)
        {
            var obj = effects[nameTag].Dequeue();
            obj.SetActive(true);
            obj.transform.position = pos;
            return obj;
        }
        else
            return null;
    }

    public void HandleDestroyEffects(string nameTag, Vector2 pos)
    {
        var obj = GetEffects(nameTag, pos);
        StartCoroutine(ReturnEffects(obj, nameTag));
    }

    private IEnumerator ReturnEffects(GameObject obj, string nameTag)
    {
        yield return new WaitForSeconds(0.5f);
        obj.SetActive(false);
        if (effects.ContainsKey(nameTag))
        {
            effects[nameTag].Enqueue(obj);
        }
    }




}
