using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wobblifier : MonoBehaviour
{
    [SerializeField, Range(1f, 3f)]
    private float maxWobble;

    [SerializeField, Range(0.5f, 1f)] 
    private float minWobble;

    [SerializeField, Range(0.3f, 2f)] 
    private float wobbleRate;

    public float MaxWobble
    {
        get => maxWobble;
        set => maxWobble = value;
    }

    public float MinWobble
    {
        get => minWobble;
        set => minWobble = value;
    }

    public float WobbleRate
    {
        get => wobbleRate;
        set => wobbleRate = value;
    }

    private Transform wobbleTransform;
    
    private Vector3 maxWobbleVector;
    private Vector3 minWobbleVector;

    private Vector3 wobbleDir;

    private bool bounce;
    private float progress;
    
    // Start is called before the first frame update
    void Start()
    {
        wobbleTransform = transform;
        maxWobbleVector = Vector3.one * maxWobble;
        minWobbleVector = Vector3.one * minWobble;

        wobbleDir = maxWobbleVector;
    }

    // Update is called once per frame
    void Update()
    {
        progress += wobbleRate * Time.deltaTime;
        wobbleTransform.localScale = Vector3.Lerp(transform.localScale, wobbleDir, progress * Time.deltaTime);

        if (progress >= 1)
        {
            progress = 0f;
            bounce = !bounce;
            wobbleDir = bounce ? minWobbleVector : maxWobbleVector;
            
        }
        
    }
}
