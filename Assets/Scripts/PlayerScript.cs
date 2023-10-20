using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;

public class PlayerScript : MonoBehaviour, IDamageable
{
    public static PlayerScript Player;

    [Range(0, .3f)][SerializeField] private float movementSmoothing = .05f;
    [SerializeField] private bool m_AirControl = false;
    [SerializeField] private LayerMask whatIsGround;
    [SerializeField] private Transform groundCheck;

    //Stats
    [Header("Stats")]
    [SerializeField] private float moveSpeed = 4f;
    [Range(0, 1f)][SerializeField] private float airControlSpeed = 0.8f;
    [SerializeField] private float jumpForce = 1.5f;
    [SerializeField] private float startTimeWaitAttack;

    public int maxHealth = 6, _currentHealth = 0;
    public int CurrentHealth
    {
        get { return _currentHealth; }
        set 
        {
            if (value > maxHealth)
            {
                _currentHealth = maxHealth;
            }
            else if (value < 0)
            {
                _currentHealth = 0;
            }
            else
            {
                _currentHealth = value;
                
            }
        }
        
    }

    [SerializeField] private bool _isMale = true;
    private bool _isAttacking = false;
    private bool _isGrounded;
    private bool _isDeath = false;
    public bool IsMale
    {
        get
        {
            return _isMale;
        }
        set
        {
            playerAnimator.SetBool("isMale", _isMale);
            _isMale = value;
        }

    }

    public bool IsAttacking
    {
        get
        {
            return _isAttacking;
        }
        set
        {
            _isAttacking = value;
            playerAnimator.SetBool("isAttacking", _isAttacking);
        }
    }

    public bool IsGrounded
    {
        get
        {
            return _isGrounded;
        }
        set
        {
            _isGrounded = value;
            playerAnimator.SetBool("isGrounded", _isGrounded);
        }
    }

    public bool IsDeath
    {
        get
        {
            return _isDeath;
        }
        set
        {
            _isDeath = value;
            
        }
    }

    const float groundedRadius = .2f;
    
    [HideInInspector]
    public bool facingRight = true;
    private Vector3 playerVelocity = Vector3.zero;


    //Components
    public Rigidbody2D rgbody2D;
    [HideInInspector]
    public Animator playerAnimator;
    [SerializeField] private GameObject projectilePrefab;


    private void Awake()
    {
        rgbody2D = GetComponent<Rigidbody2D>();
        playerAnimator = GetComponent<Animator>();

        CurrentHealth = maxHealth;

        if(Player == null || Player != this)
        {
            Player = this;
        }
    }

    private void Start()
    {
        IsMale = true;
    }

    private void FixedUpdate()
    {
        if (IsDeath) return;
        IsGrounded = false;
        Collider2D[] colliders = Physics2D.OverlapCircleAll(groundCheck.position, groundedRadius, whatIsGround);
        if (colliders.Length > 0)
            IsGrounded = true;

        bool isJump = Input.GetButton("Jump");
        float horizontalMove = Input.GetAxisRaw("Horizontal");


        playerAnimator.SetFloat("Y Velocity", rgbody2D.velocity.y);
        playerAnimator.SetBool("isJumping", false);

        Move(horizontalMove, isJump);
        
    }

    private void Update()
    {
        if (IsDeath) return;
        Attack();
        FallDamage();
    }

    public void Move(float move, bool jump)
    {
        if (IsAttacking && IsGrounded)
        {
            move = 0;
        }
        playerAnimator.SetFloat("Speed", Mathf.Abs(move));
        Vector3 targetVelocity = new Vector2(move * moveSpeed * (!IsGrounded ? airControlSpeed : 1), rgbody2D.velocity.y);
        rgbody2D.velocity = Vector3.SmoothDamp(rgbody2D.velocity, targetVelocity, ref playerVelocity, movementSmoothing);

        if(move < 0 && facingRight)
        {
            Flip();
        }
        else if(move > 0 && !facingRight)
        {
            Flip();
        }

        if(jump && IsGrounded)
        {
            Jump((Vector2.up * jumpForce) + rgbody2D.velocity);
        }
    }

    public void Jump(Vector2 _jumpdirection)
    {
        IsGrounded = false;
        IsAttacking = false;
        playerAnimator.SetBool("isJumping", true);
        rgbody2D.velocity = _jumpdirection;
    }

    private void Flip()
    {
        facingRight = !facingRight;

        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

    private void Attack()
    {
        if (Input.GetButtonDown("Fire1") && !IsAttacking)
        {
            
            IsAttacking = true;
            playerAnimator.SetTrigger("Attack");
        }
    }

    public void StopAttacking()
    {
        IsAttacking = false;
    }

    private void SpawnProjectile()
    {
        GameObject instGameobj = GameObject.Instantiate(projectilePrefab, transform.position, Quaternion.identity);
        ProjectileScript tempProjectileScript = instGameobj.GetComponent<ProjectileScript>();
        tempProjectileScript.Direction = facingRight ? Vector2.right : Vector2.left;
        tempProjectileScript.SetProjectileStats();
    }

    public void TakeDamage(int damageValue)
    {
        CurrentHealth -= damageValue;
        MainGameCanvasScript.mainGameCanvas.UpdateHealthBar();
        if (CurrentHealth <= 0 && !IsDeath)
        {
            Death();
        }
    }

    private void FallDamage()
    {
        if (Camera.main.transform.position.y - CameraScript.screenBound.y - 2f > transform.position.y)
        {
            TakeDamage(1);
            transform.position = new Vector2(Camera.main.transform.position.x - 4f, 3.7f); 
        }
    }

    public void Death()
    {
        IsDeath = true;
        playerAnimator.SetTrigger("Death");
        playerAnimator.SetBool("isDeath", IsDeath);
        rgbody2D.velocity = Vector2.zero;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(groundCheck.position, groundedRadius);
    }

    
}
