using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CPU_Pathfinding : MonoBehaviour
{
       public Transform[] waypoints;
    private int currentWaypointIndex;
    private UnityEngine.AI.NavMeshAgent agent;
    private CarController carController;
    [SerializeField] private Rigidbody rb;
    
    [Header("Steering")]
    [SerializeField] private float steeringSpeed;
    [SerializeField] private float accelSpeed;
    [SerializeField] private float dotProduct;
    [SerializeField] float NPC_Vert = 0f;
    [SerializeField] float NPC_Horiz = 0f;

    [Header("Raycasts Info")]
    private float raycastDistance = 25f;
    private float rayAngle = 20f;
    private int rayCount = 3;
    [SerializeField] bool obstruction = false;

    
    private Transform[] raycastOrigins;

    void Awake(){
        carController = GetComponent<CarController>();
    }
    void Start()
    {
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        currentWaypointIndex = 0;
        // Time.timeScale = .5f;
    }

    void Update()
    {
        CalcDirToNextWaypoint();
    }

    void SetNextWaypoint()
    {
        if (currentWaypointIndex < waypoints.Length)
        {
            // agent.SetDestination(waypoints[currentWaypointIndex].position);
            currentWaypointIndex++;
        }
    }

    void CalcDirToNextWaypoint(){
        //Starts car raycasts.
        RaycastHit hit;

        float totalAngle = (rayCount - 1) * rayAngle;
        float startAngle = -totalAngle / 2;

        //creates 3 rays at 20 degreens from cars forward vector.
        for(int i = 0; i < rayCount; i++) {
            Vector3 direction = Quaternion.Euler(0, startAngle + i * rayAngle, 0) * transform.forward;

            //NOTE: AND FACING FORWARD VIA DOT PRODUCT
            if(Physics.Raycast(transform.position, direction, out hit, raycastDistance)) {
                    obstruction = true;
                    if(i == 0) {
                        turnRight();
                        // Debug.Log("Obstruction Detected. Turning Right...");
                    }
                    else if(i == 2){
                        turnLeft();
                        // Debug.Log("Obstruction Detected. Turning Left...");
                    }
            }
            else{
                obstruction = false;
                // Debug.Log("Waypoint-Based navigation reingaging");
            }

            Debug.DrawRay(transform.position, direction * raycastDistance, obstruction ? Color.red : Color.green);
            
        }


        //IMPORTANT: Make different Vertical movement behavior if statements for multiple scenarios

        //detects whether waypoint is in front of behind car.
        Vector3 moveDirection = (waypoints[currentWaypointIndex].position - transform.position).normalized;
        float distanceFromWaypoint = Vector3.Distance(waypoints[currentWaypointIndex].position, transform.position);
        dotProduct = Vector3.Dot(transform.forward, moveDirection);
        
        // Debug.Log("[Dot-Product]: " + dotProduct);

        //accel or reverse based on wapoint location
        if(dotProduct > 0f){
            moveForwards();
        }
        //else if (dotProduct < 0f).. create function to turn around.
        else{
            moveBackwards();
        }


        //if next waypoint isnt directly in front, start turning.
        float angleToDirection =  Vector3.SignedAngle(transform.forward, moveDirection, Vector3.up);
        if(dotProduct <= .995f && !obstruction){
            if(angleToDirection > 0f){
                turnRight();
            }
            else {
                turnLeft();
            }
        }
        else if(!obstruction) {
            NPC_Horiz = 0f;
        }

        carController.cpu_braking(dotProduct);

        // Debug.Log(gameObject + " - Magnitude: " + rb.velocity.magnitude);
        // Debug.Log("NPC Move Input: " + NPC_Vert + " NPC Steering Input: " + NPC_Horiz);
        // Debug.Log("Next Waypoint #" + currentWaypointIndex + " Is at: [" + waypoints[currentWaypointIndex].position + "]");
        // Debug.Log("[Distance to Waypoint]: " + Vector3.Distance(waypoints[currentWaypointIndex].position, transform.position));

        //sends verical and horizontal movement information to car controller.
        carController.setInput(NPC_Vert, NPC_Horiz);


        // Debug.Log("Remaining distance to waypoint: " + agent.remainingDistance);

        //sets next waypoint for NPC cars
        if (agent.remainingDistance < agent.stoppingDistance || distanceFromWaypoint <= 20f)
        {
            // Debug.Log("Now Setting to waypoint #" + currentWaypointIndex);
            SetNextWaypoint();
        }
    }
    void turnRight(){
        NPC_Horiz = Mathf.Lerp(NPC_Horiz, 1f, Time.deltaTime * steeringSpeed);
        if(NPC_Horiz >= .9f){
            NPC_Horiz = 1f;
        }
    }
    void turnLeft(){
        NPC_Horiz = Mathf.Lerp(NPC_Horiz, -1f, Time.deltaTime * steeringSpeed);
        if(NPC_Horiz <= -.9f){
            NPC_Horiz = -1f;
        }
    }
    void moveForwards(){
        NPC_Vert = Mathf.Lerp(NPC_Vert, 1f, Time.deltaTime * accelSpeed);

        if(NPC_Vert >= .9f){
            NPC_Vert = 1f;
        }
    }
    void moveBackwards(){
        NPC_Vert = Mathf.Lerp(NPC_Vert, -1f, Time.deltaTime * accelSpeed);
        if(NPC_Vert <= -.9f){
            NPC_Vert = -1f;
        }
    }
}
