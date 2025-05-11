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

    public GameObject itemPanel;
    public TMP_Text itemReceivedText;
    public Image itemReceivedImage;

    private string[] dialogueLines;
    private int currentLineIndex;
    private bool isTyping = false;
    private bool lineFullyDisplayed = false;

    private string[] inventory = new string[5];
    private int currentInventoryIndex = 0;

    // Queuing system
    private string queuedMessage = null;
    private bool hasQueuedMessage = false;

    public bool IsDialogueActive => dialoguePanel.activeSelf;
    public bool IsTyping => isTyping;
    public bool IsItemPanelActive => itemPanel != null && itemPanel.activeSelf;
    public bool AnyPanelActive => IsDialogueActive || IsItemPanelActive;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        if (dialoguePanel != null)
            dialoguePanel.SetActive(false);

        if (itemPanel != null)
            itemPanel.SetActive(false);
    }

    void Update()
    {
        // Prevent advancing dialogue while item panel is active
        if (!IsItemPanelActive && dialoguePanel.activeSelf && Input.GetMouseButtonDown(0))
        {
            if (!isTyping && lineFullyDisplayed)
                ShowNextLine();
        }
    }

    public void ShowMessage(string message)
    {
        if (string.IsNullOrEmpty(message))
        {
            Debug.LogWarning("Message is empty or null.");
            return;
        }

        // If another panel is active, queue the message
        if (AnyPanelActive)
        {
            Debug.Log("Panel busy — queuing message: " + message);
            queuedMessage = message;
            hasQueuedMessage = true;
            return;
        }

        dialoguePanel.SetActive(true);
        dialogueLines = message.Split('\n');
        currentLineIndex = 0;
        ShowNextLine();
    }

    void ShowNextLine()
    {
        if (currentLineIndex < dialogueLines.Length)
        {
            string lineToType = dialogueLines[currentLineIndex];
            StartCoroutine(TypeText(lineToType));
            currentLineIndex++;
        }
        else
        {
            dialoguePanel.SetActive(false);

            // After dialogue ends, check if there’s a queued message
            if (hasQueuedMessage && !AnyPanelActive)
            {
                string messageToShow = queuedMessage;
                queuedMessage = null;
                hasQueuedMessage = false;
                ShowMessage(messageToShow);
            }
        }
    }

    IEnumerator TypeText(string line)
    {
        dialogueText.text = "";
        isTyping = true;
        lineFullyDisplayed = false;

        // Stabilize layout
        dialogueText.text = " ";
        dialogueText.ForceMeshUpdate();
        int baseLineCount = dialogueText.textInfo.lineCount;
        dialogueText.text = "";

        foreach (char c in line)
        {
            dialogueText.text += c;
            dialogueText.ForceMeshUpdate();

            int currentLineCount = dialogueText.textInfo.lineCount;
            if (currentLineCount > baseLineCount)
            {
                dialogueText.text += "\n"; // Optional stabilization
            }

            yield return new WaitForSeconds(typingSpeed);
        }

        dialogueText.text += " ...";
        isTyping = false;
        lineFullyDisplayed = true;
    }

    public void CollectItem(string displayName, string resourcePath)
    {
        if (currentInventoryIndex < 5)
        {
            inventory[currentInventoryIndex] = displayName;
            currentInventoryIndex++;

            // Make sure no panel is active before showing item
            StartCoroutine(WaitUntilNoPanelThenShowItem(displayName, resourcePath));
        }
        else
        {
            Debug.Log("Inventory full! Cannot collect more items.");
        }
    }

    private IEnumerator WaitUntilNoPanelThenShowItem(string displayName, string resourcePath)
    {
        yield return new WaitUntil(() => !AnyPanelActive);

        DisplayItemReceived(displayName, resourcePath);
    }

    private void DisplayItemReceived(string displayName, string resourcePath)
    {
        itemPanel.SetActive(true);
        itemReceivedText.text = $"{displayName} received";

        Sprite itemImage = Resources.Load<Sprite>(resourcePath);
        if (itemImage != null)
        {
            itemReceivedImage.sprite = itemImage;
        }
        else
        {
            Debug.LogWarning("Item image not found at: " + resourcePath);
        }

        StartCoroutine(HideItemPanel());
    }

    private IEnumerator HideItemPanel()
    {
        yield return new WaitForSeconds(2f);
        itemPanel.SetActive(false);

        // Check for queued dialogue after hiding item panel
        if (hasQueuedMessage && !AnyPanelActive)
        {
            string messageToShow = queuedMessage;
            queuedMessage = null;
            hasQueuedMessage = false;
            ShowMessage(messageToShow);
        }
    }
}
