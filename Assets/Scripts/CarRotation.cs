using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateCar : MonoBehaviour
{
    // Rotation speed in degrees per second
    public float rotationSpeed = 10f;

    void Update()
    {
        // Rotate the car around its up axis (y-axis)
        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
    }
}

