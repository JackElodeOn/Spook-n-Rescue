using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogueController : MonoBehaviour
{
    public TextMeshProUGUI dialogueText;
    public TextMeshProUGUI continueText;

    public RectTransform dialogueUIImage; // Renamed to make it clear for dialogue text
    public RectTransform continueUIImage; // New variable for continue text UIImage

    private string[] sentences;
    private int index = 0;
    private float dialogueSpeed;
    private bool isTyping = false; // To check if the sentence is still being typed
    private Coroutine typingCoroutine;
    private bool isDialogueActive = false; // Track if the dialogue is active

    // Variables for positioning the UI above the player
    public Transform player; // Reference to the player's transform
    public float heightAbovePlayer = 2.0f; // Height offset above the player

    // Start is called before the first frame update
    void Start()
    {
        dialogueSpeed = 0.05f;

        dialogueText.alignment = TextAlignmentOptions.Center; // Always center text
        continueUIImage.gameObject.SetActive(false); // Hide continue UIImage at start
        dialogueUIImage.gameObject.SetActive(false); // Hide dialogue UIImage at start
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
                        continueUIImage.gameObject.SetActive(true); // Show continue UIImage after full sentence
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
        isDialogueActive = true;

        if (freezeTime)
        {
            Time.timeScale = 0; // Pause the game
        }

        RepositionUI(); // Reposition first, then set active

        dialogueUIImage.gameObject.SetActive(true); // Activate the dialogue UIImage

        index = 0; // Start from the first sentence
        NextSentence(displayDuration, freezeTime); // Begin dialogue
    }

    void NextSentence(float displayDuration, bool freezeTime)
    {
        if (index < sentences.Length)
        {
            dialogueText.text = sentences[index]; // Set the new sentence
            dialogueText.maxVisibleCharacters = 0; // Start with no characters visible
            continueUIImage.gameObject.SetActive(false); // Hide continue UIImage while typing
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
            continueUIImage.gameObject.SetActive(true); // Show continue UIImage when finished if frozen
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
        continueUIImage.gameObject.SetActive(false); // Hide continue UIImage when done
        dialogueUIImage.gameObject.SetActive(false); // Hide dialogue UIImage when done
        sentences = null;
        Time.timeScale = 1; // Resume the game
    }

    void RepositionUI()
    {
        if (player != null)
        {
            // Get the player's position in world space
            Vector3 playerWorldPosition = player.position + Vector3.up * heightAbovePlayer;

            // Set the UIImage positions directly above the player
            dialogueUIImage.position = playerWorldPosition;
            continueUIImage.position = playerWorldPosition - Vector3.up;
        }
    }
}