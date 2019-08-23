﻿using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using VRTK;

public class SpawnerBox : MonoBehaviour
{
    [Tooltip("Entrer le nom de la prefab en question")]
    public string stringPostItToSpawn;

    public float spawnDelay = 1f;

    private float spawnDelayTimer;

    private void Start()
    {
        spawnDelayTimer = 0f;
    }


    //Action si un objet entre en collision avec
    private void OnTriggerStay(Collider collider)
    {
        VRTK_InteractGrab grabbingController = (collider.gameObject.GetComponent<VRTK_InteractGrab>()
            ? collider.gameObject.GetComponent<VRTK_InteractGrab>()
            : collider.gameObject.GetComponentInParent<VRTK_InteractGrab>());

        //On continue le script si l'objet en collision est une manette, qu'il peut grab un objet, que le delais d'attente avec le spawn précédent est passé et qu'il reste encore des objets à spawn
        if (CanGrab(grabbingController) && Time.time >= spawnDelayTimer)
        {
            GameObject currentObj = PhotonNetwork.Instantiate(stringPostItToSpawn, new Vector3(0, 0, 0), Quaternion.identity, 0);

            grabbingController.GetComponent<VRTK_InteractTouch>().ForceTouch(currentObj);
            grabbingController.AttemptGrab();
            spawnDelayTimer = Time.time + spawnDelay;
        }
    }
    
    //Action pour vérifier si la manette peut grab un objet
    private bool CanGrab(VRTK_InteractGrab grabbingController)
    {
        return grabbingController && grabbingController.GetGrabbedObject() == null && grabbingController.IsGrabButtonPressed();
    }
    
}
