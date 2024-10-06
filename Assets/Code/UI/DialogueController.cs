using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogueController : MonoBehaviour
{
    public TextMeshProUGUI dialogueText;
    public TextMeshProUGUI continueText;
    private string[] sentences;
    private int index = 0;
    private float dialogueSpeed;
    private bool isTyping = false; // To check if the sentence is still being typed
    private Coroutine typingCoroutine;
    private bool isDialogueActive = false; // Track if the dialogue is active

    // Variables for positioning the UI above the player
    public Transform player; // Reference to the player's transform
    public RectTransform dialogueTextUI; // The RectTransform of the dialogue text UI
    public RectTransform continueTextUI; // The RectTransform of the continue text UI
    public float heightAbovePlayer = 2.0f; // Height offset above the player

    // Start is called before the first frame update
    void Start()
    {
        dialogueSpeed = 0.05f;

        dialogueText.alignment = TextAlignmentOptions.Center; // Always center text
        continueText.gameObject.SetActive(false); // Hide continue text at start
        dialogueText.gameObject.SetActive(false); // Hide dialogue text at start
    }

    // Update is called once per frame
    void Update()
    {
        if (sentences != null)
        {
            if (isDialogueActive)
            {
                // Reposition the dialogue UI above the player's head
                RepositionUI();

                if (Input.GetKeyDown(KeyCode.T) && Time.timeScale == 0)  // Only check if time is frozen
                {
                    if (isTyping) // Check if the sentence is still being animated
                    {
                        // Complete the sentence instantly
                        StopCoroutine(typingCoroutine);
                        dialogueText.maxVisibleCharacters = sentences[index].Length; // Show the full sentence
                        isTyping = false;
                        continueText.gameObject.SetActive(true); // Show continue text after full sentence
                        index++; // Move to the next sentence, since we completed the current one
                    }
                    else
                    {
                        NextSentence(2.0f, true); // Example call, you can customize it later
                    }
                }
            }
            else if (Input.GetKeyDown(KeyCode.T) && Time.timeScale == 0) // Only start dialogue if time is frozen
            {
                StartDialogue(true, 2.0f, sentences); // Example call, you can customize it later
            }
        }

    }

    public void StartDialogue(bool freezeTime, float displayDuration, string[] dialogue)
    {
        sentences = dialogue;
        dialogueText.gameObject.SetActive(true);
        isDialogueActive = true;

        if (freezeTime)
        {
            Time.timeScale = 0; // Pause the game
        }

        index = 0; // Start from the first sentence
        NextSentence(displayDuration, freezeTime); // Begin dialogue
    }

    void NextSentence(float displayDuration, bool freezeTime)
    {
        if (index < sentences.Length)
        {
            dialogueText.text = sentences[index]; // Set the new sentence
            dialogueText.maxVisibleCharacters = 0; // Start with no characters visible
            continueText.gameObject.SetActive(false); // Hide continue text while typing
            typingCoroutine = StartCoroutine(WriteSentence(displayDuration, freezeTime));
        }
        else
        {
            EndDialogue(); // Dialogue ends when all sentences have been shown
        }
    }

    IEnumerator WriteSentence(float displayDuration, bool freezeTime)
    {
        isTyping = true; // Set typing to true at the start of the animation
        int totalCharacters = sentences[index].Length;

        for (int i = 0; i <= totalCharacters; i++)
        {
            dialogueText.maxVisibleCharacters = i; // Reveal characters one by one
            yield return new WaitForSecondsRealtime(dialogueSpeed); // Use WaitForSecondsRealtime since Time.timeScale is 0
        }

        isTyping = false; // Typing is done

        if (freezeTime)
        {
            continueText.gameObject.SetActive(true); // Show continue text when finished if frozen
            index++;
        }
        else
        {
            yield return new WaitForSeconds(displayDuration); // Wait for the display duration
            index++;
            NextSentence(displayDuration, freezeTime); // Automatically move to the next sentence
        }
    }

    void EndDialogue()
    {
        isDialogueActive = false;
        continueText.gameObject.SetActive(false); // Hide continue text when done
        dialogueText.gameObject.SetActive(false);
        sentences = null;
        Time.timeScale = 1; // Resume the game
    }

    void RepositionUI()
    {
        if (player != null)
        {
            // Get the player's position in world space
            Vector3 playerWorldPosition = player.position + Vector3.up * heightAbovePlayer;

            // Set the dialogue text UI position directly above the player
            dialogueTextUI.position = playerWorldPosition;
            continueTextUI.position = playerWorldPosition - Vector3.up * (0.5f);
        }
    }
}