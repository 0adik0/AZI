using System;
using System.Collections.Generic;
using System.Linq;
using CodeTitans.JSon;
using SmartLocalization;
using SmartLocalization.Editor;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public static LanguageManager LangManager;
    public static double _money = 0;

    public static double Money
    {
        get
        {
           // DebugPanel.Log("! " + Instance);
            if (Instance.BalanceText != null)
            {
               // DebugPanel.Log("! ");
                if (GameController.MoneyTip == 1)
                {
                    Instance.BalanceText.localizedKey = "Bottom.Balance";
                    Instance.BalanceText.GetComponent<Text>().text = LangManager.GetTextValue("Bottom.Balance");
                    //DebugPanel.Log("! ");
                    GUIController.Instance.BottomPanel.BalanceText.text = _money.ToString() + "$";
                    
                    //DebugPanel.Log("! ");
                }
                else
                {
                    Instance.BalanceText.localizedKey = "Bottom.Balance0";
                    Instance.BalanceText.GetComponent<Text>().text = LangManager.GetTextValue("Bottom.Balance0");
                    GUIController.Instance.BottomPanel.BalanceText.text = _money.ToString() + "$";
                   
                }
            }
            if (MoneyTip == 1)
                return RealMoney;
            else
                return VirtMoney;
            
        }
        set
        {
            _money = value;
            if (Instance.BalanceText != null)
            {
                if (MoneyTip == 1)
                {
                    Instance.BalanceText.localizedKey = "Bottom.Balance";
                    Instance.BalanceText.GetComponent<Text>().text = LangManager.GetTextValue("Bottom.Balance");
                    GUIController.Instance.BottomPanel.BalanceText.text = _money.ToString() + "$";
                    if (GUIController.Instance.BalanceBottomText != null)
                        GUIController.Instance.BalanceBottomText.text = _money.ToString() + "$";
                }
                else
                {
                    Instance.BalanceText.localizedKey = "Bottom.Balance0";
                    Instance.BalanceText.GetComponent<Text>().text = LangManager.GetTextValue("Bottom.Balance0");
                    GUIController.Instance.BottomPanel.BalanceText.text = _money.ToString() + "$";
                    if (GUIController.Instance.BalanceBottomText != null && InGame)
                        GUIController.Instance.BalanceBottomText.text = _money.ToString() + "$";
                    else if (GUIController.Instance.BalanceBottomText != null)
                    {
                        GUIController.Instance.BalanceBottomText.text = "";
                    }
                }
            }
        }
    }
    public static bool InGame = false;
    public static double VirtMoney { get; set; }
    public static double RealMoney { get; set; }

    /// <summary>
    /// массив идентификаторов карт
    /// </summary>
    public static Dictionary<int, Sprite> CardsID = new Dictionary<int, Sprite>();

    /// <summary>
    /// тип денег 1 реал 2 вирт
    /// </summary>
    public static int MoneyTip = 1;

    public static string Token = "";
    public static int Id=0;
    public string Version = "";
    /// <summary>
    /// время на ход 
    /// </summary>
    public static int TimeOut = 10;

    public static List<string> Log = new List<string>();

    /// <summary>
    /// комиссия за вывод средств
    /// </summary>
    public static double Komiss = 3;

    public static string Login="";
    public static string Pass="";
   // public static EAvatarType AvatarTip;
    public LocalizedText BalanceText;
    public static GameController Instance;
    public Text LatencyText;
    public static int TimeOutsCounter=0;

    /// <summary>
    /// загружаем карты
    /// </summary>
    public static void LoadCards()
    {
        Sprite[] sprites= new Sprite[28];
        sprites = Resources.LoadAll<Sprite>("GUI/карты");
        for (int i = 1; i <= 27; i++)
        {
            foreach (Sprite sprite in sprites)
            {
                if (sprite.name == i.ToString())
                {
                    CardsID.Add(i, sprite);
                }
            }
        }
        print("loaded " + sprites.Count());
    }

    public void LogOut()
    {
        print("LogOut");
        BalanceText.localizedKey = "Empty";
        Instance.BalanceText.GetComponent<Text>().text = "";
        Net.SendExit();
        GameController.InGame = false;
        Token = Pass = Login = "";
        Id = 0;
       // BottomMenu.HideLoginBtn();
        if (TableController.Instance != null)
        {
            print("end table");
            TableController.Instance.EndGame();
        }

        GUIController.Instance.ShowLoginPanel();
    }

    private void Awake()
    {
        Instance = this;
        LangManager = LanguageManager.Instance;
        LangManager.ChangeLanguage("ru");
        LoadCards();
        
    }

    private void Start()
    {
        const string projectId = "0f6d9da6-50c8-4365-b3a5-95b8cac6dd66";
        Application.targetFrameRate = 24;
        Instance = this;
        Latancy(null, new TimeSpan());
    }
    public static void Latancy(string url, TimeSpan timeSpan)
    {
        //print(timeSpan.TotalSeconds+":"+url);
        Instance.LatencyText.text = Instance.Version + " <color=#800000ff>TimeOuts:" + TimeOutsCounter + "</color> Ping:" + Math.Round(timeSpan.TotalMilliseconds).ToString();
    }
}
