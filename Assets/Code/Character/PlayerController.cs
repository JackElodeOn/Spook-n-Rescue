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
    public float moveSpeed;
    public Sprite[] sprites;
    public float jumpForce = 10f;
    public int jumpCount = 2;

    // State Tracking
    public Direction facingDirection;

    // Start is called before the first frame update
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void FixedUpdate()
    {
        if (Input.GetKey(keyUp) && jumpCount > 0)
        {
            //jumpCount;
            _rigidbody.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }
        if (Input.GetKey(keyDown))
        {
            _rigidbody.AddForce(Vector2.down * moveSpeed * Time.fixedDeltaTime, ForceMode2D.Impulse);
        }
        if (Input.GetKey(keyLeft))
        {
            _rigidbody.AddForce(Vector2.left * moveSpeed * Time.fixedDeltaTime, ForceMode2D.Impulse);
        }
        if (Input.GetKey(keyRight))
        {
            _rigidbody.AddForce(Vector2.right * moveSpeed * Time.fixedDeltaTime, ForceMode2D.Impulse);
        }
    }
    // Update is called once per frame
    void Update()
    {
        float movementSpeed = _rigidbody.velocity.sqrMagnitude;
        _animator.SetFloat("speed", movementSpeed);
        if (movementSpeed > 0.1f)
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

    private void OnCollisionStay2D(Collision2D collision)
    {
        // Check if we collided with ground
        if(collision.gameObject.layer == LayerMask.NameToLayer("Platforms"))
        {
            // Check what is directly below our character's feet
            RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, Vector2.down, 0.7f);

            // We might have multiple things beneath our character's feet
            for (int i = 0; i < hits.Length; i++)
            {
                RaycastHit2D hit = hits[i];

                // Check that we collided with ground below our feet
                if(hit.collider.gameObject.layer == LayerMask.NameToLayer("Ground"))
                {
                    // Reset jump count
                    jumpCount = 2;
                }
            }
        }

        
    }
}