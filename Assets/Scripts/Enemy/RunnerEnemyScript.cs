using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunnerEnemyScript : BaseEnemyScript
{
    [SerializeField]
    private int damageValue = 1;

    [SerializeField]
    private Transform wallDetection;

    [SerializeField]
    LayerMask wallDetectionLayer;

    private Rigidbody2D rgbody2D;
    [SerializeField]
    private float speed = 2f;
    
    RunnerEnemyScript() : base()
    {
        health = 2;
    }

    protected override void Enemy_Start()
    {
        base.Enemy_Start();
        rgbody2D = GetComponent<Rigidbody2D>();
    }

    protected override void Enemy_Update()
    {
        base.Enemy_Update();
        if (rgbody2D == null || IsDeath) return;
        if(IsPlayerDetected || onceDetected)
        {
            rgbody2D.velocity = Vector2.left * speed + Vector2.up * rgbody2D.velocity.y;
            Collider2D wall = Physics2D.OverlapCircle(wallDetection.position, 0.1f, wallDetectionLayer);
            if (wall != null)
            {
                Vector3 theScale = transform.localScale;
                theScale.x *= -1;
                transform.localScale = theScale;
                speed *= -1;
            }
        }
        
    }

    public override void TakeDamage(int damageValue)
    {
        base.TakeDamage(damageValue);
        if(health <= 0 && !IsDeath)
        {
            Death();
        }
    }

    public override void Death()
    {
        base.Death();
        animator.SetTrigger("Death");
        Destroy(gameObject, 3f);
    }

    private void Attack()
    {
        if(PlayerScript.Player) Debug.Log("Damage");
        PlayerScript.Player.TakeDamage(damageValue);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        //Todo: Give Damage
        
        if (collision.gameObject.tag == "Player" && !IsDeath)
        {
            Attack(); 
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(wallDetection.position, 0.1f);
    }
}
