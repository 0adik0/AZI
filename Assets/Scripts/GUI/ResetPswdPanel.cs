using CodeTitans.JSon;
using Ucss;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ResetPswdPanel : Menu
{
    public Text T;
    public Text ErrorText;
    public Text T1;
    public InputField IF;
    public Button Bt;

    void Start()
    {
        //Init();
    }

    public void Init()
    {
        Net.SendDefaultAuth();
        T.gameObject.SetActive(true);
        IF.gameObject.SetActive(true);
        Bt.gameObject.SetActive(true);
        T1.gameObject.SetActive(false);
    }
    public bool EmailChanged()
    {

        foreach (char c in IF.text)
        {
            if (c == '@')
            {
                ErrorText.text = "";
                return true;
            }
        }

        ErrorText.text = GameController.LangManager.GetTextValue("Reg.MailError");
        return false;
    }
    public void OnClose()
    {
        GUIController.Instance.ShowLoginPanel();
    }

    public void OnBtn()
    {
        if (EmailChanged())
        {

            SendCheckMail(IF.text);
        }
    }

    public void SendCheckMail(string login)
    {

        string link = Net.ServerURL + "/api/users/isemailused?email=" + login + "&access-token=" + GameController.Token;
        print("SendCheckLogin " + link);
        UCSS.HTTP.GetString(link, new EventHandlerHTTPString(OnCheckEmail), new EventHandlerServiceError(Net.Instance.OnHTTPError),
            new EventHandlerServiceTimeOut(Net.Instance.OnHTTPTimeOut), 10);

    }
    public void OnCheckEmail(string text, string transactionid)
    {
        print("text " + text);
        IJSonMutableObject obj = Net.ReadPack(text);
        if (obj != null)
        {
            if (!obj["isEmailUsed"].BooleanValue)
            {

                ErrorText.text = GameController.LangManager.GetTextValue("Reg.MailError0");
            }
            else
            {
                T.gameObject.SetActive(false);
                IF.gameObject.SetActive(false);
                Bt.gameObject.SetActive(false);
                T1.gameObject.SetActive(true);
                Net.ResetPswd(IF.text);
                // OnClose();
            }
        }
    }
}
