using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RadialMenuSettings : MonoBehaviour
{
    public Color hoverEnterIconColor;
    public Color hoverExitIconColor;

    public void HoverEnterColorIcon(Image icon)
    {
        icon.color = new Color();
        icon.color = hoverEnterIconColor;
    }
    
    public void HoverExitColorIcon(Image icon)
    {
        icon.color = new Color();
        icon.color = hoverExitIconColor;
    }

    public void HoverEnterAnimationArc(Animator anim)
    {
        anim.SetInteger("isEnter", 1);
    }
    
    public void HoverExitAnimationArc(Animator anim)
    {
        anim.SetInteger("isEnter", 0); 
    }
}
