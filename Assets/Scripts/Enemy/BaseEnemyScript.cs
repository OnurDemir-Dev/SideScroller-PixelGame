using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Animator))]
public class BaseEnemyScript : MonoBehaviour, IDamageable
{
    public const string isplayerdetected = "isPlayerDetected";

    public Animator animator;

    protected bool facingRight = true;
    protected bool _isPlayerDetected = false;
    protected bool onceDetected = false;

    [Header("Events")]
    [Space]

    public UnityEvent OnDeathEvent;

    [Header("Sounds")]
    [SerializeField]
    private AudioClip getHitSfx;

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
    protected bool _isDeath = false;

    public bool IsDeath
    {
        get
        { 
            return  _isDeath; 
        }
        set
        {
            _isDeath = value;
        }
    }

    public int health = 1;
    public string enemyName = "Enemy";
    void Start()
    {
        Enemy_Start();
    }

    void Update()
    {
        if (!PlayerScript.Player.IsDeath)
        {
            Enemy_Update(); 
        }
    }

    protected virtual void Enemy_Start()
    {
        animator = GetComponent<Animator>();
    }

    protected virtual void Enemy_Update()
    {

        IsPlayerDetected = CameraScript.IsObjectInCameraView(transform.position.x);
        if(!onceDetected && IsPlayerDetected)onceDetected = true;
        if (Camera.main.transform.position.x - CameraScript.screenBound.x - 0.5f > transform.position.x)
        {
            Destroy(gameObject);
        }
    }

    public virtual void TakeDamage(int damageValue)
    {
        if (!IsDeath && onceDetected)
        {
            AudioManager.Instance.PlaySound(getHitSfx);
            Debug.Log("Take Damage: " + name);
            health -= damageValue; 
        }
    }

    public virtual void Death()
    {
        Debug.Log("Death: " + name);
        if (OnDeathEvent != null)
        {
            OnDeathEvent.Invoke();
        }
        IsDeath = true;
    }

    protected void FlipToPlayer()
    {
        if (!IsDeath && onceDetected)
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
}
