using UnityEngine;

public class OutfitAutoApplier : MonoBehaviour
{
    public OutfitLibrary outfitLibrary;

    void Start()
    {
        var player = GameObject.FindGameObjectWithTag("Player");
        if (player == null || outfitLibrary == null)
        {
            Debug.LogWarning("Player o OutfitLibrary no encontrados.");
            return;
        }

        foreach (var outfit in outfitLibrary.outfits)
        {
            string slotKey = outfit.slotKey;

            // Verifica si el jugador tiene un mesh equipado en ese slot
            string equippedMeshName = GameManager.GetEquippedMesh(slotKey);
            if (string.IsNullOrEmpty(equippedMeshName)) continue;

            // Obtener transform del slot (ej. "Character/hat")
            Transform slotTransform = player.transform.Find("Character/" + slotKey);
            if (slotTransform == null)
            {
                Debug.LogWarning($"No se encontró el slot '{slotKey}' en el jugador.");
                continue;
            }

            var smr = slotTransform.GetComponent<SkinnedMeshRenderer>();
            if (smr == null)
            {
                Debug.LogWarning($"Slot '{slotKey}' no tiene un SkinnedMeshRenderer.");
                continue;
            }

            // Buscar el mesh desde la librería
            Mesh mesh = outfitLibrary.GetMesh(slotKey, equippedMeshName);
            if (mesh != null)
            {
                smr.sharedMesh = mesh;
                Debug.Log($"✔ Mesh '{equippedMeshName}' aplicado al slot '{slotKey}'");
            }
        }
    }
}