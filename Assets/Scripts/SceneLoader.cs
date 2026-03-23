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



    IEnumerator LoadSceneAsyncronously(string sceneName)
    {
        AudioManager.Instance.PlaySFX(buttonClip);

        loadingScreen.SetActive(true);
        loadingBar.value = 0;

  
        float timer = 0f;
        while (timer < 3f)
        {
            timer += Time.unscaledDeltaTime; 
            loadingBar.value = timer / 3f;
            yield return null;
        }

        loadingBar.value = 1f;

        Time.timeScale = 1f; 
        SceneManager.LoadScene(sceneName);
    }
}
