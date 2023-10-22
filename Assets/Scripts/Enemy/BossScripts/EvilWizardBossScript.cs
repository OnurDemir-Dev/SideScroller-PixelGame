using UnityEngine;
using System.Collections.Generic;
using System.Xml.Serialization;

public class EvilWizardBossScript : BaseEnemyScript
{
    private enum BossStates
    {
        Idle,
        Runing,
        Attacking,
    }

    BossStates currentState = BossStates.Idle;

    const int meleedamage = 1;

    [SerializeField]
    private float bossSpeed = 5f;

    float bossXlimit;

    //Components
    Rigidbody2D rgbody2D;

    //Spawn Buff
    [SerializeField]
    private GameObject healBuffPrefab;
    [SerializeField]
    private Transform buffSpawnTransform;

    private float spawnBuffWaitTime = 20f;
    private float spawnBuffTime = 0f;

    //Wizards
    private float summonWaitTime = 8f;
    private float summonTime = 0f;

    private List<GameObject> wizards = new List<GameObject>();
    [SerializeField]
    private Transform[] wizardSpawnPoints = new Transform[2];
    [SerializeField]
    private GameObject wizardPrefab;

    //Attack
    private float attackWaitTime = 3f;
    private float attackTime = 0f;

    [SerializeField]
    private GameObject evilWizardProjectile;
    [SerializeField]
    private Transform meleeAttackTransform;

    protected override void Enemy_Start()
    {
        base.Enemy_Start();
        rgbody2D = GetComponent<Rigidbody2D>();
        bossXlimit =  CameraScript.screenBound.x - 0.5f;
    }

    protected override void Enemy_Update()
    {
        base.Enemy_Update();
        if (onceDetected && !IsDeath)
        {
            FlipToPlayer();
            RunToPlayer();
            SummonWizards();
            SpawnBuff();
        }
    }

    public override void TakeDamage(int damageValue)
    {
        base.TakeDamage(damageValue);
        if (health <= 0 && !IsDeath)
        {
            Death();
        }
    }

    public override void Death()
    {
        base.Death();
        animator.SetTrigger("Death");
        CameraScript.SetStationary(false);
        foreach (GameObject wizardobj in wizards)
        {
            wizardobj.GetComponent<BaseEnemyScript>().Death();
        }
    }

    private void RunToPlayer()
    {
        if (currentState != BossStates.Attacking)
        {
            attackTime += Time.deltaTime;
            if (attackTime >= attackWaitTime || currentState == BossStates.Runing)
            {
                currentState = BossStates.Runing;
                attackTime = 0f;
                if (PlayerScript.Player.transform.position.y < Camera.main.transform.position.y)
                {
                    if (Mathf.Abs(transform.position.x - PlayerScript.Player.transform.position.x) > 1f && ControlLimitX(transform.position.x + 0.3f * (facingRight ? 1 : -1)))
                    {
                        rgbody2D.velocity = Vector2.right * bossSpeed * (facingRight ? 1 : -1);
                        currentState = BossStates.Runing;
                    }
                    else
                    {
                        AttackToPlayerMelee();
                        rgbody2D.velocity = Vector2.zero;
                        //AttackToPlayer();
                    } 
                }
                else
                {
                    rgbody2D.velocity = Vector2.zero;
                    AttackToPlayerRanged();
                }
            }
            animator.SetFloat("VelocityX", Mathf.Abs(rgbody2D.velocity.x));
        }
    }

    private void AttackToPlayerMelee()
    {
        currentState = BossStates.Attacking;
        animator.SetTrigger("MeleeAttack");
    }
    private void AttackToPlayerRanged()
    {
        currentState = BossStates.Attacking;
        animator.SetTrigger("RangedAttack");
    }

    public void MeleeAttack()
    {
        if (Physics2D.OverlapCircle(meleeAttackTransform.position, meleeAttackTransform.localScale.x, (int)Mathf.Pow(2f, LayerMask.NameToLayer("Player"))))
        {
            if (PlayerScript.Player)
            {
                PlayerScript.Player.TakeDamage(meleedamage);
            }
        }
    }

    public void RangedAttack()
    {
        GameObject instProjectile = Instantiate(evilWizardProjectile, gameObject.transform.position, Quaternion.identity);
        instProjectile.GetComponent<ProjectileScript>().Direction = GetNormalizeVectorForPlayer();
    }

    public void SetBossCurrentStateIdle()
    {
        currentState = BossStates.Idle;
    }

    private void SummonWizards()
    {
        if (wizards.Count <= 0)
        {
            summonTime += Time.deltaTime;
            if (summonTime >= summonWaitTime)
            {
                foreach (Transform wizardSpawnpoint in wizardSpawnPoints)
                {
                    GameObject tempwizard = Instantiate(wizardPrefab, wizardSpawnpoint.position, Quaternion.identity);
                    wizards.Add(tempwizard);

                    BaseEnemyScript tempwizardscript = tempwizard.GetComponent<BaseEnemyScript>();
                    tempwizardscript.health = 2;
                    tempwizardscript.OnDeathEvent.AddListener(() =>
                    {
                        if (!IsDeath)
                        {
                            wizards.Remove(tempwizard); 
                        }
                    });
                }
                summonWaitTime = Random.Range(6, 10);
                summonTime = 0;
            } 
        }
    }

    private void SpawnBuff()
    {
        if (healBuffPrefab)
        {
            spawnBuffTime += Time.deltaTime;
            if (spawnBuffTime >= spawnBuffWaitTime)
            {
                spawnBuffTime = 0;
                float randomX = Random.Range(buffSpawnTransform.position.x - bossXlimit, buffSpawnTransform.position.x + bossXlimit);
                Instantiate(healBuffPrefab, Vector2.right * randomX + Vector2.up * buffSpawnTransform.position.y, Quaternion.identity);
            }
        }
    }

    private bool ControlLimitX(float targetX)
    {
        return targetX > transform.parent.position.x - bossXlimit && targetX < transform.parent.position.x + bossXlimit;
    }
}
