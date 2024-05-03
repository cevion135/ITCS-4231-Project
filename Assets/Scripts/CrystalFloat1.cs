using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrystalFloat1 : MonoBehaviour
{
    // Variables for controlling the hover effect
    public float hoverHeight = 0.2f; // Adjust this value to control the height of the hover
    public float hoverSpeed = 1f;    // Adjust this value to control the speed of the hover

    private Vector3 startPosition;

    void Start()
    {
        // Save the initial position of the object
        startPosition = transform.position;
    }

    void Update()
    {
        // Calculate the new Y position based on a sine wave
        float newY = startPosition.y + Mathf.Sin(Time.time * hoverSpeed) * hoverHeight;

        // Update the object's position
        transform.position = new Vector3(startPosition.x, newY, startPosition.z);
    }
}

