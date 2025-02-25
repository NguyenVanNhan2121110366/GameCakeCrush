using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CheckDevice : MonoBehaviour
{
    private CanvasScaler canvasBar;
    private CanvasScaler canvasUI;

    void Awake()
    {
        if (canvasBar == null) canvasBar = GameObject.Find("CanvasBar").GetComponent<CanvasScaler>();
        else Debug.Log("Canvas was exist");
        if (canvasUI == null) canvasUI = GameObject.Find("CanvasUI").GetComponent<CanvasScaler>();
        else Debug.Log("Canvas was exist");
    }
    // Start is called before the first frame update
    void Start()
    {
        this.CheckAndUpdate();
    }

    private void CheckAndUpdate()
    {
        var phoneAspect = (float)1080 / 1920;
        var curentAspect = (float)Screen.width / Screen.height;
        if (phoneAspect < curentAspect)
        {
            canvasBar.matchWidthOrHeight = 0.5f;
            canvasUI.matchWidthOrHeight = 0.4f;
            Camera.main.orthographicSize = 5.2f;
        }
        else
        {
            canvasBar.matchWidthOrHeight = 0.6f;
            canvasUI.matchWidthOrHeight = 1f;
            Camera.main.orthographicSize = 6.5f;
        }
    }


}
