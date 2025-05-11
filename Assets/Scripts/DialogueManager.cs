using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance;

    public GameObject dialoguePanel;
    public TMP_Text dialogueText;
    public float typingSpeed = 0.03f;

    // New Item UI panel
    public GameObject itemPanel;  // New panel for displaying item received
    public TMP_Text itemReceivedText;  // Text to show item name
    public Image itemReceivedImage;    // Image to show item icon

    private string[] dialogueLines;
    private int currentLineIndex;
    private bool isTyping = false;
    private bool lineFullyDisplayed = false;

    private string[] inventory = new string[5];  // Inventory to hold up to 5 items
    private int currentInventoryIndex = 0;

    public bool IsDialogueActive => dialoguePanel.activeSelf;
    public bool IsTyping => isTyping;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        // Ensure dialogue panel and item panel are hidden at startup
        if (dialoguePanel != null)
        {
            dialoguePanel.SetActive(false);
        }
        if (itemPanel != null)
        {
            itemPanel.SetActive(false);  // Make sure the item panel starts inactive
        }
    }

    void Update()
    {
        if (dialoguePanel.activeSelf && Input.GetMouseButtonDown(0))
        {
            Debug.Log("Mouse Clicked - Dialogue Active");

            if (!isTyping && lineFullyDisplayed)
            {
                Debug.Log("Line fully displayed - Showing next line");
                ShowNextLine();
            }
        }
    }

    public void ShowMessage(string message)
    {
        Debug.Log("Showing new message: " + message);

        if (string.IsNullOrEmpty(message))
        {
            Debug.LogWarning("Message is empty or null.");
            return;
        }

        dialoguePanel.SetActive(true);
        dialogueLines = message.Split('\n');
        currentLineIndex = 0;
        ShowNextLine();
    }

    void ShowNextLine()
    {
        Debug.Log("Showing next line. Current line index: " + currentLineIndex);

        if (currentLineIndex < dialogueLines.Length)
        {
            string lineToType = dialogueLines[currentLineIndex];
            Debug.Log("Next line available: " + lineToType);
            StartCoroutine(TypeText(lineToType));
            currentLineIndex++;
        }
        else
        {
            Debug.Log("End of dialogue reached. Hiding panel.");
            dialoguePanel.SetActive(false);
        }
    }

    IEnumerator TypeText(string line)
    {
        Debug.Log("Typing line: " + line);
        dialogueText.text = "";
        isTyping = true;
        lineFullyDisplayed = false;

        foreach (char c in line)
        {
            dialogueText.text += c;
            yield return new WaitForSeconds(typingSpeed);
        }

        // Add continue indicator after typing finishes
        dialogueText.text += " ...";

        Debug.Log("Finished typing line.");
        isTyping = false;
        lineFullyDisplayed = true;
    }

    // Function to handle item collection
    public void CollectItem(string itemName)
    {
        if (currentInventoryIndex < 5)
        {
            // Add item to inventory
            inventory[currentInventoryIndex] = itemName;
            currentInventoryIndex++;

            // Display item received message in the new panel
            DisplayItemReceived(itemName);
        }
        else
        {
            Debug.Log("Inventory full! Cannot collect more items.");
        }
    }

    // Display item received in the new item panel
    private void DisplayItemReceived(string itemName)
    {
        // Make sure itemPanel is active
        itemPanel.SetActive(true);
        itemReceivedText.text = $"{itemName} received"; // Display item name

        // Load the image dynamically based on itemName from the Resources folder
        Sprite itemImage = Resources.Load<Sprite>(itemName); // Load sprite with the same name as itemName
        if (itemImage != null)
        {
            itemReceivedImage.sprite = itemImage; // Display item image
        }
        else
        {
            Debug.LogWarning("Item image not found for: " + itemName);
        }

        // Optionally hide the itemPanel after a short time
        StartCoroutine(HideItemPanel());
    }

    // Hide itemPanel after a brief delay
    private IEnumerator HideItemPanel()
    {
        yield return new WaitForSeconds(2f); // Keep it on screen for 2 seconds
        itemPanel.SetActive(false);
    }
}
