using UnityEngine;

public class ItemGiver : MonoBehaviour
{
    public string itemName;  // Should match the name of the image in Resources/ClueImages/

    private bool itemGiven = false;

    public void TryGiveItem()
    {
        if (itemGiven)
        {
            Debug.Log("Item already given from this object.");
            return;
        }

        if (!string.IsNullOrEmpty(itemName))
        {
            DialogueManager.Instance.CollectItem(itemName);
            itemGiven = true;
        }
        else
        {
            Debug.LogWarning("Item name is not set on ItemGiver.");
        }
    }
}
