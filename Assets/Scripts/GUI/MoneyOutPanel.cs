using System;
using CodeTitans.JSon;
using SmartLocalization.Editor;
using Ucss;
using UnityEngine;
using System.Collections;

using UnityEngine.UI;

public class MoneyOutPanel : Menu
{

    public GameObject WMPanel;
    public GameObject QiwiPanel;
    public GameObject YandexPanel;
    public GameObject PayPalPanel;
    public InputField YandexAccount;
    public InputField Sum;
    public InputField Phoneqiwi;
    public InputField PayPalPhone;
    public InputField AccountWm;
    public Text WmTipText;
    public InputField Card1;
    public InputField Card2;
    public InputField Card3;
    public InputField Card4;
    public Text Total;
    public Text TopText;
    public Text ErrorMsg;
    public ItemsSelector ItemsSelector;
    void Start()
    {
        // TipWM.InitList(new[] { "WMR", "WMZ", "WME", "WMU", "WMB", "WMK", "WMG", "WMX" });
        ErrorMsg.text = "";
        OnQiwi();


    }
    
    public void OnEndEdit()
    {
        try
        {
            double d = Double.Parse(Sum.text);
            Total.text = ((d / 100) * (100 - GameController.Komiss)).ToString();
        }
        catch (Exception)
        {
            Total.text = "0";
        }
    }

    public void OnOut()
    {
        switch (SysteType)
        {
            case ESystemType.WM:
                OnOutWM();
                break;
            case ESystemType.Yandex:
                OnOutYandex();
                break;
            case ESystemType.Qiwi:
                OnOutQiwi();
                break;
            default:
                OnOutPayPal();
                break;
        }
    }
    public void OnOutPayPal()
    {
        if (CheckFuns(Sum))
        {
            print("Out OnOutPayPal ");
            OnClose();
        }
        else
        {
            ErrorMsg.text = GameController.LangManager.GetTextValue("MoneyOut.Error");
        }
    }
    public void OnOutYandex()
    {
        if (CheckFuns(Sum))
        {
            print("OnOutYandex " + Sum.text);
            WWWForm data = new WWWForm();
            data.AddField("user_id", GameController.Id);
            data.AddField("payment_system_id", 5);
            data.AddField("sum", Sum.text);
            data.AddField("account_number", Card1.text + Card2.text + Card3.text + Card4.text);
            UCSS.HTTP.PostForm(Net.ServerURL + "/api/default/payout", data,
                new EventHandlerHTTPString(OnAnsw),
                new EventHandlerServiceError(Net.Instance.OnHTTPError),
                new EventHandlerServiceTimeOut(Net.Instance.OnHTTPTimeOut), 10);
            OnClose();
        }
        else
        {
            ErrorMsg.text = GameController.LangManager.GetTextValue("MoneyOut.Error");
        }
    }
    public void OnOutWM()
    {
        if (CheckFuns(Sum))
        {
            WWWForm data = new WWWForm();
            data.AddField("user_id", GameController.Id);
            data.AddField("payment_system_id", 4);
            data.AddField("sum", Sum.text);
            data.AddField("account_number", AccountWm.text);
            UCSS.HTTP.PostForm(Net.ServerURL + "/api/default/payout", data,
                new EventHandlerHTTPString(OnAnsw),
                new EventHandlerServiceError(Net.Instance.OnHTTPError),
                new EventHandlerServiceTimeOut(Net.Instance.OnHTTPTimeOut), 10);
            OnClose();
        }
        else
        {
            ErrorMsg.text = GameController.LangManager.GetTextValue("MoneyOut.Error");
        }
    }

    private void OnAnsw(string text, string transactionid)
    {
        print(text);
        IJSonMutableObject obj = Net.ReadPack(text);
        if (obj != null)
        {
            if (obj["status"].StringValue == "ok")
            {
                GUIController.Instance.ShowLobbiPanel();
            }
            else
            {
                ErrorMsg.text = GameController.LangManager.GetTextValue("MoneyOut.Error");
            }
        }
    }

    public void OnOutQiwi()
    {
        if (CheckFuns(Sum))
        {
            WWWForm data = new WWWForm();
            data.AddField("user_id", GameController.Id);
            data.AddField("payment_system_id", 2);
            data.AddField("sum", Sum.text);
            data.AddField("account_number", Phoneqiwi.text);
            UCSS.HTTP.PostForm(Net.ServerURL + "/api/default/payout", data,
                new EventHandlerHTTPString(OnAnsw),
                new EventHandlerServiceError(Net.Instance.OnHTTPError),
                new EventHandlerServiceTimeOut(Net.Instance.OnHTTPTimeOut), 10);
            OnClose();
        }
        else
        {
            ErrorMsg.text = GameController.LangManager.GetTextValue("MoneyOut.Error");
        }
    }

    private bool CheckFuns(InputField sum)
    {
        try
        {
            double d = Double.Parse(sum.text);
            if (d <= GameController.RealMoney)
                return true;
            else
                return false;
        }
        catch (Exception e)
        {
            return false;
        }

    }

    public void OnWM()
    {
        TopText.text = GameController.LangManager.GetTextValue("MoneyOut.Top") + " WebMoney";
        WMPanel.SetActive(true);
        QiwiPanel.SetActive(false);
        YandexPanel.SetActive(false);
        PayPalPanel.SetActive(false);
    }
    public void OnQiwi()
    {
        TopText.text = GameController.LangManager.GetTextValue("MoneyOut.Top") + " QiWi";
        QiwiPanel.SetActive(true);
        WMPanel.SetActive(false);
        YandexPanel.SetActive(false);
        PayPalPanel.SetActive(false);
    }
    public void OnYandex()
    {
        TopText.text = GameController.LangManager.GetTextValue("MoneyOut.Top") + " VISA / Mastercard";
        YandexPanel.SetActive(true);
        WMPanel.SetActive(false);
        QiwiPanel.SetActive(false);
        PayPalPanel.SetActive(false);
    }
    public void OnPayPal()
    {
        TopText.text = GameController.LangManager.GetTextValue("MoneyOut.Top") + " PayPal";
        YandexPanel.SetActive(false);
        WMPanel.SetActive(false);
        QiwiPanel.SetActive(false);
        PayPalPanel.SetActive(true);
    }
    public void OnDateSelect()
    {
        ItemsSelector.TargetField = WmTipText;
        ItemsSelector.InitList(new[] { "WMZ", "WMR", "WME", "WMU", "WMB", "WMK" });
        ItemsSelector.gameObject.transform.localPosition = Vector3.zero;
        ItemsSelector.gameObject.SetActive(true);
        //DateSelector.Show();
    }
    public void OnClose()
    {
        ErrorMsg.text = "";
        GUIController.Instance.ShowLoginPanel();
    }

    public void Init(ESystemType tip)
    {
        Sum.text = "0";
        Total.text = "0";
        SysteType = tip;
        switch (tip)
        {
            case ESystemType.WM:
                OnWM();
                break;
            case ESystemType.Yandex:
                OnYandex();
                break;
            case ESystemType.Qiwi:
                OnQiwi();
                break;
            default:
                OnPayPal();
                break;
        }
    }

    public ESystemType SysteType { get; set; }
}
