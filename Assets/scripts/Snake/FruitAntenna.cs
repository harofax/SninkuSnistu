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

    // TODO: remove the color stuff
  
    [SerializeField]
    private Color32 FAR_EMISSION_COLOUR = new Color(219, 0, 48);

    [SerializeField]
    private Color32 NEAR_EMISSION_COLOUR = new Color(0, 255, 138);

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

        eyeMaterial.SetColor(EmissionColor, Color.Lerp(NEAR_EMISSION_COLOUR, FAR_EMISSION_COLOUR, distance / proximityRange));
        // TODO: FIX THIS, try lerping hue in HSBColor instead??
    }

    public void UpdateFruitDirection()
    {
        targetVector = fruitTransform.position - transform.position;
        distance = targetVector.magnitude;
    }
}
