using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private GameObject gameOverUI;
    [SerializeField] private TextMeshProUGUI finalScoreText;
    [SerializeField] private Button returnToTitleButton;
    [SerializeField] private MonsterDataSO d;
    private int score = 0;
    private float gameTime = 180f;
    private bool isGameOver = false;

    private void Awake()
    {
        if(_instance == null)
        {
            _instance = this;

            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    public static GameManager Instance
    {
        get
        {
            if (_instance == null)
            {
                return null;
            }
            return _instance;
        }
    }

    private void Start()
    {
        ResetGame();
        StartCoroutine(GameTimer());
    }

    public void AddScore(int value)
    {
        if (isGameOver) return;

        score += value;
        UpdateScoreUI();
    }

    private void UpdateScoreUI()
    {
        if (scoreText != null)
        {
            scoreText.text = "Score: " + score;
        }
    }

    private IEnumerator GameTimer()
    {
        while (gameTime > 0 && !isGameOver)
        {
            Debug.Log("시작");
            gameTime -= Time.deltaTime;
            UpdateTimerUI();
            yield return null;
        }

        if (!isGameOver)
            GameOver();
    }

    private void UpdateTimerUI()
    {
        if (timerText != null)
        {
            int minutes = Mathf.FloorToInt(gameTime / 60);
            int seconds = Mathf.FloorToInt(gameTime % 60);
            timerText.text = $"Time: {minutes:D2}:{seconds:D2}";
        }
    }

    private void GameOver()
    {
        if (isGameOver) return;

        isGameOver = true;
        gameOverUI.SetActive(true);
        finalScoreText.text = "Final Score: " + score;
    }

    public void PlayerDied()
    {
        GameOver();
    }

    public void ReturnToTitle()
    {
        score = 0;
        gameTime = 180f;
        isGameOver = false;
        UpdateScoreUI();
        SceneManager.LoadScene("TitleScene");
    }

    private void ResetGame()
    {
        Debug.Log("리셋");
        score = 0;
        gameTime = 180f;
        isGameOver = false;
        UpdateScoreUI();
        gameOverUI.SetActive(false);
    }
}
