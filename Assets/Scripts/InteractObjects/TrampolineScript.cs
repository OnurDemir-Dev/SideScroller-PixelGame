using UnityEngine;

public class TrampolineScript : MonoBehaviour
{
    [SerializeField]
    private float jumpForce = 9f;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            Vector2 tempVelocity = Vector3.Normalize(transform.rotation * Vector2.up);
            PlayerScript.Player.Jump(tempVelocity * jumpForce);
        }
    }
}
