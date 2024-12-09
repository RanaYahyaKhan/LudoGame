using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class LoadingScreen : MonoBehaviour
{
   
    [SerializeField]
    private Slider loadingSlider;

    private void Start()
    {
        LoadScene(1);
    }
    public void LoadScene(int sceneNumber)
    {
        StartCoroutine(LoadSceneAsync(sceneNumber));
    }

    IEnumerator LoadSceneAsync(int sceneIndex)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex);

        while (!operation.isDone)
        {
            Debug.Log("Operation progress");

            float progress = Mathf.Clamp01(operation.progress / 0.9f);

            loadingSlider.value = progress;

            yield return new WaitForEndOfFrame();
        }
    }
}
