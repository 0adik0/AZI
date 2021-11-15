using System.Collections.Generic;
using CodeTitans.JSon;
using Ucss;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TestUser : MonoBehaviour
{
    public Button AuthButton;
    public Button SearchButton;
    public Button RiseButton;
    public Button PassButton;
    public Button CallButton;
    public Button YesButton;
    public Button NoButton;
    public List<Button> Cards;
    public Image Fon;
    public Text MsgText;
    public string Login;
    public string Pass;
    private string _token;
    private int _id = 0;
    private int _lastActId = 0;
    private int _lastMoveId = 0;
    private CanvasGroup _canvasGroup;

    public List<string> Log;
    private float _minBet;
    private float _maxBet;

    public void OnAuth()
    {
        CancelInvoke("CheckTable");
        WWWForm data = new WWWForm();
        data.AddField("username", Login);
        data.AddField("password", Pass);
        UCSS.HTTP.PostForm(Net.ServerURL + "/api/default/auth", data, new EventHandlerHTTPString(OnAuth), new EventHandlerServiceError(OnHTTPError), new EventHandlerServiceTimeOut(OnHTTPTimeOut), 10);
        Log.Add("OUT AUTH");
    }
    // Use this for initialization
    void Start()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
        SetStage(0);
        Log= new List<string>();
    }

    public void SetStage(int stage)
    {
        switch (stage)
        {
            case 0:
                //авторизация
                _canvasGroup.interactable = true;
                //CancelInvoke("CheckTable");
                SearchButton.gameObject.SetActive(false);
                RiseButton.gameObject.SetActive(false);
                PassButton.gameObject.SetActive(false);
                CallButton.gameObject.SetActive(false);
                YesButton.gameObject.SetActive(false);
                NoButton.gameObject.SetActive(false);
                foreach (Button button in Cards)
                {
                    button.gameObject.SetActive(false);
                }
                AuthButton.gameObject.SetActive(true);
                break;
            case 1:
                //прошли авторизацию
                AuthButton.gameObject.SetActive(false);
                //SearchButton.gameObject.SetActive(true);
                MsgText.text = "Ищем стол";
                SendSearch();
                break;
            case 2:
                //ищем стол
                // AuthButton.gameObject.SetActive(false);
                //SearchButton.gameObject.SetActive(true);
                StartCheckTable();
                SearchButton.gameObject.SetActive(false);
                 YesButton.gameObject.SetActive(false);
                NoButton.gameObject.SetActive(false);
                break;
            case 3:
                //этап торгов
                MsgText.text = "";
                SearchButton.gameObject.SetActive(false);
                AuthButton.gameObject.SetActive(false);
                RiseButton.gameObject.SetActive(true);
                PassButton.gameObject.SetActive(true);
                CallButton.gameObject.SetActive(true);
                YesButton.gameObject.SetActive(false);
                NoButton.gameObject.SetActive(false);
                break;
            case 4:
                //этап открытия карт
                RiseButton.gameObject.SetActive(false);
                PassButton.gameObject.SetActive(false);
                CallButton.gameObject.SetActive(false);
                break;
            case 5:
                //вопрос на участие в ази
                YesButton.gameObject.SetActive(true);
                NoButton.gameObject.SetActive(true);
                break;
        }
    }

    public void OnCard1()
    {
        Log.Add("OUT PLAYER_SHOW_CARD " + Cards[0].name);
        WWWForm data = new WWWForm();
        data.AddField("event", "PLAYER_SHOW_CARD");
        data.AddField("card_id", Cards[0].name);
        UCSS.HTTP.PostForm(Net.ServerURL + "/api/users/play?access-token=" + _token, data, new EventHandlerHTTPString(OnOpenCard),
            new EventHandlerServiceError(OnHTTPError),
            new EventHandlerServiceTimeOut(OnHTTPTimeOut), 10);
        Cards[0].gameObject.SetActive(false);
    }
    public void OnCard2()
    {
        Log.Add("OUT PLAYER_SHOW_CARD " + Cards[1].name);
        WWWForm data = new WWWForm();
        data.AddField("event", "PLAYER_SHOW_CARD");
        data.AddField("card_id", Cards[1].name);
        UCSS.HTTP.PostForm(Net.ServerURL + "/api/users/play?access-token=" + _token, data, new EventHandlerHTTPString(OnOpenCard),
            new EventHandlerServiceError(OnHTTPError),
            new EventHandlerServiceTimeOut(OnHTTPTimeOut), 10);
        Cards[1].gameObject.SetActive(false);
    }
    public void OnCard3()
    {
        Log.Add("OUT PLAYER_SHOW_CARD " + Cards[2].name);
        WWWForm data = new WWWForm();
        data.AddField("event", "PLAYER_SHOW_CARD");
        data.AddField("card_id", Cards[2].name);
        UCSS.HTTP.PostForm(Net.ServerURL + "/api/users/play?access-token=" + _token, data, new EventHandlerHTTPString(OnOpenCard),
            new EventHandlerServiceError(OnHTTPError),
            new EventHandlerServiceTimeOut(OnHTTPTimeOut), 10);
        Cards[2].gameObject.SetActive(false);
    }
    private void OnOpenCard(string text, string transactionid)
    {
        IJSonMutableObject obj = Net.ReadPack(text);
        Log.Add("IN OnOpenCard " + text);
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
        StartCheckTable();
    }
    /// <summary>
    /// помечаем какие карты мы можем выкладывать на стол
    /// </summary>
    /// <param name="jSonMutableObject"></param>
    public void SetActiveCards(IJSonMutableObject obj)
    {
        bool flag;
        for (int i = 0; i < obj.Count; i++)
        {
            print(_id + " " + obj[i].StringValue);
        }
        foreach (Button card in Cards)
        {
            flag = false;
            for (int i = 0; i < obj.Count; i++)
            {
             //   print(card.name + "=" + obj[i].StringValue);
                if (card.name == obj[i].StringValue)
                {
                    flag = true;
                    card.interactable = true;
                }
            }
            if (!flag)
                card.interactable = false;
        }
    }
    /// <summary>
    /// начинаем проверять статус стола
    /// </summary>
    private void StartCheckTable()
    {
        CancelInvoke("CheckTable");
        Log.Add("OUT StartCheckTable");
        InvokeRepeating("CheckTable", 0f, 2f);
    }
    /// <summary>
    /// проверяем стол
    /// </summary>
    private void CheckTable()
    {
        WWWForm data = new WWWForm();
        data.AddField("event", "CHECK_STATUS");
        data.AddField("action_history_id", _lastActId);
        data.AddField("move_history_id", _lastMoveId);
        UCSS.HTTP.PostForm(Net.ServerURL + "/api/users/play?access-token=" + _token, data, new EventHandlerHTTPString(OnCheckTable),
            new EventHandlerServiceError(OnHTTPError),
            new EventHandlerServiceTimeOut(OnHTTPTimeOut), 10);
    }
    private void OnCheckTable(string text, string transactionid)
    {
        IJSonMutableObject obj = Net.ReadPack(text);
        Log.Add("IN OnCheckTable " + text);
       // print(_id + ":" + text);
        if (obj != null)
        {
            if (obj.Contains("min_bet"))
            {
                _minBet = (float)obj["min_bet"].DoubleValue;
                _maxBet = (float)obj["max_bet"].DoubleValue;
            }
            if (obj.Contains("action_history_id"))
            {
                _lastActId = obj["action_history_id"].Int32Value;
                _lastMoveId = obj["move_history_id"].Int32Value;
            }
            if (obj.Contains("EVENT"))
            {
                switch (obj["EVENT"].StringValue)
                {
                    case "ROUND_1_TRADING":
                        InitCards(obj);
                        SetStage(3);

                        break;
                    case "ROUND_1_SHOWCARD":
                        SetStage(4);
                        break;
                    case "AZI_PREPARE":
                        MsgText.text = "";
                        if (obj["sum"].DoubleValue>0)
                            SetStage(5);
                        else
                            OnYesBtn();
                        break;
                    case "PLAYER_SHOW_CARD":
                        SetActiveCards(obj["valid_cards"].AsMutable);
                        break;
                    case "USER_IS_NOT_ONLINE":
                        SetStage(0);
                        //CancelInvoke("CheckTable");
                        break;
                }
                
            }
            else
            {
                Debug.LogError("В ответе нет event " + text);
            }
            if (obj.Contains("next_word_user_id"))
            {
                if (obj["next_word_user_id"].Int32Value == _id) SetActive();
                else
                    SetNotActive();
            }
            if (obj.Contains("game_winner_user_id"))
            {
                if (obj["game_winner_user_id"].Int32Value > 0)
                    SetStage(0);
            }
        }
    }
    /// <summary>
    /// согласились на ази аунд
    /// </summary>
    public void OnYesBtn()
    {
        SetStage(2);
        WWWForm data = new WWWForm();
        data.AddField("event", "PLAYER_AZI_ENTER");
        UCSS.HTTP.PostForm(Net.ServerURL + "/api/users/play?access-token=" + _token, data, new EventHandlerHTTPString(OnAzi),
            new EventHandlerServiceError(OnHTTPError),
            new EventHandlerServiceTimeOut(OnHTTPTimeOut), 10);
    }
    /// <summary>
    /// отказались от ази
    /// </summary>
    public void OnNoBtn()
    {
        SetStage(2);
        WWWForm data = new WWWForm();
        data.AddField("event", "PLAYER_AZI_EXIT");
        UCSS.HTTP.PostForm(Net.ServerURL + "/api/users/play?access-token=" + _token, data, new EventHandlerHTTPString(OnAzi),
            new EventHandlerServiceError(OnHTTPError),
            new EventHandlerServiceTimeOut(OnHTTPTimeOut), 10);
    }

    private void OnAzi(string text, string transactionid)
    {
    }

    /// <summary>
    /// инициализируем карты
    /// </summary>
    /// <param name="obj"></param>
    private void InitCards(IJSonMutableObject obj)
    {
       // print("InitCards ");
        foreach (Button button in Cards)
        {
            button.gameObject.SetActive(true);
        }
        IJSonMutableObject players = obj["players_data"].AsMutable;
        for (int i = 0; i < players.Count; i++)
        {
            if (players[i]["user_id"].Int32Value == _id)
            {
                IJSonMutableObject cards = players[i]["cards"].AsMutable;
                for (int j = 0; j < cards.Count; j++)
                {
                    print(j + ":" + cards[j] + ":" + GameController.CardsID[cards[j].Int32Value]);
                    Cards[j].image.sprite = GameController.CardsID[cards[j].Int32Value];
                    Cards[j].name = cards[j].StringValue;
                }
            }
        }
    }

    /// <summary>
    /// назначаем этого игрока активным
    /// </summary>
    public void SetActive()
    {
       // CancelInvoke("CheckTable");
        Fon.color = Color.red;
        if (_canvasGroup != null) _canvasGroup.blocksRaycasts = _canvasGroup.interactable = true;
    }
    /// <summary>
    /// обозначаем игрока как не активного 
    /// </summary>
    public void SetNotActive()
    {
        Fon.color = Color.yellow;
        if (_canvasGroup != null) _canvasGroup.blocksRaycasts = _canvasGroup.interactable = false;
    }
    /// <summary>
    /// поднимаем ставку
    /// </summary>
    public void OnRise()
    {
        Log.Add("OUT PLAYER_RAISE ");
        WWWForm data = new WWWForm();
        data.AddField("event", "PLAYER_RAISE");
        data.AddField("sum", _minBet.ToString());
        UCSS.HTTP.PostForm(Net.ServerURL + "/api/users/play?access-token=" + _token, data, new EventHandlerHTTPString(OnIncreaseBet),
            new EventHandlerServiceError(OnHTTPError),
            new EventHandlerServiceTimeOut(OnHTTPTimeOut), 10);
    }
    private void OnIncreaseBet(string text, string transactionid)
    {
        Log.Add("IN OnIncreaseBet " + text);
        IJSonMutableObject obj = Net.ReadPack(text);
        if (obj != null)
        {
            if (obj.Contains("status"))
            {
                if (obj["status"].StringValue != "ok" && obj["status"].StringValue != "new_round_generated")
                    Debug.LogError("ошибка повышения " + text);
            }
            else
            {
                Debug.LogError("ошибка повышения " + text);
            }
        }
        StartCheckTable();
    }
    public void OnPass()
    {
        Log.Add("OUT PLAYER_FOLD ");
        WWWForm data = new WWWForm();
        data.AddField("event", "PLAYER_FOLD");
        UCSS.HTTP.PostForm(Net.ServerURL + "/api/users/play?access-token=" + _token, data, new EventHandlerHTTPString(PassAnsw),
            new EventHandlerServiceError(OnHTTPError),
            new EventHandlerServiceTimeOut(OnHTTPTimeOut), 10);
        MsgText.text = "Пасс";
        foreach (Button button in Cards)
        {
            button.gameObject.SetActive(false);
        }
    }
    private void PassAnsw(string text, string transactionid)
    {
        Log.Add("IN PassAnsw " + text);
        IJSonMutableObject obj = Net.ReadPack(text);
        if (obj != null)
        {
            if (obj.Contains("status"))
            {
                if (obj["status"].StringValue != "ok" && obj["status"].StringValue != "new_round_generated")
                    Debug.LogError("ошибка паса");
            }
            else
            {
                Debug.LogError("ошибка паса");
            }
        }
        StartCheckTable();
    }
    public void OnCall()
    {
        Log.Add("OUT PLAYER_CALL ");
        WWWForm data = new WWWForm();
        data.AddField("event", "PLAYER_CALL");
        UCSS.HTTP.PostForm(Net.ServerURL + "/api/users/play?access-token=" + _token, data, new EventHandlerHTTPString(CallAnsw),
            new EventHandlerServiceError(OnHTTPError),
            new EventHandlerServiceTimeOut(OnHTTPTimeOut), 10);
    }
    private void CallAnsw(string text, string transactionid)
    {
        Log.Add("IN CallAnsw " + text);
        IJSonMutableObject obj = Net.ReadPack(text);
        if (obj != null)
        {
            if (obj.Contains("status"))
            {
                if (obj["status"].StringValue != "ok" && obj["status"].StringValue != "new_round_generated")
                    Debug.LogError("ошибка уравнения");
            }
            else
            {
                Debug.LogError("ошибка уравнения");
            }
        }
        StartCheckTable();
    }
    /// <summary>
    /// встаем в очередь на стол
    /// </summary>
    private void SendSearch()
    {
        WWWForm data = new WWWForm();
        data.AddField("event", "FIND_TABLE");
        data.AddField("cat_id", 1);
        data.AddField("money_cat_id", 2);
        Log.Add("OUT FIND_TABLE " );
        UCSS.HTTP.PostForm(Net.ServerURL + "/api/users/play?access-token=" + _token, data, new EventHandlerHTTPString(OnJoinQueue),
            new EventHandlerServiceError(OnHTTPError),
            new EventHandlerServiceTimeOut(OnHTTPTimeOut), 10);
    }

    private void OnJoinQueue(string text, string transactionid)
    {
        Log.Add("IN OnJoinQueue "+text);
        IJSonMutableObject obj = Net.ReadPack(text);
        if (obj != null)
        {
            if (obj.Contains("EVENT"))
            {
                StartCheckTable();
            }
            else
            {
                Debug.LogError(text);
            }
        }
    }

   
    private void OnAuth(string text, string transactionid)
    {
        IJSonMutableObject obj = Net.ReadPack(text);
        if (obj != null)
        {
            if (obj.Contains("access_token"))
            {
                _token = obj["access_token"].StringValue;
                _id = obj["user_id"].Int32Value;
                print("Получили токен " + obj["access_token"].StringValue);
                SetStage(1);
            }
            else
            {
                //Debug.LogError("не прошли авторизацию " + Login);
            }
        }
    }

    void OnHTTPError(string error, string transactionId)
    {
        //Debug.LogError("[RegPanel] [OnHTTPError] error = " + error + " (transaction [" + transactionId + "])");
    }

    void OnHTTPTimeOut(string transactionId)
    {
      //  Debug.LogError("[RegPanel] [OnHTTPTimeOut] transactionId = " + transactionId);
        UCSS.HTTP.RemoveTransaction(transactionId);
    }

    public void OnShowLog()
    {
        GUIController.ShowLog(Log);
    }
}
