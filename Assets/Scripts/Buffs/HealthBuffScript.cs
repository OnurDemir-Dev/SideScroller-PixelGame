using UnityEngine;

public class HealthBuffScript : MonoBehaviour
{
    [SerializeField]
    private AudioClip pickupSFX;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            PlayerScript.Player.TakeDamage(-1);
            AudioManager.Instance.PlaySound(pickupSFX);
            Destroy(gameObject);
        }
    }
}
