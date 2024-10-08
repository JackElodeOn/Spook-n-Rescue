using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spikes : MonoBehaviour
{
    public int damage = 50;  // Damage the spikes will do to the player

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerController player = other.GetComponent<PlayerController>();

            if (player != null)
            {
                // Deal damage to the player
                player.TakeDamage(damage);

                // Reset the player to the closest checkpoint
                if(player.currentHealth > 0)
                {
                    player.ResetToClosestCheckpoint();
                }    
            }
        }
    }
}
