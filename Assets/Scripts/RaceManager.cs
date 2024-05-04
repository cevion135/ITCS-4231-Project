using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaceManager : MonoBehaviour
{
    [SerializeField] private GameObject[] cars;
    [SerializeField] private Material[] trafficLights;
    [SerializeField] private Renderer[] rend;
    private float lightOff = 0f;
    private float lightOn = 2f;

    void Start()
    {
        populateCarList();
        startCountdown();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //based on car array, this script will determine what cars are in what place during the race.
    public void calculatePlacement(){

    }
    private void populateCarList(){
        // Find all GameObjects in the "Car" layer
        GameObject[] carObjects = GameObject.FindGameObjectsWithTag("NPC_Cars");
        cars = new GameObject[carObjects.Length + 1];



        for (int i = 0; i < carObjects.Length; i++)
        {
            cars[i] = carObjects[i];
        }
        cars[cars.Length - 1] = GameObject.FindGameObjectWithTag("Player");

        Debug.Log("Found " + cars.Length + " cars on the racetrack");   
    }

    private void startCountdown(){
        StartCoroutine(triggerTrafficLight());
    }

    IEnumerator triggerTrafficLight(){
        Color trafficRed = rend[0].material.GetColor("_Color");
        Color trafficYellow = rend[1].material.GetColor("_Color");
        Color trafficGreen = rend[2].material.GetColor("_Color");

        rend[0].material.SetColor("_EmissionColor", trafficRed * lightOff);
        rend[1].material.SetColor("_EmissionColor", trafficYellow * lightOff);
        rend[2].material.SetColor("_EmissionColor", trafficGreen * lightOff);
        yield return new WaitForSeconds(2.6f);

        rend[0].material.SetColor("_EmissionColor", trafficRed * lightOn);
        yield return new WaitForSeconds(1.4f);

        rend[0].material.SetColor("_EmissionColor", trafficRed * lightOff);
        rend[1].material.SetColor("_EmissionColor", trafficYellow * lightOn);
        yield return new WaitForSeconds(.975f);

        rend[1].material.SetColor("_EmissionColor", trafficYellow * lightOff);
         yield return new WaitForSeconds(.15f);

         rend[1].material.SetColor("_EmissionColor", trafficYellow * lightOn);
        yield return new WaitForSeconds(.975f);

        rend[1].material.SetColor("_EmissionColor", trafficYellow * lightOff);
        rend[2].material.SetColor("_EmissionColor", trafficGreen * lightOn);

        yield return new WaitForSeconds(5f);
        rend[2].material.SetColor("_EmissionColor", trafficGreen * lightOff);
    }
}
