using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour {

	[SerializeField] private Transform trans;
	[SerializeField] private Rigidbody rb;
	[SerializeField] private Transform target;
	[SerializeField] private Transform target1;
	[SerializeField] private Transform target2;
	[SerializeField] private Transform target3;
	[SerializeField] private Transform target4;
	[SerializeField] private Animator Animator;
	
	
	private float maxSpeed;
	private float radiusOfSat;
	private float detectionRadius;

	void Start () {
		maxSpeed = 3f;
		radiusOfSat = 2f;
		detectionRadius = 35f;
	}
	
	// Update is called once per frame
	void Update () {

		// Calculate vector from character to target
		Vector3 towards = target.position - trans.position;
		trans.rotation = Quaternion.LookRotation (towards);

		// If we haven't reached the target yet
		if (towards.magnitude > detectionRadius == false){
		if (towards.magnitude > radiusOfSat) {

			// Normalize vector to get just the direction
			towards.Normalize ();
			towards *= maxSpeed;

			// Move character
			rb.velocity = towards;
		} else {
			
			
			Animator.enabled = false;
			this.enabled = false;
		}
		}
		
		// Calculate vector from character to target
		Vector3 towards1 = target1.position - trans.position;
		trans.rotation = Quaternion.LookRotation (towards);

		// If we haven't reached the target yet
		//Animator.SetBool("isMoving", false);
		if (towards1.magnitude > detectionRadius == false){
		if (towards1.magnitude > radiusOfSat) {

			// Normalize vector to get just the direction
			towards1.Normalize ();
			//towards1 *= maxSpeed;
			
			// Move character
			//Animator.SetBool("isMoving", true);
			rb.velocity = towards1;
		} else {
			
			
			Animator.enabled = false;
			this.enabled = false;
			
		}
		}




				
		// Calculate vector from character to target
		Vector3 towards2 = target2.position - trans.position;
		trans.rotation = Quaternion.LookRotation (towards);

		// If we haven't reached the target yet
		//Animator.SetBool("isMoving", false);
		if (towards2.magnitude > detectionRadius == false){
		if (towards2.magnitude > radiusOfSat) {

			// Normalize vector to get just the direction
			towards1.Normalize ();
			//towards1 *= maxSpeed;
			
			// Move character
			//Animator.SetBool("isMoving", true);
			rb.velocity = towards2;
		} else {
			
			Animator.enabled = false;
			this.enabled = false;
			
		}
		}




				
		// Calculate vector from character to target
		Vector3 towards3 = target3.position - trans.position;
		trans.rotation = Quaternion.LookRotation (towards);

		// If we haven't reached the target yet
		//Animator.SetBool("isMoving", false);
		if (towards3.magnitude > detectionRadius == false){
		if (towards3.magnitude > radiusOfSat) {

			// Normalize vector to get just the direction
			towards3.Normalize ();
			//towards1 *= maxSpeed;
			
			// Move character
			//Animator.SetBool("isMoving", true);
			rb.velocity = towards3;
		} else {
			
			Animator.enabled = false;
			this.enabled = false;
			
		}
		}




				
		// Calculate vector from character to target
		Vector3 towards4 = target4.position - trans.position;
		trans.rotation = Quaternion.LookRotation (towards);

		// If we haven't reached the target yet
		//Animator.SetBool("isMoving", false);
		if (towards4.magnitude > detectionRadius == false){
		if (towards4.magnitude > radiusOfSat) {

			// Normalize vector to get just the direction
			towards4.Normalize ();
			//towards1 *= maxSpeed;
			
			// Move character
			//Animator.SetBool("isMoving", true);
			rb.velocity = towards4;
		} else {
			
			Animator.enabled = false;
			this.enabled = false;
			
		}
		}




		
	}
}
