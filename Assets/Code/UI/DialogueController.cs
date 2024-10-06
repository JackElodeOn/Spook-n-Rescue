using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogueController : MonoBehaviour
{
    public TextMeshProUGUI DialogueText;
    public TextMeshProUGUI continueText;
    private string[] sentences;
    private int index = 0;
    private float dialogueSpeed;
    private bool isTyping = false; // To check if the sentence is still being typed
    private Coroutine typingCoroutine;
    private bool isDialogueActive = false; // Track if the dialogue is active

    // Start is called before the first frame update
    void Start()
    {
        dialogueSpeed = 0.05f;
        sentences = new string[3]; // 3 for now
        sentences[0] = "Oh no...";
        sentences[1] = "Where have I wandered off to this time...";
        sentences[2] = "This looks a bit too spooky for me, I hope I can find my girlfriend quickly...";

        DialogueText.alignment = TextAlignmentOptions.Center; // Always center text
        continueText.gameObject.SetActive(false); // Hide continue text at start
    }

    // Update is called once per frame
    void Update()
    {
        if (isDialogueActive && Input.GetKeyDown(KeyCode.T))
        {
            if (isTyping) // Check if the sentence is still being animated
            {
                // Complete the sentence instantly
                StopCoroutine(typingCoroutine);
                DialogueText.maxVisibleCharacters = sentences[index].Length; // Show the full sentence
                isTyping = false;
                continueText.gameObject.SetActive(true); // Show continue text after full sentence
                index++; // Move to the next sentence, since we completed the current one
            }
            else
            {
                NextSentence();
            }
        }
    }

    public void StartDialogue()
    {
        isDialogueActive = true;
        Time.timeScale = 0; // Pause the game
        index = 0; // Start from the first sentence
        NextSentence(); // Begin dialogue
    }

    void NextSentence()
    {
        if (index < sentences.Length)
        {
            DialogueText.text = sentences[index]; // Set the new sentence
            DialogueText.maxVisibleCharacters = 0; // Start with no characters visible
            continueText.gameObject.SetActive(false); // Hide continue text while typing
            typingCoroutine = StartCoroutine(WriteSentence());
        }
        else
        {
            EndDialogue(); // Dialogue ends when all sentences have been shown
        }
    }

    IEnumerator WriteSentence()
    {
        isTyping = true; // Set typing to true at the start of the animation
        int totalCharacters = sentences[index].Length;

        for (int i = 0; i <= totalCharacters; i++)
        {
            DialogueText.maxVisibleCharacters = i; // Reveal characters one by one
            yield return new WaitForSecondsRealtime(dialogueSpeed); // Use WaitForSecondsRealtime since Time.timeScale is 0
        }

        isTyping = false; // Typing is done
        continueText.gameObject.SetActive(true); // Show continue text when finished
        index++; // Move to the next sentence index
    }

    void EndDialogue()
    {
        isDialogueActive = false;
        continueText.gameObject.SetActive(false); // Optionally hide the continue text when done
        Time.timeScale = 1; // Resume the game
    }
}
