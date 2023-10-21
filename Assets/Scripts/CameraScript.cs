using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class CameraScript : MonoBehaviour
{
    [SerializeField]
    Transform boundLObj;
    [SerializeField]
    Transform boundRObj;

    [SerializeField]
    private float smoothTime = 10f;

    [SerializeField]
    private bool isStationary = false;
    public bool IsStationary
    {
        get
        { 
            return isStationary; 
        }
        set 
        { 
            isStationary = value;
            boundRObj.gameObject.SetActive(value);
        }
    }

    private Vector2 smoothVector = Vector2.zero;

    public static Vector3 screenBound { get; private set; }
    private void Awake()
    {
        screenBound = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.transform.position.z));
    }

    void Start()
    {
        UpdateBound();

#if !UNITY_EDITOR
        Vector3 camPos = transform.position;
        camPos.x = PlayerScript.Player.transform.position.x;
        transform.position = camPos; 
#endif
    }

    private void FixedUpdate()
    {
        if (!IsStationary)
        {
            if (PlayerScript.Player.gameObject != null)
            {
                Vector3 camPos = transform.position;
                if (PlayerScript.Player.gameObject.transform.position.x > camPos.x)
                {
                    camPos.x = Vector2.SmoothDamp(transform.position, PlayerScript.Player.gameObject.transform.position, ref smoothVector, smoothTime * Time.fixedDeltaTime).x;
                    transform.position = camPos;
                }
            } 
        }
    }

    private void UpdateBound()
    {
        boundLObj.transform.position = transform.position + ((screenBound.x + 0.5f) * Vector3.left);
        boundRObj.transform.position = transform.position + ((screenBound.x + 0.5f) * Vector3.right);
    }

    public static bool IsObjectInCameraView(float targetPosX)
    {
        return targetPosX < Camera.main.transform.position.x + screenBound.x && targetPosX > Camera.main.transform.position.x - screenBound.x;
    }

    public static void SetStationary(bool _isStationary)
    {
        CameraScript tempCameraScript = Camera.main.GetComponent<CameraScript>();
        tempCameraScript.IsStationary = _isStationary;
        if (tempCameraScript.IsStationary)
        {
            tempCameraScript.transform.position = new Vector3(PlayerScript.Player.transform.position.x, tempCameraScript.transform.position.y, tempCameraScript.transform.position.z);
        }
    }
}
