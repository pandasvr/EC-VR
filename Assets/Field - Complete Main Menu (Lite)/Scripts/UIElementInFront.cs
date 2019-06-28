using UnityEngine;

public class UIElementInFront : MonoBehaviour {

	void Start () 
	{
		 this.transform.SetAsFirstSibling ();
	}
}
