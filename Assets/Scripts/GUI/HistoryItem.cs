using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class HistoryItem : MonoBehaviour
{

    public Text DatText;
    public Text SumText;
    public Text TipText;
    public Image InImage;
    public Image OutImage;
    public GameObject StatusProceed;
    public GameObject StatusWaiting;
    public GameObject StatusDecline;
    public void Init(string tip, string date, string sys, string sum, string status)
    {
        if (tip == "in")
            OutImage.gameObject.SetActive(false);
        else
            InImage.gameObject.SetActive(false);
        DatText.text = date;
        SumText.text = sum;
        TipText.text = sys;
        StatusProceed.SetActive(false);
        StatusWaiting.SetActive(false);
        StatusDecline.SetActive(false);
        transform.localScale=Vector3.one;
        switch (status)
        {
            case "proceeded":
                StatusProceed.gameObject.SetActive(true);
                //StatusText.text = GameController.LangManager.GetTextValue("History.End");
                //StatusText.color=new Color(55f/255f,77f/255f,35f/255f);
                break;
            case "waiting":
                StatusWaiting.gameObject.SetActive(true);
                //StatusText.text = GameController.LangManager.GetTextValue("History.InWork");
                //StatusText.color = Color.blue;
                break;
            case "decline":
                StatusDecline.gameObject.SetActive(true);
                //StatusText.text = GameController.LangManager.GetTextValue("History.Denied");
                //StatusText.color = Color.red;
                break;

        }
    }
}
