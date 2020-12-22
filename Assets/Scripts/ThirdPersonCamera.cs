using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour {

    private Transform follow;
    private Vector3 targetPosition;

    public float distanceAway;
    public float distanceUp;
    public float smooth;

	// Use this for initialization
	void Start () {
        follow = GameObject.FindGameObjectWithTag("Player").transform;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void LateUpdate()
    {
        Debug.Log("ThirdPersonCamerea");

        targetPosition = follow.position + follow.up * distanceUp + follow.forward * distanceAway;

        transform.position = targetPosition; // Vector3.Lerp(transform.position, targetPosition, smooth * Time.deltaTime);

        transform.LookAt(follow);
    }
}
