using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "OutfitLibrary", menuName = "Game/Outfit Library")]
public class OutfitLibrary : ScriptableObject
{
    [System.Serializable]
    public class OutfitEntry
    {
        public string slotKey;       // Ej: "hat", "pants", "shoes"
        public string meshName;      // Ej: "Shoes_04"
        public Mesh mesh;            // Referencia al mesh real
    }

    public List<OutfitEntry> outfits;

    public Mesh GetMesh(string slotKey, string meshName)
    {
        foreach (var entry in outfits)
        {
            if (entry.slotKey == slotKey && entry.meshName == meshName)
                return entry.mesh;
        }
        return null;
    }
}