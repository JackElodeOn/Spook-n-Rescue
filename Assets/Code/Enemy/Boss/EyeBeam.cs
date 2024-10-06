using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EyeBeam : MonoBehaviour
{
    private Animator animator;
    public float speed = 5f;  // Speed of the beam
    private Vector3 direction;
    private bool hasHitPlayer = false;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }
    public void Initialize(Vector3 targetPosition)
    {
        direction = (targetPosition - transform.position).normalized;
    }

    void Update()
    {
        if (!hasHitPlayer)
        {
            transform.Translate(direction * speed * Time.deltaTime);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the beam hits the player
        if (other.CompareTag("Player"))
        {
            hasHitPlayer = true;

            animator.SetTrigger("Die");

            PlayerController player = other.GetComponent<PlayerController>();
            if (player != null)
            {
                player.TakeDamage(5);  // Damage the player for 5 health
            }

            StartCoroutine(DestroyAfterAnimation());
        }
    }

    IEnumerator DestroyAfterAnimation()
    {
        // Wait for the die animation to finish
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);

        // Destroy the beam GameObject
        Destroy(gameObject);
    }
}
