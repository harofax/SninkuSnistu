using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private GridController grid;
    
    [SerializeField]
    private SnakeManager player;

    [SerializeField]
    private float tickTime = 0.2f;

    private float timer = 0f;

    // Start is called before the first frame update
    void Start()
    {
        
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
