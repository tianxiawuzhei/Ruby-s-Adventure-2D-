using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float speed = 2.0f;
    public bool vertical;
    public float changeTime = 3.0f;
    
    private Rigidbody2D enemyRb;
    private Animator _animator;
    float timer;
    int direction = 1;
    // Start is called before the first frame update
    void Start()
    {
        enemyRb = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        timer = changeTime;
    }

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;

        if (timer < 0)
        {
            direction = -direction;
            timer = changeTime;
        }
    }

    private void FixedUpdate()
    {
        Vector2 position = enemyRb.position;
        if (vertical)
        {
            position.y = position.y + Time.deltaTime * speed* direction;
            _animator.SetFloat("Move X", 0);
            _animator.SetFloat("Move Y", direction);
            
        }
        else
        {
            position.x = position.x + Time.deltaTime * speed* direction;
            _animator.SetFloat("Move X", direction);
            _animator.SetFloat("Move Y", 0);
        }
        enemyRb.MovePosition(position);
        
        
    }
    
    void OnCollisionEnter2D(Collision2D other)
    {
        RubyController player = other.gameObject.GetComponent<RubyController>();

        if (player != null)
        {
            player.ChangeHealth(-1);
        }
    }
}
