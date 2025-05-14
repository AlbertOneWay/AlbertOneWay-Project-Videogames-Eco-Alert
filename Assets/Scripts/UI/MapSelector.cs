using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

[System.Serializable]
public class MapData
{
    public string mapName; // Nombre visible (opcional para UI)
    public string sceneName;
    [Range(0, 100)]
    public float contaminationLevel;
}

public class MapSelector : MonoBehaviour
{
    [Header("UI del menú de selección de mapa")]
    public GameObject mapSelectionMenu;
    
    [Header("Botones")]
    public Button closeButton;

    [Header("Datos de los mapas")]
    public MapData[] maps; // Array para definir los mapas en el inspector
    public Button[] mapButtons; // Uno por mapa, deben estar en el mismo orden

    [Header("Textos de contaminación por mapa")]
    public TextMeshProUGUI[] mapContaminationTexts; 

    private void Start()
    {
        mapSelectionMenu.SetActive(false);

        for (int i = 0; i < maps.Length && i < mapButtons.Length; i++)
        {
            int index = i; 
            mapButtons[i].onClick.AddListener(() => LoadMap(maps[index]));
        }
        
        for (int i = 0; i < maps.Length && i < mapContaminationTexts.Length; i++)
        {
            float cont = maps[i].contaminationLevel;
            if (mapContaminationTexts[i] != null)
            {
                mapContaminationTexts[i].text = $"Contaminación: {cont.ToString("F0")}%";
            }
        }

        if (closeButton != null)
            closeButton.onClick.AddListener(CloseMenu);
    }

    public void ShowMapMenu()
    {
        mapSelectionMenu.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void LoadMap(MapData data)
    {
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        GameManager.SetContamination(data.contaminationLevel);

        SceneLoader.Instance.LoadSceneAsync(data.sceneName); 
    }

    public void CloseMenu()
    {
        mapSelectionMenu.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}
