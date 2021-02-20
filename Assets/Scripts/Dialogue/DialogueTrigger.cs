using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public DialogueInfo dialogue;

    public void TriggerDialogue()
    {
        DialogueManager.Instance.StartDialogue(dialogue);
    }
}
