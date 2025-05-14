using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIPlayerManager : MonoBehaviour
{
    public TextMeshProUGUI scoreText;

    private void Start()
    {
        UpdateScoreUI();
    }

    private void OnEnable()
    {
        GameManager.OnScoreChanged += UpdateScoreUI;
    }

    private void OnDisable()
    {
        GameManager.OnScoreChanged -= UpdateScoreUI;
    }

    public void UpdateScoreUI(int newScore)
    {
        if (scoreText != null)
            scoreText.text = $"Puntaje: {newScore}";
    }

    public void UpdateScoreUI()
    {
        if (scoreText != null)
            scoreText.text = $"Puntaje: {GameManager.playerScore}";
    }
}