using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RubyController : MonoBehaviour
{
    public int maxHealth;
    public float timeInvincible = 2.0f;
    
    private Rigidbody2D _rubyRb;

    private float _horizontal;

    private float _vertical;
    
    private int _currentHealth;
    public int Health
    {
        get { return _currentHealth; }
    }

    private bool isInvincible;

    private float invincibleTimer;
    // Start is called before the first frame update
    void Start()
    {
        //QualitySettings.vSyncCount = 0;
        //Application.targetFrameRate = 10;
        _rubyRb = GetComponent<Rigidbody2D>();
        _currentHealth = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        _horizontal = Input.GetAxis("Horizontal");
        _vertical = Input.GetAxis("Vertical");

        if (isInvincible)
        {
            invincibleTimer -= Time.deltaTime;
            if (invincibleTimer < 0)
            {
                isInvincible = false;
            }
        }
    }

    private void FixedUpdate()
    {
        Vector2 position = transform.position;
        position.x = position.x + 3.0f * _horizontal * Time.deltaTime;
        position.y = position.y + 3.0f * _vertical * Time.deltaTime;
        _rubyRb.MovePosition(position);
    }
    
    public void ChangeHealth(int amount)
    {
        if (amount < 0)
        {
            if (isInvincible)
                return;
            
            isInvincible = true;
            invincibleTimer = timeInvincible;
        }
        
        _currentHealth = Mathf.Clamp(_currentHealth + amount, 0, maxHealth);
        Debug.Log(_currentHealth + "/" + maxHealth);
    }
}
