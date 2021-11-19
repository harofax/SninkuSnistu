using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private SnakeManager player;

    [SerializeField]
    private FruitController fruit;

    [SerializeField] 
    private CinemachineVirtualCamera cinecam;

    [SerializeField]
    private float tickTime = 0.2f;

    [SerializeField]
    private TMP_Text scoreDisplay;

    [SerializeField]
    private GameObject pauseUI;

    [SerializeField, Range(1, 200)]
    private int scoreToAdvanceLevel = 4;

    private float timer = 0f;
    
    private int currentScore = 0;

    private bool isPaused;
    private readonly int MainMenuSceneIndex = 0;

    public delegate void Tick();
    public static event Tick OnTick;

    private void OnEnable()
    {
        SnakeManager.OnFruitEaten += IncreaseScore;
    }

    private void OnDisable()
    {
        SnakeManager.OnFruitEaten -= IncreaseScore;
    }

    // Start is called before the first frame update
    private void Awake()
    {
        var playerTransform = player.transform;
        cinecam.Follow = playerTransform;
        cinecam.LookAt = playerTransform;
        scoreDisplay.text = $"0 / {scoreToAdvanceLevel}";
    }

    private void Start()
    {
        LevelManager.Instance.LoadLevel(0);
    }

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePauseMenu();
        }
        timer += Time.deltaTime;
        if (timer >= tickTime)
        {
            timer -= tickTime;
            OnTick?.Invoke();
        }
    }

    public void TogglePauseMenu()
    {
        isPaused = !isPaused;
        pauseUI.SetActive(!pauseUI.activeInHierarchy);
        Time.timeScale = isPaused ? 0 : 1;
    }

    public void ReturnToMenu()
    {
        TogglePauseMenu();
        SceneManager.LoadScene(MainMenuSceneIndex);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    private void IncreaseScore()
    {
        currentScore++;
        scoreDisplay.text = $"{currentScore} / {scoreToAdvanceLevel}";
        if (currentScore >= scoreToAdvanceLevel)
        {
            LevelManager.Instance.ClearLevel();
            LevelManager.Instance.LoadNextLevel();
            player.MoveToRandomPosition();
            fruit.MoveToRandomPosition();
            currentScore = 0;
            scoreDisplay.text = $"0 / {scoreToAdvanceLevel}";
        }
    }
}
