using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public struct Emotion
{
    public Emotion(string emotion_name, string emotion_intensity)
    {
        name = emotion_name;
        intensity = emotion_intensity;
    }
    public string name;
    public string intensity;

    public string ToString()
    {
        return name + " " + intensity;
    }
}

public struct GDocsQuestionaryEntry
{
    public string id;
    public int round;
    public string high;
    public string medium;
    public string low;
}

public class WheelOfEmotionScript : MonoBehaviour
{
    private string high_emotion = "";
    private string medium_emotion = "";
    private string low_emotion = "";

    [SerializeField]
    private GameObject accept;

    [SerializeField]
    private GameObject load_spinner;

    [SerializeField]
    private GameObject legend1;

    [SerializeField]
    private GameObject legend2;

    [SerializeField]
    private GameObject legend3;

    private GameObject[] high;
    private GameObject[] medium;
    private GameObject[] low;


    private void Awake()
    {
        high = GameObject.FindGameObjectsWithTag(ButtonScript.tag_high);
        medium = GameObject.FindGameObjectsWithTag(ButtonScript.tag_medium);
        low = GameObject.FindGameObjectsWithTag(ButtonScript.tag_low);
    }

    public void AcceptClicked()
    {
        load_spinner.SetActive(true);
        accept.SetActive(false);
        legend1.SetActive(false);
        legend2.SetActive(false);
        legend3.SetActive(false);

        foreach (var h in high)
            h.SetActive(false);
        foreach (var h in medium)
            h.SetActive(false);
        foreach (var h in low)
            h.SetActive(false);


        GDocsQuestionaryEntry entry;
        entry.id = TestRunController.id;
        entry.round = TestRunController.GetCurrentRound();
        entry.low = low_emotion;
        entry.medium = medium_emotion;
        entry.high = high_emotion;


        GameObject datamanager = GameObject.FindGameObjectWithTag("DataManagment");
        datamanager.SendMessage("UploadLapQuestionary", entry);
    }

    public void ReceiveClickedEmotion(Emotion msg)
    {

        if (string.Equals(msg.name, low_emotion))
            low_emotion = "";
        if (string.Equals(msg.name, medium_emotion))
            medium_emotion = "";
        if (string.Equals(msg.name, high_emotion))
            high_emotion = "";

        switch (msg.intensity)
        {
            case ButtonScript.tag_high:
                high_emotion = msg.name;
                break;
            case ButtonScript.tag_medium:
                medium_emotion = msg.name;
                break;
            case ButtonScript.tag_low:
                low_emotion = msg.name;
                break;
        }

        if(high_emotion.Equals("") || medium_emotion.Equals("") || low_emotion.Equals(""))
        {
            accept.SetActive(false);
        }
        else
        {
            accept.SetActive(true);
        }
    }

    public void EmotionsTriggerNextScene()
    {
        TestRunController.TriggerNextScene();
    }
}
