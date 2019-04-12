using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class LoadingLoginSimulation : MonoBehaviour
{
    public VRTK_UICanvas mainCanvas;
    public VRTK_UICanvas loadingCanvas;
    public Animator loadingCanvasAnimator;
    public GameObject asDePique;

    public GameObject loginPanel;
    public GameObject mainPanel;
    
    private IEnumerator coroutine;

    public void StartSimulation(float duree)
    {
        mainCanvas.enabled = false;
        loadingCanvas.enabled = true;
        loadingCanvasAnimator.Play("Exit Panel In");
        asDePique.SetActive(true);
        coroutine = WaitTimer(duree);
        StartCoroutine(coroutine);
    }

    public void EndSimulation()
    {
        mainCanvas.enabled = true;
        loadingCanvas.enabled = false;
        loadingCanvasAnimator.Play("Exit Panel Out");
        asDePique.SetActive(false);
        
        loginPanel.SetActive(false);
        mainPanel.SetActive(true);

    }

    private IEnumerator WaitTimer(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        EndSimulation();
    }
    
}
