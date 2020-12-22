using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GhostCarScript : MonoBehaviour {

    private static int ghostStreamIndex = 0;
    private static int ghostRound = 0;

    private static Lap ghost_lap;
    private static bool ghost_enabled = false;

    [SerializeField]
    public GameObject wheelShape;

    private WheelCollider[] m_Wheels;

    // Use this for initialization
    void Start () {

        m_Wheels = transform.parent.GetComponentsInChildren<WheelCollider>();
        for (int i = 0; i < m_Wheels.Length; ++i)
        {
            var wheel = m_Wheels[i];

            // Create wheel shapes only when needed.
            if (wheelShape != null)
            {
                var ws = Instantiate(wheelShape);
                ws.transform.parent = wheel.transform;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (wheelShape)
        {
            foreach (WheelCollider wheel in m_Wheels)
            {
                Quaternion q;
                Vector3 p;
                wheel.GetWorldPose(out p, out q);

                // Assume that the only child of the wheelcollider is the wheel shape.
                Transform shapeTransform = wheel.transform.GetChild(0);
                shapeTransform.position = p;
                shapeTransform.rotation = q;
            }
        }
    }
    private void FixedUpdate()
    {
        if (ghost_lap == null)
            return;

        if (ghost_lap.timestep.Count <= ghostStreamIndex)
            ghost_enabled = false;    

        if (ghost_enabled)
        {
            GetComponent<MeshRenderer>().enabled = true;
        }
        else
        {
            // Todo: disable wheels 
           // GetComponent<MeshRenderer>().enabled = false;
            //Destroy(transform.parent.gameObject);
            
            return;
        }

        TimeStep current = ghost_lap.timestep[ghostStreamIndex];

        transform.parent.position = current.GetPosition();
        transform.parent.rotation = current.GetRotation();

        ghostStreamIndex++;

    }

    public static void StartGhost(Lap ghost)
    {
        ghost_lap = ghost;
        ghostRound++;
        ghostStreamIndex = 0;
        ghost_enabled = true;
    }
}
