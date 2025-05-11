using UnityEngine;

public class ItemGiver : MonoBehaviour
{
    [Tooltip("Name of the image file in Resources/ClueImages/ (without extension)")]
    public string itemName;  // Example: "USB Stick" if the image is Resources/ClueImages/USB Stick.png

    private bool itemGiven = false;

    public void TryGiveItem()
    {
        if (itemGiven)
        {
            Debug.Log($"Item '{itemName}' already given from {gameObject.name}.");
            return;
        }

        if (string.IsNullOrEmpty(itemName))
        {
            Debug.LogWarning($"Item name is not set on {gameObject.name}.");
            return;
        }

        // Prepend the folder path to match Resources folder structure
        string fullResourcePath = $"ClueImages/{itemName}";

        // Pass both display name and resource path
        DialogueManager.Instance.CollectItem(itemName, fullResourcePath);
        itemGiven = true;
    }
}
