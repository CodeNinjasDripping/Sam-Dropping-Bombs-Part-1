using UnityEditor;
using UnityEngine;
using Vector2 = System.Numerics.Vector2;
using TMPro;

public class GameManager : MonoBehaviour
{
    
    private Vector3 screenBounds;
    public GameObject title;
    private Spawner spawner;
    public GameObject splash;
    public GameObject playerPrefab;
    private GameObject player;
    private bool gameStarted = false;
    public TMP_Text scoreText;
    public int pointsWorth = 1;
    public int score;

    private int bestScore = 0;
    public TMP_Text bestScoreText;
    public Color normalColor;
    public Color bestScoreColor;

    private bool beatBestScore;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
        spawner = GameObject.Find("Spawner").GetComponent<Spawner>();
        screenBounds = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.transform.position.z));
        scoreText.enabled = false;
        bestScoreText.enabled = false;
    }

    // Update is called once per frame
    void Start()
    {
        spawner.active = false;
        title.SetActive(true);
        splash.SetActive(true);
        bestScore = PlayerPrefs.GetInt("BestScore");
        bestScoreText.text = "Best Score: " + bestScore.ToString();
    }

    void Update()
    {
        if (!gameStarted)
        {
            if (Input.anyKeyDown)
            {
                ResetGame();
            }
        }
        else
        {
            if (!player)
            {
                OnPlayerKilled();
            }    
        }
        if (Input.anyKeyDown)
        {
            spawner.active = true;
            title.SetActive(false);
        }

        var nextBomb = GameObject.FindGameObjectsWithTag("Bomb");
        foreach (GameObject bombObject in nextBomb)
        {
            print(-screenBounds.y);
            if (bombObject.transform.position.y < (-screenBounds.y - 12))
            {
                if (gameStarted)
                {
                    score += pointsWorth;
                    scoreText.text = "Score: " + score.ToString();
                }    
                Destroy(bombObject);
            }
        }    
    }

    void ResetGame()
    {
        bestScoreText.color = normalColor;
        spawner.active = true;
        title.SetActive(false);
        player = Instantiate(playerPrefab,new Vector3(0, 0, 0), playerPrefab.transform.rotation);
        gameStarted = true;
        splash.SetActive(false);
        score = 0;
        scoreText.enabled = true;
        scoreText.text = "Score: " + score.ToString();
        beatBestScore = false;
        bestScoreText.enabled = true;
    }

    void OnPlayerKilled()
    {
        spawner.active = false;
        gameStarted = false;
        splash.SetActive(true);
        if (score > bestScore)
        {
            bestScoreText.color = bestScoreColor;
            bestScore = score;
            PlayerPrefs.SetInt("BestScore", bestScore);
            beatBestScore = true;
            bestScoreText.text = "Best Score: " + bestScore.ToString();
        }    
    }
}
