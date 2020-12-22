using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ViewSceneScript : MonoBehaviour {

    FileController file = new FileController();

    List<Lap> laps = new List<Lap>();

    const int MIN_MMR = 500;
    const int MAX_MMR = 1500;

    const int MIN_TRAINING_MMR = 700;
    const int MAX_TRAINING_MMR = 1100;

    [SerializeField]
    Text text_filed;

    [SerializeField]
    Text text_training;
    // Use this for initialization

    float min_laptime_track1 = float.MaxValue;
    float max_laptime_track1 = float.MinValue;

    float min_laptime_track2 = float.MaxValue;
    float max_laptime_track2 = float.MinValue;

    float min_laptime_training = float.MaxValue;
    float max_laptime_training = float.MinValue;

    void Start () {
        laps = file.LoadAllFiles();

        SceneIndicies index = file.LoadIndexFile();

        foreach(var i in index.indicies)
        {
            Debug.Log("file"+i.file_index + " " + i.laptime + " " + i.mmr);
        }

        ViewList();
	}
	
    public void ViewList()
    {
        string text = "";
        string text_train = "";

        foreach (var lap in laps)
        {
            if (lap.scene_type == ScenarioType.TRAINING)
            {
                text_train += lap.laptime + ", " + lap.mmr + ", " + lap.scene_type.ToString() + "\n";
                if (lap.laptime < min_laptime_training)
                    min_laptime_training = lap.laptime;
                if (lap.laptime > max_laptime_training)
                    max_laptime_training = lap.laptime;
                text_training.text = text_train;
            }
            if (lap.scene_type == ScenarioType.TRACK1)
            {
                text += lap.laptime + ", " + lap.mmr + ", " + lap.scene_type.ToString() + "\n";
                if (lap.laptime < min_laptime_track1)
                    min_laptime_track1 = lap.laptime;
                if (lap.laptime > max_laptime_track1)
                    max_laptime_track1 = lap.laptime;
                text_filed.text = text;
            }
            if (lap.scene_type == ScenarioType.TRACK2)
            {
                text += lap.laptime + ", " + lap.mmr + ", " + lap.scene_type.ToString() + "\n";
                if (lap.laptime < min_laptime_track2)
                    min_laptime_track2 = lap.laptime;
                if (lap.laptime > max_laptime_track2)
                    max_laptime_track2 = lap.laptime;
                text_filed.text = text;
            }

        }
        
    }

	// Update is called once per frame
	void Update () {
		
	}

    public void Sort()
    {
        file.DeleteAllFiles();

        SceneIndicies index_file = new SceneIndicies();

        int i = 0;
        foreach (var lap in laps)
        {
            float t = 0;
            switch(lap.scene_type)
            {
                case ScenarioType.TRAINING:
                        t = 1f - ((lap.laptime - min_laptime_training) / (max_laptime_training - min_laptime_training));
                        lap.mmr = Mathf.RoundToInt(t * MAX_TRAINING_MMR) + MIN_TRAINING_MMR;
                    break;
                case ScenarioType.TRACK1:
                        t = 1f - ((lap.laptime - min_laptime_track1) / (max_laptime_track1 - min_laptime_track1));
                        lap.mmr = Mathf.RoundToInt(t * MAX_MMR) + MIN_MMR;
                    break;
                case ScenarioType.TRACK2:
                        t = 1f - ((lap.laptime - min_laptime_track2) / (max_laptime_track2 - min_laptime_track2));
                        lap.mmr = Mathf.RoundToInt(t * MAX_MMR) + MIN_MMR;
                    break;
                default:
                    break;
            }

            

            file.SaveFile(lap);

            index_file.indicies.Add(new SceneIndex(i, lap.mmr, lap.laptime, lap.scene_type));
            i++;

        }
        
        ViewList();
        file.SaveIndexFile(index_file);
    }
}
