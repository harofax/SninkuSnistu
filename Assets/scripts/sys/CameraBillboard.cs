using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBillboard : MonoBehaviour
{

    private Camera cam;

    private void Start()
    {
        cam = Camera.main;
    }

    // Update is called once per frame
    private void LateUpdate() {
        //var camPos = Camera.main.transform.position;
        // var currentPos = transform.position;
        //
        // var facingVector = camPos - currentPos;
        // facingVector.y = currentPos.y;
        //
        //
        //transform.forward = new Vector3(Camera.main.transform.forward.x, transform.forward.y, Camera.main.transform.forward.z);
        Transform tfm = transform;
        tfm.LookAt(cam.transform);

        transform.rotation = Quaternion.Euler(0f, tfm.rotation.eulerAngles.y, 0f);
    }
}
