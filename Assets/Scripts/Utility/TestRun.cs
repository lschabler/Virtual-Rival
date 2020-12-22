using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestRun {


    private List<Lap> laps = new List<Lap>();
    public string modus;
    public int currentmmr;
    private int round = 0;

    public void SetRound(Lap new_lap)
    {
        round++;

        List<Lap> l = laps.FindAll(x => x.scene_type == new_lap.scene_type);

        float min_time = float.MaxValue; 

        foreach (Lap lap in l)
        {
            if (lap.laptime < min_time)
            {
                min_time = lap.laptime;
            }
        }

        if(min_time > new_lap.laptime)
        {
            laps.Add(new_lap);
        }

    }

    public int GetCurrentRound()
    {
        Debug.Log("Round: " + round + laps.Count);
        return round;
    }

    public List<Lap> GetLaps()
    {
        return laps;
    }
}
