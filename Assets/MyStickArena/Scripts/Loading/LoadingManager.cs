using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingManager : MonoBehaviour
{
    private Image imgLoadingBar;

    void Awake()
    {
        if (imgLoadingBar == null) imgLoadingBar = GameObject.Find("LoadingBar").GetComponent<Image>();
        else Debug.Log("imgLoadingBar was exist");
    }
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(UpdateLoadingBar(PlayerPrefs.GetString("SceneName", "GameScene")));
    }

    private IEnumerator UpdateLoadingBar(string sceneName)
    {
        AsyncOperation async = SceneManager.LoadSceneAsync(sceneName);
        var currentProgress = 0f;
        async.allowSceneActivation = false;
        while (!async.isDone)
        {
            var progress = Mathf.Clamp01(async.progress / 0.9f);
            currentProgress = Mathf.Lerp(currentProgress, progress, 1.5f * Time.deltaTime);
            imgLoadingBar.fillAmount = currentProgress;
            yield return null;
            if (currentProgress >= 0.99f)
            {
                async.allowSceneActivation = true;
            }
        }
    }
}
