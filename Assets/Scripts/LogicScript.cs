using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class LogicScript : MonoBehaviour
{
    public ScoreTrackingScript scoreTracker;
    public TMPro.TextMeshProUGUI scoreText;
    public GameObject settingsPanel;
    public UnityEngine.UI.Slider sensitivitySlider;
    public PlayerMovement playerMovement;
    public TMPro.TextMeshProUGUI SensitivityText;

    void Awake()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void Start()
    {
        if (sensitivitySlider != null && playerMovement != null)
        {
            sensitivitySlider.value = playerMovement.mouseSensitivity;
            sensitivitySlider.onValueChanged.AddListener(OnSensitivityChanged);
        }
    }

    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name != "Main Game")
        {
            Time.timeScale = 1f;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    public void AddScore(int scoreToAdd)
    {
        if (scoreTracker != null)
        {
            scoreTracker.AddScore(scoreToAdd);
            scoreText.text = "Score: " + scoreTracker.playerScore.ToString();
        }
    }

    public void RestartGame()
    {
        SceneManager.LoadScene("Menu");
    }

    public void StartGame()
    {
        SceneManager.LoadScene("Main Game");
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    void Update()
    {
        if (SceneManager.GetActiveScene().name != "Main Game") return;

        if (Keyboard.current.digit0Key.wasPressedThisFrame)
        {
            settingsPanel.SetActive(!settingsPanel.activeSelf);
        }

        Time.timeScale = settingsPanel.activeSelf ? 0f : 1f;

        if (settingsPanel.activeSelf)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    void OnSensitivityChanged(float value)
    {
        playerMovement.mouseSensitivity = value;
        SensitivityText.text = "Sensitivity: " + value.ToString("F1");
    }
}
