using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class cursor : MonoBehaviour , IPointerEnterHandler, IPointerExitHandler
{
    public Texture2D cursorTexture;
    public CursorMode cursorMode = CursorMode.Auto;
    public Vector2 hotSpot = Vector2.zero;
    
    //Detect if the Cursor starts to pass over the GameObject
    public void OnPointerEnter(PointerEventData pointerEventData)
    {
        Cursor.SetCursor(cursorTexture, hotSpot, cursorMode);
    }

    //Detect when Cursor leaves the GameObject
    public void OnPointerExit(PointerEventData pointerEventData)
    {
        Cursor.SetCursor(null, Vector2.zero, cursorMode);
    }
}
