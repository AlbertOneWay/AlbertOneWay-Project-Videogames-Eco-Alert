using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TrashSeparationUI : MonoBehaviour
{
    public GameObject panel;

    [Header("Vista de basura")]
    public TextMeshProUGUI trashNameText;
    public Image trashIcon;

    [Header("Botones de clasificación")]
    public Button whiteButton;
    public Button greenButton;
    public Button blackButton;

    private TrashDataSO currentTrash;
    private int currentIndex = 0;

    public void Open()
    {
        panel.SetActive(true);
        currentIndex = 0;
        ShowNextTrash();
    }

    void ShowNextTrash()
    {
        if (currentIndex >= GameManager.collectedTrash.Count)
        {
            trashNameText.text = "¡Clasificación completada!";
            trashIcon.gameObject.SetActive(false);
            GameManager.ClearTrash();
            return;
        }

        currentTrash = GameManager.collectedTrash[currentIndex];
        trashNameText.text = currentTrash.trashName;
        trashIcon.sprite = currentTrash.trashIcon;
        trashIcon.gameObject.SetActive(true);

        whiteButton.onClick.RemoveAllListeners();
        whiteButton.onClick.AddListener(() => CheckAnswer(TrashCategory.Aprovechables));

        greenButton.onClick.RemoveAllListeners();
        greenButton.onClick.AddListener(() => CheckAnswer(TrashCategory.Organicos));

        blackButton.onClick.RemoveAllListeners();
        blackButton.onClick.AddListener(() => CheckAnswer(TrashCategory.NoAprovechables));
    }

    void CheckAnswer(TrashCategory selected)
    {
        if (selected == currentTrash.category)
        {
            GameManager.AddScore(10);
        }

        currentIndex++;
        ShowNextTrash();
    }

    public void Close()
    {
        panel.SetActive(false);
    }
}