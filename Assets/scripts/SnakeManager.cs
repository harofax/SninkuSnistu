using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeManager : MonoBehaviour
{
    [SerializeField]
    private GameObject snakeBodyPart;
    
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            transform.forward = -transform.right;
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            transform.forward = transform.right;
        }
    }

    public void Move(float amount)
    {
        transform.position += transform.forward * amount;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out FruitController fruit))
        {
            fruit.MoveToRandomPosition();
        }
    }
}
