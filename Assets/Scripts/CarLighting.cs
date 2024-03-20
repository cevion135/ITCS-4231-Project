using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarLighting : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;
    [Header("Lighting Values")]
    [SerializeField] private float tailLightsIdle = .3f;
    [SerializeField] private float tailLightsEngaged = 1f;
    [SerializeField] private float highBeamSpotAngle = 200f;
    [SerializeField] private float lowBeamSpotAngle = 80f;
    [SerializeField] private float highBeamIntensity = 20f;
    [SerializeField] private float lowBeamIntensity = 4f;

    [Header("Triggers")]
    [SerializeField] private bool headlights = true;
    [SerializeField] private bool tailLights = true;
    [SerializeField] private bool highbeams = false;
    [SerializeField] private bool isBraking = false;

    [Header("Lighting Object references")]
    [SerializeField] private GameObject FX_Headlight_Right;
    [SerializeField] private GameObject FX_Headlight_Left;
    [SerializeField] private Light R_HL_SpotLight;
    [SerializeField] private Light L_HL_SpotLight;
    
    [SerializeField] private Light FX_Taillight_Right;
    [SerializeField] private Light FX_Taillight_Left;


 

    // Update is called once per frame
    void Start(){
        R_HL_SpotLight.enabled = true;
        L_HL_SpotLight.enabled = true;
        FX_Taillight_Left.enabled = true;
        FX_Taillight_Right.enabled = true;
    }
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Tab)){
            toggleLights();
        }
        if(Input.GetKeyDown(KeyCode.Q)){
            toggleHighBeams();
        }

        checkForBraking();
    }

    //engages tail lights if the car is braking.
    private void checkForBraking(){
        // print(Vector3.Dot(rb.velocity, transform.forward));
        if(Input.GetKey(KeyCode.LeftControl) || (dotProduct() >= 0 && Input.GetKey(KeyCode.S))){
            isBraking = true;
            FX_Taillight_Left.intensity = tailLightsIdle;
            FX_Taillight_Right.intensity = tailLightsIdle;
        }
        else{
            isBraking = false;
            FX_Taillight_Left.intensity = tailLightsEngaged;
            FX_Taillight_Right.intensity = tailLightsEngaged;
        }
    }
    //check whether car is moving forwards or backwards
    private float dotProduct(){
        return Vector3.Dot(rb.velocity, transform.forward);
    }
    private void toggleLights(){
        //if on... turn off | Headlight
        if(headlights){
            L_HL_SpotLight.enabled = false;
            R_HL_SpotLight.enabled = false;
    
            FX_Headlight_Left.SetActive(false);
            FX_Headlight_Right.SetActive(false);
        }
        //if off... turn on 
        else{
            L_HL_SpotLight.enabled = true;
            R_HL_SpotLight.enabled = true;

            FX_Headlight_Left.SetActive(true);
            FX_Headlight_Right.SetActive(true);
        }

        //if off... turn on | TailLight
        if(tailLights){
            FX_Taillight_Left.enabled = false;
            FX_Taillight_Right.enabled = false;
        }
        else{
            FX_Taillight_Left.enabled = true;
            FX_Taillight_Right.enabled = true;
        }
        headlights = !headlights;
        tailLights = !tailLights;
    }
    private void toggleHighBeams(){
        if(headlights && !highbeams) {
            R_HL_SpotLight.intensity = lowBeamIntensity;
            L_HL_SpotLight.intensity = lowBeamIntensity;
            R_HL_SpotLight.spotAngle = highBeamSpotAngle;
            L_HL_SpotLight.spotAngle = highBeamSpotAngle;
            print("High Beams are now on.");
        }
        if(headlights && highbeams){
             R_HL_SpotLight.intensity = highBeamIntensity;
            L_HL_SpotLight.intensity = highBeamIntensity;
            R_HL_SpotLight.spotAngle = lowBeamSpotAngle;
            L_HL_SpotLight.spotAngle = lowBeamSpotAngle;
            print("High Beams are now off."); 
        }
        highbeams = !highbeams;
    }
}
