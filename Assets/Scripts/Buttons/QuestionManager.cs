using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

struct Question
{
    public Question(uint number_, string text_)
    {
        number = number_;
        text = text_;
        rating = -1;
    }

    public uint number;
    public string text;
    public int rating;

    public override string ToString()
    {
        return number + "," + text + "," + rating;
    }
};

public struct GDocsQuestionManagerEntry
{
    public string id;
    public int round;
    public int fun_rating;
    public int skill_rating;

    public GDocsQuestionManagerEntry(string id_, int round_, int fun_rating_, int skill_rating_)
    {
        id = id_;
        round = round_;
        fun_rating = fun_rating_;
        skill_rating = skill_rating_;
    }
}

public class QuestionManager : MonoBehaviour
{

    private int activeQuestion = 0;

    private GameObject[] radioButtons;

    private Text questionText;

    private List<Question> allquestions = new List<Question>();

    private Question[] questions;

    private GameObject confirmButton;


    // Use this for initialization
    void Start()
    {

        radioButtons = GameObject.FindGameObjectsWithTag("RadioButton");

        questionText = GameObject.FindGameObjectWithTag("Question").GetComponent<Text>();

        confirmButton = GameObject.FindGameObjectWithTag("ConfirmButton");

        allquestions.Add(new Question(1, "The level was fun"));
        allquestions.Add(new Question(2, "I can do better"));

        Shuffle(allquestions);

        // Todo shuffle
        questions = (Question[])allquestions.ToArray();

        questionText.text = questions[activeQuestion].text;

        UncheckAll();

        DisableConfirmButton();
    }

    void WriteQuestionFile(string name)
    {
    }

    void Shuffle(List<Question> list)
    {
        int n = list.Count;

        System.Random rng = new System.Random();
        while (n > 1)
        {
            n--;
            int k = Random.Range(0, n);
            Question value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }

    void DisableConfirmButton()
    {
        confirmButton.SetActive(false);
    }

    void EnableConfirmButton()
    {
        confirmButton.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {

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
            questionText.text = questions[activeQuestion].text;
        }
        else
        {
            int fun = -1;
            int skill = -1;
            foreach(Question i in questions)
            {
                switch(i.number)
                {
                    case 1:
                        fun = i.rating;
                        break;
                    case 2:
                        skill = i.rating;
                        break;
                    default:
                        Debug.Log("ERROR QUESTION MANAGER: Undefined question number!");
                        break;
                }
            }


            GDocsQuestionManagerEntry entry = new GDocsQuestionManagerEntry(TestRunController.id, TestRunController.GetCurrentRound(),
                fun,skill);


            GameObject datamanager = GameObject.FindGameObjectWithTag("DataManagment");
            datamanager.SendMessage("UploadBetweenQuestions", entry);
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
