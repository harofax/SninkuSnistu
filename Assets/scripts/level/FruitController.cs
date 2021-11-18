using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FruitController : MonoBehaviour
{
    // Start is called before the first frame update
    private void Start()
    {
       MoveToRandomPosition();
    }

    public void MoveToRandomPosition()
    {
        transform.position = GridController.Instance.GetRandomPosition() * GridController.Instance.GridUnit;
    }
}
