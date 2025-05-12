using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BillboardEffect : MonoBehaviour
{
    public Transform cam;

    void Awake()
    {
        cam = Camera.main.transform;
        if (cam == null)
        {
            Debug.LogError("No camera found in the scene. Please assign a camera to the BillboardEffect script.");
        }
    }

    void LateUpdate()
    {
        transform.LookAt(transform.position + cam.forward);
    }
}
