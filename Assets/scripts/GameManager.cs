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

    // Start is called before the first frame update
    private void Awake()
    {
        cinecam.Follow = player.transform;
        cinecam.LookAt = player.transform;
    }

    // Update is called once per frame
    private void Update()
    {
        timer += Time.deltaTime;
        if (timer >= tickTime)
        {
            player.Move(grid.GridUnit);
            timer = 0f;
        }
    }
}
