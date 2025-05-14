using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneLoader : MonoBehaviour
{
    public static SceneLoader Instance;

    [Header("Pantalla de carga")]
    public GameObject loadingScreen;
    public Slider progressBar;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
        if (loadingScreen != null)
            loadingScreen.SetActive(false);
    }

    public void LoadSceneAsync(string sceneName)
    {
        StartCoroutine(LoadSceneCoroutine(sceneName));
    }

    private IEnumerator LoadSceneCoroutine(string sceneName)
    {
        // Mostrar pantalla de carga
        if (loadingScreen != null) loadingScreen.SetActive(true);

        // Cargar escena asíncrona
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);
        asyncLoad.allowSceneActivation = false;

        while (!asyncLoad.isDone)
        {
            float progress = Mathf.Clamp01(asyncLoad.progress / 0.9f);
            if (progressBar != null) progressBar.value = progress;

            if (asyncLoad.progress >= 0.9f)
            {
                yield return new WaitForSeconds(0.5f); // Espera opcional
                asyncLoad.allowSceneActivation = true;
            }

            yield return null;
        }

        // Esperar 1 frame para estabilizar escena
        yield return null;

        // Ocultar pantalla de carga
        if (loadingScreen != null) loadingScreen.SetActive(false);
    }

    public void ReturnToLobby()
    {
        GameManager.SetContamination(0); // o el valor que necesites reiniciar
        LoadSceneAsync("Lobby");
    }
}