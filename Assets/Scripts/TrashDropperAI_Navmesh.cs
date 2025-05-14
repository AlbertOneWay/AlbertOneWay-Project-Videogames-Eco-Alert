using UnityEngine;
using UnityEngine.AI;

public class TrashDropperAI_Navmesh : MonoBehaviour
{
    public float trashDropInterval = 1.5f;
    public GameObject trashPrefab;
    public Transform[] patrolPoints;
    public int hitsToEscape = 2;

    private NavMeshAgent agent;
    private float dropTimer = 0f;
    private int hitsReceived = 0;
    private int currentPoint = -1;
    private Animator animator;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        GoToNextPoint();
    }

    void Update()
    {
        dropTimer += Time.deltaTime;
        if (dropTimer >= trashDropInterval)
        {
            dropTimer = 0f;
            Instantiate(trashPrefab, transform.position, Quaternion.identity);
        }

        // Si llegó al destino, va al siguiente punto aleatorio
        if (!agent.pathPending && agent.remainingDistance < 0.5f)
        {
            GoToNextPoint();
        }
        
        // Animación basada en dirección local de la velocidad
        if (animator != null && agent != null)
        {
            Vector3 velocity = agent.velocity;
            Vector3 localVelocity = transform.InverseTransformDirection(velocity);
            float hor = localVelocity.x;
            float vert = localVelocity.z;

            animator.SetFloat("Hor", hor);
            animator.SetFloat("Vert", vert);
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

    public void TakeHit()
    {
        hitsReceived++;
        if (hitsReceived >= hitsToEscape)
        {
            Debug.Log("NPC escapó tras ser golpeado");
            Destroy(gameObject);
        }
    }
}