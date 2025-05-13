using UnityEngine;

public class BigDrawer1 : MonoBehaviour
{
    void Start()
    {
        Debug.Log("Triggering dialogue...");
        // Show a test message when the script starts
        DialogueManager.Instance.ShowMessage("This is a test message.\nSecond line!");
    }
}
