using StarterAssets;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[System.Serializable]
public class ShopItem
{
    public string name;
    public int cost;
    public Sprite icon;

    [Tooltip("Clave interna usada para upgrades o tipo de ropa (ej: speed_boost, hat, shoes)")]
    public string upgradeKey;
    public string meshName; // Para ropa: nombre del mesh a aplicar
}

public class ShopMenu : MonoBehaviour
{
    [Header("Referencias UI")]
    public GameObject shopCanvas;
    public Transform itemContainer;
    public GameObject itemButtonPrefab;
    public Button closeButton;

    [Header("Botones de categoría")]
    public Button upgradesButton;
    public Button outfitsButton;

    [Header("Datos")]
    public ShopItem[] upgradeItems;
    public ShopItem[] outfitItems;
    public OutfitLibrary outfitLibrary;

    private void Start()
    {
        shopCanvas.SetActive(false);

        upgradesButton.onClick.AddListener(ShowUpgrades);
        outfitsButton.onClick.AddListener(ShowOutfits);
        closeButton.onClick.AddListener(CloseShop);
    }

    public void OpenShop()
    {
        shopCanvas.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        ShowUpgrades();
    }

    public void CloseShop()
    {
        shopCanvas.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void ShowUpgrades() => PopulateItems(upgradeItems, isOutfit: false);
    public void ShowOutfits() => PopulateItems(outfitItems, isOutfit: true);

    private void PopulateItems(ShopItem[] items, bool isOutfit)
    {
        ClearItems();

        foreach (ShopItem item in items)
        {
            GameObject buttonGO = Instantiate(itemButtonPrefab, itemContainer);
            TMP_Text nameText = buttonGO.transform.Find("ItemName").GetComponent<TMP_Text>();
            TMP_Text costText = buttonGO.transform.Find("ItemCost").GetComponent<TMP_Text>();
            Image iconImage = buttonGO.transform.Find("Sprite").GetComponent<Image>();
            Button buyButton = buttonGO.transform.Find("Button").GetComponent<Button>();
            TMP_Text buyButtonText = buyButton.GetComponentInChildren<TMP_Text>();

            nameText.text = item.name;
            costText.text = item.cost + " pts";
            if (iconImage && item.icon) iconImage.sprite = item.icon;

            bool alreadyBought = isOutfit
                ? GameManager.HasOutfit(item.upgradeKey, item.meshName)
                : GameManager.HasUpgrade(item.upgradeKey);

            if (alreadyBought)
            {
                buyButtonText.text = "Comprado";
                buyButton.interactable = false;
            }
            else
            {
                buyButtonText.text = "Comprar";
                buyButton.onClick.AddListener(() =>
                {
                    if (GameManager.playerScore >= item.cost)
                    {
                        GameManager.AddScore(-item.cost);

                        if (isOutfit)
                        {
                            GameManager.UnequipOutfit(item.upgradeKey); // desactiva anterior
                            GameManager.EquipOutfit(item.upgradeKey, item.meshName);
                            ApplyOutfit(item.upgradeKey, item.meshName);
                        }
                        else
                        {
                            GameManager.AddUpgrade(item.upgradeKey);
                            ApplyUpgrade(item.upgradeKey);
                        }

                        Debug.Log($"Compraste: {item.name}");

                        buyButtonText.text = "Comprado";
                        buyButton.interactable = false;

                        // Refrescar la tienda para reactivar otras opciones del mismo slot
                        PopulateItems(items, isOutfit);
                    }
                    else
                    {
                        NotificationManager.Instance.ShowMessage("No tienes puntos suficientes", NotificationManager.NotificationType.Error);
                    }
                });
            }
        }
    }

    private void ApplyUpgrade(string upgradeKey)
    {
        switch (upgradeKey)
        {
            case "speed_boost":
                var player = FindAnyObjectByType<ThirdPersonController>();
                if (player != null)
                {
                    player.MoveSpeed *= 1.5f;
                    player.SprintSpeed *= 1.5f;
                }
                break;

            case "cart_capacity":
                var cart = FindAnyObjectByType<TrashCart>();
                if (cart != null)
                {
                    cart.maxCapacity += 5;
                }
                break;

            // Más mejoras aquí...
        }
    }

    private void ApplyOutfit(string slotKey, string meshName)
    {
        Mesh meshToApply = outfitLibrary.GetMesh(slotKey, meshName);
        if (meshToApply == null)
        {
            Debug.LogWarning($"Mesh '{meshName}' no encontrado en slot '{slotKey}'");
            return;
        }

        var player = GameObject.FindGameObjectWithTag("Player");
        if (player == null)
        {
            Debug.LogError("No se encontró el jugador.");
            return;
        }

        Transform part = player.transform.Find("Character/" + slotKey);
        if (part == null)
        {
            Debug.LogError("No se encontró el slot de ropa: " + slotKey);
            return;
        }

        var smr = part.GetComponent<SkinnedMeshRenderer>();
        if (smr == null)
        {
            Debug.LogError("No hay SkinnedMeshRenderer en " + slotKey);
            return;
        }

        smr.sharedMesh = meshToApply;
        Debug.Log($"Aplicada vestimenta: {meshName} en slot {slotKey}");
    }

    private void ClearItems()
    {
        foreach (Transform child in itemContainer)
        {
            Destroy(child.gameObject);
        }
    }
}


