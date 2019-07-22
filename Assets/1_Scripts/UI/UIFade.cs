using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIFade : MonoBehaviour
{
    public void FadeOut()
    {
        StartCoroutine(DoFadeOut());
    }

    public void FadeIn()
    {
        StartCoroutine(DoFadeIn());
    }

    IEnumerator DoFadeOut()
    {
        CanvasGroup canvasGroup = GetComponent<CanvasGroup>();
        canvasGroup.interactable = false;
        
        while (canvasGroup.alpha > 0)
        {
            canvasGroup.alpha -= Time.deltaTime*2;
            yield return null;
        }
        
        Debug.Log("End Fade Out");
        
        yield return null;
    }
    
    IEnumerator DoFadeIn()
    {
        CanvasGroup canvasGroup = GetComponent<CanvasGroup>();

        while (canvasGroup.alpha < 1)
        {
            canvasGroup.alpha += Time.deltaTime*2;
            yield return null;
        }
        
        Debug.Log("End Fade In");
        canvasGroup.interactable = true;
        
        yield return null;
    }
}
