using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueFreezeTrigger : MonoBehaviour
{
    public DialogueController dialogueController;
    public string[] sentences;
    private bool hasBeenUsed = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!hasBeenUsed)
        {
            dialogueController.StartDialogue(true, 0.0f, sentences);
            hasBeenUsed = true;
        }
        
    }
}
