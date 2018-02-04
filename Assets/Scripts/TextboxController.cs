using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextboxController : MonoBehaviour {

    static TextboxController controller = null;

    Text mainText;
    Text promptText;
    GameObject panel;

    string prompt;

    public static IEnumerator ShowText(string text)
    {
        controller.Display(text);
        while (!Input.anyKeyDown)
            yield return null;
        controller.Hide();
        yield break;
    }

	// Use this for initialization
	void Start () {
		if(controller==null)
        {
            mainText = GetComponent<Text>();
            promptText = transform.GetComponentInChildren<Text>();
            panel = transform.parent.gameObject;

            panel.SetActive(false);
            prompt = promptText.text;
            promptText.text = "";

            controller = this;
        }
	}

    public void Display(string str)
    {
        panel.SetActive(true);
        mainText.text = str;
        promptText.text = prompt;
    }

    public void Hide()
    {
        panel.SetActive(false);
        mainText.text = "";
        promptText.text = "";
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
