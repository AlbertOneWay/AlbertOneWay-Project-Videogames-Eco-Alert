using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class FloatingPoints : MonoBehaviour
{
    public TextMeshProUGUI pointsText;
    public Image iconImage;
    public float floatDistance = 50f;
    public float duration = 1.2f;

    private RectTransform rect;
    private CanvasGroup canvasGroup;

    private void Awake()
    {
        rect = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public void Initialize(int points)
    {
        pointsText.text = (points > 0 ? "+" : "") + points;
        pointsText.color = points > 0 ? Color.green : Color.red;
        StartCoroutine(FloatUpAndFade());
    }

    private System.Collections.IEnumerator FloatUpAndFade()
    {
        Vector2 start = rect.anchoredPosition;
        Vector2 end = start + Vector2.up * floatDistance;

        float elapsed = 0f;
        while (elapsed < duration)
        {
            float t = elapsed / duration;
            rect.anchoredPosition = Vector2.Lerp(start, end, t);
            canvasGroup.alpha = 1 - t;
            elapsed += Time.deltaTime;
            yield return null;
        }

        Destroy(gameObject);
    }
}