using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedEnemy : MonoBehaviour
{
    public int UnitsToMove = 5;
    
    public int speed = 500;
    public bool _isFacingRight;

    public int health;
    private int currentHealth;
    private float startPos;
    Rigidbody2D _rb;
    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        startPos = _rb.position.x;
        currentHealth = health;
    }

    // Update is called once per frame
    void Update()
    {
        _rb.AddForce(Vector2.left*speed*Time.deltaTime, ForceMode2D.Impulse);
    }

    
}
