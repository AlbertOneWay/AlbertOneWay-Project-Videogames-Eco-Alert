using UnityEngine;

public class PlayerPersistence : MonoBehaviour
{
    private static PlayerPersistence instance;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject); // Evita duplicados
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject); // Este jugador vive entre escenas
    }
}