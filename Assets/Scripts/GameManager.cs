using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public float initialGameSpeed = 5f;
    public float gameSpeedIncrease = 0.1f;
    public float gameSpeed { get; private set; }

    public TextMeshProUGUI gameOverText;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI hiScoreText;
    public Button retryButton;

    [SerializeField] public AudioSource pointSound;
    [SerializeField] public AudioSource dieSound;

    private Player player;
    private Spawner spawner;

    private float score;

    private void Awake() {
        if (Instance == null) {
            Instance = this;
        } else {
            DestroyImmediate(gameObject);
        }
    }

    private void OnDestroy() {
        if(Instance == this) {
            Instance = null;
        }
    }

    private void Start() {
        player = FindObjectOfType<Player>();
        spawner = FindObjectOfType<Spawner>();

        NewGame();
    }

    public void NewGame() {
        Obstacle[] obstacles = FindObjectsOfType<Obstacle>();

        foreach(var obstacle in obstacles) {
            Destroy(obstacle.gameObject);
        }

        gameSpeed = initialGameSpeed;
        score = 0f;
        enabled = true;

        player.gameObject.SetActive(true);
        spawner.gameObject.SetActive(true);

        gameOverText.gameObject.SetActive(false);
        retryButton.gameObject.SetActive(false);

        UpdateHiScore();
    }

    public void GameOver() {
        gameSpeed = 0f;
        enabled = false;

        player.gameObject.SetActive(false);
        spawner.gameObject.SetActive(false);

        gameOverText.gameObject.SetActive(true);
        retryButton.gameObject.SetActive(true);

        dieSound.Play();

        UpdateHiScore();
    }

    private void Update() {
        gameSpeed += gameSpeedIncrease * Time.deltaTime;
        score += gameSpeed * Time.deltaTime;

        // pad to always have 5 digits
        scoreText.text = Mathf.FloorToInt(score).ToString("D5");
        
        PointSound();
    }

    private void PointSound() {
        if (score > 0 && Mathf.FloorToInt(score) % 100 == 0) {
            pointSound.Play();
        }
    }

    private void UpdateHiScore() {
        float hiscore = PlayerPrefs.GetFloat("hiscore", 0);

        if (score > hiscore) {
            hiscore = score;
            PlayerPrefs.SetFloat("hiscore", hiscore);
        }

        hiScoreText.text = Mathf.FloorToInt(hiscore).ToString("D5");
    }
}
