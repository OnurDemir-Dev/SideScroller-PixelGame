using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class ProjectileScript : MonoBehaviour
{
    [SerializeField]
    private BuffProjectileScriptable defaultProjectile;
    public static BuffProjectileScriptable ProjectileStats;

    [SerializeField]
    private SpriteRenderer spriteRenderer;


    [SerializeField] private string targetTag;
    public float speed = 8f;
    public float destroyTime = 2f;
    public int damageValue = 1;
    [SerializeField] private Rigidbody2D rgBody2D;
    private Vector2 _direction;
    public Vector2 Direction
    {
        get 
        {
            return _direction; 
        }
        set 
        {
            _direction = value;
            rgBody2D.velocity = speed * _direction;
            
            float angle = Mathf.Atan2(_direction.y, _direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
    }

    
    void Start()
    {
        if (ProjectileStats == null) ProjectileStats = defaultProjectile;

        rgBody2D = GetComponent<Rigidbody2D>();

        Destroy(gameObject, destroyTime);
    }

    void Update()
    {
    }

    public void SetProjectileStats() 
    {
        if (ProjectileStats)
        {
            spriteRenderer.sprite = ProjectileStats.buffSprite;
            GetComponent<BoxCollider2D>().size = ProjectileStats.colliderSize;

            rgBody2D.velocity = speed * _direction * ProjectileStats.speed;
            PlayerScript.Player.playerAnimator.speed = ProjectileStats.playerSpeed;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == targetTag)
        {
            IDamageable tempDamagable = collision.GetComponent<IDamageable>();
            tempDamagable.TakeDamage(damageValue);
            Debug.Log("Give Damage: " + damageValue.ToString() + ", " + collision.name);
            if (ProjectileStats.onceDestroy && !tempDamagable.IsDeath)
            {
                Destroy(gameObject);
            }
        }
    }
}
