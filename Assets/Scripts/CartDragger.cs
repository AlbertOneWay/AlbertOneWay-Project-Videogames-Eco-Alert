using UnityEngine;

public class TrashCartDragger : MonoBehaviour
{
    [Header("Control de arrastre")]
    public float maxDragDistance = 3f;
    public float forceMultiplier = 150f;
    public float magnetOffset = 2.5f;
    public LayerMask cartLayer;

    private Rigidbody draggedRb;
    private GameObject draggedCart;
    
    void Update()
    {
        if (Input.GetMouseButton(1))
        {
            if (draggedCart == null)
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out RaycastHit hit, maxDragDistance, cartLayer))
                {
                    if (hit.collider.CompareTag("TrashCart"))
                    {
                        draggedCart = hit.collider.gameObject;
                        draggedRb = draggedCart.GetComponent<Rigidbody>();

                        if (draggedRb != null && !draggedRb.isKinematic)
                        {
                            Debug.Log("Carro enganchado: " + draggedCart.name);
                        }
                        else
                        {
                            Debug.LogWarning("¡El carrito necesita un Rigidbody no cinemático!");
                            draggedCart = null;
                        }
                    }
                }
            }
            else if (draggedRb != null)
            {
                Vector3 targetPos = Camera.main.transform.position + Camera.main.transform.forward * magnetOffset;
                Vector3 direction = (targetPos - draggedCart.transform.position);

                Debug.DrawLine(draggedCart.transform.position, targetPos, Color.red);
                Debug.Log($"Aplicando fuerza: {direction.normalized * direction.magnitude * forceMultiplier * Time.deltaTime}");

                draggedRb.AddForce(direction.normalized * direction.magnitude * forceMultiplier * Time.deltaTime, ForceMode.Force);
            }

        }
        else if (draggedCart != null)
        {
            Debug.Log("Carro soltado.");
            draggedCart = null;
            draggedRb = null;
        }
    }
}
