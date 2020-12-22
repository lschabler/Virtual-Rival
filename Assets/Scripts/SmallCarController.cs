using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SmallCarCargoInfo
{
    public WheelCollider leftWheel;
    public WheelCollider rightWheel;

    public GameObject leftWheelMesh;
    public GameObject rightWheelMesh;

    public bool motor;
    public bool steering;
}


public class SmallCarController : MonoBehaviour {

    public List<SmallCarCargoInfo> cargoInfo;

    public List<int> gearRatio;

    public float maxMotorTorque;
    public float maxSteeringAngle;

    private float speed;

    private int gear;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void FixedUpdate()
    {
        //speed = transform.gameObject.GetComponent<Rigidbody>().velocity.magnitude * 3.6f;
        speed = transform.gameObject.GetComponent<Rigidbody>().velocity.magnitude * 2.23693629f;


        float motor = maxMotorTorque * Input.GetAxis("Vertical");
        float steering = maxSteeringAngle * Input.GetAxis("Horizontal");

        foreach(var info in cargoInfo)
        {
            if(info.steering)
            {
                info.leftWheel.steerAngle = steering;
                info.rightWheel.steerAngle = steering;
            }

            if (info.motor)
            {
                info.leftWheel.motorTorque = motor;
                info.rightWheel.motorTorque = motor;
            }

            ApplyLocalPositionToVisuals(info);
        }
        EngineSound();
    }

    public void ApplyLocalPositionToVisuals(SmallCarCargoInfo wheelPair)
    {
        //wheelPair.leftWheelMesh.transform.Rotate(Vector3.right, Time.deltaTime * wheelPair.leftWheel.rpm * 10, Space.World);
        //wheelPair.rightWheelMesh.transform.Rotate(Vector3.right, Time.deltaTime * wheelPair.rightWheel.rpm * 10, Space.World);

        //wheelPair.leftWheelMesh.transform.RotateAround(wheelPair.leftWheel.transform.position, Time.deltaTime * wheelPair.rightWheel.rpm * 10);
        //Transform visualWheel = wheelPair.leftWheelMesh.transform;

        //Vector3 position;
        //Quaternion rotation;
        //wheelPair.leftWheel.GetWorldPose(out position, out rotation);

        //visualWheel.transform.position = position;
        //visualWheel.transform.rotation = rotation;

    }

    private void EngineSound()
    {
        int next_gear;
        for (next_gear = 0; next_gear < gearRatio.Count; next_gear++)
        {
            if (gearRatio[next_gear] > speed)
            {
                //break this value
                break;
            }
        }

        float minGearValue = 0.00f;
        float maxGearValue = 0.00f;
        if (next_gear == 0)
        {
            minGearValue = 0;
        }
        else
        {
            minGearValue = gearRatio[next_gear - 1];
        }
        maxGearValue = gearRatio[next_gear];

        float pitch = ((speed - minGearValue) / (maxGearValue - minGearValue) + 0.3f * (gear + 1));

        gameObject.GetComponent<AudioSource>().pitch = pitch;

        gear = next_gear;
    }
}
