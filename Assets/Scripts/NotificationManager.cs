using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class NotificationManager : MonoBehaviour
{
    public static NotificationManager Instance;

    public GameObject notificationPrefab;
    public Transform notificationParent;
    
    public enum NotificationType { Info, Success, Error }

    public Sprite infoIcon;
    public Sprite successIcon;
    public Sprite errorIcon;

    [Header("Ajustes")]
    public float displayTime = 2f;
    public float fadeDuration = 0.5f;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void ShowMessage(string message, NotificationType type = NotificationType.Info)
    {
        GameObject go = Instantiate(notificationPrefab, notificationParent);
        go.transform.SetAsLastSibling();

        TextMeshProUGUI text = go.GetComponentInChildren<TextMeshProUGUI>();
        Image icon = go.transform.Find("Icon")?.GetComponent<Image>();
        CanvasGroup cg = go.GetComponent<CanvasGroup>();

        text.text = message;

        // Color según tipo
        switch (type)
        {
            case NotificationType.Info:
                text.color = Color.white;
                if (icon) icon.sprite = infoIcon;
                break;
            case NotificationType.Success:
                text.color = Color.green;
                if (icon) icon.sprite = successIcon;
                break;
            case NotificationType.Error:
                text.color = Color.red;
                if (icon) icon.sprite = errorIcon;
                break;
        }

        StartCoroutine(FadeAndDestroy(go, cg));
    }

    private IEnumerator FadeAndDestroy(GameObject go, CanvasGroup cg)
    {
        yield return new WaitForSeconds(displayTime);

        float elapsed = 0f;
        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            cg.alpha = 1f - (elapsed / fadeDuration);
            yield return null;
        }

        Destroy(go);
    }
}