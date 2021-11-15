using CodeTitans.JSon;

using Ucss;

using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MoneyPanel : Menu
{
    public Button OutButton;
    public Text ErrorText;
    public void Init()
    {
        print("Init");
        GUIController.Instance.BalanceP.Show();
        ErrorText.text = "";
        if (GameController.MoneyTip == 2)
        {
            OutButton.interactable = false;
        }
        else
        {
            OutButton.interactable = true;
        }
    }
    public void OnIn()
    {
        print("GameController.MoneyTip " + GameController.MoneyTip);
       
        if (GameController.MoneyTip == 2)
        {
            if (GameController.Money > 10)
            {
                ErrorText.text = GameController.LangManager.GetTextValue("Money.Error");
            }
            else
            {
                WWWForm data = new WWWForm();
                data.AddField("event", "1");

                UCSS.HTTP.PostForm(Net.ServerURL + "/api/default/addmoney?access-token=" + GameController.Token, data,
                    new EventHandlerHTTPString(OnAddVirt),
                    new EventHandlerServiceError(Net.Instance.OnHTTPError),
                    new EventHandlerServiceTimeOut(Net.Instance.OnHTTPTimeOut), 10);
                OnClose();
            }
            
        }
        else
        {
            GUIController.Instance.ShowSelectSystem(false);
            //Application.OpenURL(Net.ServerURL + "/billing/payment?user_id="+GameController.Id+"&payment_system_id=1d&sum=100");
        }
    }
    private static void OnAddVirt(string text, string transactionid)
    {
        IJSonMutableObject obj = Net.ReadPack(text);
        print(text);
        if (obj != null)
        {
            if (obj.Contains("status"))
            {
                if (obj["status"].StringValue != "ok")
                    Debug.LogWarning(text);
                else
                    GameController.VirtMoney = GameController.Money = obj["amount"].DoubleValue;
            }
            else
            {
                Debug.LogWarning(text);
            }
        }

    }
    public void OnOut()
    {
        GUIController.Instance.ShowSelectSystem(true);
        //GUIController.Instance.ShowMoneyOut();
    }

    public void OnHistory()
    {
        GUIController.Instance.ShowHistory();
    }

    public void OnRulsOut()
    {
        GUIController.Instance.ShowRulsOut();
    }

    public void OnRulsIn()
    {
        GUIController.Instance.ShowRulsIn();
    }

    public void OnClose()
    {
        GUIController.Instance.ShowLoginPanel();
    }
}
