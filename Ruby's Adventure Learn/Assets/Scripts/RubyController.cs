using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RubyController : MonoBehaviour
{
    public int maxHealth;
    public float timeInvincible = 2.0f;
    public GameObject projectilePrefab;
    
    private Rigidbody2D _rubyRb;
    private Animator _animator;
    private AudioSource _audioSource;
    
    private Vector2 lookDirection = new Vector2(1,0);

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
        _animator = GetComponent<Animator>();
        _audioSource = GetComponent<AudioSource>();
        _currentHealth = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        _horizontal = Input.GetAxis("Horizontal");
        _vertical = Input.GetAxis("Vertical");
        
        if(Input.GetKeyDown(KeyCode.C))
        {
            Launch();
        }
        
        Vector2 move = new Vector2(_horizontal, _vertical);
        
        if(!Mathf.Approximately(move.x, 0.0f) || !Mathf.Approximately(move.y, 0.0f))
        {
            lookDirection.Set(move.x, move.y);
            lookDirection.Normalize();
        }
        
        _animator.SetFloat("Look X", lookDirection.x);
        _animator.SetFloat("Look Y", lookDirection.y);
        _animator.SetFloat("Speed", move.magnitude);

        if (isInvincible)
        {
            invincibleTimer -= Time.deltaTime;
            if (invincibleTimer < 0)
            {
                isInvincible = false;
            }
        }
        
        if (Input.GetKeyDown(KeyCode.X))
        {
            RaycastHit2D hit = Physics2D.Raycast(_rubyRb.position + Vector2.up * 0.2f, lookDirection, 1.5f, LayerMask.GetMask("NPC"));
            if (hit.collider != null)
            {
                NonPlayerCharacter character = hit.collider.GetComponent<NonPlayerCharacter>();
                if (character != null)
                {
                    character.DisplayDialog();
                }  
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
            _animator.SetTrigger("Hit");
            if (isInvincible)
                return;
            
            isInvincible = true;
            invincibleTimer = timeInvincible;

            
        }
        
        _currentHealth = Mathf.Clamp(_currentHealth + amount, 0, maxHealth);
        //Debug.Log(_currentHealth + "/" + maxHealth);
        UIHealthBar.instance.SetValue(_currentHealth/(float)maxHealth);
    }
    
    void Launch()
    {
        GameObject projectileObject = Instantiate(projectilePrefab, _rubyRb.position + Vector2.up * 0.5f, Quaternion.identity);

        Projectile projectile = projectileObject.GetComponent<Projectile>();
        projectile.Launch(lookDirection, 300);

        _animator.SetTrigger("Launch");
    }
    
    public void PlaySound(AudioClip clip)
    {
        _audioSource.PlayOneShot(clip);
    }
}
