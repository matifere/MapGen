using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneLoadManager : MonoBehaviour
{
    [SerializeField] private Slider slider;
    [SerializeField] private GameObject loadPanel;

    private float loadTime = 2f; 

    public void SceneLoad(int sceneIndex)
    {
        loadPanel.SetActive(true);
        StartCoroutine(LoadAsync(sceneIndex));
    }

    IEnumerator LoadAsync(int sceneIndex)
    {
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneIndex);
        asyncOperation.allowSceneActivation = false;

        float timer = 0f;

        while (timer < loadTime)
        {
            timer += Time.deltaTime;

            float progress = Mathf.Clamp01(timer / loadTime);
            Debug.Log(progress);

            float smoothValue = Mathf.Lerp(slider.value, progress, Time.deltaTime * 5f);
            slider.value = smoothValue;

            yield return null;
        }

        
        asyncOperation.allowSceneActivation = true;
    }
}
