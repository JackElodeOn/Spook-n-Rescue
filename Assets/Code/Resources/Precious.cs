using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Precious : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the player collides with the Precious item
        if (other.CompareTag("Player"))
        {
            // Get the PlayerController script from the player
            PlayerController player = other.GetComponent<PlayerController>();

            if (player != null)
            {
                // Enable the special attack for the player
                player.EnableSpecialAttack();

                // Destroy the Precious item after it’s collected
                Destroy(gameObject);
            }
        }
    }
}
