using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class CarController : MonoBehaviour
{
    [Header("Inspector Assigned Car Parts")]
    [SerializeField] public List<Wheel> wheels;
    [SerializeField] private Rigidbody rb;

    [Header("Car Physics Variables")]
    [SerializeField] private float currentCarSpeed;
    [SerializeField] private float autoBrakeThreshold = 20f;
    [SerializeField] private float maxAccel;
    [SerializeField] private float brakeAccel;
    [SerializeField] private float maxTurnAngle = 30f;
    [SerializeField] private float turnSensitivity = 1f;
    [SerializeField] private float burnoutRotSpeed = 100000f;
    [SerializeField] private float boostForce = 1200f;
    [SerializeField] private float boostThreshold = 100f;
     [SerializeField] public float boostGuage = 0;
    //   [SerializeField] private Slider boostGuageSlider;
    [SerializeField] private float slideThreshold = 2f;
    [SerializeField] private bool burnoutPossible = false;
    private Vector3 _centerOfMass;
    [SerializeField] private Vector3 latVelo;
    [SerializeField] private float latVeloMag;
    


    [Header("User Input Variables")]
    [SerializeField] public float moveInput;
    [SerializeField] public float steeringInput;

    [Header("Various Booleans")]
    [SerializeField] private bool isBurnout = false;
    [SerializeField] private bool isSliding = false;
    [SerializeField] private bool isBraking;
    [SerializeField] private bool isBoosting = false;
    [SerializeField] private bool isNPC = false;
    [Header("Misc")]

    [SerializeField] private CameraController cameraClass;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float groundRayLength = .5f;
    [SerializeField] private ParticleSystem LeftExhaustBoost;
    [SerializeField] private ParticleSystem RightExhaustBoost;

   public enum Axels {
    Front,
    Rear
  }

  [Serializable]
  public struct Wheel {
    public GameObject wheelMesh;
    public WheelCollider wheelCollider;
    public GameObject wheelFXObj;
    public ParticleSystem FX_TireSmoke;
    public Axels axel;
  }
   void Start(){
    // boostGuageSlider.value = 0f;
    rb.centerOfMass = _centerOfMass;
    if (gameObject.tag == "Player") {
        isNPC = false;
    }
    else {
        isNPC = true;
    }
    // Debug.Log(gameObject);
   }
    private void Update(){
        if(!isNPC){
            getInput();
        }
        wheelRotation();
        calcLatVelocity();
        getCarSpeed();
        //Speed Check
        // print(rb.velocity.magnitude);
    }
    private void LateUpdate(){
        move();
        steering();
        if(!isNPC){
            braking();
        }
        if(Input.GetKey(KeyCode.LeftShift)){
            initiateBoost();
        }
        wheelVFX();
    }
   private void getInput(){
        moveInput = Input.GetAxis("Vertical");
        steeringInput = Input.GetAxis("Horizontal");
        // Debug.Log("[On Game Object: " + gameObject + "] Move Input: " + moveInput + " Steering Input: " + steeringInput);
   }
    
    public void setInput(float vert, float horiz){
        moveInput = vert;
        steeringInput = horiz;
    }
   private void move(){
    //adds movement in form of motor torque to car based on 'moveInput' variable.
    if(!isNPC){
        foreach(var wheel in wheels){
        wheel.wheelCollider.motorTorque = moveInput * 250f * maxAccel * Time.deltaTime;
        // Debug.Log("[On Game Object: " + gameObject + "] Wheel Collider Torque: " + wheel.wheelCollider.motorTorque);
            burnoutPossible = rb.velocity.magnitude <= 1f ? true : false;
        }
    }
    else{
    foreach(var wheel in wheels){
        wheel.wheelCollider.motorTorque = moveInput * 12000f * maxAccel * Time.deltaTime;
        // Debug.Log("[On Game Object: " + gameObject + "] Wheel Collider Torque: " + wheel.wheelCollider.motorTorque);
    }
    burnoutPossible = rb.velocity.magnitude <= 1f ? true : false;
    }
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

   public void cpu_braking(float dotProduct){
        // Debug.Log("Wheel Collider Brake Torque: " + wheels[0].wheelCollider.brakeTorque);
        // Debug.Log("Current Lateral Velocity: [" + latVeloMag + "]." );


        // //check to make sure brakes don't apply at low speeds.
        // if(currentCarSpeed >= autoBrakeThreshold) {
            //if the next waypoints angle is slightly off, do a light brake.
            if((currentCarSpeed >= autoBrakeThreshold) && (dotProduct <= .9f && dotProduct >= .8f) || latVeloMag > 1.5f && latVeloMag < 2f ) {
                foreach(var wheel in wheels){
                    wheel.wheelCollider.brakeTorque = 300f * brakeAccel * Time.deltaTime;
                }
                    isBraking = true;
                    // Debug.Log("Applying LIGHT brake force. | Current Car Speed: " + currentCarSpeed);
            }
            //if the next waypoints angle is moderately off, do a moderate brake.
            else if((currentCarSpeed >= autoBrakeThreshold) && (dotProduct < .8f && dotProduct >= .5f) || latVeloMag > 2f && latVeloMag < 5f){
                foreach(var wheel in wheels){
                    wheel.wheelCollider.brakeTorque = 500f * brakeAccel * Time.deltaTime;
                }
                    isBraking = true;
                    // Debug.Log("Applying MODERATE brake force. | Current Car Speed: " + currentCarSpeed);
            }
            //if the next waypoints angle is very off, do a heavy brake.
            else if(currentCarSpeed >= autoBrakeThreshold && ( dotProduct < .5f) || latVeloMag > 5f){
                foreach(var wheel in wheels){
                    wheel.wheelCollider.brakeTorque = 700f * brakeAccel * Time.deltaTime;
                }
                    isBraking = true;
                    // Debug.Log("Applying HEAVY brake force. | Current Car Speed: " + currentCarSpeed);
            }
            else{
                foreach(var wheel in wheels){
                    wheel.wheelCollider.brakeTorque = 0f;
                }
                // Debug.Log("No braking applied. | Current Car Speed: " + currentCarSpeed);
                isBraking = false;
            } 
        // }
        // else {
        //     foreach(var wheel in wheels){
        //         wheel.wheelCollider.brakeTorque = 0f;
        //     }
        //     Debug.Log("Speed is below auto-brake threshold of [" + autoBrakeThreshold + "]. Auto-braking cannot take place." );
        // }
   }
   
   private void burnout(){
    print("burnouts can be done.");
    //rotates back wheels for burnout effect
        if(Input.GetKey(KeyCode.S) && Input.GetKey(KeyCode.W)) {

            isBurnout = true;

            // print("BURNOUT HAPPENING");
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
                    // print(_rotation);
                    wheel.wheelMesh.transform.rotation *= _rotation; 
                    wheel.wheelMesh.transform.position = position;

                    wheel.FX_TireSmoke.Emit(1); 
                }
                if(wheel.axel == Axels.Front) {
                    Quaternion rotation;
                    Vector3 position;
                    wheel.wheelCollider.GetWorldPose(out position, out rotation);
                    wheel.wheelMesh.transform.position = position;
                    wheel.wheelMesh.transform.rotation = rotation;
                }

                isBurnout = true;
            }
            else{
                Quaternion rotation;
                Vector3 position;
                wheel.wheelCollider.GetWorldPose(out position, out rotation);
                wheel.wheelMesh.transform.position = position;
                wheel.wheelMesh.transform.rotation = rotation;
                isBurnout = false;
            }
        }
   }
   private void wheelVFX(){
    foreach(var wheel in wheels){
        //if braking, moving, and grounded, emit particles from all wheels.
        //if sliding and grounded... emit particles from rear wheels.
        if((isBraking && rb.velocity.magnitude >= 10f && isGrounded())||(isSliding && wheel.axel == Axels.Rear && isGrounded())) {
            wheel.wheelFXObj.GetComponentInChildren<TrailRenderer>().emitting = true;
            wheel.FX_TireSmoke.Emit(1);
        }
        else{
            wheel.wheelFXObj.GetComponentInChildren<TrailRenderer>().emitting = false;
        }

    }
}
    //function that determines whether the car is sliding. If so, a boolean is triggered so that particle effects can be applied.
    private void calcLatVelocity(){
        // print("Calculating Lateral Velocity");
        Vector3 right = transform.right;
        Vector3 latVel = Vector3.Project(rb.velocity, right);
        latVelo = latVel;
        latVeloMag = latVel.magnitude;

        // Debug.Log("[" + gameObject + "] Lateral Velocity: " + latVel.magnitude);

        if(latVel.magnitude > slideThreshold){
            isSliding = true;
            // print("Sliding Active");
        }
        else{
            isSliding = false;
            // print("Sliding not active");
        }
    }
    
    private void getCarSpeed(){
        currentCarSpeed = rb.velocity.magnitude;
    }

    //raycast to check if the cars wheels are on the ground.
    private bool isGrounded(){
        Vector3 rayOrigin = transform.position;
        RaycastHit hit;

        if (Physics.Raycast(rayOrigin, Vector3.down, out hit, groundRayLength, groundLayer))
        {
            // print("Car is grounded");
            return true;
        }
        // print("Car is !NOT! on the ground");
        return false;
    }
    // public void changeBoostingStatus(){
    //     isBoosting = !isBoosting;
    // }
    private void initiateBoost(){
        if(!isNPC && (boostGuage == boostThreshold)){
            isBoosting = !isBoosting;
            LeftExhaustBoost.Play();
            RightExhaustBoost.Play();
            // cameraClass.cameraShake();
            StartCoroutine(ShakeCoroutine());
            rb.AddForce(transform.forward * boostForce, ForceMode.Impulse);
            boostGuage = 0f;
        }


    }
    private void OnCollisionEnter(Collision collision)
    {
        if(!isNPC){
            if(collision.gameObject.transform.parent != null){
                GameObject parentObject = collision.gameObject.transform.parent.gameObject;
                // Debug.Log(parentObject);
                if (parentObject.CompareTag("Zombie"))
                {
                     if(boostGuage < 100f){
                     boostGuage += 50f;
                    //  boostGuageSlider.value = Mathf.Lerp(boostGuageSlider.value, boostGuage, 0.05f);
                     Debug.Log("zombie hit! current boost guage: " + boostGuage);
                     }
                }
            }
        }
    }
    IEnumerator ShakeCoroutine(){
        yield return new WaitForSeconds(3);
        isBoosting = !isBoosting;
    }
    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Vector3 rayOrigin = transform.position;
        Gizmos.DrawRay(rayOrigin, Vector3.down * groundRayLength);
    }
}