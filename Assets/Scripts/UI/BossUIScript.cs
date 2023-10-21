using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossUIScript : MonoBehaviour
{
    [HideInInspector]
    public BaseEnemyScript bossEnemyScript;

    [SerializeField]
    private Text bossNameText;

    [SerializeField]
    private Slider bossHealthSlider;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (bossEnemyScript)
        {
            bossHealthSlider.value = bossEnemyScript.health; 
        }
    }

    public void SetBossActive(BaseEnemyScript _bossEnemyScript)
    {
        if (_bossEnemyScript)
        {
            gameObject.SetActive(true);
            bossHealthSlider.maxValue = _bossEnemyScript.health;
            bossHealthSlider.value = _bossEnemyScript.health;

            bossNameText.text = _bossEnemyScript.enemyName;
            bossEnemyScript = _bossEnemyScript;
        }
    }
}
