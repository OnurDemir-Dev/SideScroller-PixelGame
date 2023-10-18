using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class BaseEnemyScript : MonoBehaviour, IDamageable
{
    public const string isplayerdetected = "isPlayerDetected";

    protected Animator animator;

    protected bool facingRight = true;
    protected bool _isPlayerDetected = false;
    public bool IsPlayerDetected
    {
        get
        {
            return _isPlayerDetected;
        }
        set
        { 
            _isPlayerDetected = value;
            animator.SetBool(isplayerdetected, value);
        }
    }
    protected bool isDeath = false;
    [SerializeField]
    protected int health = 1;

    void Start()
    {
        Enemy_Start();
    }

    void Update()
    {
        Enemy_Update();
    }

    protected virtual void Enemy_Start()
    {
        animator = GetComponent<Animator>();
    }

    protected virtual void Enemy_Update()
    {

        IsPlayerDetected = CameraScript.IsObjectInCameraView(transform.position.x);
        if(Camera.main.transform.position.x - CameraScript.screenBound.x - 0.5f > transform.position.x)
        {
            Destroy(gameObject);
        }
    }

    public virtual void TakeDamage(int damageValue)
    {
        if (!isDeath)
        {
            Debug.Log("Take Damage: " + name);
            health -= damageValue; 
        }
    }

    public virtual void Death()
    {
        Debug.Log("Death: " + name);
        isDeath = true;
    }

    protected void FlipToPlayer()
    {
        if (!isDeath)
        {
            if(PlayerScript.Player != null)
            {
                if(PlayerScript.Player.transform.position.x < transform.position.x && facingRight)
                {
                    Flip();
                }
                if(PlayerScript.Player.transform.position.x > transform.position.x && !facingRight) Flip();  
            }
        }
    }

    protected Vector2 GetNormalizeVectorForPlayer()
    {
        if(PlayerScript.Player != null)
        {
            return Vector3.Normalize(PlayerScript.Player.transform.position - transform.position);
        }
        return Vector2.right;
    }

    
    protected float GetDistanceForPlayer()
    {
        if (PlayerScript.Player != null)
        {
            return Vector3.Distance(PlayerScript.Player.transform.position, transform.position);
        }
        return 60f;
    }

    private void Flip()
    {
        facingRight = !facingRight;

        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Border")
            Debug.Log("Trigger Worked");
    }
}
