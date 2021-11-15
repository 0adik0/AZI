using System;

using UnityEngine;
using System.Collections;

using UnityEngine.UI;

public class MoneyInPanel : Menu
{

    public int Sys;
    public InputField Sum;

    public Text ErrorMsg;
    public Sprite Qiwi1Texture;
    public Sprite Qiwi2Texture;
    public Image Qiwi;
    public Sprite WM1Texture;
    public Sprite WM2Texture;
    public Image WM;
    public Sprite Yandex1Texture;
    public Sprite Yandex2Texture;
    public Image Yandex;
    public Sprite PayPal1Texture;
    public Sprite PayPal2Texture;
    public Image PayPal;

    void Start()
    {
        ErrorMsg.text = "";
        OnQiwi();


    }
    /// <summary>
    /// инициализируем панель
    /// </summary>
    /// <param name="type"></param>
    public void Init(ESystemType type)
    {
        switch (type)
        {
            case ESystemType.WM:
                OnWM();
                break;
            case ESystemType.PayPal:
                OnPayPal();
                break;
            case ESystemType.Yandex:
                OnYandex();
                break;
            default:
                OnQiwi();
                break;
        }
    }
    public void OnOut()
    {
        Application.OpenURL(Net.ServerURL + "/billing/payment?user_id=" + GameController.Id + "&payment_system_id=" + Sys + "&sum=" + Sum.text);
        OnClose();
    }


    public void OnWM()
    {
        Sys = 4;
        Qiwi.sprite = Qiwi1Texture;
        WM.sprite = WM2Texture;
        Yandex.sprite = Yandex1Texture;
        PayPal.sprite = PayPal1Texture;
    }
    public void OnQiwi()
    {
        Sys = 2;
        Qiwi.sprite = Qiwi2Texture;
        WM.sprite = WM1Texture;
        Yandex.sprite = Yandex1Texture;
        PayPal.sprite = PayPal1Texture;

    }
    public void OnYandex()
    {
        Sys = 1;
        Qiwi.sprite = Qiwi1Texture;
        WM.sprite = WM1Texture;
        Yandex.sprite = Yandex2Texture;
        PayPal.sprite = PayPal1Texture;

    }
    public void OnPayPal()
    {
        Sys = 3;
        Qiwi.sprite = Qiwi1Texture;
        WM.sprite = WM1Texture;
        Yandex.sprite = Yandex1Texture;
        PayPal.sprite = PayPal2Texture;

    }
    public void OnClose()
    {
        ErrorMsg.text = "";
        GUIController.Instance.ShowLoginPanel();
    }
}
