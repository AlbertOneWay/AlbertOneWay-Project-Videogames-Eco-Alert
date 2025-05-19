using System;
using System.Collections.Generic;
using UnityEngine;

public static class GameManager
{
    private static float contaminationLevel = 0f;
    public static List<TrashDataSO> collectedTrash = new();
    public static int playerScore = 0;

    public static event Action<int> OnScoreChanged;

    // Upgrades comprados (ej: "speed_boost", "cart_capacity")
    private static HashSet<string> purchasedUpgrades = new();

    // Ropa equipada actual (por slot)
    private static Dictionary<string, string> equippedOutfits = new(); // slotKey -> meshName

    // -------------------------------
    // Sistema base
    // -------------------------------

    public static void SetContamination(float value) => contaminationLevel = Mathf.Clamp(value, 0f, 100f);
    public static float GetContamination() => contaminationLevel;

    public static void AddTrash(TrashDataSO trash) => collectedTrash.Add(trash);
    public static void ClearTrash() => collectedTrash.Clear();

    public static void AddScore(int amount)
    {
        playerScore += amount;
        OnScoreChanged?.Invoke(playerScore);
    }

    // -------------------------------
    // Upgrades
    // -------------------------------

    public static void AddUpgrade(string key) => purchasedUpgrades.Add(key);
    public static bool HasUpgrade(string key) => purchasedUpgrades.Contains(key);

    // -------------------------------
    // Outfits
    // -------------------------------

    public static void EquipOutfit(string slotKey, string meshName)
    {
        // Si ya había una prenda equipada en ese slot, eliminarla
        if (equippedOutfits.TryGetValue(slotKey, out string previousMesh))
        {
            purchasedUpgrades.Remove($"{slotKey}:{previousMesh}");
        }

        equippedOutfits[slotKey] = meshName;
        purchasedUpgrades.Add($"{slotKey}:{meshName}");
    }

    public static string GetEquippedMesh(string slotKey)
    {
        return equippedOutfits.TryGetValue(slotKey, out var meshName) ? meshName : null;
    }

    public static bool HasOutfit(string slotKey, string meshName)
    {
        return purchasedUpgrades.Contains($"{slotKey}:{meshName}");
    }

    public static void UnequipOutfit(string slotKey)
    {
        if (equippedOutfits.TryGetValue(slotKey, out string prevMesh))
        {
            purchasedUpgrades.Remove($"{slotKey}:{prevMesh}");
            equippedOutfits.Remove(slotKey);
        }
    }
}
