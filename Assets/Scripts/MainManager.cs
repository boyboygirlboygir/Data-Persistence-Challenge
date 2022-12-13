using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public class MainManager : MonoBehaviour
{
    public static MainManager instance;

    public Brick BrickPrefab;
    public int LineCount = 6;
    public Rigidbody Ball;

    public Text ScoreText;
    public GameObject GameOverText;

    private bool m_Started = false;
    private int m_Points;

    public TextMeshProUGUI nameAndBestScoreText;

    public bool m_GameOver = false;

    public int highScore;
    public string highScoreName;

    // Start is called before the first frame update
    private void Awake()
    {
        instance = this;
        LoadNameAndScore();
    }
    void Start()
    {
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
        if (highScore < m_Points)
        {
            nameAndBestScoreText.text = "High Score:" + SceneSwapper.instance.nameText.text + "   |   Best Score: " + m_Points;
        }
        else
        {
            
            nameAndBestScoreText.text = "High Score:" + highScoreName + "   |   Best Score: " + highScore;
        }

    }

    void AddPoint(int point)
    {
        m_Points += point;
        ScoreText.text = $"Score : {m_Points}";
    }

    public void GameOver()
    {
        m_GameOver = true;
        GameOverText.SetActive(true);
        if (highScore < m_Points)
        {
            SaveNameAndScore();
        }
    }
    [System.Serializable]
    class SaveData
    {
        public TextMeshProUGUI nameAndBestScoreText;
        public int highScore;
        public string highScoreName;
    }
    public void SaveNameAndScore()
    {
        SaveData data = new SaveData();

            data.highScore = m_Points;
            data.highScoreName = SceneSwapper.instance.nameText.text;

        string json = JsonUtility.ToJson(data);
        File.WriteAllText(Application.persistentDataPath + "/savefile.json", json);
    }
    public void LoadNameAndScore()
    {
        string path = Application.persistentDataPath + "/savefile.json";
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            SaveData data = JsonUtility.FromJson<SaveData>(json);

            
            highScore = data.highScore;
            highScoreName = data.highScoreName;
        }
    }
    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
