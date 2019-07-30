using UnityEngine;

public class Eraser : MonoBehaviour
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

    private void UpdateRotation(Quaternion targetRotation, float followingSpeed)
    {
        if ((paintReceiver.transform.position - transform.position).z < 0.3f)
        {
            Vector3 eulerRotation = targetRotation.eulerAngles;
            eulerRotation.x = 0f;
            eulerRotation.y = 0f;

            targetRotation = Quaternion.Euler(eulerRotation);
        }
        
        GetComponent<Rigidbody>().rotation = Quaternion.Lerp(GetComponent<Rigidbody>().rotation, targetRotation, Time.deltaTime * followingSpeed);
    }
}
