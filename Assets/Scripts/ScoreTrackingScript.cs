using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
public class ScoreTrackingScript : MonoBehaviour
{
    public int playerScore = 0;
    public LogicScript logic;
    public TMPro.TextMeshProUGUI scoreText;

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
        SceneManager.sceneLoaded += OnSceneLoaded;
        Debug.Log("ScoreTrackingScript aangemaakt, score = " + playerScore);
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("Scene geladen: " + scene.name);

        if (scene.name == "Menu")
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
            Destroy(gameObject);
        }

        if (scene.name == "Game Over")
        {
            Debug.Log("Game Over scene gedetecteerd, playerScore = " + playerScore);
            StartCoroutine(SetScoreText());
        }
    }

    IEnumerator SetScoreText()
    {
        yield return null; // wacht één frame

        Debug.Log("SetScoreText gestart na 1 frame wachten");

        GameObject scoreObject = GameObject.FindGameObjectWithTag("Score");

        if (scoreObject != null)
        {
            Debug.Log("Score object gevonden: " + scoreObject.name);
            scoreText = scoreObject.GetComponent<TMPro.TextMeshProUGUI>();

            if (scoreText != null)
            {
                scoreText.text = "Final Score: " + playerScore.ToString();
                Debug.Log("Tekst succesvol gezet naar: " + scoreText.text);
            }
            else
            {
                Debug.LogError("TextMeshProUGUI component NIET gevonden op " + scoreObject.name);
            }
        }
        else
        {
            Debug.LogError("Geen GameObject gevonden met tag 'Score'! Check de tag in de Game Over scene.");
        }
    }

    public void AddScore(int scoreToAdd)
    {
        playerScore += scoreToAdd;
        Debug.Log("Score toegevoegd: +" + scoreToAdd + " | Totaal: " + playerScore);
    }
}
