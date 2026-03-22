using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public GameObject loadingScreen;
    public Slider loadingBar;
    public AudioClip buttonClip;

    public void LoadScene(string sceneName)
    {
        StartCoroutine(LoadSceneAsyncronously(sceneName));
    }

    //IEnumerator LoadSceneAsyncronously(string sceneName)
    //{
    //    AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);
    //    while (!operation.isDone)
    //    {
    //        loadingScreen.SetActive(true);
    //        loadingBar.value = operation.progress;
    //        yield return null;
    //    }
    //}


    IEnumerator LoadSceneAsyncronously(string sceneName)
    {
        AudioManager.Instance.PlaySFX(buttonClip);

        loadingScreen.SetActive(true);
        loadingBar.value = 0;

        // Fake loading bar that fills over 3 seconds
        float timer = 0f;
        while (timer < 3f)
        {
            timer += Time.unscaledDeltaTime; // Use unscaledDeltaTime instead
            loadingBar.value = timer / 3f;
            yield return null;
        }

        loadingBar.value = 1f;

        Time.timeScale = 1f; 
        SceneManager.LoadScene(sceneName);
    }
}
