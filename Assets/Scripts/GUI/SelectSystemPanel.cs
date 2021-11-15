using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public enum ESystemType
{
    WM,
    Yandex,
    PayPal,
    Qiwi
}
public class SelectSystemPanel : Menu
{
    private bool _out = true;
    /// <summary>
    /// если тру то выводим фалсе если вводим
    /// </summary>
    public bool Out {
        get { return _out; }
        set
        {
            _out = value;
            if (_out)
            {
                TopText.text = GameController.LangManager.GetTextValue("MoneyOut.Top");
            }
            else
            {
                TopText.text = GameController.LangManager.GetTextValue("MoneyIn.Top");
            }
        } }

    public Text TopText;
    public void OnWM()
    {
        if (!Out)
            Application.OpenURL(Net.ServerURL + "/billing/payment?user_id=" + GameController.Id + "&payment_system_id=4&sum=10");
        else
            GUIController.Instance.ShowMoneyOut(ESystemType.WM);
    }
    public void OnYandex()
    {
        if (!Out)
            Application.OpenURL(Net.ServerURL + "/billing/payment?user_id=" + GameController.Id + "&payment_system_id=5&sum=10");
        else
            GUIController.Instance.ShowMoneyOut(ESystemType.Yandex);
    }
    public void OnQiwi()
    {
        if (!Out)
            Application.OpenURL(Net.ServerURL + "/billing/payment?user_id=" + GameController.Id + "&payment_system_id=2&sum=10");
        else
            GUIController.Instance.ShowMoneyOut(ESystemType.Qiwi);
    }
    public void OnPayPal()
    {
        if (!Out)
            Application.OpenURL(Net.ServerURL + "/billing/payment?user_id=" + GameController.Id + "&payment_system_id=3&sum=10");
        else
            GUIController.Instance.ShowMoneyOut(ESystemType.PayPal);
    }
}
