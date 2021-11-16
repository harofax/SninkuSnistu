using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private GridController grid;
    
    [SerializeField]
    private SnakeManager player;

    [SerializeField] 
    private CinemachineVirtualCamera cinecam;

    [SerializeField]
    private float tickTime = 0.2f;

    private float timer = 0f;

    public delegate void Tick();
    public static event Tick OnTick;

    // Start is called before the first frame update
    private void Awake()
    {
        var playerTransform = player.transform;
        cinecam.Follow = playerTransform;
        cinecam.LookAt = playerTransform;
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
}
