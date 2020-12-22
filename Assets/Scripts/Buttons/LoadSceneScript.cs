using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadSceneScript : MonoBehaviour {

    private List<Lap> laps = new List<Lap>();

    private Lap lap = null;

    private bool loading = true;

    [SerializeField]
    Text text;

    [SerializeField]
    Text text2;

    [SerializeField]
    GameObject button;

    [SerializeField]
    GameObject loading_circle;

    IEnumerator LoadAllScenes()
    {
        int nr = 0;
        while(true)
        {
            string filePath = System.IO.Path.Combine(Application.streamingAssetsPath, "save_" + nr.ToString() + ".dat");

            text2.text = filePath;

            byte[] FileBytes = null;

            if (filePath.Contains("://"))
            {
                WWW www = new WWW(filePath);
                
                yield return www;

                if (!string.IsNullOrEmpty(www.error))
                {
                    loading = false;
                    break;
                }
                FileBytes = www.bytes;
                text2.text = "count=" + FileBytes.Length.ToString();
            }
            else
            {
                try
                {
                    FileBytes = File.ReadAllBytes(filePath);
                }
                catch (System.Exception)
                {
                    loading = false;
                    break;
                }


            }

            BinaryFormatter bf = new BinaryFormatter();
            MemoryStream MS = new MemoryStream(FileBytes);
            Lap data = (Lap)bf.Deserialize(MS);
            text2.text = "lap=" + data.laptime;

            nr++;
            laps.Add(data);
        }

       
    }

    IEnumerator LoadScene(int file_index)
    {
        string filePath = System.IO.Path.Combine(Application.streamingAssetsPath, "save_" + file_index.ToString() + ".dat");

        text2.text = filePath;

        byte[] FileBytes = null;

        if (filePath.Contains("://"))
        {
            WWW www = new WWW(filePath);

            yield return www;

            if (!string.IsNullOrEmpty(www.error))
            {
                text.text = "Loading Index File Failed";
            }
            FileBytes = www.bytes;
            text2.text = "count=" + FileBytes.Length.ToString();
        }
        else
        {
            FileBytes = File.ReadAllBytes(filePath);
        }

        BinaryFormatter bf = new BinaryFormatter();
        MemoryStream MS = new MemoryStream(FileBytes);
        lap = (Lap)bf.Deserialize(MS);

        loading = false;
    }

    private void FixedUpdate()
    {
        if(!loading)
        {
            if (lap != null)
            {
                TestRunController.AddNewGhostLap(lap);
                text.text = "opponent mmr = " + lap.mmr.ToString() + " current mmr = " + TestRunController.mmr.ToString();
            }
            else
            {
                TestRunController.ResetGhostLap();
                text.text = "(no ghost car active)";
            }

            button.gameObject.SetActive(true);
            loading = true;
            loading_circle.SetActive(false);
        }
    }

    private void Awake()
    {
        int file_index = TestRunController.GetClosedGhost();
        if(file_index>=0)
        {
            StartCoroutine(LoadScene(file_index));
        }
        else
        {
            // Todo: load last round as ghost if available
            lap = TestRunController.GetBestLap();
            loading = false;

        }
        
        //StartCoroutine(LoadAllScenes());
    }
	

    public void StartRun()
    {
        TestRunController.TriggerNextScene();
        //SceneManager.LoadScene(ScenarioNr.RACETRACK2.ToString());
    }
}
