using System.Collections.Generic;
using UnityEngine;

public class MainGameCanvasScript : MonoBehaviour
{
    public static MainGameCanvasScript mainGameCanvas;

    [SerializeField]
    private GameObject healthBar;
    private List<GameObject> healths = new List<GameObject>();

    private void Start()
    {
        mainGameCanvas = this;

        foreach (Transform heart in healthBar.transform)
        {
            healths.Add(heart.gameObject);
        }
    }

    public void UpdateHealthBar()
    {
        for (int i = 0; i < healths.Count; i++)
        {
            healths[i].SetActive(PlayerScript.Player.CurrentHealth > i);
        }
    }
}
