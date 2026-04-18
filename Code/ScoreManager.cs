using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    // Singleton Instance
    public static ScoreManager instance;

    private int currentScore = 0;

    [Header("UI Reference")]
    [SerializeField] private TMP_Text scoreText; 

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        
        UpdateScoreDisplay();
    }

    // Lấy điểm hiện tại (được gọi bởi PlayerCollision khi nhặt USB)
    public int GetCurrentScore()
    {
        return currentScore;
    }

    // Hàm cộng điểm (được gọi bởi mọi Enemy khi chết)
    public void AddScore(int score, Vector3 position)
    {
        if (score <= 0) return;
        
        currentScore += score;
        Debug.Log($"Score updated: +{score}. Total Score: {currentScore}");

        UpdateScoreDisplay(); 
    }

    private void UpdateScoreDisplay()
    {
        if (scoreText != null)
        {
            scoreText.text = currentScore.ToString();
        }
    }
    public int GetScore()
{
    return currentScore; // hoặc cách bạn lưu trữ điểm số
}
}