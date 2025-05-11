using System.Collections;
using UnityEngine;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance;

    public GameObject dialoguePanel;
    public TMP_Text dialogueText;
    public float typingSpeed = 0.03f;

    private string[] dialogueLines;
    private int currentLineIndex;
    private bool isTyping = false;
    private bool lineFullyDisplayed = false;

    public bool IsDialogueActive => dialoguePanel.activeSelf;
    public bool IsTyping => isTyping;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
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

        Debug.Log("Finished typing line.");
        isTyping = false;
        lineFullyDisplayed = true;
    }
}
