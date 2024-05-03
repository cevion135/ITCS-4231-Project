using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{
    public GameObject objectPrefab;
    public Transform centerPoint;
    public Transform rotationCenter; // New variable for rotation center
    public int numberOfObjects = 10;
    public float circleRadius = 10f;
    public float rotationSpeedAroundCenter = 1f; // Rotation speed around the center
    public float rotationSpeedSelf = 1f; // Rotation speed around own axis
    public float heightOffset = 5f; // Height of the objects above the center point
    public float objectSize = 1f;   // Size of the objects

    private void Start()
    {
        SpawnObjects();
    }

    private void SpawnObjects()
    {
        for (int i = 0; i < numberOfObjects; i++)
        {
            float angle = i * (360f / numberOfObjects);
            Vector3 spawnPosition = centerPoint.position + Quaternion.Euler(0, angle, 0) * (Vector3.forward * circleRadius);
            spawnPosition.y += heightOffset;
            GameObject newObj = Instantiate(objectPrefab, spawnPosition, Quaternion.identity);
            newObj.transform.localScale = Vector3.one * objectSize;
            newObj.transform.LookAt(centerPoint);
            // Add rotation script to each object
            ObjectRotation objectRotation = newObj.AddComponent<ObjectRotation>();
            objectRotation.rotationSpeedAroundCenter = rotationSpeedAroundCenter;
            objectRotation.rotationSpeedSelf = rotationSpeedSelf;
            objectRotation.rotationCenter = rotationCenter;
        }
    }
}

public class ObjectRotation : MonoBehaviour
{
    public float rotationSpeedAroundCenter = 1f; // Rotation speed around the center
    public float rotationSpeedSelf = 1f; // Rotation speed around own axis
    public Transform rotationCenter; // Rotation center

    void Update()
    {
        // Rotate the object around the rotation center
        transform.RotateAround(rotationCenter.position, Vector3.up, rotationSpeedAroundCenter * Time.deltaTime);

        // Rotate the object around its own axis
        transform.Rotate(Vector3.up, rotationSpeedSelf * Time.deltaTime);
    }
}






