using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CarMovement : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;
    [SerializeField] private float maxSpeed;
    [SerializeField] private float currentSpeed = 0f;
    [SerializeField] private Vector3 acceleration;
    [SerializeField] private Vector3 lastPosition;
    [SerializeField] private InputAction playerControls;
    [SerializeField] private Transform[] wheels;
    private float wheelRadius = 0.5f;
    private Vector3 carForwardVector;
    private Vector3 rotatedVector;
    private float turningThreshold;

    // Start is called before the first frame update
    void Start()
    {
        carForwardVector = transform.forward;
        rotatedVector = carForwardVector;
        // foreach(Transform wheel in wheels){
        //     Debug.Log(wheel.position.x);
        // }
        // Debug.Log(wheels[0].position.x);
        // Debug.Log(wheels[1].position.x);
        // Debug.Log(wheels[2].position.x);
        // Debug.Log(wheels[3].position.x);
    }

    // Update is called once per frame
    void Update()
    {
        captureCurrentSpeed();
        accelAndDecel();
        handleTurning();
        // rotateWheels();
        // Debug.Log("Acceleration:" + acceleration);
    }
    void accelAndDecel(){
        //movement by Input.GetAxis with Decelerating force.
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");
        Vector3 movement = new Vector3(0.0f, 0.0f, moveVertical).normalized;
        // Debug.Log("movement:" + movement);

        acceleration = movement * maxSpeed * 1f;

        rb.AddForce(acceleration * Time.deltaTime);
    }
    void captureCurrentSpeed(){
        Vector3 currentPosition = transform.position;
        float distance = Vector3.Distance(currentPosition, lastPosition);
        currentSpeed = distance / Time.deltaTime;
        lastPosition = currentPosition;

        // Debug.Log("Current Speed:" + currentSpeed);
    }
    void rotateWheels() {
        float linearSpeed = rb.velocity.magnitude;

        float angularSpeed = linearSpeed / (2 * Mathf.PI * wheelRadius);

        foreach(Transform wheel in wheels){
            wheel.Rotate(0f, angularSpeed, Time.deltaTime * 360);
        }

        Debug.Log(angularSpeed);
    }
    void handleTurning() {
        if(Input.GetKey(KeyCode.A)){
            rotatedVector = Quaternion.Euler(0,-.1f,0) * carForwardVector;
            rotatedVector = carForwardVector += rotatedVector;
        }
        if(Input.GetKey(KeyCode.D)){
             rotatedVector = Quaternion.Euler(0,.1f,0) * carForwardVector;
            rotatedVector = carForwardVector += rotatedVector;
        }
        Debug.Log("Vector Angle: " + rotatedVector.x);
    Debug.DrawRay(transform.position, rotatedVector * 10, Color.blue);
    }
}
