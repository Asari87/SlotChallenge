using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CanvasGroup))]
public class Fader : MonoBehaviour
{
    private CanvasGroup canvasGroup;
    private GraphicRaycaster graphicRaycaster;
    private Coroutine fadeRoutine;
    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        graphicRaycaster = GetComponent<GraphicRaycaster>();
        canvasGroup.alpha = 1.0f;
        FadeIn(3f);
    }
    

    public Coroutine FadeIn(float fadeInTime)
    {
        graphicRaycaster.enabled = false;
        return Fade(fadeInTime, 0);
    }
    public Coroutine FadeOut(float fadeOutTime) 
    {
        graphicRaycaster.enabled = false;
        return Fade(fadeOutTime, 1);
    }

    private Coroutine Fade(float fadeTime, float alphaTarget)
    {
        if (fadeRoutine != null)
            StopCoroutine(fadeRoutine);
        fadeRoutine = StartCoroutine(FadeRoutine(alphaTarget, (1 / fadeTime)));
        return fadeRoutine;
    }

    private IEnumerator FadeRoutine(float alphaTarget, float fadeTime)
    {
        while(canvasGroup.alpha != alphaTarget)
        {
            canvasGroup.alpha = Mathf.MoveTowards(canvasGroup.alpha, alphaTarget, fadeTime * Time.deltaTime);
            yield return null;
        }
    }


    
}
