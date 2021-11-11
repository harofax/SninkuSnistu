using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FruitController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //MoveToRandomPosition();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void MoveToRandomPosition()
    {
        Vector2Int gridDimensions = GridController.Instance.GridDimensions / 2;

        int x = Random.Range(-gridDimensions.x, gridDimensions.x);
        int z = Random.Range(-gridDimensions.y, gridDimensions.y);

        Vector3 newPosition = new Vector3(x, transform.position.y, z);
        transform.position = newPosition;
    }
}
