using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueController : MonoBehaviour
{
    [SerializeField] Text dialogueText;
    [SerializeField] string[] fullText;
    [SerializeField] Text txtClickToContinue;
    [SerializeField] GameObject dialogueWindow;
    [SerializeField] Level.Theme setNextTheme;
    [SerializeField] Image imgViewer;
    [SerializeField] Sprite[] images;
    [SerializeField] string txtNextThemeTitle;

    int counter = 0; 

    [Header("Text Type Effect")]
    [SerializeField] float delay = 0.1f;
    private string currentText = "";

    [Header("Countdown")]
    [SerializeField] int countdownTime;

    private void Start()
    {
        StartDialogue();
    }

    void StartDialogue()
    {
        if(counter != fullText.Length)
        {
            StartCoroutine(TypeText(dialogueText, fullText[counter]));
        }
        else
        {
            dialogueWindow.GetComponent<Fade>().FadeIn(false);
            StartCoroutine(CountDown());
        }
    }

    IEnumerator TypeText(Text text, string fullText)
    {
        // Disable blinking text
        txtClickToContinue.enabled = false;

        // Enable image
        imgViewer.GetComponent<Image>().sprite = images[counter];

        // Type text
        for (int i = 0; i < fullText.Length; i++)
        {
            currentText = fullText.Substring(0, i);
            text.text = currentText;
            yield return new WaitForSeconds(delay);
        }

        // Enable blinking text
        txtClickToContinue.enabled = true;

        yield return waitForKeyPress(KeyCode.Space, KeyCode.Mouse0);

        // Disable blinking text
        txtClickToContinue.enabled = false;

        // Increment counter
        counter++;

        // Start next dialogue
        StartDialogue();
    }


    IEnumerator CountDown()
    {
        Level.GetInstance().SetTheme(setNextTheme);
        GameAssets.GetInstance().txtCountdown.enabled = true;
        while (countdownTime > 0)
        {
            GameAssets.GetInstance().txtCountdown.text = countdownTime.ToString();

            yield return new WaitForSeconds(1f);

            countdownTime--;
        }
        GameAssets.GetInstance().txtCountdown.text = txtNextThemeTitle;
        yield return new WaitForSeconds(3f);
        GameAssets.GetInstance().txtCountdown.enabled = false;
        Bird.GetInstance().state = Bird.State.Playing;
        dialogueWindow.SetActive(false);
    }


    IEnumerator waitForKeyPress(KeyCode key1, KeyCode key2)
    {
        bool done = false;
        while (!done) // essentially a "while true", but with a bool to break out naturally
        {
            if (Input.GetKeyDown(key1) | Input.GetKeyDown(key2))
            {
                done = true; // breaks the loop
            }
            yield return null; // wait until next frame, then continue execution from here (loop continues)
        }
        // now this function returns
    }
}
