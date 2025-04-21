using UnityEngine;
using UnityEngine.Events;

public class InteractableTrigger : MonoBehaviour
{
    [Header("Key for interaction")]
    public KeyCode interactionKey = KeyCode.E;

    [Header("UI for prompt (World Space or Screen Space)")]
    public GameObject interactionUI;

    [Header("Event to invoke when interacting")]
    public UnityEvent onInteract;

    // Indica si el jugador está dentro del trigger
    private bool isPlayerInRange = false;

    private Camera mainCamera;

    private void Start()
    {
        if (interactionUI != null)
            interactionUI.SetActive(false);

        // Guardamos la referencia a la cámara principal
        mainCamera = Camera.main;
    }

    private void OnTriggerEnter(Collider other)
    {
        // Verificar si el objeto que entra tiene la etiqueta "Player"
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = true;
            
            // Mostrar el UI para indicar que se puede presionar la tecla
            if (interactionUI != null)
                interactionUI.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Cuando el Player sale del trigger
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = false;
            
            // Ocultar la UI al salir
            if (interactionUI != null)
                interactionUI.SetActive(false);
        }
    }

    private void Update()
    {
        // Si el jugador está dentro del trigger y presiona la tecla
        if (isPlayerInRange && Input.GetKeyDown(interactionKey))
        {
            onInteract.Invoke();
        }

        // Si el UI está activo, hacer que mire hacia la cámara
        if (interactionUI != null && interactionUI.activeSelf && mainCamera != null)
        {
            // Este enfoque rota el UI para mirar hacia la cámara, pero evita que se voltee
            interactionUI.transform.rotation = Quaternion.LookRotation(
                interactionUI.transform.position - mainCamera.transform.position
            );
        }
    }
}