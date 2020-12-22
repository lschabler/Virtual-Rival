using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

struct InitQuestion
{
    public InitQuestion(uint number_, string text_, string left, string right)
    {
        number = number_;
        text = text_;
        rating = -1;
        left_text = left;
        right_text = right;
    }

    public uint number;
    public string text;
    public int rating;

    public string left_text;
    public string right_text;

    public override string ToString()
    {
        return number + "," + text + "," + rating;
    }
};

public struct GDocsInitQuestionManagerEntry
{
    public string id;

    public string race_type;

    public int driving_skill;
    public int videogame_experience;

    public GDocsInitQuestionManagerEntry(string id_, RaceType type)
    {
        id = id_;
        driving_skill = -1;
        videogame_experience = -1;

        if (type == RaceType.GHOST)
            race_type = "GHOST";
        else
            race_type = "TIME";
                
    }
};

public class InitialQuestionManager : MonoBehaviour {

    private int activeQuestion = 0;

    private GameObject[] radioButtons;

    private Text questionText;

    private List<InitQuestion> allquestions = new List<InitQuestion>();

    private InitQuestion[] questions;

    private GameObject confirmButton;

    public Text left_text;
    public Text right_text;

    // Use this for initialization
    void Start () {
        radioButtons = GameObject.FindGameObjectsWithTag("RadioButton");

        questionText = GameObject.FindGameObjectWithTag("Question").GetComponent<Text>();

        confirmButton = GameObject.FindGameObjectWithTag("ConfirmButton");

        allquestions.Add(new InitQuestion(1, "Rate your car driving skill", "bad", "excellent"));
        allquestions.Add(new InitQuestion(2, "Experience with video games", "none", "a lot"));

        questions = (InitQuestion[])allquestions.ToArray();

        SetQuestion();

        UncheckAll();

        DisableConfirmButton();
    }

    void SetQuestion()
    {
        questionText.text = questions[activeQuestion].text;
        left_text.text = questions[activeQuestion].left_text;
        right_text.text = questions[activeQuestion].right_text;
    }
	
	// Update is called once per frame
	void Update () {
		
	}

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

    public void Confirm()
    {
        UncheckAll();
        activeQuestion++;
        DisableConfirmButton();
        if (activeQuestion < questions.Length)
        {
            SetQuestion();
        }
        else
        {
            GDocsInitQuestionManagerEntry entry = new GDocsInitQuestionManagerEntry(TestRunController.id, TestRunController.GetRaceType());
            // TODO: Save Questions
            foreach (var q in questions)
            {
                switch(q.number)
                {
                    case 1:
                        entry.driving_skill = q.rating;
                        break;
                    case 2:
                        entry.videogame_experience = q.rating;
                        break;
                    default:
                        Debug.Log("ERROR INITIAL QUESTION MANAGER: Undefined question number!");
                        break;
                }
            }

            GameObject datamanager = GameObject.FindGameObjectWithTag("DataManagment");
            datamanager.SendMessage("UploadInitialQuestions", entry);
        }
    }

    public void ClickButton1()
    {
        UncheckAll();
        questions[activeQuestion].rating = 1;
        EnableConfirmButton();
    }

    public void ClickButton2()
    {
        UncheckAll();
        questions[activeQuestion].rating = 2;
        EnableConfirmButton();
    }

    public void ClickButton3()
    {
        UncheckAll();
        questions[activeQuestion].rating = 3;
        EnableConfirmButton();
    }

    public void ClickButton4()
    {
        UncheckAll();
        questions[activeQuestion].rating = 4;
        EnableConfirmButton();
    }

    public void QuestionManagerTriggerNextScene()
    {
        TestRunController.TriggerNextScene();
    }
}
