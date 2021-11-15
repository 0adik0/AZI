using System;
using System.Collections.Generic;
using System.Linq;
using CodeTitans.JSon;
using Ucss;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

using Random = UnityEngine.Random;

public enum EAnimTip
{
    Close2Open,
    Open2Open,
    Close2Koloda,
    Instant
}
public class TableController : Menu
{
    /// <summary>
    /// спсиок игроков за столом
    /// </summary>
    public Dictionary<int, PlayerPanel> Players = new Dictionary<int, PlayerPanel>();
    /// <summary>
    /// ссылки на всех зрителей
    /// </summary>
    public Dictionary<int, PlayerPanel> Spectators = new Dictionary<int, PlayerPanel>();
    /// <summary>
    /// ссылки на панели игроков
    /// </summary>
    public List<PlayerPanel> PlayerLinks = new List<PlayerPanel>();
    /// <summary>
    /// расстановка игроков по порядку
    /// </summary>
    public Dictionary<int, PlayerPanel> PlayerLinksInGame = new Dictionary<int, PlayerPanel>();
    /// <summary>
    /// очередь входящих пакетов
    /// </summary>
    public Stack<IJSonMutableObject> Packets = new Stack<IJSonMutableObject>();
    /// <summary>
    /// пакет который обрабатываем в текущий момент
    /// </summary>
    public IJSonMutableObject CurrentPacket = null;
    /// <summary>
    /// идентификатор текущего раунда
    /// </summary>
    public int LastActionId = 0;
    /// <summary>
    /// идентификатор последнего обработанного действия
    /// </summary>
    public int LastMoveId = 0;
    /// <summary>
    /// карты на столе
    /// </summary>
    public Dictionary<int, Card> CardsOnTable = new Dictionary<int, Card>();
    public Card Koloda;
    public RectTransform TopTablePanel;
    public static TableController Instance;
    public static PlayerPanel Dealer;
    /// <summary>
    /// сколько игроков за столом
    /// </summary>
    private int _playersCount = 0;
    private List<Card> _cardsToSendInHands = new List<Card>();
    private List<Card> _cardsToShuffle = new List<Card>();
    private Vector2 _kolodaStartPos;
    /// <summary>
    /// по сколько карт раздаем в руки
    /// </summary>
    public int CardsOnHand = 3;
    /// <summary>
    /// место для козыря
    /// </summary>
    public RectTransform TrumpPos;
    /// <summary>
    /// идентификатор козыря
    /// </summary>
    public static int Trump = 1;
    /// <summary>
    /// ссылка на основного игрока
    /// </summary>
    public static PlayerPanel MainPlayer;

    public Single MinBet;
    public Single MaxBet;
    public Single CurBet;
    /// <summary>
    /// ссылки на надпись ази
    /// </summary>
    public Image[] AziFon;
    /// <summary>
    /// сумма на кону
    /// </summary>
    public Text KonText;
    /// <summary>
    /// флаг первой игры за столом, нужен для назначения диллера
    /// </summary>
    public bool FirstGame = true;
    /// <summary>
    /// ссылка на текущего игрока
    /// </summary>
    public static PlayerPanel ActivePlayer;
    /// <summary>
    /// фишки на кону
    /// </summary>
    public FishkiPos FishkiPos;
    /// <summary>
    /// времеенная ссылка на победителя раунда
    /// </summary>
    private PlayerPanel _winner;
    /// <summary>
    /// текущий раунд 1-торги 2-4-открытие 5-ази
    /// </summary>
    public static int CurrentRound = 0;
    private List<Fishka> _fishki2Send;
    /// <summary>
    /// ссылки на графику фишек
    /// </summary>
    public List<Fishka> FishkiList;

    public ExitPanel ExitPanel;
    public AziPanel AziPanel;
    /// <summary>
    /// идентификатор победителя игры
    /// </summary>
    public int WinnerId = 0;
    public Card TrumpCard { get; set; }
    private int _conStopki = 0;
    /// <summary>
    /// изображения для анимации логотипа ази
    /// </summary>
    public Image AziAnimFon;
    public Image AziAnimLine;
    public bool AziAnim = false;
    /// <summary>
    /// панелька закрывающая взаимодествия с игрой
    /// </summary>
    public GameObject LockInteractPanel;
    /// <summary>
    /// ссылка на карты выложенные на стол
    /// </summary>
    private List<GameObject> _cardsOnTable = new List<GameObject>();
    public GameObject AwayBtn;
    public GameObject ReturnBtn;
    public GameObject ExitGameBtn;
    private bool GameStarted = false;
    /// <summary>
    /// ссылки на позиции стопок
    /// </summary>
    public List<RectTransform> StopkiPos;
    public Vector3 DraggedCardPosition;
    public float MaxTurnTime=60;

    public void RemoveAllCardsFromTable()
    {
        print("RemoveAllCardsFromTable " + _cardsOnTable.Count);
        foreach (GameObject o in _cardsOnTable)
        {
            Destroy(o);
        }
        _cardsOnTable = new List<GameObject>();
    }
    public void StartAziAnim()
    {
        print("StartAziAnim");
        SoundController.Play(ESounds.Azi);
        AziAnimLine.gameObject.SetActive(true);
        AziAnim = true;
    }
    public void EndAziAnim()
    {
        print("EndAziAnim");
        AziAnim = false;
        AziAnimLine.rectTransform.localPosition = new Vector3(-15, 59);
        AziAnimFon.fillAmount = 0;
        AziAnimLine.gameObject.SetActive(false);
    }
    public void FixedUpdate()
    {
        if (AziAnim)
        {
            AziAnimLine.rectTransform.localPosition = new Vector3(0, AziAnimLine.rectTransform.localPosition.y + 1f);
            AziAnimFon.fillAmount += 0.008f;
            if (AziAnimLine.rectTransform.localPosition.y >= 167f)
            {
                AziAnimLine.rectTransform.localPosition = new Vector3(-15, 59);
                AziAnimFon.fillAmount = 0;
            }
        }
    }

    void Awake()
    {
        Instance = this;
        _canvasGroup = GetComponent<CanvasGroup>();

    }

    /// <summary>
    /// назначаем диллера и передаем ему колоду для раздачи карт
    /// </summary>
    /// <param name="id"></param>
    public void SetDealer(int id)
    {
        print("SetDealer " + id);
        Dealer = Players[id];
        Players[id].SetDealer();
        Koloda.EmmitCards = true;
        Koloda.ActionOnEnd = ECardAction.Koloda;
        Koloda.MoveTo(Players[id].KolodaPos, Vector3.zero, 0);
        FirstGame = false;
    }

    /// <summary>
    /// Назначаем асктивного игрока
    /// </summary>
    /// <param name="id"></param>
    /// <param name="delayTimerTime">на сколько задерживаем показ таймер, для первого играока</param>
    public void SetActivePlayer(int id, int delayTimerTime)
    {
        if (ActivePlayer != null)
        {
            ActivePlayer.TimerP.Hide();
            ActivePlayer.SetActive(false);
            //if (!ActivePlayer.Out)
            //    ActivePlayer.MsgP.HideMsg();
        }

        if (Players.ContainsKey(id))
        {
            ActivePlayer = Players[id];
            ActivePlayer.SetActive(true);
            if (delayTimerTime == 0)
                ActivePlayer.TimerP.ShowTime(GameController.TimeOut);
            else
            {
                //показываем таймер с задержкой
                ActivePlayer.TimerP.ShowTimeWithDelay(GameController.TimeOut, delayTimerTime);
            }
            if (!ActivePlayer.Out)
                ActivePlayer.MsgP.ShowMsgWhite(GameController.LangManager.GetTextValue("Msg.Think"));
            if (ActivePlayer == MainPlayer)
            {

                print("My turn!");
                BottomMenu.UnLockInteract();
                MainPlayer.CardOpened = false;
            }
        }
        else
        {
            Debug.LogWarning("нет юзера " + id);
        }
    }
    // Use this for initialization
    void Start()
    {
        _kolodaStartPos = Koloda.GetComponent<RectTransform>().position;

    }


    /// <summary>
    /// раздаем карты игрокам из колоды
    /// </summary>
    public void SendCardsToPlayers()
    {
        //создаем пачку карт для сдачи и рассылаем их всем игрокам
        int cardsCount = TableController.Instance.Players.Count * TableController.Instance.CardsOnHand;
        int i = 0;
        GameObject go = (GameObject)Resources.Load("GUI/CardCont");
        Card card = go.GetComponent<Card>();
        Card c;
        _cardsToSendInHands = new List<Card>();
        while (i < cardsCount)
        {
            c = (Card)Instantiate(card, Koloda.RTransform.position, Koloda.RTransform.rotation);
            c.name = "Card" + i;
            _cardsToSendInHands.Add(c);
            c.GetComponent<RectTransform>().SetParent(TableController.Instance.TopTablePanel);
            c.GetComponent<RectTransform>().localScale = Vector3.one;
            i++;
            //print(c.name);
        }
        //расчитываем какие карты куда слать
        int i1 = 0;
        int i2 = 0;
        PlayerPanel pp;
        pp = TableController.Dealer.NextPlayer;
        //расставляем остальных игроков
        for (i2 = 0; i2 < 3; i2++)
        {
            for (i = 0; i < TableController.Instance.Players.Count; i++)
            {
                //  print(pp.NickText.text + ":" + pp.NextPlayer + ":" + _cardsToSendInHands[i + i2 * TableController.Instance.Players.Count]);
                _cardsToSendInHands[i + i2 * TableController.Instance.Players.Count].SetTarget(pp.TakePoint, pp);
                pp = pp.NextPlayer;
                if (pp == null)
                    Debug.Break();
            }
        }
        InvokeRepeating("SendCartInHand", 0.3f, 0.3f);
    }
    /// <summary>
    /// показываем анимацию перетасовки карт
    /// </summary>
    public void ShuffleCards()
    {
        //создаем стопки в которых будем тосовать карты
        SoundController.Play(ESounds.ShaflCard,6.2f);
        SoundController.LockSoundPlay(ESounds.Card,6);
        List<Card> sk = new List<Card>();
        Card ca;
        for (int j = 0; j < StopkiPos.Count; j++)
        {
            GameObject g = (GameObject)Instantiate(Koloda.gameObject);
            g.transform.SetParent(AziPanel.transform);
            g.transform.position = StopkiPos[j].position;
            g.name = "koloda" + j;
            ca = g.GetComponent<Card>();
            ca.ActionOnEnd = ECardAction.RemoveAndShowKoloda;
            ca.Target = Koloda.RTransform;
            sk.Add(ca);
        }
        Destroy(TrumpCard.gameObject);
        Koloda.gameObject.SetActive(false);
        //создаем пачку карт для сдачи и рассылаем их всем игрокам
        int cardsCount = 20;
        int i = 0;
        GameObject go = (GameObject)Resources.Load("GUI/CardCont");
        Card card = go.GetComponent<Card>();
        Card c;
        _cardsToShuffle = new List<Card>();
        while (i < cardsCount)
        {
            for (int i1 = 0; i1 < 3; i1++)
            {
                c = (Card)Instantiate(card);
                c.name = "CardShuffle" + i;
                _cardsToShuffle.Add(c);
                c.GetComponent<RectTransform>().SetParent(TopTablePanel);
                c.RTransform.localScale = Vector3.one;
                c.RTransform.position = sk[i1].RTransform.position;

                //print(c.name);
            }
            i++;
        }
        //расчитываем какие карты куда слать

        for (i = cardsCount * 3 - 1; i >= 0; i--)
        {
            //  print(pp.NickText.text + ":" + pp.NextPlayer + ":" + _cardsToSendInHands[i + i2 * TableController.Instance.Players.Count]);
            //print(i);
            _cardsToShuffle[i].SetTarget(sk[Random.Range(0, 3)].RTransform);
        }
        //мешаем карты
        InvokeRepeating("SendCartShuffle", 0.1f, 0.1f);
        //запускаем движение стопок к позиции колоды
        foreach (Card card1 in sk)
        {
            card1.SendCartWithDelay(6.2f);
        }
        PlayerPanel pp;
        foreach (KeyValuePair<int, PlayerPanel> pair in Players)
        {
            pp = pair.Value;
            if (pp.VziatkaImage != null)
                pp.VziatkaImage.HideCards();
            //pp.CardsPanel
            pp.VziatkaText.text = "";
            foreach (CardInHand ca1 in pp.Cards)
            {
                if (ca1.gameObject.activeSelf)
                {
                    ca1.gameObject.SetActive(false);
                }
            }
        }

    }


    /// <summary>
    /// отправляем фишки победителю
    /// </summary>
    /// <param name="id"></param>
    public void SendFishkiToWinner(int id)
    {
        foreach (FishkiStopka fs in FishkiPos.Stopki)
        {
            SendFishki(Players[id].RectTransform, fs.RectTransform, fs.Fishki.Count, 3f);
            fs.RemoveFishki(3f);
        }
        //Invoke("EndGame", 6f);
        /*    int cardsCount = TableController.Instance.Players.Count * TableController.Instance.CardsOnHand;
            int i = 0;
            GameObject go = (GameObject)Resources.Load("GUI/Fishka");
            GameObject g;
            Fishka c;
            _fishki2Send = new List<Fishka>();
            while (i < 5)
            {
              //  g = (GameObject)Instantiate(go, FishkiPos.position, FishkiPos.rotation);
                g.name = "Card" + i;
                _fishki2Send.Add(g.GetComponent<Fishka>());
                g.GetComponent<RectTransform>().SetParent(TableController.Instance.TopTablePanel);
                i++;
                //print(c.name);
            }
            //расчитываем какие карты куда слать
            int i1 = 0;
            int i2 = 0;
            PlayerPanel pp;
            pp = TableController.Dealer.NextPlayer;
            //расставляем остальных игроков
            for (i2 = 0; i2 < _fishki2Send.Count; i2++)
            {
                _fishki2Send[i2].MoveTo(pp.TakePoint, i2 * 0.2f);
            }*/
    }
    /// <summary>
    /// заканчиваем игру
    /// </summary>
    public void EndGame()
    {
        CancelInvoke("ShuffleCards");
        print("EndGame");
        EndAziAnim();
        FirstGame = true;
        GameStarted = false;
        ExitGameBtn.SetActive(false);
        ReturnBtn.SetActive(false);
        AwayBtn.SetActive(false);
        Net.Instance.CancelInvoke("CheckTable");
        GUIController.Instance.CloseRestartP();
        // TopPanel.Instance.ShowLobbi();
        LastMoveId = LastActionId = 0;
        UnLockInteract(false);
        BottomMenu.EndGame();
        GUIController.Instance.EndGame();
        if (GUIController.Instance.BalanceBottomText != null)
            GUIController.Instance.BalanceBottomText.text = "";
        GameController.InGame = false;
        if (gameObject != null) gameObject.SetActive(false);
        
    }
    /// <summary>
    /// отправляем фишки
    /// </summary>
    /// <param name="target"></param>
    /// <param name="col"></param>
    public void SendFishki(RectTransform target, RectTransform startPos, int col, float delay = 0f, bool needCheck = false)
    {
        GameObject go;
        Fishka f;

        if (needCheck)
        {
            if (_conStopki < 5)
            {
                FishkiPos fs = target.GetComponent<FishkiPos>();

                if (fs != null)
                {
                    fs.AddStopka(TableController.Instance.FishkiList[Random.Range(0, 4)], col, 0.2f + delay);
                }
                _conStopki++;
            }
        }
        else
        {
            FishkiPos fs = target.GetComponent<FishkiPos>();


            if (fs != null)
            {
                if (fs.Stopki.Count > 0)
                {
                    //добавляем фишку в имеющуюся стопку
                    fs.AddFishka(TableController.Instance.FishkiList[Random.Range(0, 4)], 0.2f + delay);
                    fs.RemoveStartFishka();
                }
                else
                {
                    fs.AddStopka(TableController.Instance.FishkiList[Random.Range(0, 4)], col, 0.2f + delay);
                    fs.RemoveStartFishka();
                }

            }
        }
        // print(col);
        for (int i = 0; i < col; i++)
        {
            go = (GameObject)Instantiate(TableController.Instance.FishkiList[Random.Range(0, 4)].gameObject);
            f = go.GetComponentInChildren<Fishka>();
            f.RectTransform.SetParent(TableController.Instance.TopTablePanel);
            f.RectTransform.position = startPos.position;
            f.MoveTo(target, i * 0.2f + delay);
        }
    }

    public void SendCartShuffle()
    {
        if (_cardsToShuffle.Count > 0)
        {
            _cardsToShuffle.Last().SendCartInHand();
            _cardsToShuffle.Remove(_cardsToShuffle.Last());
        }
        else
        {
            print("EndSending");
            CancelInvoke("SendCartShuffle");

        }
    }

    public void SendCartInHand()
    {
        if (_cardsToSendInHands.Count > 0)
        {
            //   print("SendCartInHand " + _cardsToSendInHands.Count);
            _cardsToSendInHands[0].SendCartInHand();
            _cardsToSendInHands.RemoveAt(0);
        }
        else
        {
            //   print("EndSending");
            CancelInvoke("SendCartInHand");
            Koloda.EmmitCards = false;
            Koloda.ReturnKoloda(_kolodaStartPos);
        }
    }
    /// <summary>
    /// приняли на обработку новый пакет
    /// </summary>
    /// <param name="obj"></param>
    public void NewPack(IJSonMutableObject obj)
    {
        //if (obj["EVENT"].StringValue != "WAITING_FOR_PLAYER_MOVE")
        //    print(obj["EVENT"].StringValue);
        //проверяем входящий пакет 
        if (obj.Contains("action_history_id"))
        {
            if (obj["action_history_id"].Int32Value < LastActionId)
            {
                Debug.LogWarning("пришел старый пакет " + obj["action_history_id"].Int32Value);
                return;
            }
            else if (obj["move_history_id"].Int32Value < LastMoveId && obj["action_history_id"].Int32Value == LastActionId)
            {
                Debug.LogWarning("пришел старый пакет move " + obj["action_history_id"].Int32Value);
                return;
            }

        }
        if (obj.Contains("money_left"))
        {
            GameController.Money = obj["money_left"].DoubleValue;
        }
        PlayerPanel pp;
        IJSonMutableObject pd;
        if (obj.Contains("is_spectator"))
        {
            if (!GameStarted && obj["is_spectator"].BooleanValue)
            {
                gameObject.SetActive(true);
                LastActionId = obj["action_history_id"].Int32Value;
                LastMoveId = obj["move_history_id"].Int32Value;
                CurrentRound = 1;
                MinBet = 0;
                MaxBet = 1;
                if (!GameStarted)
                    NewGameSpectator(obj);
            }
        }
        if (obj.Contains("min_bet"))
        {
            MinBet = (float)obj["min_bet"].DoubleValue;
            MaxBet = (float)obj["max_bet"].DoubleValue;
        }

        if (obj["EVENT"].StringValue == "ROUND_1_TRADING")
        {
            gameObject.SetActive(true);
            LastActionId = obj["action_history_id"].Int32Value;
            LastMoveId = obj["move_history_id"].Int32Value;
            CurrentRound = 1;
            if (obj["is_azi"].BooleanValue)
                GameStarted = false;
            bool flag = false;
            if (!GameStarted)
            {
                //если в player data нет нашего игрока, значит на него не раздали, отображаем его как зрителя
                if (obj.Contains("players_data"))
                {
                    pd = obj["players_data"].AsMutable;
                    for (int i = 0; i < pd.Count; i++)
                    {
                        if (pd[i]["user_id"].Int32Value == GameController.Id)
                        {
                            flag = true;
                        }
                    }
                }
                if (flag)
                    NewGame(obj);
                else
                    NewGameSpectator(obj);
            }

            foreach (KeyValuePair<int, PlayerPanel> pair in Players)
            {
                pair.Value.RemoveWins();
            }

        }
        else
        {
            if (obj.Contains("players_data"))
            {
                pd = obj["players_data"].AsMutable;
                for (int i = 0; i < pd.Count; i++)
                {
                    if (pd[i]["user_id"].Int32Value != GameController.Id)
                    {
                        if (Players.ContainsKey(pd[i]["user_id"].Int32Value))
                        {
                            //  print(pd[i]["user_id"].Int32Value);
                            // print(Players[pd[i]["user_id"].Int32Value]);
                            if (pd[i].Contains("sum"))
                            {
                                Players[pd[i]["user_id"].Int32Value].MoneyText.text = pd[i]["sum"].StringValue;
                            }
                            else
                            {
                                Players[pd[i]["user_id"].Int32Value].Fishki.Clear();
                                Players[pd[i]["user_id"].Int32Value].MoneyText.text = "";
                            }
                            //отошел ли игрок
                            if (pd[i].Contains("is_away"))
                            {
                                if (pd[i]["is_away"].BooleanValue)
                                {
                                    Players[pd[i]["user_id"].Int32Value].Away = true;
                                    Players[pd[i]["user_id"].Int32Value].MsgP.ShowMsgGray(
                                        GameController.LangManager.GetTextValue("Game.Out"));
                                }
                                else
                                {
                                    Players[pd[i]["user_id"].Int32Value].Away = false;
                                    Players[pd[i]["user_id"].Int32Value].MsgP.HideMsg();
                                }
                            }
                            else
                            {
                                Players[pd[i]["user_id"].Int32Value].Away = false;
                                Players[pd[i]["user_id"].Int32Value].MsgP.HideMsg();
                            }
                        }
                        else
                            Debug.LogWarning("нет юзера " + pd[i]["user_id"].Int32Value + " в списке игроков");
                    }
                    else
                    {
                        if (pd[i].Contains("sum"))
                        {
                            MainPlayer.MoneyText.text = pd[i]["sum"].StringValue;
                        }
                        else
                        {
                            MainPlayer.Fishki.Clear();
                            MainPlayer.MoneyText.text = "";
                        }
                        if (pd[i]["is_away"].BooleanValue)
                        {
                            LockInteract(false);

                            MainPlayer.Away = true;
                            MainPlayer.MsgP.ShowMsgGray(
                                GameController.LangManager.GetTextValue("Game.Out"));
                        }
                        else
                        {
                            UnLockInteract(false);
                            MainPlayer.Away = false;
                            MainPlayer.MsgP.HideMsg();
                        }
                    }

                }
            }
        }

        if (obj.Contains("game_winner_user_id"))
        {
            if (obj["game_winner_user_id"].Int32Value > 0)
            {
                WinnerId = obj["game_winner_user_id"].Int32Value;
                if (CurrentRound != 1)
                {

                    SendFishkiToWinner(obj["game_winner_user_id"].Int32Value);
                    KonText.text = "";
                    // Invoke("ShowRestartP", 2f);
                }
                else
                {
                    //  Invoke("ShowRestartP", 2f);
                    SendFishki2WinnerInTrade(obj["game_winner_user_id"].Int32Value);
                }
                EndAziAnim();
                if (obj.Contains("players_data"))
                {
                    pd = obj["players_data"].AsMutable;
                    for (int i = 0; i < pd.Count; i++)
                    {
                        if (pd[i].Contains("left_card_id"))
                        {
                            if (pd[i]["left_card_id"].Int32Value > 0)
                                Players[pd[i]["user_id"].Int32Value].ShowLastCard(pd[i]["left_card_id"].Int32Value, 3f);
                        }
                    }
                }
                Invoke("ShuffleCards", 4f);
                Net.Instance.CheckBalance();
                BottomMenu.Instance.HideGame();
                GameStarted = false;
            }
        }

        if (obj.Contains("user_id"))
        {
            if (obj["user_id"].Int32Value != GameController.Id)
            {
                if (obj.Contains("action_history_id"))
                {
                    if (LastActionId != obj["action_history_id"].Int32Value ||
                        LastMoveId != obj["move_history_id"].Int32Value)
                    {
                        LastActionId = obj["action_history_id"].Int32Value;
                        LastMoveId = obj["move_history_id"].Int32Value;
                        switch (obj["EVENT"].StringValue)
                        {
                            case "ROUND_1_TRADING":

                                break;
                            case "PLAYER_AZI_ENTER":
                                Players[obj["user_id"].Int32Value].MsgP.ShowMsgWhite(GameController.LangManager.GetTextValue("Game.InAzi"));
                                break;
                            case "PLAYER_AZI_EXIT":
                                Players[obj["user_id"].Int32Value].MsgP.ShowMsgWhite(GameController.LangManager.GetTextValue("Game.OutAzi"));
                                break;
                            case "PLAYER_RAISE":

                                Players[obj["user_id"].Int32Value].Raise(obj["sum"].DoubleValue);
                                print("!!!");
                                pd = obj["players_data"].AsMutable;
                                if (WinnerId != 0)
                                {
                                    for (int i = 0; i < pd.Count; i++)
                                    {
                                        Players[pd[i]["user_id"].Int32Value].MoneyText.text = "";
                                    }
                                }
                                else
                                {
                                    for (int i = 0; i < pd.Count; i++)
                                    {
                                        print("1!!!!!!!!!+" + pd[i]["sum"].StringValue);
                                        Players[pd[i]["user_id"].Int32Value].MoneyText.text = pd[i]["sum"].StringValue;
                                    }
                                }


                                break;
                            case "PLAYER_CALL":
                                //игрок уравнял ставку
                                Players[obj["user_id"].Int32Value].Call(obj["sum"].DoubleValue);
                                pd = obj["players_data"].AsMutable;
                                if (WinnerId != 0)
                                {
                                    for (int i = 0; i < pd.Count; i++)
                                    {
                                        Players[pd[i]["user_id"].Int32Value].MoneyText.text = "";
                                    }
                                }
                                else
                                {
                                    for (int i = 0; i < pd.Count; i++)
                                    {
                                        print("!!!!!!!!!+"+ pd[i]["sum"].StringValue);
                                        Players[pd[i]["user_id"].Int32Value].MoneyText.text = pd[i]["sum"].StringValue;
                                    }
                                }

                                break;
                            case "PLAYER_FOLD":
                                //игрок уравнял ставку
                                Players[obj["user_id"].Int32Value].Pass();
                                pd = obj["players_data"].AsMutable;
                                if (WinnerId != 0)
                                {
                                    for (int i = 0; i < pd.Count; i++)
                                    {
                                        Players[pd[i]["user_id"].Int32Value].MoneyText.text = "";
                                    }
                                }
                                else
                                {
                                    for (int i = 0; i < pd.Count; i++)
                                    {
                                        Players[pd[i]["user_id"].Int32Value].MoneyText.text = pd[i]["sum"].StringValue;
                                    }
                                }

                                break;

                            case "PLAYER_SHOW_CARD":
                                CurrentRound = 2;
                                Players[obj["user_id"].Int32Value].OpenCard(obj["card_id"].Int32Value);
                                if (!obj["is_spectator"].BooleanValue)
                                    MainPlayer.SetActiveCards(obj["valid_cards"].AsMutable);
                                break;

                            default:
                                Debug.LogWarning("Нет обработчика пакета для события " + obj["EVENT"]);
                                break;

                        }
                    }
                    else
                    {
                        Debug.LogWarning("уже обрпботали этот ход " + obj["action_history_id"].Int32Value + ":" + obj["move_history_id"].Int32Value);
                    }
                }

            }
            else
            {
                if (obj.Contains("action_history_id"))
                {
                    if (LastActionId != obj["action_history_id"].Int32Value ||
                       LastMoveId != obj["move_history_id"].Int32Value)
                    {
                        LastActionId = obj["action_history_id"].Int32Value;
                        LastMoveId = obj["move_history_id"].Int32Value;
                        switch (obj["EVENT"].StringValue)
                        {
                            case "ROUND_1_TRADING":


                                break;
                            case "PLAYER_RAISE":
                                Players[obj["user_id"].Int32Value].MsgP.ShowMsgWhite(GameController.LangManager.GetTextValue("Game.Raise") + obj["sum"].StringValue);
                                pd = obj["players_data"].AsMutable;
                                if (WinnerId != 0)
                                {
                                    for (int i = 0; i < pd.Count; i++)
                                    {
                                        Players[pd[i]["user_id"].Int32Value].MoneyText.text = "";
                                    }
                                }
                                else
                                {
                                    for (int i = 0; i < pd.Count; i++)
                                    {
                                       
                                        Players[pd[i]["user_id"].Int32Value].MoneyText.text = pd[i]["sum"].StringValue;
                                        print("2!!!!!!!!!+" + pd[i]["sum"].StringValue+":"+ Players[pd[i]["user_id"].Int32Value].MoneyText.text);
                                    }
                                }
                                break;
                            case "PLAYER_FOLD":
                                //игрок уравнял ставку
                                MainPlayer.Pass();
                                MainPlayer.Away = true;
                                break;
                            //если игрок отошел показываем анимацию открытия его карты
                            case "PLAYER_SHOW_CARD":
                                MainPlayer.OpenCardAFK(obj["card_id"].Int32Value);
                                CurrentRound = 2;
                                if (!obj["is_spectator"].BooleanValue)
                                    MainPlayer.SetActiveCards(obj["valid_cards"].AsMutable);
                                break;

                            default:
                                Debug.LogWarning("Нет обработчика пакета для события " + obj["EVENT"]);
                                break;

                        }
                    }
                    else
                    {
                        Debug.LogWarning("уже обрпботали этот ход " + obj["action_history_id"].Int32Value + ":" + obj["move_history_id"].Int32Value);
                    }
                }
            }


        }
        else
        {

            //комманда без очереди на обработку
            switch (obj["EVENT"].StringValue)
            {
                case "USER_IS_NOT_ONLINE":
                    //пользователь не онлайн, то есть не играет ни за каким столом
                    Invoke("EndGame", 5f);
                    //LockInteract(false);

                    break;
                case "WAITING_FOR_PLAYERS":
                    //игрок нашел стол, но на нем недостаточно игроков для начала игры
                    GUIController.Instance.HideCurrentPanel();
                    // GUIController.ShowLoading();
                    LastActionId = 0;
                    LastMoveId = 0;

                    break;
                case "ROUND_1_RESULT":
                    if (LastActionId != obj["action_history_id"].Int32Value ||
                       LastMoveId != obj["move_history_id"].Int32Value)
                    {
                        //Invoke("RemoveAllCardsFromTable", 3f);
                        LastActionId = obj["action_history_id"].Int32Value;
                        LastMoveId = obj["move_history_id"].Int32Value;
                        CurrentRound = 3;
                        //  print(obj["winner_uses_id"].Int32Value);
                        ShowTakeAnim(obj["winner_uses_id"].Int32Value);
                        if (MainPlayer != null)
                            MainPlayer.UnlockAllCards();
                    }
                    else
                    {
                        Debug.LogWarning("уже обрпботали этот ход " + obj["action_history_id"].Int32Value + ":" + obj["move_history_id"].Int32Value);
                    }
                    break;
                case "ROUND_2_RESULT":
                    if (LastActionId != obj["action_history_id"].Int32Value ||
                      LastMoveId != obj["move_history_id"].Int32Value)
                    {
                        //Invoke("RemoveAllCardsFromTable", 3f);
                        LastActionId = obj["action_history_id"].Int32Value;
                        LastMoveId = obj["move_history_id"].Int32Value;
                        CurrentRound = 4;
                        //  print(obj["winner_uses_id"].Int32Value);
                        ShowTakeAnim(obj["winner_uses_id"].Int32Value);
                        if (MainPlayer != null)
                            MainPlayer.UnlockAllCards();
                    }
                    else
                    {
                        Debug.LogWarning("уже обрпботали этот ход " + obj["action_history_id"].Int32Value + ":" + obj["move_history_id"].Int32Value);
                    }
                    break;
                case "ROUND_1_SHOWCARD":
                    if (LastActionId != obj["action_history_id"].Int32Value ||
                      LastMoveId != obj["move_history_id"].Int32Value)
                    {
                        LastActionId = obj["action_history_id"].Int32Value;
                        LastMoveId = obj["move_history_id"].Int32Value;
                        CurrentRound = 2;
                        KonText.text = obj["kon_sum"].StringValue+ " $";
                        SendFishki2Con();
                        BottomMenu.Instance.HideGame();
                    }
                    else
                    {
                        Debug.LogWarning("уже обрпботали этот ход " + obj["action_history_id"].Int32Value + ":" + obj["move_history_id"].Int32Value);
                    }
                    break;
                case "ROUND_3_RESULT":
                    if (LastActionId != obj["action_history_id"].Int32Value ||
                 LastMoveId != obj["move_history_id"].Int32Value)
                    {
                        //Invoke("RemoveAllCardsFromTable", 3f);
                        LastActionId = obj["action_history_id"].Int32Value;
                        LastMoveId = obj["move_history_id"].Int32Value;
                        CurrentRound = 5;
                        //KonText.text = "";
                        ShowTakeAnim(obj["winner_uses_id"].Int32Value);
                        if (MainPlayer != null)
                            MainPlayer.UnlockAllCards();

                        BottomMenu.Instance.HideGame();
                    }
                    else
                    {
                        Debug.LogWarning("уже обрпботали этот ход " + obj["action_history_id"].Int32Value + ":" + obj["move_history_id"].Int32Value);
                    }
                    break;
                case "AZI_PREPARE":
                    if (obj["sum"].DoubleValue != 0)
                    {
                        if (MainPlayer != null)
                            MainPlayer.UnlockAllCards();
                        TableController.ShowAZIAsk(obj["sum"].DoubleValue);
                    }
                    break;
                case "":
                    break;
                default:
                    //   Debug.LogWarning("нет обработчика для события " + obj["EVENT"]);

                    break;
            }

        }


        if (obj.Contains("disable_call_button"))
        {
            BottomMenu.HideCall = obj["disable_call_button"].BooleanValue;
            /*if (obj["disable_call_button"].BooleanValue)
            {
                print("disable_call_button");
                BottomMenu.Instance.UravButton.gameObject.SetActive(false);
            }
            else
            {
                BottomMenu.Instance.UravButton.gameObject.SetActive(true);
            }*/

        }
        if (obj.Contains("disable_raise_button"))
        {
            /* if (obj["disable_raise_button"].BooleanValue)
             {
                 BottomMenu.Instance.IncreaseButton.gameObject.SetActive(false);
             }
             else
             {
                 BottomMenu.Instance.IncreaseButton.gameObject.SetActive(true);
             }*/
            BottomMenu.HideRaise = obj["disable_raise_button"].BooleanValue;

        }
        if (obj.Contains("next_word_user_id") && WinnerId == 0)
        {
            if (obj["next_word_user_id"].Int32Value != 0)
            {
                int time = 0;
                if (obj["EVENT"].StringValue == "ROUND_1_TRADING")
                    time = 3;
                SetActivePlayer(obj["next_word_user_id"].Int32Value, time);
            }

        }
        if (obj.Contains("action_history_id"))
        {
            LastActionId = obj["action_history_id"].Int32Value;
            LastMoveId = obj["move_history_id"].Int32Value;
        }
        if (obj.Contains("is_azi"))
        {
            if (obj["is_azi"].BooleanValue && !AziAnim)
            {
                //ази раунд
                StartAziAnim();
            }
        }
        if (obj.Contains("left_time_for_move"))
        {
            ActivePlayer.TimerP.TimeLeft = obj["left_time_for_move"].Int32Value;
        }
        //если есть рассаживаем зрителей 
        if (obj.Contains("spectators"))
        {
            IJSonMutableObject info = obj["spectators"].AsMutable;

            //добавляем новых зрителей
            for (int i = 0; i < info.Count; i++)
            {
                if (!Spectators.ContainsKey(info[i]["user_id"].Int32Value))
                {
                    for (int i1 = 0; i1 < PlayerLinks.Count; i1++)
                    {
                        if (!PlayerLinks[i1].gameObject.activeSelf)
                        {
                            //панель игрока свободна
                            if (info[i]["user_id"].Int32Value != GameController.Id)
                            {
                                PlayerLinks[i1].InitGuest(info[i]["user_id"].Int32Value,
                                         info[i]["username"].ToString(),
                                         info[i]["avatar"].ToString(), info[i]["flag"].ToString());
                                Spectators.Add(info[i]["user_id"].Int32Value, PlayerLinks[i1]);
                                break;
                            }
                        }
                    }
                }
            }
        }
        // Net.Instance.StartCheckTable(3f);
    }


    void ShowRestartP()
    {
        //EndAziAnim();
        //GUIController.Instance.ShowRestartP();
    }
    /// <summary>
    /// спрашиваем игрока о том что будет ли он играть в ази за доп плату?
    /// </summary>
    /// <param name="sum"></param>
    private static void ShowAZIAsk(double sum)
    {
        Instance.AziPanel.Show(sum);
    }

    /// <summary>
    /// отправляем ставки на кон
    /// </summary>
    public void SendFishki2Con()
    {
        PlayerPanel pp;
        print("SendFishki2Con");
        foreach (KeyValuePair<int, PlayerPanel> pair in Players)
        {
            pp = pair.Value;
            pp.MoneyText.text = "";
            print("MONEY " + pp.MoneyText.text);
            pp.Fishki.RemoveStartFishka();
            foreach (FishkiStopka fs in pp.Fishki.Stopki)
            {

                SendFishki(FishkiPos.RectTransform, fs.RectTransform, fs.Fishki.Count, 0, true);

                fs.RemoveFishki(0f);
            }
        }
    }
    /// <summary>
    /// отправляем ставки победителю в торгах
    /// </summary>
    public void SendFishki2WinnerInTrade(int id)
    {
        PlayerPanel pp;
        print("SendFishki2WinnerInTrade");
        foreach (KeyValuePair<int, PlayerPanel> pair in Players)
        {
            pp = pair.Value;
            pp.MoneyText.text = "";
            print("MONEY " + pp.MoneyText.text);
            pp.Fishki.RemoveStartFishka();
            foreach (FishkiStopka fs in pp.Fishki.Stopki)
            {

                SendFishki(Players[id].RectTransform, fs.RectTransform, fs.Fishki.Count, 0, true);

                fs.RemoveFishki(0f);
            }
        }
    }
    /// <summary>
    /// нажали кнопку уравнять ставку
    /// </summary>
    public void OnEqualize()
    {
        // SendFishkiToPlayers(4);
        MainPlayer.Call(BottomMenu.Instance.BetSlider.value);
        Net.SendEqualize();
        BottomMenu.LockInteract();
    }

    /// <summary>
    /// переходим к следующему ходу
    /// </summary>
    public void EndTurn()
    {

    }
    /// <summary>
    /// игрок спасовал
    /// </summary>
    public void OnPass()
    {
        Net.SendPass();

        // MainPlayer.Pass();
        BottomMenu.Instance.HideGame();
    }
    /// <summary>
    /// повышаем ставку
    /// </summary>
    public void OnIncrease()
    {
        double d = Math.Round(BottomMenu.Instance.BetSlider.value, 1);
        print("increase " + d);
        MainPlayer.Raise(d, false);
        MainPlayer.MoneyText.text = "";
        Net.IncreaseBet(d);
        BottomMenu.LockInteract();
    }
    /// <summary>
    /// преостанавливаем обработку интерактивности панели
    /// </summary>
    public void LockPanelInteract()
    {

    }
    /// <summary>
    /// начинаем новую игру, рассаживаем игроков
    /// </summary>
    /// <param name="obj"></param>
    public void NewGame(IJSonMutableObject obj)
    {
        print("NewGame");
        Spectators = new Dictionary<int, PlayerPanel>();
        GUIController.Instance.CloseRestartP();
        ExitGameBtn.SetActive(true);
        TopPanel.Instance.HideLobbi();
        GUIController.Instance.WaitingTimer.Hide();
        RemoveAllCardsFromTable();
        WinnerId = 0;
        GameStarted = true;
        GUIController.Instance.CloseRestartP();

        GUIController.Instance.StartGame();
        GUIController.Instance.ShowGameFon(obj["table_type_id"].StringValue);
        if (TrumpCard != null)
            Destroy(TrumpCard.gameObject);
        GameController.TimeOut = obj["timeout"].Int32Value;
        PlayerLinksInGame = new Dictionary<int, PlayerPanel>();
        Players = new Dictionary<int, PlayerPanel>();
        //       GUIController.Instance.BottomPanel.ShowGame();
        //выбираем тип стола
        AziFon[obj["table_type_id"].Int32Value - 1].gameObject.SetActive(true);
        CurBet = 0;

        BottomMenu.Instance.InitSlider(MinBet, MinBet, MaxBet);
        _playersCount = obj["players_data"].AsMutable.Count;
        IJSonMutableObject info = obj["players_data"].AsMutable;
        gameObject.SetActive(true);
        Trump = obj["trump_card_id"].Int32Value;
        print("kon_sum " + obj["kon_sum"].DoubleValue);
        if (obj["kon_sum"].DoubleValue == 0)
        {
            _conStopki = 0;
            FishkiPos.Clear();
            KonText.text = "";
        }
        foreach (PlayerPanel link in PlayerLinks)
        {
            link.gameObject.SetActive(false);
        }
        int id = 0;
        //определяем где наш игрок в списке
        for (int i = 0; i < _playersCount; i++)
        {
            if (GameController.Id == info[i]["user_id"].Int32Value)
            {
                print("my! " + i);
                MainPlayer = PlayerLinks[i];
                Players.Add(info[i]["user_id"].Int32Value, PlayerLinks[0]);
                PlayerLinksInGame.Add(0, PlayerLinks[0]);
                PlayerLinks[0].gameObject.SetActive(true);
                PlayerLinks[0].InitPlayer(info[i]["user_id"].Int32Value, info[i]["username"].ToString(), info[i]["avatar"].ToString(), info[i]["flag"].ToString(), 3, info[i]["cards"].AsMutable);
                MainPlayer.Money = info[i]["sum"].DoubleValue;
                //MainPlayer.MoneyText.text = MainPlayer.Money.ToString();
                id = i;
            }
        }
        int i1 = 0;
        int i2 = 0;
        int id3 = 0;
        //определяем ссылки на панели игроков
        double d = _playersCount / 2;
        id3 = 4 - (int)Math.Floor(d);
        //расставляем остальных игроков
        //    print(obj);
        for (int i = 1; i < _playersCount; i++)
        {
            if (id + i < _playersCount)
            {
                i1 = id + i;
            }
            else
            {
                //дошли до конца списка начинаем с начала
                i1 = i2;
                i2++;
            }
            //print("i1 " + i + ":" + id3 + ":" + PlayerLinks[id3] + ":" + info[i1]["user_id"].Int32Value);

            Players.Add(info[i1]["user_id"].Int32Value, PlayerLinks[id3]);
            PlayerLinksInGame.Add(i, PlayerLinks[id3]);
            PlayerLinks[id3].gameObject.SetActive(true);
            PlayerLinks[id3].InitPlayer(info[i1]["user_id"].Int32Value, info[i1]["username"].ToString(), info[i1]["avatar"].ToString(), info[i1]["flag"].ToString(), 3);
            id3++;
        }

        i1 = 0;
        //foreach (PlayerPanel link in PlayerLinks)
        //{
        //    print(i1+":"+(link.NickText != null ? link.NickText.text : "my"));
        //    i1++;
        //}
        //назначаем ссылки на следующего игрока 
        for (int i = 0; i < _playersCount; i++)
        {
            //            print(i);
            if (i != _playersCount - 1)
            {
                PlayerLinksInGame[i].NextPlayer = PlayerLinksInGame[i + 1];
            }
            else
            {
                PlayerLinksInGame[i].NextPlayer = PlayerLinksInGame[0];
            }

        }
        //foreach (KeyValuePair<int, PlayerPanel> pair in Players)
        //{
        //    print((pair.Value.NickText != null ? pair.Value.NickText.text : "my") + "->" + (pair.Value.NextPlayer.NickText != null ? pair.Value.NextPlayer.NickText.text : "my"));
        //}
        //print("SetDealer");
        //назначаем сидящео справа дилером
        for (int i = 0; i < _playersCount; i++)
        {
            if (PlayerLinksInGame[i].NextPlayer.Id == obj["next_word_user_id"].Int32Value)
                SetDealer(PlayerLinksInGame[i].Id);
        }
        UnLockInteract(false);
        //если раздача разблокируем пасс с задержкой
        if (obj["EVENT"].StringValue == "ROUND_1_TRADING")
        {
            BottomMenu.UnlockPassWithDelay();
        }
    }
    /// <summary>
    /// подключились к игре спектатором
    /// </summary>
    /// <param name="obj"></param>
    public void NewGameSpectator(IJSonMutableObject obj)
    {
        Spectators = new Dictionary<int, PlayerPanel>();
        GUIController.Instance.WaitingTimer.Hide();
        ExitGameBtn.SetActive(true);
        RemoveAllCardsFromTable();
        WinnerId = 0;
        GameStarted = true;
        TopPanel.Instance.HideLobbi();
        GUIController.Instance.StartGame();
        GUIController.Instance.ShowGameFon(obj["table_type_id"].StringValue);
        if (TrumpCard != null)
            Destroy(TrumpCard.gameObject);
        GameController.TimeOut = obj["timeout"].Int32Value;
        PlayerLinksInGame = new Dictionary<int, PlayerPanel>();
        Players = new Dictionary<int, PlayerPanel>();
        //       GUIController.Instance.BottomPanel.ShowGame();
        //выбираем тип стола
        AziFon[obj["table_type_id"].Int32Value].gameObject.SetActive(true);
        CurBet = 0;
        BottomMenu.Instance.HideGame();
        // BottomMenu.Instance.InitSlider(MinBet, MinBet, MaxBet);
        _playersCount = obj["players_data"].AsMutable.Count;
        IJSonMutableObject info = obj["players_data"].AsMutable;
        gameObject.SetActive(true);


        foreach (PlayerPanel link in PlayerLinks)
        {
            link.gameObject.SetActive(false);
        }
        int id = 0;
        //определяем где наш игрок в списке
        /*  for (int i = 0; i < _playersCount; i++)
          {
              if (GameController.Id == info[i]["user_id"].Int32Value)
              {
                  print("my! " + i);
                  MainPlayer = PlayerLinks[i];
                  Players.Add(info[i]["user_id"].Int32Value, PlayerLinks[0]);
                  PlayerLinksInGame.Add(0, PlayerLinks[0]);
                  PlayerLinks[0].gameObject.SetActive(true);
                  PlayerLinks[0].InitPlayer(info[i]["user_id"].Int32Value, info[i]["username"].ToString(), info[i]["avatar"].ToString(), info[i]["flag"].ToString(), 3, info[i]["cards"].AsMutable);
                  MainPlayer.Money = info[i]["sum"].DoubleValue;
                  //MainPlayer.MoneyText.text = MainPlayer.Money.ToString();
                  id = i;
              }
          }*/
        int i1 = 0;
        int i2 = 0;
        int id3 = 0;
        //определяем ссылки на панели игроков
        double d = _playersCount / 2;
        id3 = 4 - (int)Math.Floor(d);
        //расставляем остальных игроков
        //    print(obj);
        for (int i = 0; i < _playersCount; i++)
        {
            if (id + i < _playersCount)
            {
                i1 = id + i;
            }
            else
            {
                //дошли до конца списка начинаем с начала
                i1 = i2;
                i2++;
            }
            //print("i1 " + i + ":" + id3 + ":" + PlayerLinks[id3] + ":" + info[i1]["user_id"].Int32Value);

            Players.Add(info[i1]["user_id"].Int32Value, PlayerLinks[id3]);
            PlayerLinksInGame.Add(i, PlayerLinks[id3]);
            PlayerLinks[id3].gameObject.SetActive(true);
            PlayerLinks[id3].InitPlayer(info[i1]["user_id"].Int32Value, info[i1]["username"].ToString(), info[i1]["avatar"].ToString(), info[i1]["flag"].ToString(), info[i1]["cards_count"].Int32Value);
            if (info[i1]["opened_card_id"].Int32Value > 0)
            {
                PlayerLinks[id3].OpenCard(info[i1]["opened_card_id"].Int32Value, true);
            }
            for (int j = 0; j < info[i1]["wins"].Int32Value; j++)
            {
                PlayerLinks[id3].AddVziatka();
            }
            for (int j = 0; j < info[i1]["cards_count"].Int32Value; j++)
            {
                PlayerLinks[id3].AddCard(null);
            }
            id3++;
        }

        i1 = 0;
        //foreach (PlayerPanel link in PlayerLinks)
        //{
        //    print(i1+":"+(link.NickText != null ? link.NickText.text : "my"));
        //    i1++;
        //}
        //назначаем ссылки на следующего игрока 
        for (int i = 0; i < _playersCount; i++)
        {
            //print(i);
            if (i != _playersCount - 1)
            {
                PlayerLinksInGame[i].NextPlayer = PlayerLinksInGame[i + 1];
            }
            else
            {
                PlayerLinksInGame[i].NextPlayer = PlayerLinksInGame[0];
            }

        }
        //foreach (KeyValuePair<int, PlayerPanel> pair in Players)
        //{
        //    print((pair.Value.NickText != null ? pair.Value.NickText.text : "my") + "->" + (pair.Value.NextPlayer.NickText != null ? pair.Value.NextPlayer.NickText.text : "my"));
        //}
        //print("SetDealer");
        Trump = obj["trump_card_id"].Int32Value;
        if (obj["EVENT"].StringValue == "ROUND_1_TRADING")
        {
            //если этап сдачи карт
            SetDealer(obj["next_word_user_id"].Int32Value);
            print("показываем анимацию сдачи");
        }
        else
        {
            //если не этап сдачи карт

            TrumpCard = SendCard(Trump, TrumpPos, TrumpPos, EAnimTip.Instant, true);
        }
        // UnLockInteract();
    }
    /// <summary>
    /// разрешаем взаимодействовать
    /// </summary>
    public void UnLockInteract(bool needSend = true)
    {
        print("UnLockInteract");
        if (MainPlayer != null)
            MainPlayer.Away = false;
        if (LockInteractPanel != null) LockInteractPanel.SetActive(false);

        if (needSend)
        {
            AwayBtn.SetActive(true);
            ReturnBtn.SetActive(false);
            WWWForm data = new WWWForm();
            data.AddField("event", "PLAYER_RETURN");
            UCSS.HTTP.PostForm(Net.ServerURL + "/api/users/play?access-token=" + GameController.Token, data,
                new EventHandlerHTTPString(OnUnLockInteract),
                new EventHandlerServiceError(Net.Instance.OnHTTPError),
                new EventHandlerServiceTimeOut(Net.Instance.OnHTTPTimeOut), 10);
        }
    }

    private void OnUnLockInteract(string text, string transactionid)
    {
        print(text);
    }

    /// <summary>
    /// запрещаем взаимодействовать
    /// </summary>
    public void LockInteract(bool sendAway = true)
    {
        LockInteractPanel.SetActive(true);
        MainPlayer.Away = true;
        AwayBtn.SetActive(false);
        ReturnBtn.SetActive(true);
        if (sendAway)
        {
            WWWForm data = new WWWForm();
            data.AddField("event", "PLAYER_AWAY");
            UCSS.HTTP.PostForm(Net.ServerURL + "/api/users/play?access-token=" + GameController.Token, data,
                new EventHandlerHTTPString(OnLockInteract),
                new EventHandlerServiceError(Net.Instance.OnHTTPError),
                new EventHandlerServiceTimeOut(Net.Instance.OnHTTPTimeOut), 10);
        }
    }

    private void OnLockInteract(string text, string transactionid)
    {
        print(text);
    }
    public void OnAwayBtn()
    {
        if (MainPlayer.Away)
        {
            MainPlayer.Away = false;
            MainPlayer.MsgP.HideMsg();
            UnLockInteract();
        }
        else
        {
            MainPlayer.Away = true;
            LockInteract();
        }
    }
    public Card SendCard(int id, Vector3 startPos, Quaternion startRot, RectTransform endPos, EAnimTip tip, bool trumpl = false)
    {
        print("SendCard " + startPos + ":" + endPos + ":" + tip);
        SoundController.Play(ESounds.Card);
        GameObject go = (GameObject)Resources.Load("GUI/CardCont");
        Card card = go.GetComponent<Card>();
        Card c;
        SoundController.Play(ESounds.Card);
        c = (Card)Instantiate(card, startPos, startRot);
        if (!trumpl)
            _cardsOnTable.Add(c.gameObject);
        c.Id = id;
        c.name = "Card " + id;
        c.GetComponent<RectTransform>().SetParent(TableController.Instance.TopTablePanel);
        switch (tip)
        {
            case EAnimTip.Close2Open:
                c.OpenCard();
                break;
            case EAnimTip.Open2Open:
                c.Open2Open();
                break;
            case EAnimTip.Close2Koloda:
                c.Close2Koloda();
                break;

        }
        if (tip != EAnimTip.Instant)
            c.MoveTo(endPos, Vector3.zero, 0);
        else
            c.InstantOpen(endPos);
        return c;
    }
    /// <summary>
    /// кладем карту на стол
    /// </summary>
    /// <param name="id"></param>
    /// <param name="startPos"></param>
    /// <param name="endPos"></param>
    public Card SendCard(int id, RectTransform startPos, RectTransform endPos, EAnimTip tip, bool trumpl = false)
    {
        print("SendCard " + startPos + ":" + endPos + ":" + tip);
        Card c = SendCard(id, startPos.position, startPos.rotation, endPos, tip, trumpl);

        return c;
    }

    /// <summary>
    /// показываем анимацию взятки
    /// </summary>
    /// <param name="idWinner"></param>
    public void ShowTakeAnim(int idWinner)
    {
        print("ShowTakeAnim " + idWinner);
        RectTransform takePos = Players[idWinner].OpenCardPos;
        if (GameController.Id == idWinner)
        {
            MainPlayer.OpenedCard.ShowWinAnim();
            MainPlayer.OpenedCard.RTransform.SetAsLastSibling();
            //Players[idWinner].OpenedCard.RTransform.SetParent(TrumpPos);
            _winner = MainPlayer;
        }
        else
        {
            print("ShowWinAnim " + idWinner + ":" + Players[idWinner]);
            if (Players[idWinner] != null)
            {

                Players[idWinner].OpenedCard.ShowWinAnim();
                Players[idWinner].OpenedCard.RTransform.SetAsLastSibling();
                //Players[idWinner].OpenedCard.RTransform.SetParent(TrumpPos);
                _winner = Players[idWinner];
            }
        }

        foreach (KeyValuePair<int, PlayerPanel> pair in Players)
        {
            if (pair.Key != idWinner)
            {
                if (pair.Value.OpenedCard != null)
                {
                    pair.Value.OpenedCard.MoveTo(takePos, Vector2.zero, Random.Range(-40, 40));
                    pair.Value.OpenedCard.ActionOnEnd = ECardAction.StayOnTable;
                }
            }
        }
        Invoke("ShowTakeAnim2", 2f);
    }
    /// <summary>
    /// вторая часть анимации взятки
    /// </summary>
    public void ShowTakeAnim2()
    {
        print("ShowTakeAnim2");
        foreach (KeyValuePair<int, PlayerPanel> pair in Players)
        {
            if (pair.Value.OpenedCard != null)
            {
                pair.Value.OpenedCard.MoveTo(_winner.TakePoint, Vector2.zero, 0);
                pair.Value.OpenedCard.ActionOnEnd = ECardAction.Remove;
            }
        }
        _winner.AddVziatka();
        _winner = null;
        // Net.Instance.StartCheckTable();
    }

    /// <summary>
    /// карты розданны, начинаем торги
    /// </summary>
    public void StartAuction()
    {
        ActivePlayer.StartAuction();
    }
    /// <summary>
    /// показываем панель с вопросом о выходе
    /// </summary>
    public void ShowExitPanel()
    {
        print("ShowExitPanel");
        ExitPanel.Show();
    }
}
