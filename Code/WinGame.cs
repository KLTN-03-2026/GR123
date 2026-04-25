using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using TMPro;

public class WinGame : MonoBehaviour
{    
    [Header("Game Manager Reference")]
    // Tham chiếu đến GameManager, được sử dụng cho nút Next Level và Play Again
    [SerializeField] private GameManager gameManager;

    [Header("Victory Panel")]
    [SerializeField] private GameObject panelVictory;

    [Header("Score Text")]
    [SerializeField] private TMP_Text textTarget;
    [SerializeField] private TMP_Text textScore;

    // 🔥 PHẦN ĐÃ THÊM: TEXT HIỂN THỊ COIN VÀ GEM THƯỞNG 🔥
    [Header("Currency Text")]
    [SerializeField] private TMP_Text textCoinReward; 
    [SerializeField] private TMP_Text textGemReward;
    // 🔥 KẾT THÚC PHẦN THÊM TIỀN TỆ 🔥

    [Header("Stars")]
    [SerializeField] private Image[] stars; 
    [SerializeField] private Sprite spriteStarOn; 
    [SerializeField] private Sprite spriteStarOff;

    private void Start()
    {
        if (panelVictory == null)
        {
            Debug.LogError("WinGame: panelVictory is NOT assigned in the Inspector. Script disabled.");
            enabled = false;
            return;
        }

        Time.timeScale = 1f;
        AudioListener.pause = false;
        panelVictory.SetActive(false);

        if (stars != null)
        {
            foreach (var star in stars)
            {
                if (star == null) continue;
                if (spriteStarOff != null) star.sprite = spriteStarOff; // Thêm check null
                star.transform.localScale = Vector3.zero; 
            }
        }
    }

    // ✅ HÀM ĐÃ SỬA: NHẬN THÊM 2 THAM SỐ COIN/GEM THƯỞNG
    public void ShowWin(int targetScore, int finalScore, int coinReward, int gemReward)
    {
        if(panelVictory == null) return; 
        StopAllCoroutines();

        // Reset stars
        if (stars != null)
        {
            for(int i = 0; i < stars.Length; i++)
            {
                if (stars[i] == null) continue;
                if (spriteStarOff != null) stars[i].sprite = spriteStarOff; // Thêm check null
                stars[i].transform.localScale = Vector3.zero;
            }
        }

        panelVictory.SetActive(true);
        
        // Hiển thị điểm số
        if (textTarget != null) textTarget.text = targetScore.ToString();
        if (textScore != null) textScore.text = finalScore.ToString();

        // ✅ HIỂN THỊ PHẦN THƯỞNG COIN VÀ GEM
        if (textCoinReward != null) textCoinReward.text = $"+{coinReward}";
        if (textGemReward != null) textGemReward.text = $"+{gemReward}";

        int starCount = CalculateStars(finalScore, targetScore);
        Debug.Log($"WinGame: Final Score: {finalScore}, Target: {targetScore}, Stars Earned: {starCount}, Coin Reward: {coinReward}, Gem Reward: {gemReward}");

        if (starCount > 0 && spriteStarOn != null)
        {
            StartCoroutine(AnimateStars(starCount));
            StartCoroutine(DelayPauseGame(starCount)); 
        }
        else
        {
            StartCoroutine(DelayPauseGame(0));
        }
    }

    public void OnPlayAgainButton()
    {
        StopAllCoroutines(); 
        // ✅ Gọi GameManager.RestartLevel() để đảm bảo logic tái khởi động đúng
        if (gameManager != null)
        {
            gameManager.RestartLevel();
        }
        else
        {
            Debug.LogError("WinGame: GameManager chưa được gán. Không thể Restart Level!");
        }
    }

    public void OnNextLevelButton()
    {
        StopAllCoroutines();
        if (gameManager != null)
        {
            gameManager.LoadNextLevel(); 
        }
        else
        {
            Debug.LogError("WinGame: GameManager chưa được gán. Không thể chuyển Level!");
            Time.timeScale = 1f; 
            AudioListener.pause = false;
        }
    }

    private int CalculateStars(int finalScore, int targetScore)
    {
        if (targetScore <= 0) return 0;
        
        // Logic tính sao đã được giữ nguyên để phục vụ cho Coroutine
        float ratio = (float)finalScore / targetScore;
        if (ratio >= 1f) return 3;
        if (ratio >= 0.75f) return 2;
        if (ratio >= 0.5f) return 1;
        return 0; 
    }

    private IEnumerator DelayPauseGame(int starCount)
    {
        float baseDelay = (starCount > 0) ? 1.0f : 0.5f; 
        float totalAnimationTime = 0.2f + (starCount * 0.3f) + baseDelay;
        yield return new WaitForSecondsRealtime(totalAnimationTime);

        Time.timeScale = 0f;
        AudioListener.pause = true;
    }

    private IEnumerator AnimateStars(int starCount)
    {
        yield return new WaitForSecondsRealtime(0.2f); 

        for (int i = 0; i < starCount; i++)
        {
            if (i >= stars.Length || stars[i] == null) continue;

            stars[i].sprite = spriteStarOn; 
            float t = 0;
            Vector3 startScale = Vector3.zero;
            Vector3 endScale = Vector3.one;
            Vector3 popScale = Vector3.one * 1.2f;
            float duration = 0.15f; 

            while (t < 1)
            {
                t += Time.unscaledDeltaTime / duration;
                stars[i].transform.localScale = Vector3.Lerp(startScale, popScale, t); 
                yield return null;
            }
            t = 0;
            float returnDuration = 0.1f;
            while (t < 1)
            {
                t += Time.unscaledDeltaTime / returnDuration;
                stars[i].transform.localScale = Vector3.Lerp(popScale, endScale, t); 
                yield return null;
            }

            stars[i].transform.localScale = endScale; 
            yield return new WaitForSecondsRealtime(0.3f); 
        }
    }
}