using UnityEngine;

public class TrashNPCSpawner : MonoBehaviour
{
    [Header("NPC Basurero")]
    public GameObject npcPrefab;

    [Header("Puntos donde puede aparecer")]
    public Transform[] spawnPoints;

    [Header("Puntos de patrulla que se asignan al NPC")]
    public Transform[] patrolPoints;

    [Header("Tiempo entre apariciones (segundos)")]
    public float minSpawnInterval = 30f;
    public float maxSpawnInterval = 60f;

    private void Start()
    {
        StartCoroutine(SpawnLoop());
    }

    private System.Collections.IEnumerator SpawnLoop()
    {
        while (true)
        {
            float waitTime = Random.Range(minSpawnInterval, maxSpawnInterval);
            yield return new WaitForSeconds(waitTime);

            // Elegir punto aleatorio
            int index = Random.Range(0, spawnPoints.Length);
            Transform spawnPoint = spawnPoints[index];

            // Instanciar NPC
            GameObject npc = Instantiate(npcPrefab, spawnPoint.position, spawnPoint.rotation);

            // Asignar puntos de patrulla al NPC
            TrashDropperAI_Navmesh ai = npc.GetComponent<TrashDropperAI_Navmesh>();
            if (ai != null)
            {
                ai.patrolPoints = patrolPoints;
            }
        }
    }
}