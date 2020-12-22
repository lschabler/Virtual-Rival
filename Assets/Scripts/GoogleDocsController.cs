using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoogleDocsController : MonoBehaviour
{

    private static List<Lap> laps = new List<Lap>();

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
    }

    public void UploadRound(Lap lap)
    {
        Debug.Log("uploaded lap to google docs");

        StartCoroutine(PostRound(TestRunController.id, lap.round.ToString(), lap.PositionToString(), lap.TimeToString(), lap.SpeedToString(), 
            lap.opponent_id, lap.opponent_round.ToString(), lap.mmr.ToString(), lap.scene_type.ToString(), lap.avg_fps.ToString("N1"), lap.max_fps.ToString(), 
            lap.ResetpointsToString(), lap.ContactpointsToString()));
    }

    IEnumerator PostRound(string id, string round, string data, string time, string speed, string oppid, string oppround, string mmr, 
        string type, string avg_fps, string max_fps, string resetpoints, string conntactpoints)
    {
        // form action
        string Base_URL = "https://docs.google.com/forms/d/e/1FAIpQLSd3ZX-85Qe4Qf40P0zyLBGGHSYeSrM5JvtAkw7WJLevuJu88A/formResponse";
        WWWForm form = new WWWForm();
        form.AddField("entry.1614371190", id);
        form.AddField("entry.306343255", round);
        form.AddField("entry.1370521960", data);
        form.AddField("entry.405827058", time);
        form.AddField("entry.2142598625", speed);
        form.AddField("entry.141393872", oppid);
        form.AddField("entry.1334400903", oppround);
        form.AddField("entry.747666729", mmr);
        form.AddField("entry.231219547", type);
        form.AddField("entry.394157703", avg_fps);
        form.AddField("entry.2140799325", max_fps);
        form.AddField("entry.581630225", resetpoints);
        form.AddField("entry.245920403", conntactpoints);
        WWW www = new WWW(Base_URL, form.data);
        yield return www;

        TestRunController.TriggerNextScene();
    }

    public void UploadLapQuestionary(GDocsQuestionaryEntry entry)
    {
        Debug.Log("uploaded questionary to google docs");

        StartCoroutine(PostLapQuestionary(entry.id, entry.round, entry.high, entry.medium, entry.low));
    }

    IEnumerator PostLapQuestionary(string id, int round, string high, string medium, string low)
    {
        string Base_URL = "https://docs.google.com/forms/d/e/1FAIpQLSemXH8zwVc8uisy4aLUW990i9BBcLqR0R89U_UDDMsBoTvWPw/formResponse";
        WWWForm form = new WWWForm();
        form.AddField("entry.1233701858", id);
        form.AddField("entry.1049965446", round);
        form.AddField("entry.1952122733", high);
        form.AddField("entry.600115171", medium);
        form.AddField("entry.462399844", low);
        WWW www = new WWW(Base_URL, form.data);
        yield return www;

        GameObject datamanager = GameObject.FindGameObjectWithTag("DataManagment");
        datamanager.SendMessage("EmotionsTriggerNextScene");
    }

    public void UploadBetweenQuestions(GDocsQuestionManagerEntry entry)
    {
        Debug.Log("upload between questions to google docs");
        StartCoroutine(PostBetweenQuestions(entry.id, entry.round, entry.fun_rating, entry.skill_rating));
    }

    IEnumerator PostBetweenQuestions(string id, int round, int fun, int skill)
    {
        string Base_URL = "https://docs.google.com/forms/d/e/1FAIpQLSf0pGRwatKkkSYk-1bvZNF_pX_25E4LKqEihcnekiQdQdsMAA/formResponse";
        WWWForm form = new WWWForm();
        form.AddField("entry.1319038938", id);
        form.AddField("entry.917292570", round);
        form.AddField("entry.929870134", fun);
        form.AddField("entry.1484980464", skill);
        WWW www = new WWW(Base_URL, form.data);
        yield return www;

        GameObject datamanager = GameObject.FindGameObjectWithTag("DataManagment");
        datamanager.SendMessage("QuestionManagerTriggerNextScene");
    }

    public void UploadBig5Questions(GDocsBigFiveQuestionManagerEntry entry)
    {
        Debug.Log("upload between big 5 to google docs");
        StartCoroutine(PostBig5Questions(entry.id, entry.agreeableness.ToString(), entry.conscientiousness.ToString(), entry.extraversion.ToString(), entry.neuroticism.ToString(), entry.openness.ToString(),
            entry.reserved_rating_1, entry.trust_rating_2, entry.lazy_rating_3, entry.stress_rating_4, entry.artistic_rating_5,
            entry.sozial_rating_6, entry.fault_rating_7, entry.job_rating_8, entry.nervous_rating_9, entry.imagination_rating_10));
        Debug.Log(entry.agreeableness.ToString());
        //Debug.Log(entry.agreeableness);
    }

    IEnumerator PostBig5Questions(string id, string agreeableness, string conscientiousness, string extraversion, string neuroticism, string openness,
        int reserved_1, int trust_2, int lazy_3, int stress_4, int artistic_5,
        int sozial_6, int fault_7, int job_8, int nervous_9, int imagination_10)
    {
        string Base_URL = "https://docs.google.com/forms/d/e/1FAIpQLSeC8D-taISJo1Lr--pIXr4vweYqlLuaB9ZPbu2388InmwKs_g/formResponse";
        WWWForm form = new WWWForm();
        form.AddField("entry.1426296430", id);
        form.AddField("entry.345350860", agreeableness);
        form.AddField("entry.1350764584", conscientiousness);
        form.AddField("entry.188715868", extraversion);
        form.AddField("entry.977959719", neuroticism);
        form.AddField("entry.2102337265", openness);

        form.AddField("entry.1097491537", reserved_1.ToString());
        form.AddField("entry.715410856", trust_2.ToString());
        form.AddField("entry.1369914652", lazy_3.ToString());
        form.AddField("entry.407465352", stress_4.ToString());
        form.AddField("entry.175625305", artistic_5.ToString());
        form.AddField("entry.106750218", sozial_6.ToString());
        form.AddField("entry.1085228240", fault_7.ToString());
        form.AddField("entry.2107677931", job_8.ToString());
        form.AddField("entry.322928645", nervous_9.ToString());
        form.AddField("entry.728921133", imagination_10.ToString());

        WWW www = new WWW(Base_URL, form.data);
        yield return www;

        GameObject datamanager = GameObject.FindGameObjectWithTag("DataManagment");
        datamanager.SendMessage("QuestionManagerTriggerNextScene");
    }

    public void UploadSensationSeekingQuestions(GDocsSensationSeekingQuestionManagerEntry entry)
    {
        Debug.Log("upload sensation seeking to google docs");
        StartCoroutine(PostSensationSeekingQuestions(entry.id, entry.experienceseeking.ToString(), entry.disinhibition.ToString(), entry.boredomsusceptibility.ToString(), entry.thrillandadventureseeking.ToString(),
            entry.strange_places_1, entry.restless_home_2, entry.frightening_things_3, entry.like_parties_4,
            entry.no_preplanned_5, entry.friends_unpredictable_6, entry.try_bungee_7, entry.exciting_experiences_8));
    }

    IEnumerator PostSensationSeekingQuestions(string id, string experienceseeking, string disinhibition, string boredomsusceptibility, string thrillandadventureseeking,
        int strange_places_1, int restless_home_2, int frightening_things_3, int like_parties_4, 
        int no_preplanned_5, int friends_unpredictable_6, int try_bungee_7, int exciting_experiences_8)
    {
        string Base_URL = "https://docs.google.com/forms/d/e/1FAIpQLSfrsh-x0mZ8MKc399XqUPSU0K_5-8lsUUW5-hsmtii5hH4bXQ/formResponse";
        WWWForm form = new WWWForm();
        form.AddField("entry.2146043283", id);
        form.AddField("entry.2000490741", experienceseeking);
        form.AddField("entry.304446421", disinhibition);
        form.AddField("entry.2076963905", boredomsusceptibility);
        form.AddField("entry.1390878193", thrillandadventureseeking);

        form.AddField("entry.1176980851", strange_places_1.ToString());
        form.AddField("entry.944243803", restless_home_2.ToString());
        form.AddField("entry.68166701", frightening_things_3.ToString());
        form.AddField("entry.1713868041", like_parties_4.ToString());
        form.AddField("entry.1996145197", no_preplanned_5.ToString());
        form.AddField("entry.137636894", friends_unpredictable_6.ToString());
        form.AddField("entry.414252725", try_bungee_7.ToString());
        form.AddField("entry.168783149", exciting_experiences_8.ToString());

        WWW www = new WWW(Base_URL, form.data);
        yield return www;

        GameObject datamanager = GameObject.FindGameObjectWithTag("DataManagment");
        datamanager.SendMessage("QuestionManagerTriggerNextScene");
    }

    public void UploadInitialQuestions(GDocsInitQuestionManagerEntry entry)
    {
        Debug.Log("upload between initial questions to google docs");
        StartCoroutine(PostInitialQuestions(entry.id, entry.race_type, entry.driving_skill, entry.videogame_experience));
    }

    IEnumerator PostInitialQuestions(string id, string type, int driving, int videogame)
    {
        string Base_URL = "https://docs.google.com/forms/d/e/1FAIpQLSdp_1b5IAVsLAb9morDdXXAZzZOSrAwXW8R7F3Ss60JohLIVQ/formResponse";
        WWWForm form = new WWWForm();
        form.AddField("entry.1373375777", id);
        form.AddField("entry.396628572", type);
        form.AddField("entry.1404458208", driving.ToString());
        form.AddField("entry.113067831", videogame.ToString());
        WWW www = new WWW(Base_URL, form.data);
        yield return www;

        GameObject datamanager = GameObject.FindGameObjectWithTag("DataManagment");
        datamanager.SendMessage("QuestionManagerTriggerNextScene");
    }

    public void UploadAgeQuestions(GDocsAgeQuestionManagerEntry entry)
    {
        Debug.Log("upload between age to google docs");
        StartCoroutine(PostAgeQuestions(entry.id, entry.age));
    }

    IEnumerator PostAgeQuestions(string id, int age)
    {
        string Base_URL = "https://docs.google.com/forms/d/e/1FAIpQLSc31fN4_jTD4oG0rgBNuYZLlGvnoTyxKI3z8Ehj1Wd_nHV1Ow/formResponse";
        WWWForm form = new WWWForm();
        form.AddField("entry.810392353", id);
        form.AddField("entry.351840087", age);
        WWW www = new WWW(Base_URL, form.data);
        yield return www;

        GameObject datamanager = GameObject.FindGameObjectWithTag("DataManagment");
        datamanager.SendMessage("QuestionManagerTriggerNextScene");
    }
}
