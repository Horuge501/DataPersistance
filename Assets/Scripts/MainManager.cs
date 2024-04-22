using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class MainManager : MonoBehaviour
{
    public Brick BrickPrefab;
    public int LineCount = 6;
    public Rigidbody Ball;

    public TMP_Text playerNameText;
    public TMP_Text ScoreText;
    public TMP_Text HighScoreText;
    public GameObject GameOverText;

    private bool m_Started = false;
    private bool m_GameOver = false;
    private int m_Points;
    private int m_HighScore;
    private string m_HighScorePlayerName = "";

    void Start()
    {
        LoadHighScore();
        UpdateHighScoreText();
        LoadPlayerName();

        playerNameText.text = m_HighScorePlayerName;

        const float step = 0.6f;
        int perLine = Mathf.FloorToInt(4.0f / step);

        int[] pointCountArray = new[] { 1, 1, 2, 2, 5, 5 };
        for (int i = 0; i < LineCount; ++i)
        {
            for (int x = 0; x < perLine; ++x)
            {
                Vector3 position = new Vector3(-1.5f + step * x, 2.5f + i * 0.3f, 0);
                var brick = Instantiate(BrickPrefab, position, Quaternion.identity);
                brick.PointValue = pointCountArray[i];
                brick.onDestroyed.AddListener(AddPoint);
            }
        }
    }

    private void Update()
    {
        if (!m_Started)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                m_Started = true;
                float randomDirection = Random.Range(-1.0f, 1.0f);
                Vector3 forceDir = new Vector3(randomDirection, 1, 0);
                forceDir.Normalize();

                Ball.transform.SetParent(null);
                Ball.AddForce(forceDir * 2.0f, ForceMode.VelocityChange);
            }
        }
        else if (m_GameOver)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
    }

    void AddPoint(int point)
    {
        m_Points += point;
        ScoreText.text = $"Score : {m_Points}";

        if (m_Points > m_HighScore)
        {
            m_HighScore = m_Points;
            m_HighScorePlayerName = playerNameText.text;
            SaveHighScore();
            UpdateHighScoreText();
        }
    }

    public void GameOver()
    {
        m_GameOver = true;
        GameOverText.SetActive(true);
    }

    private void SaveHighScore()
    {
        PlayerPrefs.SetInt("HighScore", m_HighScore);
        PlayerPrefs.SetString("HighScorePlayerName", m_HighScorePlayerName);
        PlayerPrefs.Save();
    }

    private void LoadHighScore()
    {
        m_HighScore = PlayerPrefs.GetInt("HighScore", 0);
        m_HighScorePlayerName = PlayerPrefs.GetString("HighScorePlayerName", "");
    }

    private void LoadPlayerName()
    {
        m_HighScorePlayerName = PlayerPrefs.GetString("PlayerName", "");
    }

    private void UpdateHighScoreText()
    {
        HighScoreText.text = $"High Score: {m_HighScorePlayerName} - {m_HighScore}";
    }
}