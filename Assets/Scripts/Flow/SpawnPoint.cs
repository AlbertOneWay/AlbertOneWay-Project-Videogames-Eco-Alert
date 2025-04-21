using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    public string spawnID = "DefaultSpawn";

    private void OnValidate()
    {
        gameObject.name = spawnID;
    }
}