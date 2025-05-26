using UnityEngine;
using UnityEngine.AI;

public class TrashDropperAI_Navmesh : MonoBehaviour
{
    [Header("Basura")]
    public GameObject[] trashPrefabs; // Prefabs disponibles
    public float trashDropInterval = 1.5f;

    [Header("Patrullaje")]
    public Transform[] patrolPoints;

    [Header("Escape")]
    public float timeToEscape = 10f; // Tiempo antes de desaparecer automáticamente

    private NavMeshAgent agent;
    private Animator animator;
    private float dropTimer = 0f;
    private int currentPoint = -1;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        GoToNextPoint();
        AudioManager.Instance.PlayMusic(MusicType.Enemy);
        
        StartCoroutine(EscapeAfterDelay()); // Inicia autodestrucción
    }

    void Update()
    {
        dropTimer += Time.deltaTime;
        if (dropTimer >= trashDropInterval)
        {
            dropTimer = 0f;
            DropRandomTrash();
        }

        // Avanzar si llegó al destino
        if (!agent.pathPending && agent.remainingDistance < 0.5f)
        {
            GoToNextPoint();
        }

        // Animación según dirección local de movimiento
        if (animator != null && agent != null)
        {
            Vector3 velocity = agent.velocity;
            Vector3 localVelocity = transform.InverseTransformDirection(velocity);
            animator.SetFloat("Hor", localVelocity.x);
            animator.SetFloat("Vert", localVelocity.z);
        }
    }

    void DropRandomTrash()
    {
        if (trashPrefabs.Length == 0)
        {
            Debug.LogWarning("❌ No hay prefabs de basura asignados al NPC.");
            return;
        }

        int index = Random.Range(0, trashPrefabs.Length);
        GameObject prefab = trashPrefabs[index];

        if (prefab != null)
        {
            Instantiate(prefab, transform.position, Quaternion.identity);
        }
        else
        {
            Debug.LogWarning($"⚠️ Prefab vacío en posición {index}.");
        }
    }

    void GoToNextPoint()
    {
        if (patrolPoints.Length == 0) return;

        int next;
        do
        {
            next = Random.Range(0, patrolPoints.Length);
        } while (next == currentPoint);

        currentPoint = next;
        agent.SetDestination(patrolPoints[currentPoint].position);
    }

    private System.Collections.IEnumerator EscapeAfterDelay()
    {
        yield return new WaitForSeconds(timeToEscape);
        Debug.Log("🕒 El NPC desaparece automáticamente tras " + timeToEscape + " segundos.");
        Destroy(gameObject);
        AudioManager.Instance.PlayRandomCalmMusic();
    }
}
