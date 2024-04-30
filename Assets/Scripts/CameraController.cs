using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform carTransform;
    [SerializeField] private float timeMultiplyer;
    [SerializeField] private float shakeDuration = 3f;
    [SerializeField] float cameraRotationSpeed = 5f;
    private Quaternion initialRotationOffset;
    void Start()
    {
       initialRotationOffset = Quaternion.Inverse(carTransform.rotation) * transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        Quaternion targetRotation = carTransform.rotation * initialRotationOffset;
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, cameraRotationSpeed * Time.deltaTime);
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
