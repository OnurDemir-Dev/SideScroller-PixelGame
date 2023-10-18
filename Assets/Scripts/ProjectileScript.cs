using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileScript : MonoBehaviour
{
    [SerializeField] private string targetTag;
    public float speed = 8f;
    public float destroyTime = 3f;
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
        }
    }

    
    void Start()
    {
        rgBody2D = GetComponent<Rigidbody2D>();
        Destroy(gameObject, destroyTime);
    }

    void Update()
    {
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == targetTag)
        {
            collision.GetComponent<IDamageable>().TakeDamage(1);
        }
    }
}
