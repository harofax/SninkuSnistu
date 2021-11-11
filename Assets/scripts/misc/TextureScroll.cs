using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextureScroll : MonoBehaviour
{
    [SerializeField]
    private Vector2 offset;

    private Material mat;
    
    // Start is called before the first frame update
    void Start()
    {
        mat = GetComponent<MeshRenderer>().material;
    }

    // Update is called once per frame
    void Update()
    {

        mat.mainTextureOffset += offset * Time.deltaTime;
    }
}
