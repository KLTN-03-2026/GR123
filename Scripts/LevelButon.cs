using UnityEngine;
using TMPro;
using System.Collections;

public class LevelButton : MonoBehaviour
{
    public int levelIndex;
    public TMP_Text messageText;

    public void OpenLevel()
    {
        int unlockedLevel = PlayerPrefs.GetInt("UnlockedLevel", 1);

        if (levelIndex <= unlockedLevel)
        {
            GameManager.Instance.LoadLevel(levelIndex);
        }
        else
        {
            StartCoroutine(ShowMessage());
        }
    }

    IEnumerator ShowMessage()
    {
        messageText.gameObject.SetActive(true);
        messageText.text = "Bạn chưa mở khóa Level này!";

        yield return new WaitForSeconds(1f);

        messageText.gameObject.SetActive(false);
    }
}