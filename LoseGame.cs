using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class LoseGame : MonoBehaviour
{
    [Header("Lose Panel References")]
    [SerializeField] private GameObject panelLose;

    [Header("Score Text")]
    [SerializeField] private TMP_Text textTarget;
    [SerializeField] private TMP_Text textScore;

    [Header("Reward Text (Loss)")]
    [SerializeField] private TMP_Text textCoinReward; // Chỉ hiển thị Coin thưởng khi thua

    private void Start()
    {
        if (panelLose == null)
        {
            Debug.LogError("LoseGame: panelLose is NOT assigned in the Inspector. Script disabled.");
            enabled = false;
            return;
        }

        // Đảm bảo ban đầu panel bị tắt
        panelLose.SetActive(false);
    }

    // HÀM GỌI KHI NGƯỜI CHƠI THUA (được gọi từ GameManager)
    public void ShowLose(int targetScore, int finalScore, int coinReward)
    {
        if (panelLose == null) return;

        // Kích hoạt panel thua
        panelLose.SetActive(true);
        
        // Hiển thị điểm
        if (textTarget != null) textTarget.text = targetScore.ToString();
        if (textScore != null) textScore.text = finalScore.ToString();

        // Hiển thị thưởng Coin (dùng N0 để format có dấu phẩy)
        if (textCoinReward != null)
            textCoinReward.text = $"+{coinReward:N0}"; 
    }

    // Hàm gọi khi bấm nút Restart/Play Again
    public void OnRestartButton()
    {
        GameManager gm = FindObjectOfType<GameManager>();
        if (gm != null)
        {
            gm.RestartLevel();
        }
    }

    // Hàm gọi khi bấm nút Back Home 
    public void OnHomeButton()
    {
        GameManager gm = FindObjectOfType<GameManager>();
        if (gm != null)
        {
            gm.QuitToMainMenu(); 
        }
    }
    
    // ✅ HÀM GỌI KHI BẤM NÚT SHOP TỪ LOSE MENU
    public void OnShopButton()
    {
        GameManager gm = FindObjectOfType<GameManager>();
        if (gm != null)
        {
            // Gọi hàm Shop() trong GameManager để mở giao diện Shop
            gm.Shop(); 
        }
    }
}