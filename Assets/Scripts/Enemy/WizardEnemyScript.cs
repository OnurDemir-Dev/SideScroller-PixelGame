using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WizardEnemyScript : BaseEnemyScript
{
    [SerializeField]
    private GameObject projectile;

    protected override void Enemy_Start()
    {
        base.Enemy_Start();
        animator.SetBool("isPlayerDetected", true);
    }

    protected override void Enemy_Update()
    {
        base.Enemy_Update();
        FlipToPlayer();
    }

    public override void TakeDamage(int damageValue)
    {
        base.TakeDamage(damageValue);
        if (health <= 0 && !isDeath)
        {
            Death();
        }
    }

    //AnimatonEvent
    public override void Death()
    {
        base.Death();
        animator.SetTrigger("Death");
        Destroy(gameObject, 3f);

    }

    //AnimationEvent
    public void Attack()
    {
        GameObject instProjectile = Instantiate(projectile, gameObject.transform.position, Quaternion.identity);
        Debug.Log(GetNormalizeVectorForPlayer());
        instProjectile.GetComponent<ProjectileScript>().Direction = GetNormalizeVectorForPlayer();
    }

    
    
}
