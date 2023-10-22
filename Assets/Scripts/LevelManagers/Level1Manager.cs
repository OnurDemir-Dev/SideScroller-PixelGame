using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level1Manager : MonoBehaviour
{
    [SerializeField]
    private AudioClip levelMusic;

    void Start()
    {
        AudioManager.Instance.PlayMusic(levelMusic);
    }


}
