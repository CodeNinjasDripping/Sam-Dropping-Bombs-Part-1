using UnityEditor;
using UnityEngine;
using Vector2 = System.Numerics.Vector2;
using TMPro;

public class GameManager : MonoBehaviour
{
    
    private Vector2 screenBounds;
    public GameObject title;
    private Spawner spawner;
    public GameObject splash;
    public GameObject playerPrefab;
    private GameObject player;
    private bool gameStarted = false;
    public TMP_Text scoreText;
    public int pointsWorth = 1;
    private int score;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
        spawner = GameObject.Find("Spawner").GetComponent<Spawner>();
        scoreText.enabled = false;
    }

    // Update is called once per frame
    void Start()
    {
        spawner.active = false;
        title.SetActive(true);
        splash.SetActive(true);
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
            if (bombObject.transform.position.y < (-screenBounds.Y - 12))
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
        spawner.active = true;
        title.SetActive(false);
        player = Instantiate(playerPrefab,new Vector3(0, 0, 0), playerPrefab.transform.rotation);
        gameStarted = true;
        splash.SetActive(false);
        score = 0;
        scoreText.enabled = true;
        scoreText.text = "Score: " + score.ToString();
    }

    void OnPlayerKilled()
    {
        spawner.active = false;
        gameStarted = false;
        splash.SetActive(true);
    }
}
