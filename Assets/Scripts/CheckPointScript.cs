using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPointScript : MonoBehaviour {

    [SerializeField]
    private int CheckPointNumber = 0;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter(Collider other)
    {
        if(other.transform.parent != null && other.transform.parent.CompareTag("Player"))
        {
            LapTimeController.ActivateCheckPoint(CheckPointNumber, transform);
        }
            
    }
}
