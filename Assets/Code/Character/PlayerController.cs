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

    private float horizontal;
    private float speed = 3f;
    private float jumpingPower = 12f;
    private bool isFacingRight = true;

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

    /**
    private void Flip()
    {
        if (isFacingRight && horizontal < 0f || !isFacingRight && horizontal > 0f)
        {
            isFacingRight = !isFacingRight;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;
        }
    }
    */

    // Draw the ground check circle in the editor
    private void OnDrawGizmosSelected()
    {
        if (groundCheck == null)
            return;

        Gizmos.color = Color.red; // Choose the color for the gizmo
        Gizmos.DrawWireSphere(groundCheck.position, checkRadius); // Draw a wireframe circle
    }
}