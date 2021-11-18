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

    [SerializeField, Range(0, 20)]
    private int startBodyCount;

    [Space(15f)]
    
    [SerializeField, Range(0.5f, 1f), Tooltip("The minimum potential scale the wobble will reach during a wobble")]
    private float minWobbleScale = 0.8f;
    
    [SerializeField, Range(1f, 2f),   Tooltip("The maximum potential scale the wobble will reach during a wobble")]
    private float maxWobbleScale = 1.5f;
    
    [Space(10f)]
    
    [SerializeField, Range(0.5f, 0.8f), Tooltip("The minimum potential duration a wobble can have (in seconds)")]
    private float minWobbleRate = 0.5f;

    [SerializeField, Range(0.8f, 1.8f), Tooltip("The maximum potential duration a wobble can have (in seconds)")]
    private float maxWobbleRate = 1.0f;

    private readonly LinkusListus<Transform> snakedList = new LinkusListus<Transform>();
    private readonly HashSet<Vector3Int> bodyPartPositions = new HashSet<Vector3Int>();
    
    private GridController grid;

    private Vector3 previousPosition;
    private Vector3 nextDir;
    
    private bool isEating;
    private bool jump;
    private bool isGrounded;

    public delegate void EatFruit();
    public static event EatFruit OnFruitEaten;
    
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
        grid = GridController.Instance;

        float START_HEIGHT = grid.GridUnit * 6;
        
        Vector3 startPos = grid.GetRandomPosition() * grid.GridUnit;
        transform.position = startPos;

        Vector3 startDirection = GetStartDirection(startPos);
        transform.rotation = Quaternion.LookRotation(startDirection);

        nextDir = startDirection;

        AddStartingBody(startBodyCount);
    }

    private static Vector3 GetStartDirection(Vector3 startPos)
    {
        
        Vector3Int middleOfGrid = GridController.Instance.GridDimensions / 2;
        Vector3 middleTilePos = GridController.Instance.ConvertToWorldSpace(middleOfGrid);

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

        if (!jump && isGrounded && Input.GetKeyDown(KeyCode.Space))
        {
            jump = true;
            isGrounded = false;
        }
    }

    private void AddBodyPart()
    {
        var bodyPart = Instantiate(snakeBodyPart, snakedList.Tail.position, Quaternion.identity);
        bodyPart.name = "body segment #" + snakedList.Count;
        bodyPart.InitializeWobble(minWobbleScale, maxWobbleScale, minWobbleRate, maxWobbleRate);

        Vector3Int bodyPartGridPos = grid.ConvertToGridPos(bodyPart.transform.position);

        snakedList.Add(bodyPart.transform);
        bodyPartPositions.Add(bodyPartGridPos);
        grid.OccupiedCells.Add(bodyPartGridPos);
    }

    private void AddStartingBody(int count)
    {
        var firstBodyPart = Instantiate(snakeBodyPart, previousPosition, Quaternion.identity);
        
        firstBodyPart.name = "startBody";
        firstBodyPart.InitializeWobble(minWobbleScale, maxWobbleScale, minWobbleRate, maxWobbleScale);
        
        snakedList.Add(firstBodyPart.transform);
        bodyPartPositions.Add(grid.ConvertToGridPos(firstBodyPart.transform.position));
        grid.OccupiedCells.Add(grid.ConvertToGridPos(firstBodyPart.transform.position));

        count--;

        for (int i = 0; i < count; i++)
        {
            AddBodyPart();
        }
    }

    

    private void Move()
    {
        Transform headTransform = transform;
        headTransform.forward = nextDir;
        Vector3 position = headTransform.position;
        
        previousPosition = position;

        Vector3 nextPosition = position + headTransform.forward * grid.GridUnit;
        Vector3Int nextGridPos = grid.WrapGridPos(grid.ConvertToGridPos(nextPosition));

        nextPosition = grid.ConvertToWorldSpace(nextGridPos);
        
        if (jump || bodyPartPositions.Contains(grid.ConvertToGridPos(nextPosition)))
        {
            nextPosition += transform.up * grid.GridUnit;
        }
        else
        {
            ApplyGravity(ref nextPosition);
        }

        if (bodyPartPositions.Contains(grid.ConvertToGridPos(nextPosition)))
        {
            print("U DIED LMAO LOSER");
        }

        headTransform.position = nextPosition;
        MoveBodyParts();
        // TODO: Integrate with new grid system
        if (IsHovering())
        {
            ApplyGravityToBodyParts();
        }
        
        
    }

    private void ApplyGravity(ref Vector3 nextPosition)
    {
        Vector3 downDelta = nextPosition + grid.GridUnit * Vector3.down;
        Vector3Int gridDeltaPos = grid.ConvertToGridPos(downDelta);

       // TODO: integrate with new grid system
        if (!bodyPartPositions.Contains(gridDeltaPos) && !grid.OccupiedCells.Contains(gridDeltaPos))
        {
            nextPosition = downDelta;
        }
        else
        {
            isGrounded = true;
        }
    }

    private void ApplyGravityToBodyParts()
    {
        for (int i = 0; i < snakedList.Count; i++)
        {
            Transform bodyTransform = snakedList[i];
            Vector3 segmentPos = bodyTransform.position;
            Vector3Int segmentGridPos = grid.ConvertToGridPos(segmentPos);

            bodyPartPositions.Remove(segmentGridPos);
            grid.OccupiedCells.Remove(segmentGridPos);
            
            ApplyGravity(ref segmentPos);
            bodyTransform.position = segmentPos;
            segmentGridPos = grid.ConvertToGridPos(segmentPos);

            bodyPartPositions.Add(segmentGridPos);
            grid.OccupiedCells.Add(segmentGridPos);
        }
    }

    // TODO: Integrate with new grid system
    private bool IsHovering()
    {
        bool hover = false;
        for (int i = 0; i < snakedList.Count; i++)
        {
            Vector3 downDelta = snakedList[i].position + Vector3.down * grid.GridUnit;
            Vector3Int downGridPos = grid.ConvertToGridPos(downDelta);
            hover = !grid.OccupiedCells.Contains(downGridPos) && !bodyPartPositions.Contains(downGridPos);
        }
    
        return hover;
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
        
        bodyPartPositions.Remove(grid.ConvertToGridPos(position));
        grid.OccupiedCells.Remove(grid.ConvertToGridPos(position));
        
        position = previousPosition;
        //ApplyGravity(ref position);
        lastBodyPart.position = position;
        
        bodyPartPositions.Add(grid.ConvertToGridPos(position));
        grid.OccupiedCells.Add(grid.ConvertToGridPos(position));

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
            OnFruitEaten?.Invoke();
            fruit.MoveToRandomPosition();
            AddBodyPart();
        }
    }
}
