using System;
using System.Collections;
using System.Collections.Generic;
using ADT;
using UnityEngine;

public class SnakeManager : MonoBehaviour
{
    [SerializeField]
    private SnakeBodyController snakeBodyPart;

    [SerializeField]
    private FruitAntenna antenna;

    [Space(15f)]
    
    [SerializeField, Range(0.5f, 1f), Tooltip("The minimum potential scale the wobble will reach during a wobble")]
    private float minWobbleScale = 0.8f;
    
    [SerializeField, Range(1f, 2f),   Tooltip("The maximum potential scale the wobble will reach during a wobble")]
    private float maxWobbleScale = 1.5f;
    
    [Space(10f)]
    
    [SerializeField, Range(0.1f, 0.8f), Tooltip("The minimum potential duration a wobble can have (in seconds)")]
    private float minWobbleRate = 0.5f;

    [SerializeField, Range(0.8f, 1.8f), Tooltip("The maximum potential duration a wobble can have (in seconds)")]
    private float maxWobbleRate = 1.0f;
    
    private readonly LinkusListus<Transform> snakedList = new LinkusListus<Transform>();

    private Vector3 previousPosition;
    
    private bool nom;
    
    private const float START_HEIGHT = 1f;

    private void Start()
    {
        Vector3 startPos = GridController.Instance.GetRandomPosition(START_HEIGHT);
        transform.position = startPos;

        var firstBodyPart = Instantiate(snakeBodyPart, transform.position, Quaternion.identity);
        firstBodyPart.InitializeWobble(minWobbleScale, maxWobbleScale, minWobbleRate, maxWobbleRate);
        snakedList.Add(firstBodyPart.transform);
        
        var secondBodyPart = Instantiate(snakeBodyPart, firstBodyPart.transform.position, Quaternion.identity);
        secondBodyPart.InitializeWobble(minWobbleScale, maxWobbleScale, minWobbleRate, maxWobbleRate);
        snakedList.Add(secondBodyPart.transform);
    }

    // Update is called once per frame
    private void Update()
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

    private void AddBodyPart()
    {
        var bodyPart = Instantiate(snakeBodyPart, previousPosition, Quaternion.identity);
        bodyPart.InitializeWobble(minWobbleScale, maxWobbleScale, minWobbleRate, maxWobbleRate);
        snakedList.Add(bodyPart.transform);
    }

    public void Move(float amount)
    {
        var headTransform = transform;
        previousPosition = headTransform.position;
        headTransform.position += headTransform.forward * amount;
        MoveBodyParts();
        antenna.UpdateFruitDirection();
    }

    private void MoveBodyParts()
    {
        for (int i = 0; i < snakedList.Count; i++)
        {
            (snakedList[i].position, previousPosition) = (previousPosition, snakedList[i].position);
        }
        nom = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!nom && other.TryGetComponent(out FruitController fruit))
        {
            nom = true;
            fruit.MoveToRandomPosition();
            AddBodyPart();
        }
    }
}
