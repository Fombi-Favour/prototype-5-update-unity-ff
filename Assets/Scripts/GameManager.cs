using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public List<GameObject> targets;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI timeText;
    public TextMeshProUGUI gameOverText;
    public TextMeshProUGUI livesText;
    public GameObject titleScreen;
    public GameObject pauseScreen;
    public Button restartButton;
    public bool takingAway = false;
    public bool isGameActive;
    private bool paused;
    private int score;
    public int secondsLeft;
    private int lives;
    private float spawnRate = 1.0f;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            ChangedPaused();
        }

        if (takingAway == false && secondsLeft > 0)
        {
            StartCoroutine(TimeTake());
        }
    }

    IEnumerator SpawnTarget()
    {
        while (isGameActive)
        {
            yield return new WaitForSeconds(spawnRate);
            int index = Random.Range(0, targets.Count);
            Instantiate(targets[index]);
        }
    }

    IEnumerator TimeTake()
    {
        takingAway = true;
        yield return new WaitForSeconds(1);
        secondsLeft -= 1;
        if (secondsLeft < 10)
        {
            timeText.text = "Time: 0" + secondsLeft;
        }
        else
        {
            timeText.text = "Time: " + secondsLeft;
        }
        if (secondsLeft == 0)
        {
            GameOver();
        }

        takingAway = false;
    }

    public void UpdateScore(int scoreToAdd)
    {
        score += scoreToAdd;
        scoreText.text = "Score: " + score;
    }

    public void UpdateLives(int livesToChange)
    {
        lives += livesToChange;
        livesText.text = "Lives : " + lives;
        if(lives <= 0)
        {
            GameOver();
        }
    }

    public void GameOver()
    {
        restartButton.gameObject.SetActive(true);
        gameOverText.gameObject.SetActive(true);
        isGameActive = false;
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void StartGame(int difficulty)
    {
        isGameActive = true;
        StartCoroutine(SpawnTarget());
        score = 0;
        secondsLeft = 61;
        UpdateScore(0);
        UpdateLives(3);
        titleScreen.gameObject.SetActive(false);
        spawnRate /= difficulty;
    }

    void ChangedPaused()
    {
        if (!paused)
        {
            paused = true;
            pauseScreen.SetActive(true);
            Time.timeScale = 0;
        }
        else
        {
            paused = false;
            pauseScreen.SetActive(false);
            Time.timeScale = 1;
        }
    }
}
