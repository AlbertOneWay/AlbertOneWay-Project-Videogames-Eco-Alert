using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Cinemachine;

public class SceneLoader : MonoBehaviour
{
    public static SceneLoader Instance;

    [Header("Cámaras")]
    public Camera mainCamera;
    public CinemachineVirtualCamera virtualCamera;

    [Header("Pantalla de carga")]
    public GameObject loadingScreen;
    public Slider progressBar;

    private string spawnPointName = "DefaultSpawn";

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

    public void LoadSceneAsync(string sceneName, string spawnPoint = "DefaultSpawn")
    {
        spawnPointName = spawnPoint;
        StartCoroutine(LoadSceneCoroutine(sceneName));
    }

    private IEnumerator LoadSceneCoroutine(string sceneName)
    {
        // Mostrar pantalla de carga
        if (loadingScreen != null) loadingScreen.SetActive(true);

        // Desactivar cámaras antes de cargar
        if (mainCamera != null) mainCamera.gameObject.SetActive(false);
        if (virtualCamera != null) virtualCamera.gameObject.SetActive(false);

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

        // Esperar 1 frame para que la escena se estabilice
        yield return null;

        // Teletransportar
        SpawnPlayerAtPoint();

        // Activar cámaras de nuevo
        if (mainCamera != null) mainCamera.gameObject.SetActive(true);
        if (virtualCamera != null) virtualCamera.gameObject.SetActive(true);

        // Ocultar loading
        if (loadingScreen != null) loadingScreen.SetActive(false);
    }

    private void SpawnPlayerAtPoint()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player == null) return;

        GameObject spawnPoint = GameObject.Find(spawnPointName);
        if (spawnPoint == null) return;

        player.transform.position = spawnPoint.transform.position;
        player.transform.rotation = spawnPoint.transform.rotation;
    }
    
    public void ReturnToLobby()
    {
        // Opcional: podés resetear el nivel de contaminación si querés
        if (GameManager.Instance != null)
        {
            GameManager.Instance.SetContamination(0); // o el valor que tenga el lobby
        }

        LoadSceneAsync("Lobby", "Lobby_Spawn");
    }
}
