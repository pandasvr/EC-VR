using System;
using UnityEngine;

public class Painter : MonoBehaviour
{
    [SerializeField]
    private PaintMode paintMode;

    [SerializeField]
    private Transform paintingTransform;

    [SerializeField]
    private float raycastLength = 0.01f;

    [SerializeField]
    private Texture2D brush;

    [SerializeField]
    private float spacing = 1f;
    
    private float currentAngle = 0f;
    private float lastAngle = 0f;

    private PaintReceiver paintReceiver;
    private Collider paintReceiverCollider;

    //Old variable (récupérer le draggable object depuis VRTK)
    //private DraggableObject paintingObject;

    private Stamp stamp;

    public GameObject paintinghead;
    [SerializeField]
    public Color color;

    private Vector2? lastDrawPosition = null;
    public PaintReceiver newPaintReceiver;

    

    public void Initialize(PaintReceiver newPaintReceiver)
    {
        try 
        {
            stamp = new Stamp(brush);
            stamp.mode = paintMode;
            //newPaintReceiver = GameObject.FindGameObjectWithTag("paintReceiver").GetComponent<PaintReceiver>();
            paintReceiver = newPaintReceiver;
            paintReceiverCollider = newPaintReceiver.GetComponent<Collider>();
        }
        catch(NullReferenceException){}
    }

    private void Update()
    {
        
        currentAngle = -transform.rotation.eulerAngles.z;

        Ray ray = new Ray(paintingTransform.position, paintingTransform.forward);
        RaycastHit hit;

        Debug.DrawRay(ray.origin, ray.direction * raycastLength);

        try
        {
            if (paintinghead == null)
            {
                paintinghead = GameObject.FindGameObjectWithTag("markerColouredParts");
            }

            color = new Color();
            color = paintinghead.GetComponent<Renderer>().material.color;
            
            if (paintReceiverCollider.Raycast(ray, out hit, raycastLength))
            {
                if (lastDrawPosition.HasValue && lastDrawPosition.Value != hit.textureCoord)
                {
                    paintReceiver.DrawLine(stamp, lastDrawPosition.Value, hit.textureCoord, lastAngle, currentAngle,
                        color, spacing);
                }
                else
                {
                    paintReceiver.CreateSplash(hit.textureCoord, stamp, color, currentAngle);
                }

                lastAngle = currentAngle;

                lastDrawPosition = hit.textureCoord;
            }
            else
            {
                lastDrawPosition = null;
            }
        }
        catch(NullReferenceException){}
    }

    public void ChangeColour(Color newColor)
    {
        color = newColor;
    }

    public void SetRotation(float newAngle)
    {
        currentAngle = newAngle;
    }
}
