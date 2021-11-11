using System;
using System.Collections;
using System.Collections.Generic;
using ADT;
using UnityEngine;

public class SnakeManager : MonoBehaviour
{
    [SerializeField]
    private GameObject snakeBodyPart;

    private LinkusListus<Transform> snakedList = new LinkusListus<Transform>();

    private Vector3 previousPosition;
    
    private bool nom;

    void Start()
    {
         var firstBodyPart = Instantiate(snakeBodyPart, transform.position, Quaternion.identity);
         snakedList.Add(firstBodyPart.transform);
         var secondBodyPart = Instantiate(snakeBodyPart, firstBodyPart.transform.position, Quaternion.identity);
         snakedList.Add(secondBodyPart.transform);
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

    private void AddBodyPart()
    {
        var bodyPart = Instantiate(snakeBodyPart, previousPosition, Quaternion.identity);
        snakedList.Add(bodyPart.transform);
    }

    public void Move(float amount)
    {
        var headTransform = transform;
        previousPosition = headTransform.position;
        headTransform.position += headTransform.forward * amount;
        MoveBodyParts();
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
