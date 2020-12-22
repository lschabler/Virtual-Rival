using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LapTimeController : MonoBehaviour
{

    [SerializeField]
    Text LapTimeTextField;

    [SerializeField]
    Text LapTimeDifferenceTextField;

    [SerializeField]
    int NrOfCheckpoints;

    [SerializeField]
    public GameObject Ghost;

    static float checkpoint_time_difference;

    static bool enabled_checkpoint;

    static int nrcheckpoints;

    static float startTime;

    static bool firstlap = false;

    static float currentTime;

    static int nextCheckpoint;

    static Dictionary<int, CheckPointActivation> current_checkpoints = new Dictionary<int, CheckPointActivation>();
    static Dictionary<int, CheckPointActivation> ghost_checkpoints = new Dictionary<int, CheckPointActivation>();

    private void Awake()
    {
        nrcheckpoints = NrOfCheckpoints;
        LapTimeDifferenceTextField.GetComponentInParent<Transform>().parent.gameObject.SetActive(false);

        ResetController();
    }

    public static void ResetController()
    {
        Debug.Log("Reset Controller");

        enabled_checkpoint = false;
        firstlap = false;

        ResetCurrentCheckpoints();

        // reset ghost checkpoints
        //if (ghost_checkpoints.Count > 0)
        //    return;

        for (int i = 0; i < nrcheckpoints; i++)
        {
            CheckPointActivation newPoint = new CheckPointActivation(i);
            ghost_checkpoints[i] = newPoint;
        }
    }

    public static void SetGhost(Lap ghostlap)
    {
        ghost_checkpoints = new Dictionary<int, CheckPointActivation>();
        foreach (CheckPoint cp in ghostlap.checkpoint)
        {
            CheckPointActivation newPoint = new CheckPointActivation(cp.nr, true, cp.time);
            ghost_checkpoints[cp.nr] = newPoint;
        }
    }

    // Use this for initialization
    void Start()
    {
        startTime = Time.time;
        LapTimeTextField.text = ParseTimeString(0);

        GameObject event_manager = GameObject.FindGameObjectWithTag("EventSystem");
        event_manager.SendMessage("SetUpGhost");
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        currentTime = Time.time - startTime;
        if (firstlap)
            LapTimeTextField.text = ParseTimeString(currentTime);
        else
            LapTimeTextField.text = "0: 00.000";

        if (enabled_checkpoint)
        {
            enabled_checkpoint = false;

            StartCoroutine(Fade());
        }

    }

    public void SetUpGhost()
    {
        Debug.Log("SetUpGhost");
        Instantiate(Ghost);
    }

    public static void ResetLapTime()
    {
        if (!firstlap)
        {
            //WheelDrive.clearGhostHistory();
            firstlap = true;
            startTime = Time.time;
            TestRunController.StartFinishLine();
            return;
        }

        if (nextCheckpoint != nrcheckpoints)
            return;

        TestRunController.PassFinishLine(current_checkpoints);

        //startTime = Time.time;

        nextCheckpoint = 0;

        //ghost_checkpoints = new Dictionary<int, CheckPointActivation>(current_checkpoints);
        //resetCurrentCheckpoints();
        //WheelDrive.startNewGhost();
    }

    private string ParseTimeString(float time)
    {
        string minutes = ((int)(time / 60)).ToString();

        string seconds = "";
        if (time % 60 >= 10)
            seconds += ((int)time % 60).ToString();
        else
            seconds += "0" + ((int)time % 60).ToString();

        string milliseconds = ((int)((time % 1) * 1000)).ToString();

        return minutes + ": " + seconds + "." + milliseconds;
    }

    static public float GetCurrentTime()
    {
        return currentTime;
    }

    static public void ActivateCheckPoint(int nr, Transform position)
    {
        if (nextCheckpoint == nr)
        {
            CheckPointActivation point = current_checkpoints[nr];
            point.activated = true;
            point.time = currentTime;
            point.SetTransformation(position);
            current_checkpoints[nr] = point;

            GameObject.FindGameObjectWithTag("Player").GetComponent<WheelDrive>().SetCheckpoint(position);

            CheckPointActivation ghost_point = ghost_checkpoints[nr];
            if (ghost_point.activated)
            {
                float ghost_point_time = ghost_point.time;

                checkpoint_time_difference = ghost_point_time - point.time;

                enabled_checkpoint = true;
            }

            nextCheckpoint++;
        }
    }

    static private void ResetCurrentCheckpoints()
    {
        for (int i = 0; i < nrcheckpoints; i++)
        {
            CheckPointActivation newPoint = new CheckPointActivation(i);
            current_checkpoints[i] = newPoint;
        }
    }

    IEnumerator Fade()
    {
        LapTimeDifferenceTextField.GetComponent<Transform>().parent.transform.gameObject.SetActive(true);

        LapTimeDifferenceTextField.text = ParseTimeString(Mathf.Abs(checkpoint_time_difference));

        if (checkpoint_time_difference < 0)
            LapTimeDifferenceTextField.GetComponent<Transform>().parent.transform.gameObject.GetComponent<Image>().color = new Color(1, 0, 0);  //red
        else
            LapTimeDifferenceTextField.GetComponent<Transform>().parent.transform.gameObject.GetComponent<Image>().color = new Color(12.0f / 255f, 109.0f / 255, 10.0f / 255f); // green

        yield return new WaitForSeconds(5f);

        LapTimeDifferenceTextField.GetComponent<Transform>().parent.transform.gameObject.SetActive(false);

        LapTimeDifferenceTextField.text = "";
    }

    public void UpdateAverageFPSCount(float avg_fps)
    {
        TestRunController.current_drive.avg_fps = avg_fps;
    }

    public void UpdateMaximumFPSCount(int max_fps)
    {
        TestRunController.current_drive.max_fps = max_fps;
    }
}