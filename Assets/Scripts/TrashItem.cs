using UnityEngine;
using System;

public class TrashItem : MonoBehaviour
{
    public TrashDataSO trashInfo;

    public static event Action<TrashItem> OnTrashDestroyed;

    public void Collect()
    {
        GameManager.AddTrash(trashInfo);

        OnTrashDestroyed?.Invoke(this); // Notifica destrucción
        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        OnTrashDestroyed?.Invoke(this); // Garantiza notificación incluso si no es por Collect()
    }
}