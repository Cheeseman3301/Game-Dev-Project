using UnityEngine;
using UnityEngine.SceneManagement;

public class ClickableObject : MonoBehaviour
{
    public string messageToDisplay;  // Message shown in dialogue
    public string objectID;          // Identifier for object (e.g. "Door", "Drawer", etc.)

    private void OnMouseDown()
    {

        if (DialogueManager.Instance.IsDialogueActive())
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
                message = "Nothing but old clothes. Nothing useful here.";
                break;
            case "BigDrawer4":
                message = "A locked drawer. You'll need a key.";
                break;
            case "BigDrawer5":
                message = "Loose papers rustle. One has something scribbled in red ink.";
                break;
            case "BigDrawer6":
                message = "This drawer would not budge. It is stuck.";
                break;
            case "RoomFemale":
                message = "You feel a strange presence in the room.";
                break;
            case "Door":
                message = "You slowly reach for the handle...";
                SceneManager.LoadScene("NextSceneName"); // Replace with your actual scene name
                break;
            case "Cabinet":
                message = "You open the cabinet. Rows of strange bottles line the shelves.";
                break;
            case "UnderBed":
                message = "Something glints under the bed, just out of reach.";
                break;
            case "Desk":
                message = "The desk is cluttered. A diary lies open to a torn page.";
                break;
            default:
                message = messageToDisplay; // fallback message if set in Inspector
                break;
        }

        DialogueManager.Instance.ShowMessage(message);
    }
}
