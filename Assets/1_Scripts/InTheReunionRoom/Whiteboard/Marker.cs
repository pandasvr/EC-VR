﻿using UnityEngine;

public class Marker : MonoBehaviour
{
    [SerializeField]
    private Color color;

    [SerializeField]
    private MeshRenderer[] colouredParts;

    [SerializeField]
    private Painter painter;

    [SerializeField]
    private PaintReceiver paintReceiver;

    public void Awake()
    {
        foreach (MeshRenderer renderer in colouredParts)
        {
            renderer.material.color = color;
        }

        paintReceiver = GameObject.FindGameObjectWithTag("paintReceiver").GetComponent<PaintReceiver>();
        painter.Initialize(paintReceiver);
        painter.ChangeColour(color);
    }
}
