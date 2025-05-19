using UnityEngine;

public class TrashCart : MonoBehaviour
{
    [Header("Capacidad del carrito")]
    public int maxCapacity = 10;

    private int currentTrashCount = 0;
    
    void Start()
    {
        if (GameManager.HasUpgrade("cart_capacity"))
        {
            maxCapacity += 5;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        TrashItem trash = other.GetComponent<TrashItem>();
        if (trash != null)
        {
            if (currentTrashCount < maxCapacity)
            {
                GameManager.AddTrash(trash.trashInfo);
                currentTrashCount++;
                Destroy(trash.gameObject);
                Debug.Log("Basura recogida. Total: " + currentTrashCount + "/" + maxCapacity);
            }
            else
            {
                NotificationManager.Instance.ShowMessage("El carrito está lleno. No se puede recoger más basura.", NotificationManager.NotificationType.Error);
                // Aquí podrías reproducir un sonido o mostrar un mensaje visual
            }
        }
    }

    public int GetCurrentCount() => currentTrashCount;

    public void EmptyCart()
    {
        currentTrashCount = 0;
        GameManager.ClearTrash();
        Debug.Log("Carrito vaciado.");
    }

    public bool IsFull() => currentTrashCount >= maxCapacity;
}