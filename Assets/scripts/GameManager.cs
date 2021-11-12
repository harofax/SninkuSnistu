using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

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
    private const float YPlane = 1f;

    // Start is called before the first frame update
    void Awake()
    {
        Vector3 startPos = GridController.Instance.GetRandomPosition(YPlane);
        player = Instantiate(player, startPos, Quaternion.identity);
        
        cinecam.Follow = player.transform;
        cinecam.LookAt = player.transform;
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= tickTime)
        {
            player.Move(grid.GridUnit);
            timer = 0f;
        }
    }
}
