using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunnerEnemyScript : BaseEnemyScript
{
    private Rigidbody2D rgbody2D;
    [SerializeField]
    private float speed = 2f;
    private bool onceDetected = false;
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
        if (rgbody2D == null || isDeath) return;
        if(IsPlayerDetected || onceDetected)
        {
            onceDetected = true;
            rgbody2D.velocity = Vector2.left * speed + Vector2.up * rgbody2D.velocity.y;
        }
        
    }

    public override void TakeDamage(int damageValue)
    {
        base.TakeDamage(damageValue);
        if(health <= 0)
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
}
