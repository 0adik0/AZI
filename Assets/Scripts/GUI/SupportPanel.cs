using UnityEngine;
using System.Collections;

using UnityEngine.UI;

public class SupportPanel : Menu
{
    public Text MsgText;
    public Text EmailText;
   

    public void Start()
    {
        Invoke("Init",2f);
    }
 

    public void OnSend()
    {
        Net.SendSupport(EmailText.text, MsgText.text);
        GUIController.Instance.OnBackBtn();
    }
}
