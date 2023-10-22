using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level2Manager : MonoBehaviour
{
    [Header("ThreeWizard")]
    [SerializeField]
    private GameObject[] wizardTrambolines = new GameObject[2];
    private int threeWizardCount = 0;

    [Space]
    [Header("Sound")]
    [SerializeField]
    private AudioClip levelMusic;

    private void Start()
    {
        AudioManager.Instance.PlayMusic(levelMusic);
    }

    public void Event_ThreeWizard()
    {
        threeWizardCount++;
        if (threeWizardCount >= 3)
        {
            foreach (GameObject tramboline in wizardTrambolines)
            {
                tramboline.SetActive(true);
            }
            CameraScript.SetStationary(false);
        }
    } 

    public void Event_EvilWizardEnter(Transform targetTransform)
    {
        Vector3 camPos = Camera.main.transform.position;
        camPos.x = targetTransform.position.x;
        Camera.main.transform.position = camPos;
    }
}
