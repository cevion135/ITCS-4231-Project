using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RaceManager : MonoBehaviour
{
    [SerializeField] private List<Transform> trackNodes;
    [SerializeField] private List<GameObject> carsList;
    [SerializeField] private Dictionary<GameObject, int> carPlacements;
    [SerializeField] private TextMeshProUGUI placementText;
    [SerializeField] private Transform placementTextLocation;

    private Vector3 startPlacementPos = new Vector3(-126f, 140f, 0f);
     private Vector3 endPlacementPos = new Vector3(-140f, 140f, 0f);
    public Color gold;
    public Color silver;
    public Color bronze;


    // [SerializeField] private Material[] trafficLights;
    [SerializeField] private Renderer[] trafficLight;
    private float lightOff = 0f;
    private float lightOn = 2f;
    

    void Start()
    {
        carPlacements = new Dictionary<GameObject, int>();
        populateCarList();
        startCountdown();
    }

    // Update is called once per frame
    void Update()
    {
        calculatePlacement();
    }

    //based on car array, this script will determine what cars are in what place during the race.
    private void calculatePlacement(){
         //order the list...
       carsList.Sort((car1, car2) =>
        {
            float distance1 = distanceToNode(car1.transform);
            float distance2 = distanceToNode(car2.transform);
            return distance1.CompareTo(distance2);
        });
        
        //assign values accordingly
        for (int i = 0; i < carsList.Count; i++)
        {
           if (!carPlacements.ContainsKey(carsList[i]))
            {
                carPlacements.Add(carsList[i], i + 1); // Assign placement from 1 to N
            }
        }
        updatePlayerPlacement();
    }
    private void updatePlayerPlacement(){
        int playerCarIndex = -1;
        for (int i = 0; i < carsList.Count; i++)
        {
            if (carsList[i].CompareTag("Player"))
            {
                playerCarIndex = i;
                break;
            }
        }

        switch (playerCarIndex){
            case 0:
                placementText.text = "1st";
                placementText.color = gold;
                break;
            case 1:
                placementText.text = "2nd";
                placementText.color = silver;
                break;
            case 2:
                placementText.text = "3rd";
                placementText.color = bronze;
                break;    
            case 3:
                placementText.text = "4th";
                placementText.color = Color.red;
                break;  
            case 4:
                placementText.text = "5th";
                placementText.color = Color.red;
                break;    
            default:
                Debug.Log("Player placement could not be determined");
                break;
        }
    }

    private void disableMovement(){
        foreach(GameObject car in carsList){
            car.GetComponent<CarController>().enabled = false;
        }
    }
    private void enableMovement(){
        foreach(GameObject car in carsList){
            car.GetComponent<CarController>().enabled = true;
        }
    }
    //determines cars distance from the nearest track node
    private float distanceToNode(Transform carTransform){
        float minDistance = Mathf.Infinity;
        foreach (Transform node in trackNodes)
        {
            float distance = Vector3.Distance(carTransform.position, node.position);
            if (distance < minDistance)
            {
                minDistance = distance;
            }
        }
        return minDistance;

    }
 
    //fills in the list with the cars in the scene
    private void populateCarList(){
        GameObject[] foundObjects = GameObject.FindGameObjectsWithTag("NPC_Cars");
        foreach (GameObject obj in foundObjects)
        {
            carsList.Add(obj);
        }
        carsList.Add(GameObject.FindGameObjectWithTag("Player"));
    }

    private void startCountdown(){
        disableMovement();
        StartCoroutine(triggerTrafficLight());
    }

    //handles traffic light lighting.
    IEnumerator triggerTrafficLight(){
        Color trafficRed = trafficLight[0].material.GetColor("_Color");
        Color trafficYellow = trafficLight[1].material.GetColor("_Color");
        Color trafficGreen = trafficLight[2].material.GetColor("_Color");

        trafficLight[0].material.SetColor("_EmissionColor", trafficRed * lightOff);
        trafficLight[1].material.SetColor("_EmissionColor", trafficYellow * lightOff);
        trafficLight[2].material.SetColor("_EmissionColor", trafficGreen * lightOff);
        yield return new WaitForSeconds(2.6f);


        // placementTextLocation.position = Vector3.Lerp(startPlacementPos, endPlacementPos, 3f * Time.deltaTime);
        trafficLight[0].material.SetColor("_EmissionColor", trafficRed * lightOn);
        yield return new WaitForSeconds(1.4f);

        trafficLight[0].material.SetColor("_EmissionColor", trafficRed * lightOff);
        trafficLight[1].material.SetColor("_EmissionColor", trafficYellow * lightOn);
        yield return new WaitForSeconds(.975f);

        trafficLight[1].material.SetColor("_EmissionColor", trafficYellow * lightOff);
         yield return new WaitForSeconds(.15f);

         trafficLight[1].material.SetColor("_EmissionColor", trafficYellow * lightOn);
        yield return new WaitForSeconds(.975f);

        enableMovement();
        trafficLight[1].material.SetColor("_EmissionColor", trafficYellow * lightOff);
        trafficLight[2].material.SetColor("_EmissionColor", trafficGreen * lightOn);

        yield return new WaitForSeconds(5f);
        trafficLight[2].material.SetColor("_EmissionColor", trafficGreen * lightOff);
    }
}
