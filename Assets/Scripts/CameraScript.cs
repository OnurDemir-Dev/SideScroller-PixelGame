using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class CameraScript : MonoBehaviour
{
    GameObject playerObj;
    [SerializeField]
    Transform boundObj;

    public static Vector3 screenBound { get; private set; }
    private void Awake()
    {
        playerObj = GameObject.FindGameObjectWithTag("Player");
        screenBound = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.transform.position.z));
    }

    void Start()
    {
        UpdateBound();
    }

    private void Update()
    {
        
        if (playerObj != null)
        {
            Vector3 camPos = transform.position;
            if (playerObj.transform.position.x > camPos.x)
            {
                camPos.x = playerObj.transform.position.x;
                transform.position = camPos;
            }
        }
    }

    private void UpdateBound()
    {
        boundObj.transform.position = transform.position + ((screenBound.x + 0.5f) * Vector3.left);
    }

    public static bool IsObjectInCameraView(float targetPosX)
    {
        return targetPosX < Camera.main.transform.position.x + screenBound.x && targetPosX > Camera.main.transform.position.x - screenBound.x;
    }
}
