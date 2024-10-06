using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EyeBeam : MonoBehaviour
{
    public float speed = 10f;   // Speed of the beam
    private Vector2 targetPosition;  // The position to which the beam will move

    // Method to initialize the beam's direction
    public void Initialize(Vector3 playerPosition)
    {
        // Calculate the direction from the beam's current position to the player's position
        Vector2 direction = (playerPosition - transform.position).normalized;

        // Set the beam's velocity in the direction of the player
        GetComponent<Rigidbody2D>().velocity = direction * speed;
    }

    // This method is called when the beam hits something
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Example: Deal damage to the player or trigger other effects
            other.GetComponent<PlayerController>().TakeDamage(10);
            Destroy(gameObject);  // Destroy the beam after hitting the player
        }
        else if (other.CompareTag("Wall") || other.CompareTag("Obstacle"))
        {
            // Destroy the beam if it hits an obstacle
            Destroy(gameObject);
        }
    }
}
