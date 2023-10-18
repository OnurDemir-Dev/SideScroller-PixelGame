using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundsScript : MonoBehaviour
{
    Vector2 camStartPosition;
    Camera mainCamera;

    private void Awake()
    {
        mainCamera = Camera.main;
        camStartPosition = mainCamera.transform.position;
    }

    void Start()
    {
        
    }


    void FixedUpdate()
    {
        Vector2 backgroundsPos = transform.position;
        backgroundsPos.x = mainCamera.transform.position.x;
        transform.position = backgroundsPos;

    }
}
