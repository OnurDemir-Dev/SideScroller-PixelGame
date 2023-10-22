using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainGameCanvasScript : MonoBehaviour
{
    public static MainGameCanvasScript mainGameCanvas;

    [SerializeField]
    private GameObject healthBar;
    private List<GameObject> healths = new List<GameObject>();

    //Gameover UI
    const string gameoverstring = "Game Over", puasetext = "Pause Game";

    [Header("GameOver UI")]
    [SerializeField]
    private GameObject gameOverScreen;
    [SerializeField]
    private Text gameOverText;

    [Space]
    [Header("Sounds")]
    [SerializeField]
    private AudioClip pauseAudio;

    public static bool isGameOver = false;

    private void Start()
    {
        Time.timeScale= 1.0f;
        mainGameCanvas = this;
        isGameOver = false;
        foreach (Transform heart in healthBar.transform)
        {
            healths.Add(heart.gameObject);
        }
    }

    private void Update()
    {
        if (Input.GetButtonDown("Cancel"))
        {
            ToggleGameOverUI();
        }
    }

    public void UpdateHealthBar()
    {
        for (int i = 0; i < healths.Count; i++)
        {
            healths[i].SetActive(PlayerScript.Player.CurrentHealth > i);
        }
    }

    public void ToggleGameOverUI()
    {
        if (isGameOver)
        {
            gameOverText.text = gameoverstring;
            gameOverScreen.SetActive(true);
        }
        else if (!isGameOver)
        {
            gameOverScreen.SetActive(!gameOverScreen.activeSelf);
            if (gameOverScreen.activeSelf)
            {
                Time.timeScale = 0f;
                AudioManager.Instance.PlaySound(pauseAudio);
            }
            else
            {
                Time.timeScale = 1f;
            }
        }
    }

    public void ToggleMuteMusicButton()
    {
        AudioManager.Instance.ToggleMusic();
    }

    public void ToggleMuteSoundButton()
    {
        AudioManager.Instance.ToggleSound();
    }

    public static void ResetMap()
    {
        Debug.Log("ResetMap");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void ReturnToMainMenu()
    {
        Debug.Log("Return to MainMenu");
        SceneManager.LoadScene("MainMenu");
        
    }

    public static void QuitGame()
    {
        Debug.Log("Quit Game");
        Application.Quit();
    }
}
