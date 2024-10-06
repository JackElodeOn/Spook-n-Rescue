using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueActiveTrigger : MonoBehaviour
{
    public DialogueController dialogueController;
    public float textDisplayDuration = 2.0f;
    public string[] sentences;
    private bool hasBeenUsed = false;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(!hasBeenUsed)
        {
            dialogueController.StartDialogue(false, textDisplayDuration, sentences);
            hasBeenUsed = true;
        }
        
    }
}
