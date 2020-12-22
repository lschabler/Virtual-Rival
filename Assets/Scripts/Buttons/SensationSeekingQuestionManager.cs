using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

enum SensationSeeking
{
    ExperienceSeeking,
    BoredomSusceptibility,
    ThrillAndAdventureSeeking,
    Disinhibition
}

struct SensationSeekingQuestion
{
    public SensationSeekingQuestion(uint number_, string text_, SensationSeeking seeking_)
    {
        number = number_;
        text = text_;
        rating = -1;
        seeking = seeking_;
    }

    public uint number;
    public string text;
    public int rating;
    public SensationSeeking seeking;

    public override string ToString()
    {
        return number + "," + text;
    }

    public double GetRating()
    {
        return rating;
    }

};

public struct GDocsSensationSeekingQuestionManagerEntry
{
    public string id;

    public int strange_places_1;
    public int no_preplanned_5;
    public int restless_home_2;
    public int friends_unpredictable_6;
    public int frightening_things_3;
    public int try_bungee_7;
    public int like_parties_4;
    public int exciting_experiences_8;

    public double experienceseeking;
    public double boredomsusceptibility;
    public double thrillandadventureseeking;
    public double disinhibition;

    public GDocsSensationSeekingQuestionManagerEntry(string id_)
    {
        id = id_;

        strange_places_1 = -1;
        no_preplanned_5 = -1;

        restless_home_2 = -1;
        friends_unpredictable_6 = -1;

        frightening_things_3 = -1;
        try_bungee_7 = -1;

        like_parties_4 = -1;
        exciting_experiences_8 = -1;

        experienceseeking = -1.0;
        boredomsusceptibility = -1.0;
        thrillandadventureseeking = -1.0;
        disinhibition = -1.0;
    }
}

public class SensationSeekingQuestionManager : MonoBehaviour
{

    private string q = "I see myself as someone who ";

    private int activeQuestion = 0;

    private GameObject[] radioButtons;

    private Text questionText;

    private List<SensationSeekingQuestion> allquestions = new List<SensationSeekingQuestion>();

    private SensationSeekingQuestion[] questions;

    private GameObject confirmButton;

    [SerializeField]
    GameObject loading_circle;

    void Start()
    {
        radioButtons = GameObject.FindGameObjectsWithTag("RadioButton");

        questionText = GameObject.FindGameObjectWithTag("Question").GetComponent<Text>();

        confirmButton = GameObject.FindGameObjectWithTag("ConfirmButton");

        allquestions.Add(new SensationSeekingQuestion(1, "I would like to explore strange places.", SensationSeeking.ExperienceSeeking));
        allquestions.Add(new SensationSeekingQuestion(5, "I would like to take off on a trip with no pre-planned routes or timetables.", SensationSeeking.ExperienceSeeking));
        allquestions.Add(new SensationSeekingQuestion(2, "I get restless when I spend too much time at home.", SensationSeeking.BoredomSusceptibility));
        allquestions.Add(new SensationSeekingQuestion(6, "I prefer friends who are excitingly unpredictable.", SensationSeeking.BoredomSusceptibility));
        allquestions.Add(new SensationSeekingQuestion(3, "I like to do frightening things.", SensationSeeking.ThrillAndAdventureSeeking));
        allquestions.Add(new SensationSeekingQuestion(7, "I would like to try bungee jumping.", SensationSeeking.ThrillAndAdventureSeeking));
        allquestions.Add(new SensationSeekingQuestion(4, "I like wild parties.", SensationSeeking.Disinhibition));
        allquestions.Add(new SensationSeekingQuestion(8, "I would love to have new and exciting experiences, even if they are illegal.", SensationSeeking.Disinhibition));

        Shuffle(allquestions);

        // Todo shuffle
        questions = (SensationSeekingQuestion[])allquestions.ToArray();

        SetQuestionText();

        UncheckAll();

        DisableConfirmButton();
    }

    public void Confirm()
    {
        Dictionary<SensationSeeking, double> seeking = new Dictionary<SensationSeeking, double>();

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
                if (!seeking.ContainsKey(q.seeking))
                    seeking[q.seeking] = 0.0f;
                seeking[q.seeking] += q.GetRating();
            }

            foreach (var item in seeking)
            {
                Debug.Log(item.Key.ToString() + ": " + item.Value.ToString());
            }

            // Save to Gdocs
            GDocsSensationSeekingQuestionManagerEntry entry = new GDocsSensationSeekingQuestionManagerEntry(TestRunController.id);

            foreach (var q in questions)
            {
                switch (q.number)
                {
                    case 1:
                        entry.strange_places_1 = q.rating;
                        break;
                    case 2:
                        entry.restless_home_2 = q.rating;
                        break;
                    case 3:
                        entry.frightening_things_3 = q.rating;
                        break;
                    case 4:
                        entry.like_parties_4 = q.rating;
                        break;
                    case 5:
                        entry.no_preplanned_5 = q.rating;
                        break;
                    case 6:
                        entry.friends_unpredictable_6 = q.rating;
                        break;
                    case 7:
                        entry.try_bungee_7 = q.rating;
                        break;
                    case 8:
                        entry.exciting_experiences_8 = q.rating;
                        break;
                    default:
                        Debug.Log("ERROR SENSATION SEEKING MANAGER: Undefined question number!");
                        break;
                }
            }

            entry.experienceseeking = seeking[SensationSeeking.ExperienceSeeking];
            entry.disinhibition = seeking[SensationSeeking.Disinhibition];
            entry.boredomsusceptibility = seeking[SensationSeeking.BoredomSusceptibility];
            entry.thrillandadventureseeking = seeking[SensationSeeking.ThrillAndAdventureSeeking];

            GameObject datamanager = GameObject.FindGameObjectWithTag("DataManagment");
            datamanager.SendMessage("UploadSensationSeekingQuestions", entry);
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
        questions[activeQuestion].rating = 4;
        EnableConfirmButton();
    }

    public void ClickButton4()
    {
        UncheckAll();
        questions[activeQuestion].rating = 5;
        EnableConfirmButton();
    }

    public void QuestionManagerTriggerNextScene()
    {
        TestRunController.TriggerNextScene();
    }

    void SetQuestionText()
    {
        questionText.text = questions[activeQuestion].text;
    }

    void Shuffle(List<SensationSeekingQuestion> list)
    {
        int n = list.Count;

        System.Random rng = new System.Random();
        while (n > 1)
        {
            n--;
            int k = Random.Range(0, n);
            SensationSeekingQuestion value = list[k];
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
}
