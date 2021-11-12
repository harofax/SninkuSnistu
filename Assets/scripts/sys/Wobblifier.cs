using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wobblifier : MonoBehaviour
{
    [SerializeField]
    private float maxWobble;

    [SerializeField] 
    private float minWobble;

    [SerializeField] 
    private float wobbleRate;

    public float MaxWobble
    {
        get => maxWobble;
        set
        {
            maxWobble = value;
            maxWobbleVector = Vector3.one * maxWobble;
        }
    }

    public float MinWobble
    {
        get => minWobble;
        set
        {
            minWobble = value;
            minWobbleVector = Vector3.one * minWobble;
        }
    }

    public float WobbleRate
    {
        get => wobbleRate;
        set => wobbleRate = value;
    }

    //private Transform wobbleTransform;
    
    private Vector3 maxWobbleVector;
    private Vector3 minWobbleVector;

    private Vector3 wobbleDir;

    private bool bounce;
    //private float progress = 0f;
    
    // Start is called before the first frame update
    private void Start()
    {
        //wobbleTransform = transform;
        maxWobbleVector = Vector3.one * maxWobble;
        minWobbleVector = Vector3.one * minWobble;

        wobbleDir = maxWobbleVector;

        StartWobbling();
    }

    public void StartWobbling()
    {
        StopAllCoroutines();
        StartCoroutine(WobbleOverTime(wobbleRate));
    } 

    private IEnumerator WobbleOverTime(float duration)
    {
        float progress = 0;
        Vector3 currentScale = transform.localScale;
        
        while (progress < duration)
        {
            transform.localScale = Vector3.Lerp(currentScale, wobbleDir, progress / duration);
            progress += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        transform.localScale = wobbleDir;
        
        // toggle the wobbleDir from grow to shrink
        wobbleDir = wobbleDir == maxWobbleVector ? minWobbleVector : maxWobbleVector;
        StartCoroutine(WobbleOverTime(wobbleRate));
    }

    // Update is called once per frame
    private void Update()
    {
        // print("----- " + this.name + " -------");
        // progress += wobbleRate * Time.deltaTime;
        // print("progress now: " + progress);
        // print("amount to lerp: " + progress * Time.deltaTime);
        // transform.localScale = Vector3.Lerp(transform.localScale, wobbleDir, progress * Time.deltaTime);
        //
        // if (progress >= 1)
        // {
        //     
        //     print("ping, progress=" + progress);
        //     progress = 0f;
        //     print("progress cleared: " + progress);
        //     bounce = !bounce;
        //     wobbleDir = bounce ? minWobbleVector : maxWobbleVector;
        //     
        // }
        // print("----------------------------");
    }
    
    /* ----- ASK VOLODYMYR
    private IEnumerator WobbleGrow(float duration)
    {
        float progress = 0;
        Vector3 currentScale = transform.localScale;
        while (progress < duration)
        {
            transform.localScale = Vector3.Lerp(currentScale, maxWobbleVector, progress / duration);
            progress += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        transform.localScale = maxWobbleVector;
        StartCoroutine(WobbleShrink(wobbleRate));
    }

    private IEnumerator WobbleShrink(float duration)
    {
        float progress = 0;
        Vector3 currentScale = transform.localScale;
        while (progress < duration)
        {
            transform.localScale = Vector3.Lerp(currentScale, minWobbleVector, progress / duration);
            progress += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        transform.localScale = minWobbleVector;
        StartCoroutine(WobbleGrow(wobbleRate));
    }
     */
}
