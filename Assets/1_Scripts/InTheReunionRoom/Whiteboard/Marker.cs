using UnityEngine;
using UnityEngine.UI;

public class Marker : MonoBehaviour
{
    public GameObject paintingHead;
    private Image paintingHeadImage;
    
    static public Color color;

    [SerializeField]
    public MeshRenderer[] colouredParts;

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
        paintingHeadImage = paintingHead.GetComponent<Image>();
    }

    private void Update()
    {
        color = paintingHeadImage.color;
    }
}
