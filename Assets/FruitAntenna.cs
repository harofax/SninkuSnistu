using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FruitAntenna : MonoBehaviour
{
    [SerializeField]
    private Transform fruitTransform;

    [SerializeField]
    private Material eyeMaterial;
    
    [SerializeField, Range(1f, 8f)]
    private float turningSpeed;
    
    private float proximityRange;
    
    private Vector3 targetVector;

    //private Color FAR_COLOUR = new Color(255, 0, 176);
    private Color FAR_EMISSION_COLOUR = new Color(255, 0, 48);

    //private Color NEAR_COLOUR = new Color();
    private Color NEAR_EMISSION_COLOUR = new Color(0, 255, 121);

    private float distance;
    private static readonly int EmissionColor = Shader.PropertyToID("_EmissionColor");

    // Start is called before the first frame update
    private void Start()
    {
        targetVector = fruitTransform.position - transform.position;
        proximityRange = GridController.Instance.GridDimensions.magnitude * GridController.Instance.GridUnit;
    }

    // Update is called once per frame
    private void Update()
    {
        transform.up = Vector3.MoveTowards(transform.up, targetVector, Time.deltaTime * turningSpeed);

        eyeMaterial.SetColor(EmissionColor, Color.Lerp(FAR_EMISSION_COLOUR, NEAR_EMISSION_COLOUR, distance / proximityRange));
        // TODO: FIX THIS, try lerping hue in HSBColor instead??
    }

    public void UpdateFruitDirection()
    {
        targetVector = fruitTransform.position - transform.position;
        distance = targetVector.magnitude;
    }
}
