using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartScript : MonoBehaviour {

    private SceneIndicies index_file = null;

    private bool loading = true;

    [SerializeField]
    Text text;

    [SerializeField]
    Text text2;

    [SerializeField]
    GameObject button;

    IEnumerator LoadIndexFile()
    {
            string filePath = System.IO.Path.Combine(Application.streamingAssetsPath, "index.dat");
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
            index_file = (SceneIndicies)bf.Deserialize(MS);

            loading = false;
        }

    public string GenerateUniqueID()
    {
        string rng = Random.Range(10, 4000000).ToString();
        string date = System.DateTime.Now.ToString();
        System.Security.Cryptography.SHA256 shaAlgorithm = new System.Security.Cryptography.SHA256Managed();
        byte[] shaDigest = shaAlgorithm.ComputeHash(System.Text.ASCIIEncoding.ASCII.GetBytes(rng+date));
        Debug.Log(rng+date);
        Debug.Log(System.BitConverter.ToString(shaDigest));
        return System.BitConverter.ToString(shaDigest);
    }
    

    private void FixedUpdate()
    {
        if(!loading)
        {
            button.gameObject.SetActive(true);
            float r = Random.Range(0.0f, 1.0f);
            Debug.Log(r);
            if (r >= 0.4)
            {
                Debug.Log("GHOST");
                text2.text = "GHOST = " + r;
                TestRunController.InitTestRunController(index_file, RaceType.GHOST);
            }
            else
            {
                Debug.Log("TIME");
                text2.text = "TIME = " + r;
                TestRunController.InitTestRunController(index_file, RaceType.TIME);
            }
            //TestRunController.InitTestRunController(index_file, RaceType.GHOST);
            
            loading = true;
        }
    }

    private void Awake()
    {
        Cursor.SetCursor(null, Vector2.zero, CursorMode.ForceSoftware);

        TestRunController.id = GenerateUniqueID();
        StartCoroutine(LoadIndexFile());
    }

    // Use this for initialization
    void Start ()
    {
        Application.targetFrameRate = -1;
    }

    public void StartRun()
    {
        TestRunController.TriggerNextScene();
    }
}
