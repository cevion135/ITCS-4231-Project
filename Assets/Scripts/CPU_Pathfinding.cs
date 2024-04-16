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
        float NPC_Vert = 1f;
        float NPC_Horiz = 0f;

        //detects whether waypoint is in front of behind car
        Vector3 moveDirection = (waypoints[currentWaypointIndex].position - transform.position).normalized;
        float distanceFromWaypoint = Vector3.Distance(waypoints[currentWaypointIndex].position, transform.position);
        float dotProduct = Vector3.Dot(transform.forward, moveDirection);
        Debug.Log("[Dot-Product]: " + dotProduct);

        //accel or reverse based on wapoint location
        if(dotProduct > 0f){
            NPC_Vert = 1f;
        }
        else {
            NPC_Vert = -1f;
        }


        //if next waypoint isnt directly in front, start turning.
        float angleToDirection =  Vector3.SignedAngle(transform.forward, moveDirection, Vector3.up);
        if(dotProduct <= .995f){
            if(angleToDirection > 0f){
                NPC_Horiz = 1f;
            }
            else {
                NPC_Horiz = -1f;
            }
        }
        else {
            NPC_Horiz = 0f;
        }

        // Debug.Log(gameObject + " - Magnitude: " + rb.velocity.magnitude);
        // Debug.Log("NPC Move Input: " + NPC_Vert + " NPC Steering Input: " + NPC_Horiz);
        Debug.Log("Next Waypoint #" + currentWaypointIndex + " Is at: [" + waypoints[currentWaypointIndex].position + "]");
        Debug.Log("[Distance to Waypoint]: " + Vector3.Distance(waypoints[currentWaypointIndex].position, transform.position));

        //sends verical and horizontal movement information to car controller.
        carController.setInput(NPC_Vert, NPC_Horiz);


        // Debug.Log("Remaining distance to waypoint: " + agent.remainingDistance);
        if (agent.remainingDistance < agent.stoppingDistance || distanceFromWaypoint <= 20f)
        {
            Debug.Log("Now Setting to waypoint #" + currentWaypointIndex);
            SetNextWaypoint();
        }
    }
}
