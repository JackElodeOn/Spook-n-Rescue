using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Direction
{
    Left = 0,
    Right = 1
}

public class PlayerController : MonoBehaviour
{
    // Outlets
    Rigidbody2D _rigidbody;
    Animator _animator;
    SpriteRenderer _spriteRenderer;
    public Transform[] attackZones;

    // Configuration
    public KeyCode keyUp;
    public KeyCode keyDown;
    public KeyCode keyLeft;
    public KeyCode keyRight;
    public Sprite[] sprites;

    // Health stats
    public int maxHealth = 100;
    public int currentHealth;

    // Movement
    private float horizontal;
    private float speed = 3f;
    private float jumpingPower = 12f;

    public bool specialAttackTriggered = false;

    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask platformLayer;
    private float checkRadius = 0.2f; // The radius of the ground check

    // State Tracking
    public Direction facingDirection;

    public int attackDamage = 5;
    public float attackRange = 1f;
    public LayerMask enemyLayers;

    // Safe position (checkpoint)
    [SerializeField] private List<Transform> checkpoints;

    // Start is called before the first frame update
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _spriteRenderer = GetComponent<SpriteRenderer>();

        currentHealth = maxHealth;
    }
    // Update is called once per frame
    void Update()
    {
        horizontal = Input.GetAxisRaw("Horizontal");

        // Jump logic
        if (Input.GetKeyDown(KeyCode.W) && IsGrounded())
        {
            _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, jumpingPower);
        }

        if (Input.GetKeyUp(KeyCode.W) && _rigidbody.velocity.y > 0f)
        {
            _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, _rigidbody.velocity.y * 0.5f);
        }

        float currentSpeed = _rigidbody.velocity.sqrMagnitude;
        _animator.SetFloat("speed", currentSpeed);

        if (currentSpeed > 0.1f)
        {
            _animator.SetFloat("movementX", _rigidbody.velocity.x);
            _animator.SetFloat("movementY", _rigidbody.velocity.y);
        }

        // Attack logic
        if (Input.GetKeyDown(KeyCode.Space))
        {
            _animator.SetBool("isAttacking", true);
            Attack();
        }

        // Check if attack animation is playing
        AnimatorStateInfo stateInfo = _animator.GetCurrentAnimatorStateInfo(0);
        if (stateInfo.IsName("Attack") && stateInfo.normalizedTime >= 1.0f)
        {
            _animator.SetBool("isAttacking", false); // Reset after animation ends
        }

        // Perform the special attack for the final boss
        if (Input.GetKeyDown(KeyCode.P))
        {
            PerformSpecialAttack();
        }
    }

    private void FixedUpdate()
    {
        _rigidbody.velocity = new Vector2(horizontal * speed, _rigidbody.velocity.y);
    }

    void LateUpdate()
    {
        for (int i = 0; i < sprites.Length; i++)
        {
            if (_spriteRenderer.sprite == sprites[i])
            {
                facingDirection = (Direction)i;
                break;
            }
        }
    }

    private bool IsGrounded()
    {
        bool temp = Physics2D.OverlapCircle(groundCheck.position, checkRadius, platformLayer);
        return temp;
    }


    // Draw the ground check circle in the editor
    private void OnDrawGizmosSelected()
    {
        if (groundCheck == null)
            return;

        Gizmos.color = Color.red; // Choose the color for the gizmo
        Gizmos.DrawWireSphere(groundCheck.position, checkRadius); // Draw a wireframe circle
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        Debug.Log("Player is dead!");
        _animator.SetTrigger("Die");

        // StartCoroutine(DestroyAfterDelay());
    }

    IEnumerator DestroyAfterDelay()
    {
        // Wait for 2 seconds
        yield return new WaitForSeconds(2f);

        // Destroy the GameObject
        Destroy(gameObject);
    }

    public void Heal(int amount)
    {
        currentHealth += amount;

        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
    }

    public void PerformSpecialAttack()
    {
        if (!specialAttackTriggered)
        {
            specialAttackTriggered = true;
        }
    }

    // Atack Logic
    void Attack()
    {
        int facingDirectionIndex = (int)facingDirection;

        Transform attackZone = attackZones[facingDirectionIndex];
        
        // Detect enemies in range of attack
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackZone.position, attackRange, enemyLayers);

        // Damage each enemy hit
        foreach (Collider2D enemy in hitEnemies)
        {
            FinalBossController finalBoss = enemy.GetComponent<FinalBossController>();

            if (finalBoss) {
                finalBoss.TakeDamage(attackDamage);
            }

            MeleeEnemy meleeEnemy = enemy.GetComponent<MeleeEnemy>();
            if (meleeEnemy)
            {
                meleeEnemy.TakeDamage(attackDamage);
            }
        }
    }

    private void OnDrawGizmos()
    {
        if (attackZones != null)
        {
            foreach (Transform attackZone in attackZones)
            {
                Gizmos.DrawWireSphere(attackZone.position, attackRange);
            }
        }
    }


    // Find the closest checkpoint to the player when falling onto spikes
    public void ResetToClosestCheckpoint()
    {
        Transform closestCheckpoint = FindClosestCheckpoint();
        if (closestCheckpoint != null)
        {
            transform.position = closestCheckpoint.position;
            _rigidbody.velocity = Vector2.zero;  // Stop any momentum
        }
    }

    // Find the closest checkpoint from the current player position
    Transform FindClosestCheckpoint()
    {
        Transform closestCheckpoint = null;
        float closestDistance = Mathf.Infinity;

        foreach (Transform checkpoint in checkpoints)
        {
            float distance = Vector3.Distance(transform.position, checkpoint.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestCheckpoint = checkpoint;
            }
        }

        return closestCheckpoint;
    }
}