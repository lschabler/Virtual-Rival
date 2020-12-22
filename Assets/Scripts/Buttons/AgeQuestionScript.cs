using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public struct GDocsAgeQuestionManagerEntry
{
    public GDocsAgeQuestionManagerEntry(string id_, int age_) 
    {
        id = id_;
        age = age_;
    }

    public string id;

    public int age;
};

public class AgeQuestionScript : MonoBehaviour {

    private int age = 0;

    private GameObject confirmButton;

    private GameObject[] radioButtons;

    // Use this for initialization
    void Start () {
        radioButtons = GameObject.FindGameObjectsWithTag("RadioButton");

        confirmButton = GameObject.FindGameObjectWithTag("ConfirmButton");

        UncheckAll();

        DisableConfirmButton();
    }

    public void Confirm()
    {
        UncheckAll();
        DisableConfirmButton();

        // Save to Gdocs
        GDocsAgeQuestionManagerEntry entry = new GDocsAgeQuestionManagerEntry(TestRunController.id, age);

        GameObject datamanager = GameObject.FindGameObjectWithTag("DataManagment");
        datamanager.SendMessage("UploadAgeQuestions", entry);
    }
    

    public void ClickButton1()
    {
        UncheckAll();
        age = 1;
        EnableConfirmButton();
    }

    public void ClickButton2()
    {
        UncheckAll();
        age = 2;
        EnableConfirmButton();
    }

    public void ClickButton3()
    {
        UncheckAll();
        age = 3;
        EnableConfirmButton();
    }

    public void ClickButton4()
    {
        UncheckAll();
        age = 4;
        EnableConfirmButton();
    }

    public void ClickButton5()
    {
        UncheckAll();
        age = 5;
        EnableConfirmButton();
    }

    public void QuestionManagerTriggerNextScene()
    {
        TestRunController.TriggerNextScene();
    }

    // Update is called once per frame
    void DisableConfirmButton()
    {
        confirmButton.SetActive(false);
    }

    void EnableConfirmButton()
    {
        confirmButton.SetActive(true);
    }

    void UncheckAll()
    {
        foreach (GameObject r in radioButtons)
        {
            r.GetComponentInChildren<Image>().gameObject.SetActive(false);
        }
    }
}
