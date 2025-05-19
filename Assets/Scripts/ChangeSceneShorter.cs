using UnityEngine;

public class ChangeSceneShorter : MonoBehaviour
{
    public void ChangeScenetoShorter()
    {
        if (GameManager.collectedTrash != null && GameManager.collectedTrash.Count > 0)
        {
            SceneLoader.Instance.LoadSceneAsync("TrashSortingMinigame");
        }
        else
        {
            NotificationManager.Instance.ShowMessage("No hay basura para clasificar", NotificationManager.NotificationType.Error);
            // Aquí puedes mostrar un mensaje en pantalla si lo deseas
        }
    }
}