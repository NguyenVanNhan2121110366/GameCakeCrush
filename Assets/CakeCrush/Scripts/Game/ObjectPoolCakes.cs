using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolCakes : MonoBehaviour
{
    private static ObjectPoolCakes instance;
    [SerializeField] private GameObject[] allCakes = new GameObject[5];
    private Dictionary<string, Queue<GameObject>> cakes = new();
    private Transform parentCakes;
    public static ObjectPoolCakes Instance { get => instance; set => instance = value; }

    void Awake()
    {
        if (parentCakes == null) parentCakes = GameObject.Find("Dots").transform;
        if (instance == null) instance = this; else Destroy(gameObject);

    }
    // Start is called before the first frame update
    void Start()
    {
        this.AddCakeIntoArray();
        this.SpawnCakes();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void AddCakeIntoArray()
    {
        for (var i = 0; i < parentCakes.childCount; i++)
        {
            allCakes[i] = parentCakes.GetChild(i).gameObject;
        }
    }

    private void SpawnCakes()
    {
        for (var i = 0; i < allCakes.Length; i++)
        {
            var nameTag = "Cake" + (i + 1);
            cakes[nameTag] = new Queue<GameObject>();
            for (var j = 0; j < 10; j++)
            {
                var obj = Instantiate(allCakes[i]);
                obj.transform.parent = transform;
                obj.SetActive(false);
                cakes[nameTag].Enqueue(obj);
            }
        }
    }

    public GameObject GetCakes(string nameTag, Vector2 pos)
    {
        if (cakes.ContainsKey(nameTag) && cakes[nameTag].Count > 0)
        {
            var obj = cakes[nameTag].Dequeue();
            obj.SetActive(true);
            obj.transform.position = pos;
            return obj;
        }
        else
            return null;
    }

    public void HandleCakes(string nameTag, Vector2 pos)
    {
        var obj = GetCakes(nameTag, pos);
        //StartCoroutine(ReturnCakes(obj, nameTag));
    }

    public void ReturnCakes(GameObject obj, string nameTag)
    {
        obj.SetActive(false);
        if (cakes.ContainsKey(nameTag))
        {
            cakes[nameTag].Enqueue(obj);
        }
    }

}
