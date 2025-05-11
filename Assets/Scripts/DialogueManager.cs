using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance;

    public TextMeshProUGUI dialogueText;         // Assign via Inspector
    public GameObject dialogueBox;    // Parent panel for dialogue UI

    private bool isDisplaying = false;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void ShowMessage(string message)
    {
        StopAllCoroutines(); // Cancel any existing message
        StartCoroutine(DisplayMessageRoutine(message));
    }

    private IEnumerator DisplayMessageRoutine(string message)
    {
        isDisplaying = true;
        dialogueBox.SetActive(true);
        dialogueText.text = message;

        // Wait until player clicks
        yield return new WaitUntil(() => Input.GetMouseButtonDown(0));

        dialogueBox.SetActive(false);
        isDisplaying = false;
    }

    public bool IsDialogueActive()
    {
        return isDisplaying;
    }

    public void HideDialogue()
    {
        if (isDisplaying)
        {
            StopAllCoroutines();
            dialogueBox.SetActive(false);
            isDisplaying = false;
        }
    }

    public void ShowDialogue()
    {
        if (!isDisplaying)
        {
            dialogueBox.SetActive(true);
            isDisplaying = true;
        }
    }

    public void SetDialogueText(string text)
    {
        dialogueText.text = text;
    }

    


}
