using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextboxController : MonoBehaviour {

    static TextboxController controller = null;

    [SerializeField]
    Texture defaultTexture;

    Text mainText;
    Text promptText;
    GameObject panel;
    CanvasRenderer canvasRenderer;

    string prompt;

    public static IEnumerator ShowText(string text, Texture image=null)
    {
        controller.Display(text, image);
        while (!Input.anyKeyDown)
            yield return null;
        controller.Hide();
        yield break;
    }

	// Use this for initialization
	void Awake () {
		if(controller==null)
        {
            mainText = GetComponent<Text>();
            promptText = transform.GetComponentInChildren<Text>();
            panel = transform.parent.gameObject;
            canvasRenderer = panel.GetComponent<CanvasRenderer>();

            
            panel.SetActive(false);
            prompt = promptText.text;
            promptText.text = "";

            controller = this;
        }
	}

    public void Display(string str, Texture image=null)
    {
        panel.SetActive(true);
        if (image == null)
        {
            if(defaultTexture!=null)
            canvasRenderer.SetTexture(defaultTexture);
        } else
        {
            canvasRenderer.SetTexture(image);
        }
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
