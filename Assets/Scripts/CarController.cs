using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CarController : MonoBehaviour
{
    [Header("Inspector Assigned Car Parts")]
    [SerializeField] public List<Wheel> wheels;
    [SerializeField] private Rigidbody rb;

    [Header("Car Physics Variables")]
    [SerializeField] private float maxAccel = 30f;
    [SerializeField] private float brakeAccel = 50f;
    [SerializeField] private float maxTurnAngle = 30f;
    [SerializeField] private float turnSensitivity = 1f;
    [SerializeField] private float burnoutRotSpeed = 100000f;
    [SerializeField] private bool burnoutPossible = false;
    private Vector3 _centerOfMass;
    


    [Header("User Input Variables")]
    [SerializeField] public float moveInput;
    [SerializeField] public float steeringInput;

    [Header("Various Booleans")]
    [SerializeField] private bool isBurnout = false;
    [SerializeField] private bool isBraking = false;
    [SerializeField] private bool isBoosting = false;
    [SerializeField] private bool headlights = false;
    // [Header("VFX")]

   public enum Axels {
    Front,
    Rear
  }

  [Serializable]
  public struct Wheel {
    public GameObject wheelMesh;
    public WheelCollider wheelCollider;
    public GameObject wheelFXObj;
    public Axels axel;
  }
   void Start(){
    rb.centerOfMass = _centerOfMass;
   }
    private void Update(){
        getInput();
        wheelRotation();

        //Speed Check
        print(rb.velocity.magnitude);
    }
    private void LateUpdate(){
        move();
        steering();
        braking();
        wheelSkidEffect();
    }
   private void getInput(){
    moveInput = Input.GetAxis("Vertical");
    steeringInput = Input.GetAxis("Horizontal");
    
   }
   private void move(){
    //adds movement in form of motor torque to car based on 'moveInput' variable.
    foreach(var wheel in wheels){
        wheel.wheelCollider.motorTorque = moveInput * 600f * maxAccel * Time.deltaTime;
    }
    burnoutPossible = rb.velocity.magnitude <= 1f ? true : false;
   }
   private void steering(){
    //steers front axel wheels based on 'steeringInput' variable.
    foreach(var wheel in wheels){
        if(wheel.axel == Axels.Front){
            var steerAngle = steeringInput * turnSensitivity * maxTurnAngle;
            wheel.wheelCollider.steerAngle = Mathf.Lerp(wheel.wheelCollider.steerAngle, steerAngle, 0.6f);
        }
    }
   }
   private void braking(){
    //adds brake force when left control is pressed.
        if(Input.GetKey(KeyCode.LeftControl)) {

            //sets trigger booleans accordingly to queue VFX.
           isBraking = true;

           foreach(var wheel in wheels){
                wheel.wheelCollider.brakeTorque = 300f * brakeAccel * Time.deltaTime;
           }
        }
        //removes brake force.
        else{

            //sets trigger booleans accordingly to queue VFX.
            isBraking = false;

            foreach(var wheel in wheels){
                wheel.wheelCollider.brakeTorque = 0f;
            }
        }
   }
   private void burnout(){
    print("burnouts can be done.");
    //rotates back wheels for burnout effect
        if(Input.GetKey(KeyCode.S) && Input.GetKey(KeyCode.W)) {

            isBurnout = true;

            print("BURNOUT HAPPENING");
            foreach(var wheel in wheels){
                if(wheel.axel == Axels.Rear){
                    Quaternion _rotation = Quaternion.Euler(0f, burnoutRotSpeed * Time.deltaTime, 0f);
                    wheel.wheelMesh.transform.rotation *= _rotation;
                    
                }
            }
        }
        else{
            isBurnout = false;
        }
    }
   
   private void wheelRotation(){

        foreach(var wheel in wheels) {

            if(burnoutPossible && Input.GetKey(KeyCode.S) && Input.GetKey(KeyCode.W) ){
                if(moveInput < 0) {
                    moveInput = 0;
                }
                if(wheel.axel == Axels.Rear) {
                    Vector3 position;
                    Quaternion rot;
                    wheel.wheelCollider.GetWorldPose(out position, out rot);
                    Quaternion _rotation = Quaternion.Euler(100000f * burnoutRotSpeed * Time.deltaTime, 0f, 0f);
                    print(_rotation);
                    wheel.wheelMesh.transform.rotation *= _rotation; 
                    wheel.wheelMesh.transform.position = position;  
                }
                if(wheel.axel == Axels.Front) {
                    Quaternion rotation;
                    Vector3 position;
                    wheel.wheelCollider.GetWorldPose(out position, out rotation);
                    wheel.wheelMesh.transform.position = position;
                    wheel.wheelMesh.transform.rotation = rotation;
                }
            }
            else{
                Quaternion rotation;
                Vector3 position;
                wheel.wheelCollider.GetWorldPose(out position, out rotation);
                wheel.wheelMesh.transform.position = position;
                wheel.wheelMesh.transform.rotation = rotation;
            }
        }
   }
   private void wheelSkidEffect(){
    foreach(var wheel in wheels){
        if(isBraking || isBurnout) {
            wheel.wheelFXObj.GetComponentInChildren<TrailRenderer>().emitting = true;
        }
        else{
            wheel.wheelFXObj.GetComponentInChildren<TrailRenderer>().emitting = false;
        }
    }
}
}
