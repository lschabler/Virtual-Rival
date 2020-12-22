using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SceneIndicies
{
    public List<SceneIndex> indicies = new List<SceneIndex>();
}

[System.Serializable]
public class SceneIndex {
    public int file_index;
    public int mmr;
    public float laptime;
    public ScenarioType scene_type;

    public SceneIndex(int index, int lap_mmr, float time, ScenarioType scene)
    {
        file_index = index;
        mmr = lap_mmr;
        laptime = time;
        scene_type = scene;
    }
}
