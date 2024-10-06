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
    public int currentHealth;
    private float startPos;
    private float endPos;
    
    public Transform target;
    public float minDistance;

    public bool isWaiting;
    public float waitTime;

    public bool isAttacking;
    public float attackRange;
    public int attackDamage;
    public float attackCoolDown;
    public float attacked;

    
    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        startPos = _rb.position.x;
        endPos = startPos + UnitsToMove;
        isFacingRight = transform.localScale.x > 0;

        currentHealth = health;

        if(target == null)
        {
            target = FindObjectOfType<PlayerController>().GetComponent<Transform>();
        }
        
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

        /*float distance = Vector2.Distance(transform.position, target.position);
        RaycastHit2D hits = Physics2D.Raycast((Vector2)transform.position, (Vector2)transform.forward, minDistance);
        if(hits.collider != null && hits.collider.tag=="Player")
        {
            _animator.SetTrigger("attack");
            if(!isAttacking)
            {
                isAttacking = true;
            }
        }
        else
        {
            _animator.ResetTrigger("attack");
            isAttacking = false;
        }*/

        LookForPlayer();

        EnemyMovement();
        attacked += Time.deltaTime;

    }

    void EnemyMovement()
    {
        if (!isAttacking)
        {
            if (!isWaiting)
            {
                if (moveRight)
                {
                    _rb.AddForce(Vector2.right * speed * Time.deltaTime);
                }

                if (!moveRight)
                {
                    _rb.AddForce(-Vector2.right * speed * Time.deltaTime);
                }


            }
            if (_rb.position.x >= endPos)
            {
                StartCoroutine(Wait());
                moveRight = false;
                if (isFacingRight)
                    Flip();
            }



            if (_rb.position.x <= startPos)
            {
                StartCoroutine(Wait());
                moveRight = true;
                if (!isFacingRight)
                    Flip();
            }
        }
        
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        if(currentHealth <= 0)
        {
            Die();
        }
    }
    void Die()
    {
        Destroy(gameObject);
    }

    void Attack()
    {
        
        if (attacked >= attackCoolDown)
        {
            _animator.SetTrigger("attack");
            isAttacking = true;
            Collider2D[] hitPlayer = Physics2D.OverlapCircleAll(transform.position, attackRange);
            foreach (Collider2D hit in hitPlayer)
            {
                if (hit.tag == "Player")
                {
                    PlayerController player = hit.GetComponent<PlayerController>();
                    player.TakeDamage(attackDamage);
                    Debug.Log("Hit Player");
                }

            }

            AnimatorStateInfo stateInfo = _animator.GetCurrentAnimatorStateInfo(0);
            if ((stateInfo.IsName("AttackLeft") || stateInfo.IsName("AttackRight"))  && stateInfo.normalizedTime >= 1.0f)
            {
                _animator.SetBool("isAttacking", false); // Reset after animation ends
            }
            attacked = 0;
        }
        

    }
    void LookForPlayer()
    {
        //precompute raysettings
        Vector3 start = transform.position;
        Vector3 direction = target.transform.position - transform.position;
        direction.Normalize();

        float distance = minDistance;

        RaycastHit2D[] hits = Physics2D.RaycastAll(start, direction, distance);
        bool playerFound = false;

        for (int it = 0; it < hits.Length; it++)
        {
            if (hits[it].collider != null && hits[it].collider.tag == "Player")
            {
                playerFound = true;
            }
        }

        //draw ray in editor
        if (playerFound)
        {
            Attack();
            Debug.DrawLine(start, start + (direction * distance), Color.green, 2f, false);
        }
        else
        {
            _animator.ResetTrigger("attack");
            isAttacking = false;
            Debug.DrawLine(start, start + (direction * distance), Color.red, 2f, false);
        }
    }

    private void FixedUpdate()
    {
        _animator.SetFloat("speed", _rb.velocity.magnitude);
        if (_rb.velocity.magnitude > 0)
        {
            _animator.speed = _rb.velocity.magnitude / 3f;
        }
        else
        {
            _animator.speed = 1f;
        }
        
    }

    public void Flip()
    {
        transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
        isFacingRight = transform.localScale.x > 0;
    }

    private IEnumerator Wait()
    {
        isWaiting = true;
        yield return new WaitForSeconds(waitTime);
        isWaiting = false;
    }

}
