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

    private readonly LinkusListus<Transform> snakedList = new LinkusListus<Transform>();
    private readonly HashSet<Vector3Int> bodyPartPositions = new HashSet<Vector3Int>();
    private HashSet<Vector3Int> tilePositions;

    private int gridUnit;
    
    private Vector3 previousPosition;
    private Vector3 nextDir;
    
    private bool isEating;
    private bool jump;
    private bool isGrounded;

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
        tilePositions = GridController.Instance.TilePositions;
        gridUnit = GridController.Instance.GridUnit;
        
        float START_HEIGHT = gridUnit * 2;
        
        Vector3 startPos = GridController.Instance.GetRandomPosition(START_HEIGHT);
        transform.position = startPos;

        Vector3 startDirection = GetStartDirection(startPos);
        transform.rotation = Quaternion.LookRotation(startDirection);

        nextDir = startDirection;

        AddStartingBody(8);
    }
    
    // TODO: Add a lower outer ring of map where u can jump up again using ur own body, if ur pro
    // TODO: Rethink death n jumping. Autojump = more fun, more snake:y
    // TODO: How die? hmmm

    private static Vector3 GetStartDirection(Vector3 startPos)
    {
        Vector2Int middleOfGrid = GridController.Instance.GridDimensions / 2;
        Vector3 middleTilePos = GridController.Instance.GetTile(middleOfGrid.x, middleOfGrid.y).transform.position;

        float xDiff = middleTilePos.x - startPos.x;
        float zDiff = middleTilePos.z - startPos.z;

        Vector3 startDirection = Mathf.Abs(xDiff) > Mathf.Abs(zDiff)
            ? Vector3.right * Mathf.Sign(xDiff)
            : Vector3.forward * Mathf.Sign(zDiff);
        
        return startDirection;
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

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            jump = true;
            isGrounded = false;
        }
    }

    public bool Occupied(Vector3Int gridPos)
    {
        return bodyPartPositions.Contains(gridPos);
    }

    private void AddBodyPart()
    {
        var bodyPart = Instantiate(snakeBodyPart, snakedList.Tail.position, Quaternion.identity);
        bodyPart.name = "body segment #" + snakedList.Count;
        bodyPart.InitializeWobble(minWobbleScale, maxWobbleScale, minWobbleRate, maxWobbleRate);

        snakedList.Add(bodyPart.transform);
        bodyPartPositions.Add(ConvertToGridPos(bodyPart.transform.position));
    }

    private void AddStartingBody(int count)
    {
        var firstBodyPart = Instantiate(snakeBodyPart, previousPosition, Quaternion.identity);
        
        firstBodyPart.name = "startBody";
        firstBodyPart.InitializeWobble(minWobbleScale, maxWobbleScale, minWobbleRate, maxWobbleScale);
        
        snakedList.Add(firstBodyPart.transform);
        bodyPartPositions.Add(ConvertToGridPos(firstBodyPart.transform.position));

        for (int i = 0; i < count; i++)
        {
            AddBodyPart();
        }
    }

    Vector3Int ConvertToGridPos(Vector3 pos)
    {
        return Vector3Int.RoundToInt(pos / gridUnit);
    }

    private void Move()
    {
        Transform headTransform = transform;
        headTransform.forward = nextDir;
        Vector3 position = headTransform.position;
        
        previousPosition = position;

        Vector3 nextPosition = position + headTransform.forward * gridUnit;

        if (jump)//bodyPartPositions.Contains(GetGridPos(nextPosition)))
        {
            nextPosition += transform.up * gridUnit;
        }
        else
        {
            ApplyGravity(ref nextPosition);
        }

        if (bodyPartPositions.Contains(ConvertToGridPos(nextPosition)))
        {
            print("U DIED LMAO LOSER");
        }

        headTransform.position = nextPosition;
        MoveBodyParts();
        
    }

    private void ApplyGravity(ref Vector3 nextPosition)
    {
        Vector3 downDelta = nextPosition + gridUnit * Vector3.down;
        Vector3Int gridDeltaPos = ConvertToGridPos(downDelta);

        if (!bodyPartPositions.Contains(gridDeltaPos) && !tilePositions.Contains(gridDeltaPos))
        {
            isGrounded = false;
            nextPosition = downDelta;
        }
        else
        {
            isGrounded = true;
        }
    }

    // private void ApplyGravityToBodyParts()
    // {
    //     for (int i = 0; i < snakedList.Count - 1; i++)
    //     {
    //         Transform bodyTransform = snakedList[i];
    //         Vector3 nextPos = bodyTransform.position;
    //
    //         bodyPartPositions.Remove(nextPos);
    //         ApplyGravity(ref nextPos);
    //         bodyTransform.position = nextPos;
    //         bodyPartPositions.Add(nextPos);
    //     }
    // }

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
        
        bodyPartPositions.Remove(ConvertToGridPos(position));
        
        position = previousPosition;
        ApplyGravity(ref position);
        lastBodyPart.position = position;
        
        bodyPartPositions.Add(ConvertToGridPos(position));

        snakedList.RemoveAt(tailIndex);
        snakedList.AddFirst(lastBodyPart);

        isEating = false;
        jump = false;
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
