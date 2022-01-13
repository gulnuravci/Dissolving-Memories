using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fade : MonoBehaviour
{
    public float duration = 0.4f;

    public void FadeIn(bool fade)
    {
        var canvGroup = GetComponent<CanvasGroup>();

        StartCoroutine(IFade(canvGroup, canvGroup.alpha, fade ? 1 : 0));
    }

    public IEnumerator IFade(CanvasGroup canvGroup, float start, float end)
    {
        float counter = 0f;

        while(counter < duration)
        {
            counter += Time.deltaTime;
            canvGroup.alpha = Mathf.Lerp(start, end, counter / duration);

            yield return null;
        }
    }
}
