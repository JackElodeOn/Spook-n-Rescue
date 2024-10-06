using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Direction
{
    Up = 0,
    Down = 1,
    Left = 2,
    Right = 3
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
    private int currentHealth;

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

        if (Input.GetKeyDown(KeyCode.W) && IsGrounded())
        {
            _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, jumpingPower);
        }

        if (Input.GetKeyUp(KeyCode.W) && _rigidbody.velocity.y > 0f)
        {
            _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, _rigidbody.velocity.y * 0.5f);
        }

        //Flip();

        float currentSpeed = _rigidbody.velocity.sqrMagnitude;
        _animator.SetFloat("speed", currentSpeed);
        if (currentSpeed > 0.1f)
        {
            _animator.SetFloat("movementX", _rigidbody.velocity.x);
            _animator.SetFloat("movementY", _rigidbody.velocity.y);
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            _animator.SetBool("isAttacking", true);
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
        Debug.Log(temp);
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
}