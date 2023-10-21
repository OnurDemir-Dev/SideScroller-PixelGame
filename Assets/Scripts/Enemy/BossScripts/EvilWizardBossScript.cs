using UnityEngine;
using System.Collections.Generic;

public class EvilWizardBossScript : BaseEnemyScript
{
    [SerializeField]
    private float bossSpeed = 5f;

    float bossXlimit;

    //Components
    Rigidbody2D rgbody2D;


    //Wizards
    const int maxwizardnum = 2;
    private List<GameObject> wizards = new List<GameObject>();
    private int currentWizardCount = 0;


    protected override void Enemy_Start()
    {
        base.Enemy_Start();
        rgbody2D = GetComponent<Rigidbody2D>();
        bossXlimit =  CameraScript.screenBound.x - 0.5f;
    }

    protected override void Enemy_Update()
    {
        base.Enemy_Update();
        if (onceDetected)
        {
            FlipToPlayer();
            RunToPlayer(); 
        }
    }

    public override void TakeDamage(int damageValue)
    {
        base.TakeDamage(damageValue);
    }

    public override void Death()
    {
        base.Death();
    }

    private void RunToPlayer()
    {
        if (GetDistanceForPlayer() > 1f)
        {
            rgbody2D.velocity = Vector2.right * bossSpeed * (facingRight ? 1 : -1); 
        }
        else
        {
            rgbody2D.velocity = Vector2.zero;
        }
    }

    private bool ControlLimitX(float targetX)
    {
        return targetX < transform.parent.position.x - bossXlimit || targetX > transform.parent.position.x + bossXlimit;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(transform.parent.position, new Vector3(CameraScript.screenBound.x * 2 - 1f, CameraScript.screenBound.y * 2 ,20f));
    }
}
