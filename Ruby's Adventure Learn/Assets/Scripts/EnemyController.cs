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

    private bool broken = true;
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
        if(!broken)
        {
            return;
        }
        timer -= Time.deltaTime;

        if (timer < 0)
        {
            direction = -direction;
            timer = changeTime;
        }
    }

    private void FixedUpdate()
    {
        if(!broken)
        {
            return;
        }
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
    
    //使用 public 的原因是我们希望像飞弹脚本一样在其他地方调用这个函数
    public void Fix()
    {
        broken = false;
        enemyRb.simulated = false;
    }
}
