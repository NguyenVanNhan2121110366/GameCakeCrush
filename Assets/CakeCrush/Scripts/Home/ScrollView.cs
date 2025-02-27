using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
public class ScrollView : MonoBehaviour
{
    private RawImage rawImage;
    private float posY;

    void Awake()
    {
        if (rawImage == null) rawImage = GameObject.Find("ScrollBgr").GetComponent<RawImage>();
        else Debug.Log("rawImage was exist");
    }
    // Start is called before the first frame update
    void Start()
    {
        posY = 0.008f;
        this.CheckInit();
    }

    // Update is called once per frame
    void Update()
    {
        rawImage.uvRect = new Rect(rawImage.uvRect.position + new Vector2(0, posY) * 0.8f * Time.deltaTime, rawImage.uvRect.size);
        if (rawImage.uvRect.position.y >= 0.03f)
        {
            posY = -0.008f;
        }
        else if (rawImage.uvRect.position.y <= -0.01f)
        {
            posY = 0.008f;
        }
    }

    private void CheckInit()
    {
        Debug.Log(rawImage.GetComponent<RectTransform>().rect.size.x);
    }
}
