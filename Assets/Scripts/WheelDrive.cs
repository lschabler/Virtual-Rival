using UnityEngine;
using System;
using System.Collections.Generic;
using UnityEngine.UI;
using System.IO;

using System.Collections;

[Serializable]
public enum DriveType
{
    RearWheelDrive,
    FrontWheelDrive,
    AllWheelDrive
}

public struct GhostEvent
{
    public Vector3 position;
    public Quaternion rotation;
    public Vector3 velocity;

    public float angle;
    public float torque;
    public float laptime;

    public GhostEvent(Vector3 pos, Quaternion rot, float time)
    {
        position = pos;
        rotation = rot;
        laptime = time;
        angle = 0.0f;
        torque = 0.0f;
        velocity = Vector3.zero;
    }
};

public class WheelDrive : MonoBehaviour
{
    private float rpm;

    public int bhp;
    public float torque;

    public float[] gearRatio;
    public int currentGear;

    public float currentSpeed;
    public int maxSpeed;
    public int maxRevSpeed;

    public float SteerAngle;

    public float engineRPM;
    public float gearUpRPM;
    public float gearDownRPM;

    private bool gear_lock = true;

    public Text speedText;

    public Text currentGearText;

    public List<Image> speedStrip;

    [Tooltip("Maximum steering angle of the wheels")]
	public float maxAngle = 30f;
	[Tooltip("Maximum torque applied to the driving wheels")]
	public float maxTorque = 300f;
	[Tooltip("Maximum brake torque applied to the driving wheels")]
	public float brakeTorque = 30000f;
	[Tooltip("If you need the visual wheels to be attached automatically, drag the wheel shape here.")]
	public GameObject wheelShape;

	[Tooltip("The vehicle's speed when the physics engine can use different amount of sub-steps (in m/s).")]
	public float criticalSpeed = 5f;
	[Tooltip("Simulation sub-steps when the speed is above critical.")]
	public int stepsBelow = 5;
	[Tooltip("Simulation sub-steps when the speed is below critical.")]
	public int stepsAbove = 1;

	[Tooltip("The vehicle's drive type: rear-wheels drive, front-wheels drive or all-wheels drive.")]
	public DriveType driveType;

    private WheelCollider[] m_Wheels;

    public static List<GhostEvent> replayCarEventStream = new List<GhostEvent>();
    public static List<GhostEvent> replayGhostEventStream = new List<GhostEvent>();

    Rigidbody rigid;

    private SoundScript soundSript;

    private WheelCollider tractionWheel;

    private Vector3 initialPosition;
    private Quaternion initialRotation;

    private Rigidbody rigidbody;

    // Find all the WheelColliders down in the hierarchy.

    private void Awake()
    {
        initialPosition = transform.position;
        initialRotation = transform.rotation;
    }

    void Start()
	{
        soundSript = GetComponent<SoundScript>();

        rigid = GetComponent<Rigidbody>();

        m_Wheels = GetComponentsInChildren<WheelCollider>();

        rigidbody = GetComponent<Rigidbody>();

		for (int i = 0; i < m_Wheels.Length; ++i) 
		{
			var wheel = m_Wheels [i];

			// Create wheel shapes only when needed.
			if (wheelShape != null)
			{
				var ws = Instantiate (wheelShape);
				ws.transform.parent = wheel.transform;
			}
		}

        rpm = 0;
        foreach (WheelCollider wheel in m_Wheels)
        {
            if (wheel.transform.localPosition.z < 0)
            {
                tractionWheel = wheel;
                break;
            }
        }
    }

	// This is a really simple approach to updating wheels.
	// We simulate a rear wheel drive car and assume that the car is perfectly symmetric at local zero.
	// This helps us to figure our which wheels are front ones and which are rear.
	void Update()
	{
        // Key Handle
        float input_horizontal = Input.GetAxis("Horizontal");
        float input_vertical = Input.GetAxis("Vertical");
        bool input_reset = Input.GetKey(KeyCode.R);


        m_Wheels[0].ConfigureVehicleSubsteps(criticalSpeed, stepsBelow, stepsAbove);


        float angle = maxAngle * input_horizontal;
		//float torque = maxTorque * Input.GetAxis("Vertical");

		//float handBrake = Input.GetKey(KeyCode.X) ? brakeTorque : 0;

        rpm = tractionWheel.rpm;

        AutoGears();
        ResetCarToCheckPoint(input_reset);
        Accelerate(input_vertical);

        currentSpeed = rigidbody.velocity.magnitude * 3.6f;
        
        engineRPM = Mathf.Round((rpm * gearRatio[currentGear]));
        torque = bhp * gearRatio[currentGear];

        soundSript.SetSoundRPM(rpm);

        foreach (WheelCollider wheel in m_Wheels)
		{
			// A simple car where front wheels steer while rear ones drive.
			if (wheel.transform.localPosition.z > 0)
				wheel.steerAngle = angle;

			// Update visual wheels if any.
			if (wheelShape) 
			{
				Quaternion q;
				Vector3 p;
				wheel.GetWorldPose (out p, out q);

				// Assume that the only child of the wheelcollider is the wheel shape.
				Transform shapeTransform = wheel.transform.GetChild (0);
				shapeTransform.position = p;
				shapeTransform.rotation = q;
			}
		}


    }

    void Accelerate(float input_vertical)
    {
        float motorTorque = torque * input_vertical;

        foreach (WheelCollider wheel in m_Wheels)
        {
            if (currentSpeed < maxSpeed && currentSpeed > maxRevSpeed && engineRPM <= gearUpRPM)
            {
                if (wheel.transform.localPosition.z < 0 && driveType != DriveType.FrontWheelDrive)
                {
                    wheel.motorTorque = motorTorque;
                    wheel.brakeTorque = 0;
                }

                if (wheel.transform.localPosition.z >= 0 && driveType != DriveType.RearWheelDrive)
                {
                    wheel.motorTorque = motorTorque;
                    wheel.brakeTorque = 0;
                }
            }
            else
            {
                //if (wheel.transform.localPosition.z < 0)
                //{
                wheel.motorTorque = 0;
                wheel.brakeTorque = brakeTorque;
                //}
                
            }

            if (engineRPM > 0 && input_vertical < 0 && engineRPM <= gearUpRPM)
            {
                if(wheel.transform.localPosition.z >= 0)
                    wheel.brakeTorque = brakeTorque;
            }
            else
            {
                if (wheel.transform.localPosition.z >= 0)
                    wheel.brakeTorque = 0;
            }
        }
    }

    void AutoGears()
    {

        int appropriateGear = currentGear;

        if (engineRPM >= gearUpRPM)
        {

            for (var i = 0; i < gearRatio.Length; i++)
            {
                if (rpm * gearRatio[i] < gearUpRPM)
                {
                    appropriateGear = i;
                    break;
                }
            }
        }
        if (engineRPM <= gearDownRPM)
        {
            appropriateGear = currentGear;
            for (var j = gearRatio.Length - 1; j >= 0; j--)
            {
                if (rpm * gearRatio[j] > gearDownRPM-10)
                {
                    appropriateGear = j;
                    break;
                }
            }
        }
        if(currentGear != appropriateGear && gear_lock)
        {
            currentGear = appropriateGear;

            gear_lock = false;
            StartCoroutine(Wait(1));
        }
        
    }

    private IEnumerator Wait(float time)
    {
        yield return new WaitForSeconds(time);
        gear_lock = true;
    }

    void ResetCarToCheckPoint(bool input_reset)
    {
        if (input_reset)
        {
            Debug.Log("Reset Lap");
            currentSpeed = 0.0f;
            engineRPM = 0.0f;
            torque = 0.0f;
            rpm = 0.0f;
            currentGear = 0;
            tractionWheel.brakeTorque = Mathf.Infinity;

            TestRunController.current_drive.AddResetpoints(LapTimeController.GetCurrentTime(), transform.position, initialPosition);

            transform.rotation = initialRotation;
            transform.position = initialPosition;

            GetComponent<Rigidbody>().velocity = Vector3.zero;
            GetComponent<Rigidbody>().angularVelocity = Vector3.zero;

            // Insert resettime to current drive
            foreach (WheelCollider wheel in m_Wheels)
            {
                wheel.brakeTorque = Mathf.Infinity;
            }
        }
    }

    private void FixedUpdate()
    {
        Vector3 position = rigid.position;
        Quaternion rotation = rigid.rotation;
        Vector3 velocity = rigid.velocity;

        //AutoGears();

        GhostEvent ghost = new GhostEvent(rigid.position, rigid.rotation, LapTimeController.GetCurrentTime());

        replayCarEventStream.Add(ghost);

        TimeStep step = new TimeStep();
        step.SetPosition(rigid.position);
        step.SetRotation(rigid.rotation);
        step.speed = currentSpeed;
        TestRunController.AddCurrentTimeStep(step);


        speedText.text = string.Format("{0:0}", rigid.velocity.magnitude * 3.6);

        currentGearText.text = string.Format("{0}",currentGear+1);

        SetEngineSpeedStrip();

        //setSpeedStrip(rigid.velocity.magnitude * 3.6);
    }

    public void SetSpeedStrip(double speed)
    {
        if (speed < 1)
            return;

        int nr = Convert.ToUInt16(Mathf.Lerp(0, speedStrip.Count, (float)(speed/130)));

        for (int i = 0; i < speedStrip.Count; i++)
        {
            if (i < nr)
                speedStrip[i].GetComponent<Image>().color = new Color(255, 0, 0);
            else
                speedStrip[i].GetComponent<Image>().color = new Color(255, 255, 255);
        }
            
    }

    public void SetEngineSpeedStrip()
    {
        int nr = Convert.ToUInt16(Mathf.Lerp(0, speedStrip.Count, (float)(engineRPM / gearUpRPM)));

        for (int i = 0; i < speedStrip.Count; i++)
        {
            if (i < nr)
                speedStrip[i].GetComponent<Image>().color = new Color(255, 0, 0);
            else
                speedStrip[i].GetComponent<Image>().color = new Color(255, 255, 255);
        }

    }

    public void SetCheckpoint(Transform transform)
    {
        initialPosition = transform.position;
        initialRotation = transform.rotation;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Rail")
        {
            TestRunController.current_drive.AddRailContact(LapTimeController.GetCurrentTime(), transform.position);
        }
    }
}
