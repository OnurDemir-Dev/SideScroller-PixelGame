using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class EventTrigger : MonoBehaviour
{
    [SerializeField]
    private bool isOnceDestroy = true;

    [SerializeField]
    private string[] triggerTags;

    [Header("Events")]
    [Space]

    [SerializeField]
    public UnityEvent OnTriggerEnterEvent;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (OnTriggerEnterEvent != null)
        {
            foreach (string tag in triggerTags) 
            {
                if (collision.tag == tag)
                {
                    OnTriggerEnterEvent.Invoke();
                    if (isOnceDestroy) Destroy(gameObject);
                    return;
                }
            }
        }
    }
}
