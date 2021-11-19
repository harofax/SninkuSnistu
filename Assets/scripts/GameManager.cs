using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

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
    
    [SerializeField, Range(1, 200)]
    private int scoreToAdvanceLevel = 4;

    private float timer = 0f;
    
    private int currentScore = 0;
    
    

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
        timer += Time.deltaTime;
        if (timer >= tickTime)
        {
            timer -= tickTime;
            OnTick?.Invoke();
        }
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
