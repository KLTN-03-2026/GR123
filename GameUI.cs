using UnityEngine;
using UnityEngine.SceneManagement;

public class GameUI : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;

    public void StartGame()
    {
        if (gameManager != null)
        {
            gameManager.StartGame();
        }
        else
        {
            Debug.LogWarning("GameManager chưa được gán!");
        }
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Thoát game"); // Dùng để kiểm tra khi chạy trong Editor
    }

    public void ContinueGame()
    {
        if (gameManager != null)
        {
            gameManager.ResumeGame();
        }
        else
        {
            Debug.LogWarning("GameManager chưa được gán!");
        }
    }

    public void MainMenu()
    {
        // Sửa lỗi hàm lấy tên scene hiện tại
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
