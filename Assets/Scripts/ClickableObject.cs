using UnityEngine;
using UnityEngine.SceneManagement;

public class ClickableObject : MonoBehaviour
{
    public string messageToDisplay;  // Optional fallback message from Inspector
    public string objectID;          // Optional identifier for the object

    private void OnMouseDown() {

        Debug.Log($"Clicked on: {gameObject.name}");

        if (DialogueManager.Instance == null) {
            Debug.LogError("DialogueManager.Instance is null!");
            return;
        }

        // Prevent clicking if dialogue is active or still typing
        if (DialogueManager.Instance.IsDialogueActive || DialogueManager.Instance.IsTyping)
            return;

        string message = "";


        switch (gameObject.name)
        {
            case "BigDrawer1":
                message = "It's empty. Just dust and a spider.";
                break;
            case "BigDrawer2":
                message = "A faded photograph lies inside, barely visible.";
                break;
            case "BigDrawer3":
                message = "Nothing but old clothes.\nNothing useful here.";
                break;
            case "BigDrawer4":
                message = "A locked drawer. You'll need a key.";
                break;
            case "BigDrawer5":
                message = "Loose papers rustle.\nOne has something scribbled in red ink.";
                break;
            case "BigDrawer6":
                message = "This drawer would not budge. It is stuck.";
                break;
            case "RoomFemale":
                message = "You feel a strange presence in the room.";
                break;
            case "Door":
                message = "You slowly reach for the handle...";
                DialogueManager.Instance.ShowMessage(message);
                StartCoroutine(LoadSceneAfterDialogue("KitchenScene")); // Replace with actual name
                return;
            case "Cabinet":
                message = "You open the cabinet.\nRows of strange bottles line the shelves.";
                break;
            case "UnderBed":
                message = "Something glints under the bed, just out of reach.";
                break;
            case "Desk":
                message = "The desk is cluttered.\nA diary lies open to a torn page.";
                break;
            case "Painting":
                message = "Behind the painting, there’s a small hole in the wall.\nSomething’s inside.";
                break;
            default:
                message = messageToDisplay; // Fallback from Inspector if no match
                break;
        }

        DialogueManager.Instance.ShowMessage(message);
    }

    private System.Collections.IEnumerator LoadSceneAfterDialogue(string sceneName)
    {
        // Wait until dialogue is finished before transitioning
        yield return new WaitUntil(() => !DialogueManager.Instance.IsDialogueActive && !DialogueManager.Instance.IsTyping);
        SceneManager.LoadScene(sceneName);
    }
}
