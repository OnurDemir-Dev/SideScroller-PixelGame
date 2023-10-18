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

    public int maxHealth = 4, currentHealth = 0;

    [SerializeField] private bool _isMale = true;
    private bool _isAttacking = false;
    private bool _isGrounded;
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

    const float groundedRadius = .2f;
    const float attackRadius = .4f;
    
    [HideInInspector]
    public bool facingRight = true;
    private Vector3 playerVelocity = Vector3.zero;


    //Components
    private Rigidbody2D rgbody2D;
    private Animator playerAnimator;
    [SerializeField] private GameObject projectilePrefab;


    private void Awake()
    {
        rgbody2D = GetComponent<Rigidbody2D>();
        playerAnimator = GetComponent<Animator>();

        currentHealth = maxHealth;

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
        Attack();
    }

    public void Move(float move, bool jump)
    {
        if (IsAttacking && IsGrounded)
        {
            move = 0;
        }
        Vector3 targetVelocity = new Vector2(move * moveSpeed * (!IsGrounded ? airControlSpeed : 1), rgbody2D.velocity.y);
        rgbody2D.velocity = Vector3.SmoothDamp(rgbody2D.velocity, targetVelocity, ref playerVelocity, movementSmoothing);
        playerAnimator.SetFloat("Speed", Mathf.Abs(move));

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
            IsGrounded = false;
            IsAttacking = false;
            playerAnimator.SetBool("isJumping", true);
            rgbody2D.velocity += Vector2.up * jumpForce;
        }
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
        instGameobj.GetComponent<ProjectileScript>().Direction = facingRight ? Vector2.right : Vector2.left;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(groundCheck.position, groundedRadius);
    }

    public void TakeDamage(int damageValue)
    {
        
    }

    public void Death()
    {

    }
}
