using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Wobblifier))]
public class SnakeBodyController : MonoBehaviour
{
    private Wobblifier wobble;
    // Start is called before the first frame update
    void Start()
    {
        wobble = GetComponent<Wobblifier>();

        float NORMAL_SCALE = 1f + GridController.Instance.GridUnit;

        float deviation = Random.Range(0.2f, 1.44f);
        
        wobble.MaxWobble = Random.Range(NORMAL_SCALE,             NORMAL_SCALE + deviation);
        wobble.MinWobble = Random.Range(NORMAL_SCALE - deviation, NORMAL_SCALE);

        wobble.WobbleRate = Random.Range(1f, 3.5f);
    }
}
