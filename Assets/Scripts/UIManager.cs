using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public TextMeshProUGUI speedText;
    public TextMeshProUGUI distanceText;
    public TextMeshProUGUI scoreText;

    public TextMeshProUGUI totalScoreText;
    public TextMeshProUGUI totalDistanceText;
    public TextMeshProUGUI maxSpeedText;

    public CarController carController;
    public GameObject gameOverPanel;
    public GameObject pausePanel;

    public GameObject speedIcon;
    public GameObject distanceIcon;
    public GameObject scoreIcon;

    private float currentSpeed = 0f;
    private float currentDistance = 0f;
    private float currentScore = 0f;
    private float maxSpeed = 0f;

    public AudioClip buttonClip;
    public AudioClip cashCollectClip;

    public Button collectButton;
    public TextMeshProUGUI cashEarnedText;
    private int cashEarned = 0;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        pausePanel.SetActive(false);
        gameOverPanel.SetActive(false);
        speedIcon.SetActive(true);
        distanceIcon.SetActive(true);
        scoreIcon.SetActive(true);

        Time.timeScale = 1f;
    }

    public void SetCarController(CarController controller)
    {
        carController = controller;
    }

    // Update is called once per frame
    void Update()
    {
        DistanceUI();
        SpeedUI();
        ScoreUI();
        MaxSpeed();
    }

    public void OnGasPress() 
    { 
        carController?.OnGasPress(); 
    }
    public void OnGasRelease() 
    {
        carController?.OnGasRelease(); 
    }

    public void OnBrakePress() 
    { 
        carController?.OnBrakePress(); 
    }
    public void OnBrakeRelease() 
    {
        carController?.OnBrakeRelease(); 
    }

    public void OnMoveRightPress() 
    { 
        carController?.OnMoveRightPress(); 
    }
    public void OnMoveRightRelease()
    { 
        carController?.OnMoveRightRelease();
    }

    public void OnMoveLeftPress()
    {
        carController?.OnMoveLeftPress(); 
    }
    public void OnMoveLeftRelease() 
    { 
        carController?.OnMoveLeftRelease(); 
    }

    void SpeedUI()
    {
        currentSpeed = carController.CarSpeed();
        speedText.text = currentSpeed.ToString("0" + " km/h");
    }

    void DistanceUI()
    {
        currentDistance = carController.transform.position.z / 1000;
        distanceText.text = currentDistance.ToString("0.00" + " km");
    }

    void ScoreUI()
    {
        currentScore = carController.transform.position.z * 6;
        scoreText.text = currentScore.ToString("0");

        cashEarned = Mathf.FloorToInt(carController.transform.position.z / 100) * 10;
    }

    public void GameOver()
    {
        Time.timeScale = 0f;
        gameOverPanel.SetActive(true);
        speedIcon.SetActive(false);
        distanceIcon.SetActive(false);
        scoreIcon.SetActive(false);
        totalScoreText.text = currentScore.ToString("0");
        totalDistanceText.text = currentDistance.ToString("0.00" + " km");

        cashEarnedText.text = cashEarned.ToString("0");

        PlayerPrefs.SetInt("CashValue", PlayerPrefs.GetInt("CashValue", 0) + cashEarned);

        collectButton.gameObject.SetActive(true); 
    }

    void MaxSpeed()
    {
        float currentSpeed = carController.CarSpeed();

        if (currentSpeed > maxSpeed)
        {
            maxSpeed = currentSpeed;
        }

        maxSpeedText.text = maxSpeed.ToString("0" + " km/h");
    }

    public void PauseGame()
    {
        carController.engineSound.Pause();

        Time.timeScale = 0f;
        AudioManager.Instance.PlaySFX(buttonClip);
        pausePanel.SetActive(true);
    }

    public void ResumeGame()
    {
        carController.engineSound.UnPause();
        Time.timeScale = 1f;
        AudioManager.Instance.PlaySFX(buttonClip);
        pausePanel.SetActive(false);
    }

    public void RestartGame()
    {
        AudioManager.Instance.PlaySFX(buttonClip);
        var currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
    }

    public void GarageButton()
    {
        AudioManager.Instance.PlaySFX(buttonClip);
        SceneManager.LoadScene("Garage Scene");
    }

    public void CollectCash()
    {
        AudioManager.Instance.PlaySFX(cashCollectClip);
        collectButton.gameObject.SetActive(false);
    }
}
