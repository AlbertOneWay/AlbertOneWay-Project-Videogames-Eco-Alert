using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class TrashDraggable : MonoBehaviour
{
    public TrashDataSO trashData;
    public TrashSortingMinigame minigame;

    private Vector3 originalPosition;
    private bool isDragging = false;

    void OnMouseDown()
    {
        originalPosition = transform.position;
        isDragging = true;
    }

    void OnMouseUp()
    {
        isDragging = false;

        // Desactivar temporalmente el collider del objeto arrastrado
        Collider2D myCollider = GetComponent<Collider2D>();
        if (myCollider != null)
            myCollider.enabled = false;

        // Raycast para detectar la zona
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.zero);

        // Reactivar el collider
        if (myCollider != null)
            myCollider.enabled = true;

        if (hit.collider != null)
        {
            TrashDropZone zone = hit.collider.GetComponent<TrashDropZone>();
            if (zone != null)
            {
                Debug.Log("Soltado sobre: " + hit.collider.name);

                bool correcto = (trashData.category == zone.category);

                // Llamar al minijuego con el resultado
                minigame.OnTrashDropped(trashData, zone.category);

                if (correcto)
                {
                    Destroy(gameObject); // Solo si acertó
                }
                else
                {
                    // Volver a la posición original si se equivocó
                    transform.position = originalPosition;
                }

                return;
            }
        }

        // Si no tocó ninguna zona válida, regresar al origen
        transform.position = originalPosition;
    }



    void Update()
    {
        if (isDragging)
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            transform.position = new Vector3(mousePos.x, mousePos.y, 0f);
        }
    }
}