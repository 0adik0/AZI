using System.Collections.Generic;
using Ucss;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GUIController : MonoBehaviour
{
    public MoneyPanel MoneyMenu;
    public Menu CurrentMenu;
    public Menu LoginPanel;
    public GameObject LobbiFon;
    public LobbiPanel LobbiPanel;
    public BottomMenu BottomPanel;
    public Menu RegPanel;
    public Menu ResetPswdPanel;
    public GameFon GameFon;
    public TableController GameTableController;
    public static GUIController Instance;
    public NewsPanel NewsPanel;
    public GameObject TestPanel;
    public Text RegedText;
    public Text OnlineText;
    public LogPanel LogP;
    public Menu EditProfilePanel;
    public Button SupportButton;
    public Menu SupportMenu;
    public HistoryPanel HistoryP;
    public TextPanel TextP;
    public GameObject LoadingText;
    public MoneyOutPanel MoneyOut;
    public MoneyInPanel MoneyIn;
    public BalancePanel BalanceP;
    public SelectSystemPanel SelectSystemP;
    public DebugPanel DebugPanel;
    public TimerPanel WaitingTimer;
    private Menu _parentMenu=null;
    public ExitPanel ExitPanel;
    public Menu QuestionMenu;
    public Text BalanceBottomText;
    public Image LobbyFon;
    public void OnDebugPanel()
    {
        print("OnDebugPanel");
        if (DebugPanel.LogGameObject.activeSelf)
            DebugPanel.OnClose();
        else
            DebugPanel.OnOpen();
    }
    public void ShowMoneyOut(ESystemType wm)
    {
        MoneyOut.Init(wm);
        ShowMenu(MoneyOut,SelectSystemP);
    }
    public static void ShowLoading()
    {
        Instance.LoadingText.gameObject.SetActive(true);
    }
    public static void HideLoading()
    {
        Instance.LoadingText.gameObject.SetActive(false);
    }

    private bool _inMainScreen = false;
    public void OnBackBtn()
    {
        if (!GameController.InGame)
        {
            if (_inMainScreen)
                ShowExitPanel();
            if (_parentMenu != null)
            {
                ShowMenu(_parentMenu);
                _parentMenu = null;
                
            }
            else
            {
                _inMainScreen = true;
                ShowMainScreen();
               
            }
            
        }
        else
        {
            TableController.Instance.ShowExitPanel();
        }
    }
    public void ShowMainScreen()
    {
        print("ShowMainScreen");
        ShowMenu(null);
        TopPanel.Instance.ShowMainMenu();
        BottomPanel.ShowMainScreen();
        BalanceP.Show();
        LobbyFon.gameObject.SetActive(true);
    }
    public void ShowExitPanel()
    {
       // ShowMenu(ExitPanel);
        ExitPanel.IsOpen = true;
    }
    public void Start()
    {
       // ShowLoginPanel();
        ShowMainScreen();
        Instance = this;
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        // BottomPanel.ShowMainScreen();
    }
    /// <summary>
    /// показываем панель с текстом правил
    /// </summary>
    public void ShowRuls()
    {
        TopPanel.Instance.HideMainMenu();
        BottomPanel.HideMainScreen();
        ShowMenu(TextP);
        TextP.ShowRuls();
        BalanceP.Hide();
    }
    public void ShowRulsIn()
    {
        TopPanel.Instance.HideMainMenu();
        BottomPanel.HideMainScreen();
        ShowMenu(TextP,MoneyMenu);
        TextP.ShowRulsIn();
        BalanceP.Hide();
    }
    public void ShowRulsOut()
    {
        TopPanel.Instance.HideMainMenu();
        BottomPanel.HideMainScreen();
        BalanceP.Hide();
        ShowMenu(TextP, MoneyMenu);
        TextP.ShowRulsOut();
    }
    public void ShowMoneyPanel()
    {
        if (!WaitingTimer.isActiveAndEnabled)
        {
            TopPanel.Instance.HideMainMenu();
            BalanceP.Show();
            BottomPanel.HideMainScreen();
//        MoneyMenu.BalanceText.text = GameController.Money.ToString();
            ShowMenu(MoneyMenu);
        }
    }
    public void ShowHistory()
    {
        BalanceP.Hide();
        HistoryP.Init();
        ShowMenu(HistoryP);
    }
    public void ShowSupportMenu()
    {
        TopPanel.Instance.HideMainMenu();
        BottomPanel.HideMainScreen();
    
        BalanceP.Hide();
        ShowMenu(SupportMenu);
    }
    public void ShowQuestion()
    {
        print("! " + SupportMenu);
        ShowMenu(QuestionMenu);
    }
    public void OnTestPanelBrn()
    {
        if (TestPanel.activeInHierarchy)
            TestPanel.SetActive(false);
        else
        {
            TestPanel.SetActive(true);
            TestPanel.SendMessage("Init");
        }
    }
    public void ShowMenu(Menu menu,Menu parentMenu =null)
    {
        LobbyFon.gameObject.SetActive(false);
        if (parentMenu != null)
            _parentMenu = parentMenu;
        else
            _parentMenu = null;
        if (CurrentMenu != null)
            CurrentMenu.IsOpen = false;
        if (menu != null)
        {
            CurrentMenu = menu;
            CurrentMenu.IsOpen = true;
            CurrentMenu.SendMessage("Init",SendMessageOptions.DontRequireReceiver);
            _inMainScreen = false;
        }
        else
        {
           Debug.LogWarning("нет активного меню");
        }


    }
    /// <summary>
    /// показываем панель редактирования профиля
    /// </summary>
    public void ShowEditProfile()
    {
       // LobbiPanel.InitPanel();
        if (GameController.Id > 0)
        {
            TopPanel.Instance.HideMainMenu();
            BottomPanel.HideMainScreen();
            ShowMenu(EditProfilePanel);
            BalanceP.Show();
            EditProfilePanel.gameObject.SendMessage("LoadInfo");
        }
    }
    /// <summary>
    /// показываем лобби
    /// </summary>
    public void StartLobbi()
    {
        if (CurrentMenu != null)
            CurrentMenu.IsOpen = false;
    }

    public void ShowResetPswd()
    {
        ShowMenu(ResetPswdPanel);
    }
    public void ShowRegPanel()
    {
        Net.SendDefaultAuth();
        ShowMenu(RegPanel);
    }
    public void ShowLoginPanel()
    {
        TopPanel.Instance.HideMainMenu();
        BottomPanel.HideMainScreen();
        if (GameController.Id != 0)
        {
            ShowMenu(LobbiPanel);
            BalanceP.Show();
        }
        else
        {
            BalanceP.Hide();
            ShowMenu(LoginPanel);
            LoginPanel.SendMessage("LoadPrefs");
        }

    }

    public void HideCurrentPanel()
    {
        if (CurrentMenu != null)
            CurrentMenu.IsOpen = false;
    }
    public void ShowLobbiPanel()
    {
       // DebugPanel.Log("33 " );
        LobbiPanel.InitPanel();
      //  DebugPanel.Log("333 ");
        ShowMenu(LobbiPanel);
      //  DebugPanel.Log("3333 ");
        BalanceP.Show();
    }

    public void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape)) {OnBackBtn(); }
    }
    public void StartGame()
    {
        ShowMenu(GameTableController);
        BottomPanel.HideMainScreen();
        BalanceP.Hide();
        SupportButton.gameObject.SetActive(false);
       // BottomPanel.LoginBtn.gameObject.SetActive(false);
    }

    public void ShowGameFon(string tip)
    {
        LobbiFon.SetActive(false);
        GameFon.ShowFon(tip);
    }

    public void EndGame()
    {
        if (GameTableController != null) GameTableController.gameObject.SetActive(false);
        BottomPanel.HideGame();
        ShowLobbiPanel();
        ShowLobbiFon();
        //SupportButton.gameObject.SetActive(true);
     //   BottomPanel.LoginBtn.gameObject.SetActive(true);
    }
    /// <summary>
    /// показываем текущий баланс
    /// </summary>
    public static void ShowBalance()
    {
        if (GameController.MoneyTip == 1)
        {
            // Instance.BalanceText.text = GameController.RealMoney.ToString() + "$";
        }
        else
        {
            //  Instance.BalanceText.text = GameController.VirtMoney.ToString() + "$";
        }
    }
    public void ShowLobbiFon()
    {
        LobbiFon.SetActive(true);
        // GameFon.HideFon();
    }
    public void ShowNews()
    {
        BalanceP.Hide();
        TopPanel.Instance.HideMainMenu();
        BottomPanel.HideMainScreen();
        NewsPanel.GetNews();
        ShowMenu(NewsPanel);
    }

    public static void ShowLog(List<string> log)
    {
        if (!Instance.LogP.gameObject.activeInHierarchy)
            Instance.LogP.gameObject.SetActive(true);
        Instance.LogP.ShowLog(log);
    }

    public void OnResetBD()
    {
        UCSS.HTTP.GetString(Net.ServerURL + "/api/default/refreshdb", new EventHandlerHTTPString(OnResetBD), new EventHandlerServiceError(Net.Instance.OnHTTPError), new EventHandlerServiceTimeOut(Net.Instance.OnHTTPTimeOut), 10);
    }

    private void OnResetBD(string text, string transactionid)
    {
        print("reset " + text);
    }

    /// <summary>
    /// показываем панель таймера 
    /// </summary>
    /// <param name="time"></param>
    public void ShowTimer(int time)
    {
        
        WaitingTimer.ShowTime(time);
    }
    public void CloseRestartP()
    {
        print("не обработан CloseRestartP");
    }
    /// <summary>
    /// показываем панель ввода денег
    /// </summary>
    /// <param name="tip"></param>
    public static void ShowMoneyIn(ESystemType tip)
    {
        Instance.MoneyIn.Init(tip);
        Instance.ShowMenu(Instance.MoneyIn);
    }
    /// <summary>
    /// показываем панель выбора платежной системы
    /// </summary>
    /// <param name="out_par"></param>
    public void ShowSelectSystem(bool out_par)
    {
        SelectSystemP.Out = out_par;
        ShowMenu(SelectSystemP,MoneyMenu);
    }
}
