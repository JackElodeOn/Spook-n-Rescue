using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Potion : MonoBehaviour
{
    public int healAmount = 20;  // Amount of health to restore

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the player collides with the potion
        if (other.CompareTag("Player"))
        {
            // Get the PlayerController script from the player
            PlayerController player = other.GetComponent<PlayerController>();

            if (player != null)
            {
                // Heal the player
                player.Heal(healAmount);

                // Destroy the potion after healing
                Destroy(gameObject);
            }
        }
    }
}
