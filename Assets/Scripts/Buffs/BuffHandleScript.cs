using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffHandleScript : MonoBehaviour
{
    [SerializeField]
    BuffProjectileScriptable projectileScriptable;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            ProjectileScript.ProjectileStats = projectileScriptable;
            Destroy(gameObject);
        }
    }
}
