using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class CameraController : MonoBehaviour
{
    [SerializeField] private float timeMultiplyer;
    [SerializeField] private float shakeDuration = 3f;

    public Transform target; // The target to follow (your car)
    public float smoothSpeed = 0.125f; // The smoothness of the camera's movement
    public Vector3 offset;
    void Start()
    {
    //    offset = new Vector3(0f, 3.9f, -10);
    }

    // Update is called once per frame
    void Update()
    {
    }

    // void LateUpdate(){
    //     if (target != null)
    //     {
    //         // Vector3 desiredPosition = target.position + offset;
    //         // Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
    //         // transform.position = smoothedPosition;

    //         // transform.LookAt(target);
    //         // cameraFollow();
    //     }
    // }
    private void cameraFollow(){
        Vector3 desiredPosition = target.position + offset;

            // Calculate the desired Y rotation of the camera
            float desiredYRotation = target.eulerAngles.y;

            // Smoothly move and rotate the camera towards the desired position and rotation
            transform.position = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
            transform.rotation = Quaternion.Euler(0f, desiredYRotation, 0f);
    }
    public void cameraShake(){
        StartCoroutine(ShakeCoroutine());   
    }
     IEnumerator ShakeCoroutine(){
        // _isBoosting = !_isBoosting;
        float elapsedTime = 0f;
        float shakeMagnitude = .003f;
        Vector3 originalPosition = transform.localPosition;

        //shake camera for set amount of seconds.
        while(elapsedTime < shakeDuration){
            // Method 1
            Vector3 shakeOffset = transform.position + Random.insideUnitSphere * shakeMagnitude;
            transform.position = shakeOffset;
            //Method 2
            // Vector3 shakeOffset = originalPosition + Random.insideUnitSphere * shakeMagnitude;
            // transform.position = Vector3.Lerp(transform.position, shakeOffset, Time.deltaTime * 10f); // Adjust the lerp speed as needed
            // print(elapsedTime); 

            elapsedTime += (Time.deltaTime);
            yield return null;
        }
        print("Camera done shaking.");
        transform.localPosition = originalPosition;
        // _isBoosting = !_isBoosting;
     }
}
