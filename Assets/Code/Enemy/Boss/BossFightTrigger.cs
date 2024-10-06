using Cinemachine;
using UnityEngine;

public class BossFightTrigger : MonoBehaviour
{
    public Animator bossAnimator;                 // Assign the Boss's Animator component in the Inspector
    public CinemachineVirtualCamera playerCamera; // The camera that follows the player
    public CinemachineVirtualCamera bossCamera;   // The camera that focuses on the boss
    public float focusTime = 3f;                  // Duration to focus on the boss
    public FinalBossController finalBossController;

    void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the player enters the trigger
        if (other.CompareTag("Player"))
        {
            // Start the wakeup animation for the boss
            bossAnimator.SetTrigger("Wakeup");

            // Switch to boss camera
            bossCamera.Priority = 10;
            playerCamera.Priority = 5;

            // Call function to switch back to player camera after the focus time
            Invoke("ReturnToPlayerCamera", focusTime);
        }
    }

    // Function to return the camera focus back to the player
    void ReturnToPlayerCamera()
    {
        playerCamera.Priority = 10;
        bossCamera.Priority = 5;

        Invoke("StartAbilities", 2f);

        Destroy(gameObject);
    }

    void StartAbilities()
    {
        finalBossController.StartAbilities();
    }
}
