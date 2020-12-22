using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

public class ButtonScript : MonoBehaviour {

    [SerializeField]
    public string emotion_name;
    private const char seperator = ' ';
    public const string tag_high = "High";
    public const string tag_medium = "Medium";
    public const string tag_low = "Low";

    private Transform transform_high = null;
    private Transform transform_medium = null;
    private Transform transform_low = null;

    private Button button_high = null;
    private Button button_medium = null;
    private Button button_low = null;

    private string last_clicked = "";

    // Use this for initialization
    void Start () {
       Transform[] children = GetComponentsInChildren<Transform>();
       foreach(var child in children)
       {
            switch(child.tag)
            {
                case tag_high:
                    transform_high = child;
                    break;
                case tag_medium:
                    transform_medium = child;
                    break;
                case tag_low:
                    transform_low = child;
                    break;
                default:
                    break;
            }
       }
        Assert.AreNotEqual(emotion_name, "", "emotion name in script is not set");

        Assert.IsNotNull(transform_high, "hightag not set");
        Assert.IsNotNull(transform_medium, "mediumtag not set");
        Assert.IsNotNull(transform_low, "lowtag not set");

        button_high = transform_high.gameObject.GetComponent(typeof(Button)) as Button;
        button_medium = transform_medium.gameObject.GetComponent(typeof(Button)) as Button;
        button_low = transform_low.gameObject.GetComponent(typeof(Button)) as Button;

    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void OnClickedEmotionButton()
    {
        switch(last_clicked)
        {
            case tag_high:
                button_high.interactable = true;
                break;
            case tag_medium:
                button_medium.interactable = true;
                break;
            case tag_low:
                button_low.interactable = true;
                break;
            default:
                break;
        }

        if (!button_high.IsInteractable())
        {
            //this.transform.parent.BroadcastMessage("ResetButtons", emotion_name + seperator + tag_high);
            button_medium.interactable = true;
            button_low.interactable = true;
            last_clicked = tag_high;
        }
        else if (!button_medium.IsInteractable())
        {
            //this.transform.parent.BroadcastMessage("ResetButtons", emotion_name + seperator + tag_medium);
            button_high.interactable = true;
            button_low.interactable = true;
            last_clicked = tag_medium;
        }
        else if (!button_low.IsInteractable())
        {
            //this.transform.parent.BroadcastMessage("ResetButtons", emotion_name + seperator + tag_low);
            button_high.interactable = true;
            button_medium.interactable = true;
            last_clicked = tag_low; 
        }
        this.transform.parent.BroadcastMessage("ReceiveClickedEmotion", new Emotion(emotion_name,last_clicked));
    }

    public void ReceiveClickedEmotion(Emotion msg)
    {
        if (string.Equals(msg.name,emotion_name))
            return;

        if (string.Equals(msg.intensity, last_clicked))
            last_clicked = "";

        switch (msg.intensity)
        {
            case tag_high:
                button_high.interactable = true;
                break;
            case tag_medium:
                button_medium.interactable = true;
                break;
            case tag_low:
                button_low.interactable = true;
                break;
            default:
                break;
        }

    }


}
