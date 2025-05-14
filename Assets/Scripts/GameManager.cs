using System;
using System.Collections.Generic;
using UnityEngine;

public static class GameManager
{
    private static float contaminationLevel = 0f;
    public static List<TrashDataSO> collectedTrash = new List<TrashDataSO>();
    public static int playerScore = 0;
    
    public static event Action<int> OnScoreChanged;

    public static void SetContamination(float value)
    {
        contaminationLevel = Mathf.Clamp(value, 0f, 100f);
    }

    public static float GetContamination() => contaminationLevel;

    public static void AddTrash(TrashDataSO trash) => collectedTrash.Add(trash);

    public static void AddScore(int amount)
    {
        playerScore += amount;
        OnScoreChanged?.Invoke(playerScore);
    }

    public static void ClearTrash() => collectedTrash.Clear();
}