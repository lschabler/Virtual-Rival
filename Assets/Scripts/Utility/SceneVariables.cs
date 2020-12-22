using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ScenarioNr
{
    START,
    INITIALQUESTIONNAIRE,
    BIGFIVE,
    AGE,
    SENSATIONSEEKING,
    INSTRUCTIONS,
    EVALUATIONTRACK,
    LOADTRACK,
    RACETRACK1,
    RACETRACK2,
    WHEELOFEMOTIONS,
    Questionnaire,
    END,
}

public struct CheckPointActivation
{
    public int nr;
    public bool activated;
    public float time;
    public Vector3 position;
    public Quaternion rotation;

    public void SetTransformation(Transform trans)
    {
        position = trans.position;
        rotation = trans.rotation;
    }


    public CheckPointActivation(int nr_)
    {
        nr = nr_;
        activated = false;
        time = 0.0f;
        position = Vector3.zero;
        rotation = Quaternion.identity;
    }

    public CheckPointActivation(int nr_, bool activated_, float time_)
    {
        nr = nr_;
        activated = activated_;
        time = time_;
        position = Vector3.zero;
        rotation = Quaternion.identity;
    }
};

public enum RaceType
{
    GHOST,
    TIME
}

public enum ScenarioType
{
    TRAINING,
    TRACK1,
    TRACK2,
}

public enum Game_Score
{
    WON,
    DRAW,
    LOSS,
}


