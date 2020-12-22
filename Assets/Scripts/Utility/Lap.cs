using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct Resetpoint
{
    public Resetpoint(float resettime, Vector3 resetpoint, Vector3 resetcheckpoint)
    {
        time = resettime;
        point = new Vector3Serializable(resetpoint);
        checkpoint = new Vector3Serializable(resetcheckpoint);
    }

    override public string ToString() 
    {
        string ret = "";

        ret += time.ToString("N1") + " ";
        ret += point.ToString() + " ";
        ret += checkpoint.ToString();

        return ret;
    }

    float time;
    Vector3Serializable point;
    Vector3Serializable checkpoint;
}

[System.Serializable]
public struct RailContactPoint
{
    public RailContactPoint(float contacttime, Vector3 contactpoint)
    {
        time = contacttime;
        point = new Vector3Serializable(contactpoint);
    }

    override public string ToString()
    {
        string ret = "";

        ret += time.ToString("N1") + " ";
        ret += point.ToString();

        return ret;
    }

    float time;
    Vector3Serializable point;
}

[System.Serializable]
public class Lap {

    public List<TimeStep> timestep = new List<TimeStep>();
    public List<CheckPoint> checkpoint = new List<CheckPoint>();
    public List<Resetpoint> resetpoints = new List<Resetpoint>();
    public List<RailContactPoint> contactpoints = new List<RailContactPoint>();

    public float laptime;
    public ScenarioType scene_type;
    public RaceType race_type;
    public int mmr;
    public int round;
    public string myid;
    public string opponent_id;
    public int opponent_round;

    public int max_fps;
    public float avg_fps;

    public Lap(ScenarioType scene, RaceType race, int curent_mmr)
    {
        scene_type = scene;
        race_type = race;
        mmr = curent_mmr;
    }

    public void AddCheckpoints(Dictionary<int, CheckPointActivation> checkpoints)
    {

        foreach (var i in checkpoints.Values)
        {

            CheckPoint point = new CheckPoint();
            point.nr = i.nr;
            point.time = i.time;
            checkpoint.Add(point);
        }

    }

    public string PositionToString()
    {
        string ret = "";

        for (int i = 0; i < timestep.Count; i++)
        {
            if(i%10 == 0)
            {
                ret += timestep[i].PositionToString();
            }   
        }
        return ret;
    }

    public string TimeToString()
    {
        string ret = "";

        for (int i = 0; i < timestep.Count; i++)
        {
            if (i % 10 == 0)
            {
                ret += timestep[i].time.ToString("N1") + " ";
            }
        }
        return ret;
    }
    public string SpeedToString()
    {
        string ret = "";

        for (int i = 0; i < timestep.Count; i++)
        {
            if (i % 10 == 0)
            {
                ret += timestep[i].speed.ToString("N1") + " ";
            }
        }
        return ret;
    }

    public void AddResetpoints(float time, Vector3 position, Vector3 checkpoint)
    {
        resetpoints.Add(new Resetpoint(time,position,checkpoint));
    }

    public void AddRailContact(float time, Vector3 position)
    {
        contactpoints.Add(new RailContactPoint(time, position));
    }

    public string ResetpointsToString()
    {
        string ret = "";

        for (int i = 0; i < resetpoints.Count; i++)
        {
            ret += resetpoints[i].ToString() + " ";
        }
        return ret;
    }

    public string ContactpointsToString()
    {
        string ret = "";

        for (int i = 0; i < contactpoints.Count; i++)
        {
            ret += contactpoints[i].ToString() + " ";
        }
        return ret;
    }
}
