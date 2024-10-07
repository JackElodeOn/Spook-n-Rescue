using TMPro;
using UnityEngine;

public class HealthUIController : MonoBehaviour
{
    public TextMeshProUGUI healthText;  // Reference to the TextMeshPro UI element
    public PlayerController player;     // Reference to the PlayerController script

    void Start()
    {
        // Initialize the health display
        UpdateHealthText(player.currentHealth);
    }

    // Method to update the health text display
    public void UpdateHealthText(int health)
    {
        healthText.text = "HP: " + health;
    }
}
