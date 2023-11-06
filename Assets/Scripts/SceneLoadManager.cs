using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;



public class SceneLoadManager : MonoBehaviour
{
    [SerializeField] private Slider slider;
    [SerializeField] private GameObject loadPanel;

    public void SceneLoad(int sceneIndex)
    {
        loadPanel.SetActive(true);
        StartCoroutine(LoadAsync(sceneIndex));
    }
    IEnumerator LoadAsync(int sceneIndex)
    {
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneIndex);
        while (!asyncOperation.isDone)
        {
            Debug.Log(asyncOperation.progress);
            slider.value = asyncOperation.progress;
            yield return null;
        }
    }
}
