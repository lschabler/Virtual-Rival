using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public class EndScript : MonoBehaviour {

    string sub_id;

	// Use this for initialization
	void Start () {
        sub_id = "C2-1D-80-9B-61-33-B7-9B-6B-93-12-52-62-E1-74-75-C8-92-39-3C-F5-5D-39-DE-4C-08-7B-8B-1E-83-BB-2D".Substring(84);
        //sub_id = TestRunController.id.Substring(72);

    }

    // Update is called once per frame
    void Update()
    {
        //string sub = "C2-1D-80-9B-61-33-B7-9B-6B-93-12-52-62-E1-74-75-C8-92-39-3C-F5-5D-39-DE-4C-08-7B-8B-1E-83-BB-2D".Substring(72);
        //string sub = TestRunController.id.Substring(72);
        //GetComponent<InputField>().text = TestRunController.id.Substring(3);
        GetComponent<InputField>().text = sub_id;
    }

    public void CopyToClipboard()
    {
        TextEditor te = new TextEditor();
        //te.content = new GUIContent(sub_id);
        te.text = sub_id;
        te.SelectAll();
        te.Copy();

        
    }
}
