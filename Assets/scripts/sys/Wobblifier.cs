using System;
using System.Collections;
using System.Collections.Generic;
using Freya;
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
        set => maxWobble = value;
    }

    public float MinWobble
    {
        set => minWobble = value;
    }

    public float WobbleRate
    {
        get => wobbleRate;
        set => wobbleRate = value;
    }

    private void Start()
    {
        float baseScale = transform.localScale.x;
        minWobble *= baseScale;
        maxWobble *= baseScale;
    }

    // Update is called once per frame
    private void Update()
    {
        float t = Mathf.PingPong(Time.time * wobbleRate, 1f).Smooth01();
        float s = Mathf.Lerp(minWobble, maxWobble, t);
        transform.localScale =  new Vector3(s, s, s);
    }
}
