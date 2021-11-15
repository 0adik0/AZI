using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using CodeTitans.JSon;
using Ucss;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Debug = UnityEngine.Debug;
using System.Diagnostics;
public class Net : MonoBehaviour
{
    //public static string ServerURL = "https://azi.my-green.ru";
    public static string ServerURL = "https://azigame.com";
    public static Net Instance;
    static IJSonMutableObject requestPar;
    private string _testPackNewGame = "{\"EVENT\":\"ROUND_1_TRADING\",\"current_round\":\"ROUND_1_TRADING\",\"is_spectator\":true,\"action_history_id\":778,\"move_history_id\":0,\"next_word_user_id\":8,\"timeout\":20,\"kon_sum\":0,\"money_left\":\"74.90\",\"trump_card_id\":3,\"players_data\":[{\"user_id\":7,\"cards_count\":3,\"opened_card_id\":0,\"wins\":0,\"is_away\":false,\"username\":\"user7\",\"avatar\":\"/img/avatars/user7.jpg\",\"flag\":\"/img/flags/af.png\",\"country_id\":1},{\"user_id\":8,\"cards_count\":3,\"opened_card_id\":0,\"wins\":0,\"is_away\":false,\"username\":\"user8\",\"avatar\":\"/img/avatars/user8.jpg\",\"flag\":\"/img/flags/af.png\",\"country_id\":1}],\"spectators\":[{\"user_id\":5,\"username\":\"user5\",\"avatar\":\"/img/avatars/user5.jpg\",\"flag\":\"/img/flags/af.png\",\"country_id\":1}],\"min_bet\":0.2,\"max_bet\":10,\"table_type_id\":1,\"disable_call_button\":true,\"disable_raise_button\":false,\"is_azi\":false}";
    private string _testPackNewGame1 = "{\"EVENT\":\"ROUND_3_RESULT\",\"action_history_id\":1193,\"move_history_id\":0,\"winner_uses_id\":10,\"game_winner_user_id\":0}";
    public GameObject DisconnectImage;
    private string _authPack =
        "{\"access_token\":\"QWl1ZGppcFRuQ0ZXMGNMaXcyak5UbG9QWWZnWndOYXA=\",\"user_id\":1,\"username\":\"user10\",\"real_money\":\"100.00\",\"virtual_money\":\"99.80\",\"current_money_cat_id\":2,\"avatar\":\"/img/avatars/user10.jpg\"}";

    private string _okMsg = "{\"status\": \"ok\"}";
    //игрок повысил ставку
    private string _raiseMsg0 =
       "{\"EVENT\":\"AZI_PREPARE\",\"action_history_id\":1194,\"move_history_id\":0,\"next_word_user_id\":0,\"timeout\":20,\"sum\":0}";

    private string _rm =
        "{\"EVENT\":\"ROUND_1_TRADING\",\"current_round\":\"ROUND_1_TRADING\",\"is_spectator\":false,\"action_history_id\":1195,\"move_history_id\":0,\"next_word_user_id\":10,\"timeout\":20,\"kon_sum\":0.6,\"money_left\":\"70.50\",\"trump_card_id\":6,\"players_data\":[{\"user_id\":7,\"cards_count\":3,\"opened_card_id\":0,\"wins\":0,\"sum\":\"70.50\",\"is_away\":false,\"username\":\"user7\",\"avatar\":\"/img/avatars/user7.jpg\",\"flag\":\"/img/flags/af.png\",\"country_id\":1,\"cards\":[25,19,12]},{\"user_id\":9,\"cards_count\":3,\"opened_card_id\":0,\"wins\":0,\"is_away\":false,\"username\":\"user9\",\"avatar\":\"/img/avatars/user9.jpg\",\"flag\":\"/img/flags/af.png\",\"country_id\":1},{\"user_id\":10,\"cards_count\":3,\"opened_card_id\":0,\"wins\":0,\"is_away\":false,\"username\":\"user10\",\"avatar\":\"/img/avatars/user10.jpg\",\"flag\":\"/img/flags/af.png\",\"country_id\":1}],\"spectators\":[],\"min_bet\":0.2,\"max_bet\":10,\"table_type_id\":1,\"disable_call_button\":true,\"disable_raise_button\":false,\"is_azi\":true}";
    private string _raiseMsg =
        "{\"EVENT\":\"PLAYER_RAISE\",\"action_history_id\":1,\"move_history_id\":2,\"next_word_user_id\":5,\"user_id\":4,\"sum\":\"0.75\",\"timeout\":30}";
    //игрок уравнял ставку
    private string _callMsg =
        "{\"EVENT\":\"PLAYER_CALL\",\"action_history_id\":1,\"move_history_id\":3,\"next_word_user_id\":6,\"user_id\":5,\"sum\":\"0.75\",\"timeout\":30}";
    //игрок сбросил карты
    private string _passMsg =
        "{\"EVENT\":\"PLAYER_FOLD\",\"action_history_id\":1,\"move_history_id\":4,\"next_word_user_id\":123,\"user_id\":6,\"timeout\":30}";
    private string _callMsg2 =
      "{\"EVENT\":\"PLAYER_CALL\",\"action_history_id\":1,\"move_history_id\":5,\"next_word_user_id\":3,\"user_id\":123,\"sum\":\"0.75\",\"timeout\":30}";
    private string _callMsg3 =
      "{\"EVENT\":\"PLAYER_CALL\",\"action_history_id\":1,\"move_history_id\":6,\"next_word_user_id\":4,\"user_id\":3,\"sum\":\"0.75\",\"timeout\":30}";
    //начало 1го круга вскрытия карт
    private string _showCard1 =
        "{\"EVENT\":\"ROUND_1_SHOWCARD\",\"action_history_id\":2,\"move_history_id\":0,\"next_word_user_id\":123,\"timeout\":30}";
    //начало 2го круга вскрытия карт
    private string _showCard2 =
        "{\"EVENT\":\"ROUND_2_SHOWCARD\",\"action_history_id\":2,\"move_history_id\":0,\"next_word_user_id\":3,\"timeout\":30}";
    //начало 3го круга вскрытия карт
    private string _showCard3 =
        "{\"EVENT\":\"ROUND_3_SHOWCARD\",\"action_history_id\":2,\"move_history_id\":0,\"next_word_user_id\":3,\"timeout\":30}";
    //игрок показал карту
    private string _openCard1 =
        "{\"EVENT\":\"PLAYER_SHOW_CARD\",\"action_history_id\":2,\"move_history_id\":1,\"next_word_user_id\":3,\"user_id\":123,\"card_id\":1,\"timeout\":30}";
    private string _openCard2 =
       "{\"EVENT\":\"PLAYER_SHOW_CARD\",\"action_history_id\":2,\"move_history_id\":2,\"next_word_user_id\":4,\"user_id\":3,\"card_id\":2,\"timeout\":30}";
    private string _openCard3 =
       "{\"EVENT\":\"PLAYER_SHOW_CARD\",\"action_history_id\":2,\"move_history_id\":3,\"next_word_user_id\":5,\"user_id\":4,\"card_id\":3,\"timeout\":30}";
    private string _openCard4 =
       "{\"EVENT\":\"PLAYER_SHOW_CARD\",\"action_history_id\":2,\"move_history_id\":4,\"next_word_user_id\":123,\"user_id\":5,\"card_id\":4,\"timeout\":30}";

    private string _result1 = "{\"EVENT\":\"ROUND_1_RESULT\",\"action_history_id\":3,\"move_history_id\":0,\"winner_uses_id\":123}";
    private string _result2 = "{\"EVENT\":\"ROUND_2_RESULT\",\"action_history_id\":3,\"move_history_id\":0,\"winner_uses_id\":3}";
    //результаты 3го круга и победителя игры
    //либо 0, если ничья, тогда играется АЗИ раунд
    private string _result3 = "{\"EVENT\":\"ROUND_3_RESULT\",\"action_history_id\":7,\"move_history_id\":0,\"winner_uses_id\":123,\"game_winner_user_id\":0}";
    //раунд ставок для участия в АЗИ
    //генерится когда никто из игроков не взял более 1й взятки
    private string _azi =
        "{\"EVENT\":\"AZI_PREPARE\",\"action_history_id\":8,\"move_history_id\":0,\"next_word_user_id\":3,\"sum\":300,\"timeout\":30}";

    private string _asiStart =
        "{\"EVENT\":\"PLAYER_AZI_ENTER\",\"action_history_id\":8,\"move_history_id\":17,\"next_word_user_id\":4,\"user_id\":3,\"sum\":\"1.50\",\"card_id\":null,\"timeout\":30,\"players_data\":[{\"user_id\":1,\"sum\":\"10.00\"},{\"user_id\":2,\"sum\":\"20.00\"},{\"user_id\":3,\"sum\":\"30.00\"},{\"user_id\":4,\"sum\":\"40.00\"}]}";

    private string _tableFound = "{\"EVENT\":\"TABLE_FOUND_CHECK_STATUS\"}";
    public bool LocalTest = true;
    public TableController GameTableController;
    public static int ActionHistoryId = 0;
    private int _nomPack = 0;
    private static int MoveHistoryId;
    private float _checkStatusInterval = 1f;
    /// <summary>
    /// если false то не отправляем проверку стола
    /// </summary>
    public bool Flag = true;



    public void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        if (LocalTest)
        {
            // Invoke("TestStart",1f);
        }
        //LoadUsersInfo();
    }

    public void LoadUsersInfo()
    {

        //WWWForm data = new WWWForm();
        //data.AddField("username", "admin1");
        // UCSS.HTTP.PostForm(ServerURL + "/api/default/getregistered", data, new EventHandlerHTTPString(OnReged), new EventHandlerServiceError(Instance.OnHTTPError), new EventHandlerServiceTimeOut(Instance.OnHTTPTimeOut), 10);
        //UCSS.HTTP.PostForm(ServerURL + "/api/default/getonline", data, new EventHandlerHTTPString(OnOnline), new EventHandlerServiceError(Instance.OnHTTPError), new EventHandlerServiceTimeOut(Instance.OnHTTPTimeOut), 10);
    }
    private static void OnReged(string text, string transactionid)
    {
        IJSonMutableObject obj = ReadPack(text);
        if (obj != null)
        {
            GUIController.Instance.RegedText.text = obj["count"].StringValue;
        }
    }
    private static void OnOnline(string text, string transactionid)
    {
        print(text);
        IJSonMutableObject obj = ReadPack(text);
        if (obj != null)
        {
            int i = obj["beginner"].Int32Value + obj["pro"].Int32Value + obj["vip"].Int32Value;
            GUIController.Instance.OnlineText.text = i.ToString();
        }
    }
    void TestStart()
    {
        GameController.Token = "asd";
        GameController.Id = 3;
        GUIController.Instance.StartGame();
        //       GUIController.Instance.BottomPanel.ShowGame();
        GameTableController.NewPack(ReadPack(_testPackNewGame));
    }
    public void SendResetPswd(Text email)
    {

    }
    /// <summary>
    /// отправляем изначальный запрос реги, для работы до авторизации
    /// </summary>
    public static void SendDefaultAuth()
    {
        WWWForm data = new WWWForm();
        data.AddField("username", "admin1");
        data.AddField("password", "admin1");
        UCSS.HTTP.PostForm(ServerURL + "/api/default/auth", data, new EventHandlerHTTPString(OnDefaultAuth), new EventHandlerServiceError(Instance.OnHTTPError), new EventHandlerServiceTimeOut(Instance.OnHTTPTimeOut), 10);
    }

    public static void OnDefaultAuth(string text, string transactionid)
    {
        IJSonMutableObject obj = ReadPack(text);
        if (obj != null)
        {
            GameController.Token = obj["access_token"].StringValue;
            print("GameController.Token " + GameController.Token);
        }
    }


    public static IJSonMutableObject ReadPack(string str)
    {
        IJSonMutableObject obj = null;

        try
        {
            JSonReader reader = new JSonReader();
            obj = reader.ReadAsJSonMutableObject(str);


            if (obj.Contains("action_history_id"))
            {
                ActionHistoryId = obj["action_history_id"].Int32Value;
                MoveHistoryId = obj["move_history_id"].Int32Value;
            }
        }
        catch (Exception e)
        {
            Debug.LogWarning("[NET] Cant parse:" + str);
        }
        return obj;
    }

    public void OnHTTPError(string error, string transactionId)
    {
        if (error.Substring(0, 3) == "401" && !LocalTest)
        {
            print("logout");
            GameController.Id = 0;
            GameController.Token = "";
            GUIController.Instance.ShowLoginPanel();
        }

        if (GameController.InGame)
            StartCheckTable(_checkStatusInterval);
        else
        {
            GUIController.Instance.LoginPanel.gameObject.SendMessage("WrongLogin");
        }
        DebugPanel.Log("[OnHTTPError] error = " + error + UCSS.HTTP.GetTransaction(transactionId).data);
        Debug.LogWarning("[OnHTTPError] error = " + error + UCSS.HTTP.GetTransaction(transactionId).data);
        DisconnectImage.SetActive(true);
    }


    public void OnHTTPErrorAuth(string error, string transactionId)
    {
        GUIController.Instance.LoginPanel.gameObject.SendMessage("WrongLogin");
    }

    public void LogStack()
    {
        StackTrace stackTrace = new StackTrace();           // get call stack
        StackFrame[] stackFrames = stackTrace.GetFrames();  // get method calls (frames)

        // write call stack method names
        foreach (StackFrame stackFrame in stackFrames)
        {
            DebugPanel.Log(stackFrame.GetMethod().Name);   // write method name
        }
    }
    public void OnHTTPTimeOut(string transactionId)
    {
        if (GameController.InGame)
            StartCheckTable(_checkStatusInterval);
        else
        {
            GUIController.Instance.LoginPanel.gameObject.SendMessage("WrongLogin");
        }
        DisconnectImage.SetActive(true);
        LogStack();
        DebugPanel.Log("[OnHTTPTimeOut] transactionId = " + UCSS.HTTP.GetTransaction(transactionId).data);
        Debug.LogWarning("[OnHTTPTimeOut] transactionId = " + UCSS.HTTP.GetTransaction(transactionId).data);
        UCSS.HTTP.RemoveTransaction(transactionId);

    }
    /// <summary>
    /// стандартная авторизация
    /// </summary>
    /// <param name="login"></param>
    /// <param name="pass"></param>
    public static void Auth(string login, string pass)
    {

        if (Instance.LocalTest)
        {
            OnAuth(Instance._authPack, "123");
            Instance.CheckTable();
        }
        else
        {
            WWWForm data = new WWWForm();
            print(login + ":" + pass);
            data.AddField("username", login);
            data.AddField("password", pass);
            UCSS.HTTP.PostForm(ServerURL + "/api/default/auth", data, new EventHandlerHTTPString(OnAuth),
                new EventHandlerServiceError(Instance.OnHTTPErrorAuth),
                new EventHandlerServiceTimeOut(Instance.OnHTTPTimeOut), 10);
        }
    }
    public static void StartCheckBalance()
    {
        //print("StartCheckBalance " + GameController.Id);
        if (GameController.Id > 0)
        {
            Instance.InvokeRepeating("CheckBalance", 0f, 10f);
        }
    }
    public void CheckBalance()
    {
        //   print("CheckBalance " + GameController.Id);
        if (GameController.Id > 0)
        {
            WWWForm data = new WWWForm();
            data.AddField("event", "1");
            UCSS.HTTP.PostForm(ServerURL + "/api/default/getbalance?access-token=" + GameController.Token, data,
                new EventHandlerHTTPString(OnBalance),
                new EventHandlerServiceError(Instance.OnHTTPError),
                new EventHandlerServiceTimeOut(Instance.OnHTTPTimeOut), 10);
        }
        else
        {
            CancelInvoke("CheckBalance");
        }
    }
    /// <summary>
    /// ОбНОВЛЯЕМ бАЛАНС 
    /// </summary>
    /// <param name="text"></param>
    /// <param name="transactionid"></param>
    private void OnBalance(string text, string transactionid)
    {
        //   print("OnBalance "+text);
        IJSonMutableObject obj = ReadPack(text);
        if (obj != null)
        {
            print("OnBalance " + obj["virtual_money"].DoubleValue);
            GameController.MoneyTip = obj["current_money_cat_id"].Int32Value;
            GameController.RealMoney = obj["real_money"].DoubleValue;
            GameController.VirtMoney = obj["virtual_money"].DoubleValue;
            BalancePanel.UpdateBalance();
        }
    }
    private static void OnAuth(string text, string transactionid)
    {
        //Instance.LogStack();
        // DebugPanel.Log(text);
        IJSonMutableObject obj = ReadPack(text);

        if (obj != null)
        {
            if (obj.Contains("access_token"))
            {
                DebugPanel.Log("1 " + obj["user_id"].StringValue);
                GameController.Token = obj["access_token"].StringValue;
                GameController.Id = obj["user_id"].Int32Value;
                GameController.MoneyTip = obj["current_money_cat_id"].Int32Value;
                GameController.RealMoney = obj["real_money"].DoubleValue;
                GameController.VirtMoney = obj["virtual_money"].DoubleValue;
                GameController.Login = obj["username"].StringValue;
                print("Получили токен " + obj["access_token"].StringValue + ":" + GameController.RealMoney + ":" +
                      GameController.VirtMoney);
                //    DebugPanel.Log("2 " + GUIController.Instance);
                GUIController.Instance.ShowLobbiPanel();
                //   DebugPanel.Log("3 " + obj["user_id"].StringValue);
                BottomMenu.ShowLoginBtn(obj["username"].StringValue);
                //  DebugPanel.Log("4 " + obj["user_id"].StringValue);
                BalancePanel.UpdateBalance();
                // DebugPanel.Log("5 " + obj["user_id"].StringValue);
                StartCheckBalance();
                // DebugPanel.Log("6 " + obj["user_id"].StringValue);
            }
            else
            {
                GUIController.Instance.LoginPanel.SendMessage("WrongLogin");
            }
        }
        else
        {
            DebugPanel.Log("4");
        }
    }
    public static void GetBalance()
    {
        if (Instance.LocalTest)
        {
        }
        else
        {
            WWWForm data = new WWWForm();
            data.AddField("id", GameController.Id);
            UCSS.HTTP.PostForm(ServerURL + "/api/default/getbalance?access-token=" + GameController.Token, data, new EventHandlerHTTPString(OnGetBalance),
                new EventHandlerServiceError(Instance.OnHTTPErrorAuth),
                new EventHandlerServiceTimeOut(Instance.OnHTTPTimeOut), 10);
        }
    }

    private static void OnGetBalance(string text, string transactionid)
    {
        IJSonMutableObject obj = ReadPack(text);
        print(text);
        if (obj != null)
        {
            GameController.RealMoney = obj["real_money"].DoubleValue;
            GameController.VirtMoney = obj["virtual_money"].DoubleValue;
            if (obj["current_money_cat_id"].Int32Value == 1)
                GameController.Money = GameController.RealMoney;
            else
                GameController.Money = GameController.VirtMoney;
        }
    }

    /// <summary>
    /// встаем в очередь на игру
    /// </summary>
    /// <param name="tip"></param>
    public static
    void JoinQueue(ETableTip tip)
    {
        print("JoinQueue " + tip);
        if (Instance.LocalTest)
        {
            OnJoinQueue(Instance._tableFound, "123");
        }
        else
        {
            WWWForm data = new WWWForm();
            data.AddField("event", "FIND_TABLE");
            switch (tip)
            {
                case ETableTip.Noob:
                    data.AddField("cat_id", 1);
                    break;
                case ETableTip.Prof:
                    data.AddField("cat_id", 2);
                    break;
                case ETableTip.Vip:
                    data.AddField("cat_id", 3);
                    break;
            }
            data.AddField("money_cat_id", GameController.MoneyTip);
            UCSS.HTTP.PostForm(ServerURL + "/api/users/play?access-token=" + GameController.Token, data, new EventHandlerHTTPString(OnJoinQueue),
                new EventHandlerServiceError(Instance.OnHTTPError),
                new EventHandlerServiceTimeOut(Instance.OnHTTPTimeOut), 10);
        }
    }
    private static void OnJoinQueue(string text, string transactionid)
    {
        print(text);
        IJSonMutableObject obj = ReadPack(text);
        if (obj != null)
        {
            if (obj.Contains("EVENT"))
            {
                print("Ищем стол");
                GUIController.Instance.HideCurrentPanel();
                Instance.StartCheckTable(Instance._checkStatusInterval);
            }
            else
            {
                if (obj.Contains("error"))
                {
                    switch (obj["error"].StringValue)
                    {
                        case "All tables are full":
                            GUIController.Instance.LobbiPanel.SendMessage("Error", GameController.LangManager.GetTextValue("Lobbi.AllFull"));
                            break;
                        default:
                            GUIController.Instance.LobbiPanel.SendMessage("Error", GameController.LangManager.GetTextValue("Lobbi.CatNotFound"));
                            break;
                    }
                }
                GUIController.Instance.LobbiPanel.SendMessage("WrongLogin");
            }
        }
    }
    /// <summary>
    /// начинаем ждать создание стола
    /// </summary>
    public void StartCheckTable(float starttime = 1f)
    {
        GameController.InGame = true;
        // print("StartCheckTable " + Flag);
        CancelInvoke("CheckTable");
        CancelInvoke("CheckTable");
        Invoke("CheckTable", starttime);
        Invoke("CheckTable", starttime + 10f);
        // CancelInvoke("CheckTable");
        //InvokeRepeating("CheckTable", starttime, 2f);
        //InvokeRepeating("CheckTable", starttime, 3f);
    }
    /// <summary>
    /// проверяем готов ли стол для игры
    /// </summary>
    private void CheckTable()
    {
        //Debug.LogError("CheckTable");
        //        print("CheckTable");
        CancelInvoke("CheckTable");
        if (Instance.LocalTest)
        {
            // print("pack "+_nomPack);
            switch (_nomPack)
            {
                case 0:
                    OnCheckTable(Instance._testPackNewGame, "123");
                    // OnCheckTable(Instance._rm, "123");
                    break;
                case 1:
                    //OnCheckTable(Instance._okMsg, "123");
                    //OnCheckTable(Instance._raiseMsg, "123");
                    break;
                case 2:
                    OnCheckTable(Instance._testPackNewGame1, "123");
                    break;
                case 3:
                    OnCheckTable(Instance._raiseMsg0, "123");
                    break;
                case 4:
                    OnCheckTable(Instance._rm, "123");
                    break;
                case 5:
                    OnCheckTable(Instance._okMsg, "123");
                    break;
                case 6:
                    //начали открывать
                    OnCheckTable(Instance._showCard1, "123");
                    break;
                case 7:
                    OnCheckTable(Instance._openCard1, "123");
                    break;
                case 8:
                    OnCheckTable(Instance._openCard2, "123");
                    break;
                case 9:
                    OnCheckTable(Instance._openCard3, "123");
                    break;
                case 10:
                    OnCheckTable(Instance._openCard4, "123");
                    break;
                case 11:
                    OnCheckTable(Instance._result1, "123");
                    break;
                case 12:
                    OnCheckTable(Instance._result3, "123");
                    break;
                case 13:
                    OnCheckTable(Instance._azi, "123");
                    break;
                case 14:
                    OnCheckTable(Instance._testPackNewGame1, "123");
                    break;

            }
            _nomPack++;

        }
        else
        {
            WWWForm data = new WWWForm();
            data.AddField("event", "CHECK_STATUS");
            //  print("CHECK_STATUS " + GameTableController.LastActionId + ":" + GameTableController.LastMoveId);
            data.AddField("action_history_id", GameTableController.LastActionId);
            data.AddField("move_history_id", GameTableController.LastMoveId);
            UCSS.HTTP.PostForm(ServerURL + "/api/users/play?access-token=" + GameController.Token, data, new EventHandlerHTTPString(OnCheckTable),
                new EventHandlerServiceError(Instance.OnHTTPError),
                new EventHandlerServiceTimeOut(Instance.OnHTTPTimeOut), 10);
        }
    }
    private void OnCheckTable(string text, string transactionid)
    {
        IJSonMutableObject obj = ReadPack(text);
        GameController.Log.Add(text);

        //if (obj.Contains("EVENT"))
        //    if (obj["EVENT"].StringValue != "WAITING_FOR_PLAYERS" && obj["EVENT"].StringValue != "WAITING_FOR_PLAYER_MOVE" && obj["EVENT"].StringValue != "WAITING_FOR_GAME_END")
        print(text);
        if (obj != null)
        {
            if (obj["EVENT"].StringValue == "WAITING_FOR_PLAYERS")
            {
                if (obj.Contains("timer_is_running"))
                {
                    if (obj["timer_is_running"].BooleanValue)
                    {
                        GUIController.Instance.ShowTimer(obj["seconds_left"].Int32Value);
                    }
                    else
                    {
                        GUIController.Instance.ShowTimer(0);
                    }
                }
                else
                {
                    GUIController.Instance.ShowTimer(0);
                }
            }
            if (obj["EVENT"].StringValue != "USER_IS_NOT_ONLINE")
                StartCheckTable(_checkStatusInterval);
            else
            {
                Instance.CancelInvoke("CheckTable");
                Invoke("DoEndGame", 5f);
            }
            /*if (!obj.Contains("game_winner_user_id"))
            {

               
            }
            else
            {
                if (obj["game_winner_user_id"].Int32Value == 0)
                    Invoke("CheckTable", 1f);
            }*/
            if (obj.Contains("EVENT"))
            {
                if (obj["EVENT"].StringValue != "WAITING_FOR_PLAYER_MOVE")
                {
                    //Instance.CancelInvoke("CheckTable");
                    Instance.GameTableController.NewPack(obj);
                }
            }
            else
            {
                Debug.LogWarning("В ответе нет event " + text);
            }
        }
        else
        {
            StartCheckTable(_checkStatusInterval);
        }

    }

    private void DoEndGame()
    {
        if (TableController.Instance != null)
            TableController.Instance.EndGame();
        if (GUIController.Instance != null)
            GUIController.Instance.CloseRestartP();
        if (GUIController.Instance != null) GUIController.Instance.ShowLobbiPanel();
    }
    /// <summary>
    /// отправляем пасс
    /// </summary>
    public static void SendPass()
    {
        print("[NET]SendPass");
        if (Instance.LocalTest)
        {

        }
        else
        {
            WWWForm data = new WWWForm();
            data.AddField("event", "PLAYER_FOLD");
            UCSS.HTTP.PostForm(Net.ServerURL + "/api/users/play?access-token=" + GameController.Token, data,
                new EventHandlerHTTPString(Instance.PassAnsw),
                new EventHandlerServiceError(Instance.OnHTTPError),
                new EventHandlerServiceTimeOut(Instance.OnHTTPTimeOut), 10);
        }
    }
    private void PassAnsw(string text, string transactionid)
    {
        IJSonMutableObject obj = Net.ReadPack(text);
        if (obj != null)
        {
            if (obj.Contains("status"))
            {
                if (obj["status"].StringValue != "ok" || obj["status"].StringValue != "new_round_generated")
                    Debug.LogWarning(text);
            }
            else
            {
                Debug.LogWarning(text);
            }
        }
        CheckTable();
    }
    /// <summary>
    /// отправляем уравнять ставку
    /// </summary>
    public static void SendEqualize()
    {
        print("[NET]SendEqualize");
        WWWForm data = new WWWForm();
        data.AddField("event", "PLAYER_CALL");
        UCSS.HTTP.PostForm(Net.ServerURL + "/api/users/play?access-token=" + GameController.Token, data, new EventHandlerHTTPString(Instance.CallAnsw),
            new EventHandlerServiceError(Instance.OnHTTPError),
            new EventHandlerServiceTimeOut(Instance.OnHTTPTimeOut), 10);
    }
    private void CallAnsw(string text, string transactionid)
    {
        IJSonMutableObject obj = Net.ReadPack(text);
        if (obj != null)
        {
            if (obj.Contains("status"))
            {
                if (obj["status"].StringValue != "ok" || obj["status"].StringValue != "new_round_generated")
                    Debug.LogWarning(text);
            }
            else
            {
                Debug.LogWarning(text);
            }
        }
        CheckTable();
    }

    public void OnDestroy()
    {
        //SendExit();
    }

    public void OnApplicationQuit()
    {
       // SendExit();
    }
    /// <summary>
    /// Повышаем ставку на столе
    /// </summary>
    /// <param name="val"></param>
    public static void IncreaseBet(double val)
    {
        print("[NET]IncreaseBet " + val);
        if (Instance.LocalTest)
        {
            TableController.Instance.LastMoveId++;
            OnIncreaseBet(Instance._okMsg, "123");
        }
        else
        {
            WWWForm data = new WWWForm();
            data.AddField("event", "PLAYER_RAISE");
            data.AddField("sum", val.ToString());
            UCSS.HTTP.PostForm(ServerURL + "/api/users/play?access-token=" + GameController.Token, data, new EventHandlerHTTPString(OnIncreaseBet),
                new EventHandlerServiceError(Instance.OnHTTPError),
                new EventHandlerServiceTimeOut(Instance.OnHTTPTimeOut), 10);
        }
    }
    private static void OnIncreaseBet(string text, string transactionid)
    {
        IJSonMutableObject obj = ReadPack(text);
        if (obj != null)
        {
            if (obj.Contains("status"))
            {
                if (obj["status"].StringValue != "ok")
                    Debug.LogWarning(text);
            }
            else
            {
                Debug.LogWarning(text);
            }
        }

    }
    /// <summary>
    /// открываем карту 
    /// </summary>
    /// <param name="id"></param>
    public static void OpenCard(int id)
    {
        print("[NET]OpenCard " + id);
        if (Instance.LocalTest)
        {
            OnOpenCard(Instance._okMsg, "123");
        }
        else
        {
            WWWForm data = new WWWForm();
            data.AddField("event", "PLAYER_SHOW_CARD");
            data.AddField("card_id", id);
            UCSS.HTTP.PostForm(ServerURL + "/api/users/play?access-token=" + GameController.Token, data, new EventHandlerHTTPString(OnOpenCard),
                new EventHandlerServiceError(Instance.OnHTTPError),
                new EventHandlerServiceTimeOut(Instance.OnHTTPTimeOut), 10);
        }

    }
    private static void OnOpenCard(string text, string transactionid)
    {

        IJSonMutableObject obj = ReadPack(text);
        if (obj != null)
        {
            if (obj.Contains("status"))
            {
                if (obj["status"].StringValue != "ok")
                    Debug.LogWarning(text);

            }
            else
            {
                Debug.LogWarning(text);
            }
        }
        Instance.CheckTable();
    }

    public static void SendAziAnswer(bool flag)
    {
        print("[NET]SendAziAnswer " + flag);
        if (Instance.LocalTest)
        {
            OnAziAnswer(Instance._okMsg, "123");
        }
        else
        {
            WWWForm data = new WWWForm();
            if (flag)
                data.AddField("event", "PLAYER_AZI_ENTER");
            else
                data.AddField("event", "PLAYER_AZI_EXIT");
            UCSS.HTTP.PostForm(ServerURL + "/api/users/play?access-token=" + GameController.Token, data, new EventHandlerHTTPString(OnAziAnswer),
                new EventHandlerServiceError(Instance.OnHTTPError),
                new EventHandlerServiceTimeOut(Instance.OnHTTPTimeOut), 10);
        }
    }
    private static void OnAziAnswer(string text, string transactionid)
    {
        IJSonMutableObject obj = ReadPack(text);
        if (obj != null)
        {
            if (obj.Contains("status"))
            {
                if (obj["status"].StringValue != "ok")
                    Debug.LogWarning(text);

            }
            else
            {
                Debug.LogWarning(text);
            }
        }

    }

    public static void SendExit()
    {

        print("[NET]SendExit ");
        if (Instance.LocalTest)
        {
            OnExit(Instance._okMsg, "123");
        }
        else
        {
            WWWForm data = new WWWForm();
            data.AddField("event", "PLAYER_EXIT");
            UCSS.HTTP.PostForm(ServerURL + "/api/users/play?access-token=" + GameController.Token, data, new EventHandlerHTTPString(OnExit),
                new EventHandlerServiceError(Instance.OnHTTPError),
                new EventHandlerServiceTimeOut(Instance.OnHTTPTimeOut), 10);
        }
        Instance.CancelInvoke("CheckTable");
    }
    private static void OnExit(string text, string transactionid)
    {
        IJSonMutableObject obj = ReadPack(text);
        if (obj != null)
        {
            if (obj.Contains("status"))
            {
                if (obj["status"].StringValue != "ok")
                    Debug.LogWarning(text);

            }
            else
            {
                Debug.LogWarning(text);
            }
        }
        //если игрок не вышел из игры
        //if (TableController.Instance.gameObject.activeSelf)
        //    Instance.StartCheckTable(3f);

    }
    /// <summary>
    /// отправляем запрос на сброс пароля
    /// </summary>
    /// <param name="email"></param>
    public static void ResetPswd(string email)
    {
        WWWForm data = new WWWForm();
        data.AddField("email", email);
        UCSS.HTTP.PostForm(ServerURL + "/api/default/forgotpassword", data, new EventHandlerHTTPString(OnResetPswd),
            new EventHandlerServiceError(Instance.OnHTTPError),
            new EventHandlerServiceTimeOut(Instance.OnHTTPTimeOut), 10);
    }
    private static void OnResetPswd(string text, string transactionid)
    {
        IJSonMutableObject obj = ReadPack(text);
        if (obj != null)
        {
            if (obj.Contains("status"))
            {
                if (obj["status"].StringValue != "ok")
                    Debug.LogWarning(text);

            }
            else
            {
                Debug.LogWarning(text);
            }
        }
        //если игрок не вышел из игры
        //if (TableController.Instance.gameObject.activeSelf)
        //    Instance.StartCheckTable(3f);

    }
    /// <summary>
    /// шлем запрос админу
    /// </summary>
    /// <param name="email"></param>
    /// <param name="msg"></param>
    public static void SendSupport(string email, string msg)
    {

        WWWForm data = new WWWForm();
        data.AddField("message", msg);
        data.AddField("address", email);
        UCSS.HTTP.PostForm(ServerURL + "/api/default/support", data, new EventHandlerHTTPString(OnSendSupport),
            new EventHandlerServiceError(Instance.OnHTTPError),
            new EventHandlerServiceTimeOut(Instance.OnHTTPTimeOut), 10);
    }
    private static void OnSendSupport(string text, string transactionid)
    {
        IJSonMutableObject obj = ReadPack(text);
        if (obj != null)
        {
            if (obj.Contains("status"))
            {
                if (obj["status"].StringValue != "ok")
                    Debug.LogWarning(text);

            }
            else
            {
                Debug.LogWarning(text);
            }
        }
        //если игрок не вышел из игры
        //if (TableController.Instance.gameObject.activeSelf)
        //    Instance.StartCheckTable(3f);

    }
}
