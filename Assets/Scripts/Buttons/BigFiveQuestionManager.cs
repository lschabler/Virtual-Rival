using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

enum Personality
{
    Extraversion,
    Agreeableness,
    Conscientiousness,
    Neuroticism,
    Openness
}

struct BigFiveQuestion
{
    public BigFiveQuestion(uint number_, string text_, bool inverse_, Personality personality_)
    {
        number = number_;
        text = text_;
        rating = -1;
        inverse = inverse_;
        personality = personality_;
    }
    
    public uint number;
    public string text;
    public int rating;
    public bool inverse;
    public Personality personality;

    public override string ToString()
    {
        return number + "," + text;
    }

    public double GetRating()
    {
        if(inverse)
        {
            switch(rating)
            {
                case 1:
                    return 0.5;
                case 2:
                    return 0.25;
                case 3:
                    return -0.25;
                case 4:
                    return -0.5;
            }
            //return (6-rating) / 10.0f;

        }
        else
        {
            switch (rating)
            {
                case 1:
                    return -0.5;
                case 2:
                    return -0.25;
                case 3:
                    return 0.25;
                case 4:
                    return 0.5;
            }
            //return rating / 10.0f;
        }
        return 0;
    }
    
};

public struct GDocsBigFiveQuestionManagerEntry
{
    public string id;

    public int reserved_rating_1;
    public int trust_rating_2;
    public int lazy_rating_3;
    public int stress_rating_4;
    public int artistic_rating_5;
    public int sozial_rating_6;
    public int fault_rating_7;
    public int job_rating_8;
    public int nervous_rating_9;
    public int imagination_rating_10;

    public double extraversion;
    public double agreeableness;
    public double conscientiousness;
    public double neuroticism;
    public double openness;

    public GDocsBigFiveQuestionManagerEntry(string id_)
    {
        id = id_;
        reserved_rating_1 = -1;
        trust_rating_2 = -1;
        artistic_rating_5 = -1;
        lazy_rating_3 = -1;
        stress_rating_4 = -1;
        sozial_rating_6 = -1;
        fault_rating_7 = -1;
        job_rating_8 = -1;
        nervous_rating_9 = -1;
        imagination_rating_10 = -1;

        extraversion = -1.0;
        agreeableness = -1.0;
        conscientiousness = -1.0;
        neuroticism = -1.0;
        openness = -1.0;
    }
}

public class BigFiveQuestionManager : MonoBehaviour {

    private string q = "";

    private int activeQuestion = 0;

    private GameObject[] radioButtons;

    private Text questionText;

    private List<BigFiveQuestion> allquestions = new List<BigFiveQuestion>();

    private BigFiveQuestion[] questions;

    private GameObject confirmButton;

    [SerializeField]
    GameObject loading_circle;

    // Use this for initialization
    void Start () {
        radioButtons = GameObject.FindGameObjectsWithTag("RadioButton");

        questionText = GameObject.FindGameObjectWithTag("Question").GetComponent<Text>();

        confirmButton = GameObject.FindGameObjectWithTag("ConfirmButton");

        allquestions.Add(new BigFiveQuestion(1, "is reserved", true, Personality.Extraversion));
        allquestions.Add(new BigFiveQuestion(2, "is generally trusting", false, Personality.Agreeableness));
        allquestions.Add(new BigFiveQuestion(3, "tends to be lazy", true,Personality.Conscientiousness));
        allquestions.Add(new BigFiveQuestion(4, "is relaxed, handles stress well", true, Personality.Neuroticism));
        allquestions.Add(new BigFiveQuestion(5, "has few artistic interests", true, Personality.Openness));
        allquestions.Add(new BigFiveQuestion(6, "is outgoing, sociable", false, Personality.Extraversion));
        allquestions.Add(new BigFiveQuestion(7, "tends to find fault with others", true, Personality.Agreeableness));
        allquestions.Add(new BigFiveQuestion(8, "does a thorough job", false, Personality.Conscientiousness));
        allquestions.Add(new BigFiveQuestion(9, "gets nervous easily", false, Personality.Neuroticism));
        allquestions.Add(new BigFiveQuestion(10, "has an active imagination", false, Personality.Openness));

        Shuffle(allquestions);

        // Todo shuffle
        questions = (BigFiveQuestion[])allquestions.ToArray();

        SetQuestionText();

        UncheckAll();

        DisableConfirmButton();
    }

    void SetQuestionText()
    {
        questionText.text = "I see myself as someone who " + questions[activeQuestion].text;
    }

    void Shuffle(List<BigFiveQuestion> list)
    {
        int n = list.Count;

        System.Random rng = new System.Random();
        while (n > 1)
        {
            n--;
            int k = Random.Range(0, n);
            BigFiveQuestion value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }

    // Update is called once per frame
    void DisableConfirmButton()
    {
        confirmButton.SetActive(false);
    }

    void EnableConfirmButton()
    {
        loading_circle.SetActive(true);
        StartCoroutine(WaitAndSetActive(1.0f));
    }

    private IEnumerator WaitAndSetActive(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        confirmButton.SetActive(true);
        loading_circle.SetActive(false);
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
        Dictionary<Personality, double> personality = new Dictionary<Personality, double>();

        UncheckAll();
        activeQuestion++;
        DisableConfirmButton();
        if (activeQuestion < questions.Length)
        {
            SetQuestionText();
        }
        else
        {
            // Debug Output
            foreach (var q in questions)
            {
                if (!personality.ContainsKey(q.personality))
                    personality[q.personality] = 0.0f;
                personality[q.personality] += q.GetRating();
            }

            foreach (var item in personality)
            {
                Debug.Log(item.Key.ToString() + ": " + item.Value.ToString());
            }

            // Save to Gdocs

            GDocsBigFiveQuestionManagerEntry entry = new GDocsBigFiveQuestionManagerEntry(TestRunController.id);

            foreach(var q in questions)
            {
                switch(q.number)
                {
                    case 1:
                        entry.reserved_rating_1 = q.rating;
                        break;
                    case 2:
                        entry.trust_rating_2 = q.rating;
                        break;
                    case 3:
                        entry.lazy_rating_3 = q.rating;
                        break;
                    case 4:
                        entry.stress_rating_4 = q.rating;
                        break;
                    case 5:
                        entry.artistic_rating_5 = q.rating;
                        break;
                    case 6:
                        entry.sozial_rating_6 = q.rating;
                        break;
                    case 7:
                        entry.fault_rating_7 = q.rating;
                        break;
                    case 8:
                        entry.job_rating_8 = q.rating;
                        break;
                    case 9:
                        entry.nervous_rating_9 = q.rating;
                        break;
                    case 10:
                        entry.imagination_rating_10 = q.rating;
                        break;
                    default:
                        Debug.Log("ERROR BIG 5 QUESTION MANAGER: Undefined question number!");
                        break;
                }
            }

            entry.agreeableness = personality[Personality.Agreeableness];
            entry.conscientiousness = personality[Personality.Conscientiousness];
            entry.extraversion = personality[Personality.Extraversion];
            entry.neuroticism = personality[Personality.Neuroticism];
            entry.openness = personality[Personality.Openness];

            GameObject datamanager = GameObject.FindGameObjectWithTag("DataManagment");
            datamanager.SendMessage("UploadBig5Questions", entry);
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
