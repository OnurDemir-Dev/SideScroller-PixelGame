using UnityEngine;

public class HealthBuffScript : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            PlayerScript.Player.TakeDamage(-1);
            Destroy(gameObject);
        }
    }
}
