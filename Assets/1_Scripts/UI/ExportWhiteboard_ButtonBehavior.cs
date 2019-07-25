using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExportWhiteboard_ButtonBehavior : MonoBehaviour
{

    public GameObject Button_AddPPT1;
    public GameObject Button_AddPPT2;
    public GameObject Button_AddPPT3;
    public GameObject Text_Info;
    public Animator ExportWhiteboard;

    // Start is called before the first frame update
    void Start()
    {

        DisableButton2();
        DisableButton3();
    }

    //Etape de création du document
    public void Step1()
    {
        EnableTextInfo("Document créé");
        DisableButton1();
        EnableButton2();
    }
    
    //Etape d'ajout de slide
    public void Step2()
    {
        EnableTextInfo("Slide ajoutée au document");
        EnableButton3();
    }
    
    //Etape d'export du document
    public void Step3()
    {
        EnableTextInfo("Document exporté");
        EnableButton1();
        DisableButton2();
        DisableButton3();
    }

    void EnableTextInfo(string text)
    {
        ExportWhiteboard.Rebind();
        Text_Info.GetComponent<Text>().text = text;
        ExportWhiteboard.Play("FadeOut_TextInfo");
    }

    void EnableButton1()
    {
        Button_AddPPT1.GetComponent<Button>().interactable = true;
        Button_AddPPT1.transform.Find("Link Content").gameObject.transform.Find("Text").gameObject.GetComponent<Text>().color = Color.white;
        Button_AddPPT1.transform.Find("Link Content").gameObject.transform.Find("Image").gameObject.GetComponent<Image>().color = Color.white;
    }
    
    void EnableButton2()
    {
        Button_AddPPT2.GetComponent<Button>().interactable = true;
        Button_AddPPT2.transform.Find("Link Content").gameObject.transform.Find("Text").gameObject.GetComponent<Text>().color = Color.white;
        Button_AddPPT2.transform.Find("Link Content").gameObject.transform.Find("Image").gameObject.GetComponent<Image>().color = Color.white;
    }
    
    void EnableButton3()
    {
        Button_AddPPT3.GetComponent<Button>().interactable = true;
        Button_AddPPT3.transform.Find("Link Content").gameObject.transform.Find("Text").gameObject.GetComponent<Text>().color = Color.white;
        Button_AddPPT3.transform.Find("Link Content").gameObject.transform.Find("Image").gameObject.GetComponent<Image>().color = Color.white;
    }
    
    void DisableButton1()
    {
        Button_AddPPT1.GetComponent<Button>().interactable = false;
        Button_AddPPT1.transform.Find("Link Content").gameObject.transform.Find("Text").gameObject.GetComponent<Text>().color = Color.grey;
        Button_AddPPT1.transform.Find("Link Content").gameObject.transform.Find("Image").gameObject.GetComponent<Image>().color = Color.grey;
    }
    
    void DisableButton2()
    {
        Button_AddPPT2.GetComponent<Button>().interactable = false;
        Button_AddPPT2.transform.Find("Link Content").gameObject.transform.Find("Text").gameObject.GetComponent<Text>().color = Color.grey;
        Button_AddPPT2.transform.Find("Link Content").gameObject.transform.Find("Image").gameObject.GetComponent<Image>().color = Color.grey;
    }
    
    void DisableButton3()
    {
        Button_AddPPT3.GetComponent<Button>().interactable = false;
        Button_AddPPT3.transform.Find("Link Content").gameObject.transform.Find("Text").gameObject.GetComponent<Text>().color = Color.grey;
        Button_AddPPT3.transform.Find("Link Content").gameObject.transform.Find("Image").gameObject.GetComponent<Image>().color = Color.grey;
    }
}
