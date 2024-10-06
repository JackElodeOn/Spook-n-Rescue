using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalBossController : MonoBehaviour
{
    public GameObject enemyPrefab;          // Normal enemy prefab to spawn
    public Transform[] spawnPoints;         // Spawn points for enemies
    public GameObject beamPrefab;           // Beam projectile prefab
    public Transform beamSpawnPoint;        // Position where the beam spawns
    public Animator bossAnimator;           // Animator for the boss
    public int bossHealth = 100;            // Boss health
    public GameObject blackoutScreen;       // Blackout screen for the big damage ability
    public PlayerController player;         // Reference to the player
    private bool isAttacking = false;       // To prevent attacks when waiting for a special attack
    private bool hasDoneBigDamageAttack = false; // To ensure big damage attack only happens once
    public GameObject playerObject;

    void Update()
    {
        if (bossHealth <= 0)
        {
            StartCoroutine(Die());  // Trigger death when health reaches 0
        }
    }

    // Method for the boss's two normal abilities
    public void StartAbilities()
    {
        if (!isAttacking && !hasDoneBigDamageAttack) // Ensure it's not in the middle of the big damage ability
        {
            // Randomly decide which ability to use
            //int randomAbility = Random.Range(1, 3);  // 1 or 2 for simplicity

            int randomAbility = 2;


            if (randomAbility == 1)
            {
                StartCoroutine(SpawnEnemies());
            }
            else if (randomAbility == 2)
            {
                StartCoroutine(ShootBeam());
            }
        }
    }

    // Ability 1: Spawn Enemies
    IEnumerator SpawnEnemies()
    {
        Debug.Log("Final boss started spawning enemies!");

        foreach (Transform spawnPoint in spawnPoints)
        {
            Instantiate(enemyPrefab, spawnPoint.position, spawnPoint.rotation);
        }

        yield return new WaitForSeconds(2f); // Wait before another ability
        StartAbilities();
    }

    // Ability 2: Shoot a Beam
    IEnumerator ShootBeam()
    {
        Debug.Log("Final boss started shooting beam!");

        // Instantiate the beam projectile and aim it at the player
        GameObject beam = Instantiate(beamPrefab, beamSpawnPoint.position, Quaternion.identity);
        EyeBeam beamComponent = beam.GetComponent<EyeBeam>();

        beamComponent.Initialize(playerObject.transform.position);

        yield return new WaitForSeconds(2f); // Wait before another ability
        StartAbilities();
    }

    // Ability 3: Big Damage Attack
    public void BigDamageAttack()
    {
        if (!isAttacking)
        {
            isAttacking = true;

            // Start the big attack animation
            bossAnimator.SetTrigger("BigDamageAttack");

            // Start the blackout and delayed kill unless interrupted
            StartCoroutine(BigDamageCoroutine());
        }
    }

    IEnumerator BigDamageCoroutine()
    {

        Debug.Log("Final boss started big damage attack!");
        
        // Show blackout screen
        blackoutScreen.SetActive(true);

        yield return new WaitForSeconds(3f); // Give 3 seconds to perform the special attack

        // If the player doesn't do the special attack, kill them
        if (!player.specialAttackTriggered)
        {
            player.Die(); // Replace with actual method to kill the player
        }
        else
        {
            // Cancel the attack if the player does the special attack
            CancelBigDamageAttack();
        }
    }

    // Cancel the big damage attack and reset after 5 seconds
    void CancelBigDamageAttack()
    {
        // Play the attack end animation
        bossAnimator.SetTrigger("AttackEnd");

        // Disable the blackout screen
        blackoutScreen.SetActive(false);

        // Reset the attacking state and wait for 5 seconds
        StartCoroutine(ResetAfterAttack());
    }

    IEnumerator ResetAfterAttack()
    {
        yield return new WaitForSeconds(5f);

        isAttacking = false;  // Allow other abilities to resume
        StartAbilities();     // Resume ability cycle
    }

    // Boss death logic
    IEnumerator Die()
    {
        bossAnimator.SetTrigger("Die");
        yield return new WaitForSeconds(2f); // Play death animation for 2 seconds

        // Destroy the boss after the death animation
        Destroy(gameObject);
    }

    // Method to reduce the boss's health (e.g., when hit by the player)
    public void TakeDamage(int damage)
    {
        bossHealth -= damage;

        // Check if boss health is 40 or below and the big damage attack has not been done yet
        if (bossHealth <= 40 && !hasDoneBigDamageAttack)
        {
            hasDoneBigDamageAttack = true;  // Ensure it only triggers once
            BigDamageAttack();              // Trigger the big damage attack
        }
    }
}

