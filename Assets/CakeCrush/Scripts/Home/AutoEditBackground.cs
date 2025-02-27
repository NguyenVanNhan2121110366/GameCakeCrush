using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AutoEditBackground : MonoBehaviour
{
    [SerializeField] private SpriteRenderer[] sprBackground;
    [SerializeField] private GameObject[] objBackground;
    // Start is called before the first frame update
    void Start()
    {
        this.CheckAndEdit();
    }

    private void CheckAndEdit()
    {
        var heightScreen = Camera.main.orthographicSize * 2;
        var widthScreen = heightScreen * ((float)Screen.width / Screen.height);
        for (var i = 0; i < objBackground.Length; i++)
        {
            var scale = new Vector3(widthScreen / sprBackground[i].bounds.size.x, heightScreen / sprBackground[i].bounds.size.y, 1);
            objBackground[i].transform.localScale = scale;
        }
    }
}
