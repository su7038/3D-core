using UnityEngine;
using UnityEngine.SceneManagement;

public class LogicScript : MonoBehaviour
{
    public int playerScore = 0;
    public TMPro.TextMeshProUGUI scoreText;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Add Score
     public void AddScore(int scoreToAdd)
    {
        playerScore = playerScore + scoreToAdd;
        scoreText.text = "Score: " + playerScore.ToString();
    }   

        public void RestartGame()
    {
        
        SceneManager.LoadScene("Menu");
    }
}
