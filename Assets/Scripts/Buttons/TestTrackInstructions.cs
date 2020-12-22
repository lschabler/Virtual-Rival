using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TestTrackInstructions : MonoBehaviour {

    [SerializeField]
    private GameObject button;

    [SerializeField]
    private GameObject loading_circle;

    private bool loading = true;

    // Use this for initialization
    void Start () {
        StartCoroutine(Post(GenerateUniqueID()));
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    private void FixedUpdate()
    {
        if (!loading)
        {
            button.SetActive(true);
            loading_circle.SetActive(false);

            loading = true;
        }
    }

    public void Accept()
    {
        SceneManager.LoadScene("TestTrackLap");
    }

    public string GenerateUniqueID()
    {
        string rng = Random.Range(10, 4000000).ToString();
        string date = System.DateTime.Now.ToString();
        System.Security.Cryptography.SHA256 shaAlgorithm = new System.Security.Cryptography.SHA256Managed();
        byte[] shaDigest = shaAlgorithm.ComputeHash(System.Text.ASCIIEncoding.ASCII.GetBytes(rng + date));
        Debug.Log(rng + date);
        Debug.Log(System.BitConverter.ToString(shaDigest));
        return System.BitConverter.ToString(shaDigest);
    }

    IEnumerator Post(string id)
    {
        // inspect + search "form action"
        string Base_URL = "https://docs.google.com/forms/d/e/1FAIpQLSdBWf2SbLLKBJbdwo6t4abYXk2-LMGt0ZdKXroAD4dJ8y5QTA/formResponse";
        WWWForm form = new WWWForm();
        form.AddField("entry.1210707150", id);

        WWW www = new WWW(Base_URL, form.data);
        yield return www;

        loading = false;
    }
}
