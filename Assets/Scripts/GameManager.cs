using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Range(0, 100)]
    private float contaminationLevel = 0f;
    
    public List<TrashDataSO> collectedTrash = new List<TrashDataSO>();
    public int playerScore = 0;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void SetContamination(float value)
    {
        contaminationLevel = Mathf.Clamp(value, 0f, 100f);
    }

    public float GetContamination()
    {
        return contaminationLevel;
    }
    
    public void AddTrash(TrashDataSO trash)
    {
        collectedTrash.Add(trash);
    }

    public void AddScore(int amount)
    {
        playerScore += amount;
    }

    public void ClearTrash()
    {
        collectedTrash.Clear();
    }
}