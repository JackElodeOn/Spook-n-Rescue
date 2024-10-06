using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MeleeEnemy : MonoBehaviour
{
    Rigidbody2D _rb;
    Animator _animator;

    public int UnitsToMove = 5;
    
    public int speed = 500;
    public bool isFacingRight;
    public bool moveRight;

    public int health;
    private int currentHealth;
    private float startPos;
    private float endPos;
    
    public Transform target;
    public float minDistance;

    
    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        startPos = _rb.position.x;
        endPos = startPos + UnitsToMove;
        isFacingRight = transform.localScale.x > 0;

        currentHealth = health;

        /*if(target == null)
        {
            target = FindObjectOfType<PlayerController>().GetComponent<Transform>();
        }*/
        
    }

    // Update is called once per frame
    void Update()
    {

        float movementSpeed = _rb.velocity.sqrMagnitude;
        _animator.SetFloat("speed", movementSpeed);
        if(movementSpeed > 0.1f )
        {
            _animator.SetFloat("movementX", _rb.velocity.x);
            _animator.SetFloat("movementY", _rb.velocity.y);
        }

        if(Input.GetKeyDown(KeyCode.Space))
        {
            _animator.SetTrigger("attack");
        }
        /*float distance = Vector2.Distance(transform.position, target.position);
        RaycastHit2D hit = Physics2D.Raycast((Vector2)transform.position, (Vector2)transform.forward, distance);
        if(hit.collider == target)
        {
            _rb.AddForce((Vector2)transform.forward * speed * Time.deltaTime);
        }
        else
        {
            
        }*/

        if (moveRight)
        {
            _rb.AddForce(Vector2.right * speed * Time.deltaTime);
            if (!isFacingRight)
                Flip();
        }

        if (_rb.position.x >= endPos)
            moveRight = false;

        if (!moveRight)
        {
            _rb.AddForce(-Vector2.right * speed * Time.deltaTime);
            if (isFacingRight)
                Flip();
        }
        if (_rb.position.x <= startPos)
            moveRight = true;


    }

    public void Flip()
    {
        transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
        isFacingRight = transform.localScale.x > 0;
    }

}
