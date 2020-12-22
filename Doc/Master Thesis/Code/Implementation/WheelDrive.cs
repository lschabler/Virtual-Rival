using UnityEngine;
using System;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Collections;

[Serializable]
public enum DriveType
{
    RearWheelDrive,
    FrontWheelDrive,
    AllWheelDrive
}

public class WheelDrive : MonoBehaviour
{
    private float rpm;
    private int bhp;
    private float torque;
    private float[] gearRatio;
    private float SteerAngle;
    private float engineRPM;

    void Start()
	{
        // Setup Components attached to the car (Sound, Rigidbody)
        SetupComponents();
        // Attach wheels to the chassis 
        InitialiseWheels();
    }

	void Update()
	{
        // Get user input
        float input_horizontal = Input.GetAxis("Horizontal");
        float input_vertical = Input.GetAxis("Vertical");
        bool input_reset = Input.GetKey(KeyCode.R);

        // Set lower accuracy at high speeds (performance optimization)
        m_Wheels.ConfigureVehicleSubsteps(criticalSpeed, stepsBelow, stepsAbove);

        // Change gears when necessary (automatic transmission)
        AutoGears();
        ResetCarToCheckPoint(input_reset);
        Accelerate(input_vertical);

        // Update engine params
        currentSpeed = rigidbody.velocity.magnitude * 3.6f;
        engineRPM = Mathf.Round((rpm * gearRatio[currentGear]));
        torque = bhp * gearRatio[currentGear];

        // Update engine sound
		soundSript.SetSoundRPM(rpm);

        // Update revolutions and steering angle of the wheels
        UpdateWheels();
    }

    // Detect Collisions
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Rail")
        {
            TestRunController.current_drive
			        .AddRailContact(LapTimeController.GetCurrentTime(), transform.position);
        }
    }
}
