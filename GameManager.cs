using UnityEngine;
using UnityEngine.UI;
using Unity.Cinemachine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using System.Collections;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance; // Đã thêm Singleton Instance
    public static bool ShouldStartImmediately = false;
    private int currentEnergy;
    private bool bossCalled = false;
    private bool optionsFromPause = false;

    // 🔥 QUẢN LÝ VÀ LƯU TRỮ TIỀN TỆ 🔥
    private const string COIN_KEY = "PlayerCoins";
    private const string GEM_KEY = "PlayerGems";
    private int totalCoins;
    private int totalGems;
    
    [Header("Main Settings")]
    [SerializeField] private int energyThreshold = 12; // Ngưỡng gọi Boss
    [SerializeField] private int levelTargetScore = 500; // Điểm mục tiêu để đạt 3 sao

    [Header("Currency Rewards")]
    [SerializeField] private int baseCoinReward = 100; // Coin thưởng cơ bản cho 1 sao
    [SerializeField] private int baseGemReward = 5;    // Gem thưởng cơ bản cho 1 sao
    [SerializeField] private int lossCoinReward = 50;  // Coin thưởng cố định khi thua
    
    [Header("Currency Display")] 
    [SerializeField] private TMP_Text textTotalCoins; 
    [SerializeField] private TMP_Text textTotalGems;  
    
    [Header("Main References")]
    [SerializeField] private GameObject spawnEnemy;
    [SerializeField] private Image energyBar;
    [SerializeField] private GameObject gameUI;
    [SerializeField] private GameObject red;
    [SerializeField] private Audio audio; // AudioManager
    [SerializeField] private CinemachineCamera cam;
    [SerializeField] private GameObject boss; // <-- ĐÃ KHẮC PHỤC LỖI CS0103

    [Header("UI Menus")]
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject optionsMenu;
    [SerializeField] private GameObject levelsMenu;
    [SerializeField] private GameObject shopMenu;
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject loseMenu; // GameObject Lose Panel
    [SerializeField] private GameObject winMenu; // GameObject Win Panel

    [Header("Menu Scripts Reference")]
    [SerializeField] private WinGame winGame; 
    [SerializeField] private LoseGame loseGame; 

    [Header("Audio Mixer")]
    [SerializeField] private AudioMixer masterMixer;
    private const string MUSIC_VOLUME = "MusicVolume";
    private const string SFX_VOLUME = "SFXVolume";

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    void Start()
    {
        currentEnergy = 0;
        bossCalled = false;
        UpdateEnergyBar();
        if (boss != null) boss.SetActive(false);
        if (red != null) red.SetActive(false);
        
        LoadCurrency(); 

        if (ShouldStartImmediately)
        {
            ShouldStartImmediately = false;
            StartGame();
        }
        else
        {
            HideAllUI();
            if (mainMenu != null) mainMenu.SetActive(true);
            Time.timeScale = 0f;
            if (audio != null)
            {
                audio.StopAllAudio();
                audio.PlayMenuAudio();
            }
            if (cam != null) cam.Lens.OrthographicSize = 6f;
            AudioListener.pause = false; 
        }
    }
    
    private void LoadCurrency()
    {
        totalCoins = PlayerPrefs.GetInt(COIN_KEY, 0);
        totalGems = PlayerPrefs.GetInt(GEM_KEY, 0);
        UpdateCurrencyUI(); 
        Debug.Log($"Loaded Currency: Coins: {totalCoins}, Gems: {totalGems}");
    }

    private void SaveCurrency()
    {
        PlayerPrefs.SetInt(COIN_KEY, totalCoins);
        PlayerPrefs.SetInt(GEM_KEY, totalGems);
        PlayerPrefs.Save();
        Debug.Log($"Saved Currency: Coins: {totalCoins}, Gems: {totalGems}");
    }

    public void AddCurrency(int coins, int gems)
    {
        if (coins > 0) totalCoins += coins;
        if (gems > 0) totalGems += gems;
        UpdateCurrencyUI(); 
    }
    
    public void UpdateCurrencyUI()
    {
        if (textTotalCoins != null)
        {
            textTotalCoins.text = totalCoins.ToString("N0"); 
        }
        if (textTotalGems != null)
        {
            textTotalGems.text = totalGems.ToString("N0");
        }
    }

    private int CalculateStars(int finalScore, int targetScore)
    {
        if (targetScore <= 0) return 0;
        
        float star3Threshold = 1f; 
        float star2Threshold = 0.75f; 
        float star1Threshold = 0.5f; 

        float ratio = (float)finalScore / targetScore;

        if (ratio >= star3Threshold) return 3; 
        if (ratio >= star2Threshold) return 2; 
        if (ratio >= star1Threshold) return 1; 
        return 0; 
    }
    
    // ✅ HÀM NÀY CHỈ NHẬN finalScore (Khắc phục lỗi CS1501)
    public void WinGameLevel(int finalScore)
    {
        HideAllUI(); 
        if (winMenu != null) winMenu.SetActive(true); 

        // 1. Tính toán số sao và thưởng (Dùng levelTargetScore đã định nghĩa)
        int starCount = CalculateStars(finalScore, levelTargetScore);
        int finalCoinReward = baseCoinReward * starCount;
        int finalGemReward = baseGemReward * starCount;

        // 2. Cộng và lưu tiền tệ
        AddCurrency(finalCoinReward, finalGemReward);
        SaveCurrency(); 

        if (winGame != null)
        {
            winGame.ShowWin(levelTargetScore, finalScore, finalCoinReward, finalGemReward); 
        }
        
        if (audio != null)
        {
            audio.StopAllMusic();
            audio.PlayWinAudio();
        }
        Time.timeScale = 0f; 
        AudioListener.pause = true;
    }
    
    public void GameOverMenu()
    {
        // Lấy điểm số hiện tại
        int finalScore = 0;
        if (ScoreManager.instance != null)
        {
            finalScore = ScoreManager.instance.GetCurrentScore();
        }
        
        HideAllUI();
        if (loseMenu != null) loseMenu.SetActive(true);

        // 1. Tính toán phần thưởng khi thua
        int finalCoinReward = lossCoinReward; 
        int finalGemReward = 0; 

        // 2. Cộng và lưu tiền tệ
        AddCurrency(finalCoinReward, finalGemReward);
        SaveCurrency();

        // 3. Hiển thị UI thua
        if (loseGame != null)
        {
            loseGame.ShowLose(levelTargetScore, finalScore, finalCoinReward);
        }
        
        Time.timeScale = 0f;
        AudioListener.pause = true;
    }

    public void AddEnergy()
    {
        if (bossCalled) return;
        currentEnergy++;
        UpdateEnergyBar();
        Debug.Log("Energy: " + currentEnergy);

        if (audio != null) audio.PlayEnergySound();

        if (currentEnergy >= energyThreshold)
        {
            CallBoss();
        }
    }

    private void CallBoss()
    {
        bossCalled = true;
        if (boss != null) boss.SetActive(true); // <-- SỬ DỤNG BIẾN 'boss'
        if (spawnEnemy != null) spawnEnemy.SetActive(false);
        if (gameUI != null) gameUI.SetActive(false);
        if (audio != null) audio.PlayBossAudio();
        if (cam != null) cam.Lens.OrthographicSize = 6f;
        if (red != null) red.SetActive(true);
    }

    private void UpdateEnergyBar()
    {
        if (energyBar != null)
        {
            energyBar.fillAmount = Mathf.Clamp01((float)currentEnergy / energyThreshold);
        }
    }

    private void HideAllUI()
    {
        if (gameUI != null) gameUI.SetActive(false);
        if (mainMenu != null) mainMenu.SetActive(false);
        if (pauseMenu != null) pauseMenu.SetActive(false);
        if (winMenu != null) winMenu.SetActive(false);
        if (loseMenu != null) loseMenu.SetActive(false);
        if (optionsMenu != null) optionsMenu.SetActive(false);
        if (levelsMenu != null) levelsMenu.SetActive(false);
        if (shopMenu != null) shopMenu.SetActive(false);
    }
    
    public void StartGame()
    {
        HideAllUI();
        if (gameUI != null) gameUI.SetActive(true);
        Time.timeScale = 1f;

        if (audio != null)
        {
            audio.StopAllMusic();
            audio.PlayDefaultAudio();
        }
        AudioListener.pause = false;

        if (spawnEnemy != null) spawnEnemy.SetActive(true);
        if (red != null) red.SetActive(false);
        if (cam != null) cam.Lens.OrthographicSize = 5f;
    }

    public void ResumeGame()
    {
        HideAllUI();
        if (gameUI != null) gameUI.SetActive(true);
        Time.timeScale = 1f;
        AudioListener.pause = false;
    }

    public void Options()
    {
        optionsFromPause = pauseMenu.activeSelf;
        HideAllUI();
        if (optionsMenu != null) optionsMenu.SetActive(true);
    }

    public void Shop()
    {
        HideAllUI();
        if (shopMenu != null) shopMenu.SetActive(true);
        UpdateCurrencyUI(); 
    }
    
    public void MainMenu()
    {
        if (audio != null)
        {
            audio.StopAllMusic();
            audio.PlayMenuAudio();
        }
        HideAllUI();
        mainMenu.SetActive(true);
        Time.timeScale = 0f;
    }

    public void Levels()
    {
        HideAllUI();
        if (levelsMenu != null) levelsMenu.SetActive(true);
    }

    public void BackToMenu()
    {
        HideAllUI();
        if (optionsFromPause)
        {
            if (pauseMenu != null) pauseMenu.SetActive(true);
        }
        else
        {
            if (mainMenu != null) mainMenu.SetActive(true);
        }
    }

    public void PauseGameMenu()
    {
        HideAllUI();
        if (pauseMenu != null) pauseMenu.SetActive(true);
        Time.timeScale = 0f;
        AudioListener.pause = true;
    }
    
    public void LoadNextLevel()
    {
        int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
        Time.timeScale = 1f;
        AudioListener.pause = false;
        
        if (audio != null) audio.StopAllMusic();

        if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(nextSceneIndex);
        }
        else
        {
            Debug.LogWarning("Không tìm thấy Level tiếp theo! Quay về Level đầu tiên (Index 0).");
            SceneManager.LoadScene(0); 
        }
    }

    public void QuitToMainMenu()
    {
        Time.timeScale = 1f;
        if (audio != null) audio.StopAllMusic();
        SceneManager.LoadScene(0); // Giả định Scene 0 là Main Menu
    }

    public void RestartLevel()
    {
        ShouldStartImmediately = true;

        if (winGame != null)
            winGame.StopAllCoroutines();

        Time.timeScale = 1f;
        AudioListener.pause = false;

        if (audio != null)
            audio.StopAllMusic();

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    
    public void QuitGame()
    {
        Debug.Log("Quit Game!");
        Application.Quit();
    }

    public void LoadLevel(int levelIndex)
    {
        Time.timeScale = 1f;
        AudioListener.pause = false;
        if (audio != null) audio.StopAllMusic();
        SceneManager.LoadScene(levelIndex);
    }

    public void SetMusicVolume(float volume)
    {
        float volume_dB = Mathf.Log10(Mathf.Max(volume, 0.0001f)) * 20;
        if (masterMixer != null) masterMixer.SetFloat(MUSIC_VOLUME, volume_dB);
    }

    public void SetSFXVolume(float volume)
    {
        float volume_dB = Mathf.Log10(Mathf.Max(volume, 0.0001f)) * 20;
        if (masterMixer != null) masterMixer.SetFloat(SFX_VOLUME, volume_dB);
    }
}
