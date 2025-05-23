using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UIElements;
using UnityEngine.UI;
using Button = UnityEngine.UI.Button;
using Cursor = UnityEngine.Cursor; // Asegúrate de importar si usas TextMeshPro

public class TrashSortingMinigame : MonoBehaviour
{
    [Header("Posición donde aparece la basura")]
    public Transform spawnPoint;

    [Header("Prefab de sprite de basura")]
    public GameObject trashSpritePrefab;

    [Header("Zonas de clasificación")]
    public TrashDropZone zoneAprovechables;
    public TrashDropZone zoneOrganicos;
    public TrashDropZone zoneNoAprovechables;

    [Header("UI")]
    public TextMeshProUGUI trashNameText; // arrástralo desde la escena

    [Header("UI de resultados")]
    public GameObject resultPanel;
    public TextMeshProUGUI resultText;
    public Button returnLobby;
    
    private List<TrashDataSO> trashList;
    private GameObject currentTrashInstance;
    
    [Header("UI Feedback")]
    public GameObject floatingPointsPrefab;
    public Transform floatingCanvasParent;

    private void Awake()
    {
        if (returnLobby != null)
        {
            returnLobby.onClick.AddListener(ReturnToLobby);
        }
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        trashList = GameManager.collectedTrash;

        if (trashList.Count == 0)
        {
            Debug.LogWarning("No hay basura para clasificar.");
            return;
        }

        SpawnNextTrash();
    }

    void SpawnNextTrash()
    {
        if (GameManager.collectedTrash.Count == 0)
        {
            Debug.Log("✅ Clasificación completa.");
            if (trashNameText != null)
                trashNameText.text = "Clasificación completada 🎉";
            if (resultPanel != null)
            {
                resultPanel.SetActive(true);
                resultText.text = $"¡Buen trabajo!\nPuntaje final: {GameManager.playerScore} puntos";
            }
            return;
        }

        TrashDataSO data = GameManager.collectedTrash[0]; 

        currentTrashInstance = Instantiate(trashSpritePrefab, spawnPoint.position, Quaternion.identity);
        TrashDraggable drag = currentTrashInstance.GetComponent<TrashDraggable>();
        drag.trashData = data;
        drag.minigame = this;

        SpriteRenderer sr = currentTrashInstance.GetComponent<SpriteRenderer>();
        if (sr != null && data.trashIcon != null)
            sr.sprite = data.trashIcon;

        if (trashNameText != null)
            trashNameText.text = $"Basura: {data.trashName}";
    }


    public void OnTrashDropped(TrashDataSO trash, TrashCategory dropZone)
    {
        if (trash.category == dropZone)
        {
            GameManager.AddScore(10);
            ShowFloatingPoints(10); // ✅ Animación positiva
            GameManager.collectedTrash.Remove(trash);
            StartCoroutine(NextTrashWithDelay(0.3f));
        }
        else
        {
            GameManager.AddScore(-10);
            ShowFloatingPoints(-10); // ❌ Animación negativa
        }
    }

    private System.Collections.IEnumerator NextTrashWithDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        if (currentTrashInstance != null)
            Destroy(currentTrashInstance);

        SpawnNextTrash();
    }
    
    public void ReturnToLobby()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        SceneLoader.Instance.LoadSceneAsync("Lobby");
    }
    
    public void ShowFloatingPoints(int amount)
    {
        GameObject go = Instantiate(floatingPointsPrefab, floatingCanvasParent);
        go.transform.localPosition = Vector3.zero;

        FloatingPoints feedback = go.GetComponent<FloatingPoints>();
        feedback.Initialize(amount);
    }
}
