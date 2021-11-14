using System;
using System.Collections;
using System.Collections.Generic;
using System.Security;
using ADT;
using Unity.VisualScripting;
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
    
    private const float START_HEIGHT = 2f;
    
    private readonly LinkusListus<Transform> snakedList = new LinkusListus<Transform>();
    private readonly HashSet<Vector3> bodyPartPositions = new HashSet<Vector3>();

    private int moveAmount;
    
    private Vector3 previousPosition;
    private Vector3 nextDir;
    
    private bool isEating;
    
    private void OnEnable()
    {
        GameManager.OnTick += Move;
    }

    private void OnDisable()
    {
        GameManager.OnTick -= Move;
    }

    private void Start()
    {
        // print("initial forward: " + transform.forward);
        Vector3 startPos = GridController.Instance.GetRandomPosition(START_HEIGHT);
        
        Vector2Int middleOfGrid = GridController.Instance.GridDimensions / 2;
        
        transform.position = startPos;

        Vector3 middleTilePos = GridController.Instance.GetTile(middleOfGrid.x, middleOfGrid.y).transform.position;
        
        // print("midlZ - startZ: " + (middleTilePos.z - startPos.z));
        // print("midlX - startX: " + (middleTilePos.x - startPos.x));
        //
        // print("startpos2d: " + startPos2D);
        //
        // print("midl vec - startvec: " + (middleOfGrid - startPos2D));

        float xDiff = middleTilePos.x - startPos.x;
        float zDiff = middleTilePos.z - startPos.z;
        
        if (Mathf.Abs(xDiff) > Mathf.Abs(zDiff))
        {
            transform.forward = Vector3.right * Mathf.Sign(xDiff);
        }
        else
        {
            transform.forward = Vector3.forward * Mathf.Sign(zDiff);
        }

        // if (startPos.z > middleTilePos.z)
        // {
        //     print("start z is bigger");
        //     transform.forward = Vector3.right;
        // }
        //
        // if (startPos.x > middleTilePos.x)
        // {
        //     print("start x is bigger");
        //     transform.forward = Vector3.left;
        // }
        //
        // print("adjusted forward: " + transform.forward);

        nextDir = transform.forward;

        moveAmount = GridController.Instance.GridUnit;
        
        AddBodyPart();
        AddBodyPart();
    }

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            nextDir = -transform.right;
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            nextDir = transform.right;
        }
    }

    private void AddBodyPart()
    {
        var bodyPart = Instantiate(snakeBodyPart, previousPosition, Quaternion.identity);
        bodyPart.name = "body segment #" + snakedList.Count;
        bodyPart.InitializeWobble(minWobbleScale, maxWobbleScale, minWobbleRate, maxWobbleRate);

        snakedList.Add(bodyPart.transform);
        bodyPartPositions.Add(bodyPart.transform.position);
    }

    public void Move()
    {
        Transform headTransform = transform;
        headTransform.forward = nextDir;
        Vector3 position = headTransform.position;
        
        previousPosition = position;

        Vector3 nextPosition = position + headTransform.forward * moveAmount;

        if (bodyPartPositions.Contains(nextPosition))
        {
            nextPosition += transform.up * moveAmount;
        }

        headTransform.position = nextPosition;
        MoveBodyParts();
    }

    private void MoveBodyParts()
    {
        // for (int i = 0; i < snakedList.Count; i++)
        // {
        //     (snakedList[i].transform.position, previousPosition) = (previousPosition, snakedList[i].transform.position);
        // }
        
        if (snakedList.Count == 0) return;

        int tailIndex = snakedList.Count - 1;
        
        Transform lastBodyPart = snakedList.Tail.transform;
        Vector3 position = lastBodyPart.position;
        
        bodyPartPositions.Remove(position);
        
        position = previousPosition;
        lastBodyPart.position = position;
        
        bodyPartPositions.Add(position);

        snakedList.RemoveAt(tailIndex);
        snakedList.AddFirst(lastBodyPart);

        isEating = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!isEating && other.TryGetComponent(out FruitController fruit))
        {
            isEating = true;
            fruit.MoveToRandomPosition();
            AddBodyPart();
        }
    }
}
