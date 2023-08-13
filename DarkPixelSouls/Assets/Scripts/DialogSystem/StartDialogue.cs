using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartDialogue : MonoBehaviour
{
    public Dialogue dialogue;
    DialogueManager dialogueManager;

    private void Awake()
    {
        dialogueManager = DialogueManager.Instance;
    }

    public void TriggerDialog()
    {
        dialogueManager.StartDialog(dialogue);
    }
}