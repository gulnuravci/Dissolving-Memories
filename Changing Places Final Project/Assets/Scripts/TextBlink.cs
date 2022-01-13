using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextBlink : MonoBehaviour
{
    void Start()
    {
        GetComponent<Text>().color = new Color(GetComponent<Text>().color.r, GetComponent<Text>().color.g, GetComponent<Text>().color.b, 0);
        StartCoroutine(WaitToStart());
    }

    private IEnumerator WaitToStart()
    {
        yield return new WaitForSeconds(5);
        StartCoroutine(FadeIn());
    }


    private IEnumerator FadeIn()
    {
        StartCoroutine(FadeTextToFullAlpha(1f, GetComponent<Text>()));
        yield return new WaitForSeconds(1.5f);
        StartCoroutine(FadeOut());
    }

    private IEnumerator FadeOut()
    {
        StartCoroutine(FadeTextToZeroAlpha(1f, GetComponent<Text>()));
        yield return new WaitForSeconds(1.5f);
        StartCoroutine(FadeIn());
    }

    private IEnumerator FadeTextToFullAlpha(float t, Text i)
    {
        i.color = new Color(i.color.r, i.color.g, i.color.b, 0);
        while (i.color.a < 1.0f)
        {
            i.color = new Color(i.color.r, i.color.g, i.color.b, i.color.a + (Time.deltaTime / t));
            yield return null;
        }
    }

    private IEnumerator FadeTextToZeroAlpha(float t, Text i)
    {
        i.color = new Color(i.color.r, i.color.g, i.color.b, 1);
        while (i.color.a > 0.0f)
        {
            i.color = new Color(i.color.r, i.color.g, i.color.b, i.color.a - (Time.deltaTime / t));
            yield return null;
        }
    }
}
