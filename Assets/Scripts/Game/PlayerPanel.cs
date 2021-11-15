
using System;
using System.Collections.Generic;
using System.Linq;

using CodeTitans.JSon;
using Ucss;

using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;

using Random = UnityEngine.Random;


public class PlayerPanel : MonoBehaviour
{

    public RectTransform CardsPanel;
    public Text NickText;
    public RawImage Avatar;
    public RawImage FlagImage;
    public Image DealerImage;
    public VziatkaPanel VziatkaImage;
    public Text VziatkaText;
    public Text MoneyText;
    public List<CardInHand> Cards = new List<CardInHand>();
    public bool Turn = false;
    public PlayerPanel NextPlayer;
    public int Id;
    /// <summary>
    /// место для открытой карты на столе
    /// </summary>
    public RectTransform OpenCardPos;
    /// <summary>
    /// точка для получения карт для главного игрока
    /// </summary>
    public RectTransform TakePoint;

    /// <summary>
    /// является ли панель панелью основного игрока 
    /// </summary>
    public bool My = false;

    /// <summary>
    /// место куда приходит колода при сдаче
    /// </summary>
    public RectTransform KolodaPos;
    /// <summary>
    /// сколько карт уже взяли в руки
    /// </summary>
    private int _cardsInHands = 0;
    public MsgPanel MsgP;
    public List<int> CardsInHands;
    private double _money;
    private int _id2Open = 0;
    /// <summary>
    /// ссылка на открытую игроком карту
    /// </summary>
    public Card OpenedCard;
    /// <summary>
    /// сколько игрок взял взяток
    /// </summary>
    public int Vziatki = 0;
    /// <summary>
    /// ссылка на панель времени
    /// </summary>
    public TimerPanel TimerP;
    /// <summary>
    /// ссылка на фишки
    /// </summary>
    public FishkiPos Fishki;

    public RectTransform RectTransform;
    /// <summary>
    /// открыл ли игрок уже карту
    /// </summary>
    public bool CardOpened = false;
    /// <summary>
    /// отошел ли игрок
    /// </summary>
    public bool Out = false;
    /// <summary>
    /// занавеска скрывает пасующего
    /// </summary>
    public GameObject Zanaveska;
    /// <summary>
    /// фон подсвечивающий активного игрока
    /// </summary>
    public GameObject ActivePlayerFon;
    /// <summary>
    /// отошел ли игрок
    /// </summary>
    public bool Away = false;
    public void OnPointerClick(PointerEventData data)
    {
        ShowLastCard(10);
        TableController.Instance.ShuffleCards();
    }
    void Awake()
    {
        RectTransform = GetComponent<RectTransform>();
    }
    public double Money
    {
        get { return _money; }
        set
        {
            _money = value;
            if (My)
            {
                if (GameController.MoneyTip == 1)
                    BottomMenu.Instance.BalanceText.text = _money.ToString() + "$";
                else
                    BottomMenu.Instance.BalanceText.text = _money.ToString() + "$";
            }
        }
    }

    public void SetActive(bool flag)
    {
      Debug.Log("SetActive Image***************");
  
        ActivePlayerFon.SetActive(flag);
    }
    public void InitPlayer(int id, string nick, string avatar, string flag, int colCards, IJSonMutableObject cards = null)
    {
        print("InitPlayer " + id + ":" + flag);
        TimerP.Hide();
        _cardsInHands = 0;
        Vziatki = 0;
        if (Zanaveska != null)
            Zanaveska.SetActive(false);
        CardsInHands = new List<int>();
        UCSS.HTTP.GetTexture(Net.ServerURL + avatar, new EventHandlerHTTPTexture(this.OnAvatarDownloaded), new EventHandlerServiceError(this.OnHTTPError), new EventHandlerServiceTimeOut(this.OnHTTPTimeOut), 10);
        UCSS.HTTP.GetTexture(Net.ServerURL + flag, new EventHandlerHTTPTexture(this.OnFlagDownloaded), new EventHandlerServiceError(this.OnHTTPError), new EventHandlerServiceTimeOut(this.OnHTTPTimeOut), 10);
        //UCSS.HTTP.GetTexture(Net.ServerURL + "/flags/" + flag, new EventHandlerHTTPTexture(this.OnFlagDownloaded), new EventHandlerServiceError(this.OnHTTPError), new EventHandlerServiceTimeOut(this.OnHTTPTimeOut), 10);
        //VziatkaImage.color = new Color(255f, 255f, 255f, 0);
        if (VziatkaImage != null)
            VziatkaImage.HideCards();
        VziatkaText.text = "";
        MoneyText.text = (TableController.Instance.MinBet/2f).ToString();
        Fishki.RemoveStartFishka();
        Fishki.AddStartFishka();

        // Fishki.gameObject.SetActive(false);
        //LoadAvatar(avatar);
        Id = id;
        if (GameController.Id == id)
        {
            TableController.MainPlayer = this;
            My = true;
        }

        if (!My)
            NickText.text = nick;
        //  MoneyText.text = money.ToString();
        foreach (CardInHand card in Cards)
        {
            card.gameObject.SetActive(false);
        }
        DealerImage.gameObject.SetActive(false);
        if (cards != null)
        {
            for (int i = 0; i < colCards; i++)
            {
                CardsInHands.Add(cards[i].Int32Value);
                //    print("card in hand " + cards[i].Int32Value);
            }
        }
        else
        {
            for (int i = 0; i < colCards; i++)
            {

                CardsInHands.Add(Random.Range(100, 300));
            }
        }
        if (MsgP != null) MsgP.HideMsg();
        if (VziatkaImage != null)
        {
            VziatkaImage.HideCards();
        }
        //  CardsPanel.gameObject.SetActive(false);
    }
    /// <summary>
    /// показываем анимацию открытия карты если игрок отошел
    /// </summary>
    /// <param name="id"></param>
    public void OpenCardAFK(int id)
    {
        TimerP.Hide();
        print("OpenCardAFK " + id + " " + this.name);
        foreach (CardInHand card in Cards)
        {
            if (card.Id == id)
            {
                card.GetComponent<CardAnim>().OpenCard();
                break;
            }
        }
    }
    private void OnAvatarDownloaded(Texture texture, string transactionId)
    {
        Debug.Log("[Player] [OnAvatarDownloaded] texture.width = " + texture.width + ", texture.height = " +
                  texture.height);

        Avatar.texture = texture;
    }

    private void OnFlagDownloaded(Texture texture, string transactionId)
    {
        Debug.Log("[Player] [OnFlagDownloaded] texture.width = " + texture.width + ", texture.height = " +
                  texture.height);
        FlagImage.texture = texture;
    }

    private void OnHTTPError(string error, string transactionId)
    {
        Debug.LogError("[Player] [OnHTTPError] error = " + error + " (transaction [" + transactionId + "])");
        UCSS.HTTP.RemoveTransaction(transactionId);
    }

    private void OnHTTPTimeOut(string transactionId)
    {
        Debug.LogError("[Player] [OnHTTPTimeOut] url = " + transactionId);
        UCSS.HTTP.RemoveTransaction(transactionId);
    }

    /// <summary>
    /// назначаем игрока раздающим
    /// </summary>
    public void SetDealer()
    {
        DealerImage.gameObject.SetActive(true);
    }

    /// <summary>
    /// добавляем карту в руку
    /// </summary>
    public void AddCard(Card c)
    {
        // print("AddCard " + c);
        CardAnim ca;
        CardInHand ch;
        if (My)
        {
            foreach (CardInHand card in Cards)
            {
                if (!card.gameObject.activeSelf)
                {
                    card.gameObject.SetActive(true);
                    ca = card.GetComponent<CardAnim>();

                    if (ca != null)
                    {
                        ca.Init(CardsInHands[_cardsInHands]);
                        card.Id = CardsInHands[_cardsInHands];
                        ca.Take();
                        _cardsInHands++;
                    }

                    break;
                }
            }
            Destroy(c.gameObject);
        }
        else
        {
            foreach (CardInHand card in Cards)
            {
                if (!card.gameObject.activeSelf)
                {
                    card.gameObject.SetActive(true);
                    ch = card.GetComponent<CardInHand>();
                    if (ch != null)
                        ch.Take();
                    break;
                }
            }
        }
    }
    public void LoadAvatar(string link)
    {
        //   this.cube1.material.mainTexture = null;
        UCSS.HTTP.GetTexture(link, new EventHandlerHTTPTexture(OnAvatarDownloaded), new EventHandlerServiceError(this.OnHTTPError), new EventHandlerServiceTimeOut(this.OnHTTPTimeOut), 10);
    }

    private void OnFlagDownloaded(Texture2D texture, string transactionid)
    {
        //print("OnFlagDownloaded" +texture);
        if (FlagImage != null)
            FlagImage.texture = texture;
    }
    private void OnAvatarDownloaded(Texture2D texture, string transactionid)
    {
        if (Avatar != null)
            Avatar.texture = texture;
    }

    /// <summary>
    /// начинаем торги
    /// </summary>
    public void StartAuction()
    {
        if (My)
        {
            GUIController.Instance.BottomPanel.ShowGame();


        }
        else
        {
            if (!Out)
                MsgP.ShowMsgWhite(GameController.LangManager.GetTextValue("Msg.Think"));
        }

    }

    /// <summary>
    /// закончилось время
    /// </summary>
    public void EndTime()
    {
        if (My)
        {
            //Net.Instance.StartCheckTable();
        }
    }

    /// <summary>
    /// игрок пасует
    /// </summary>
    public void Pass()
    {
        print("Pass");
        CardAnim ca;
        if (Zanaveska != null)
            Zanaveska.SetActive(true);
        foreach (CardInHand card in Cards)
        {
            if (card.gameObject.activeSelf)
            {
                if (My)
                {
                    ca = card.GetComponent<CardAnim>();
                    if (ca != null)
                        ca.PutCard();
                }
                else
                {
                    card.Drop();
                }
            }
        }
        TimerP.Hide();
        MsgP.ShowMsgGray(GameController.LangManager.GetTextValue("Game.Pass"));
        InvokeRepeating("PassAnim", 0.3f, 0.1f);
    }

    /// <summary>
    /// показываем анимацию пасса
    /// </summary>
    public void PassAnim()
    {
        int id;
        TimerP.Hide();
        print("CardsInHands " + CardsInHands.Count);
        if (CardsInHands.Count > 0)
        {
            id = CardsInHands.First();
            CardsInHands.Remove(id);
            TableController.Instance.SendCard(id, TakePoint, TableController.Instance.Koloda.RTransform, EAnimTip.Close2Koloda);

        }
        else
        {
            CancelInvoke("PassAnim");
            print("закончили анмацию пасса");
        }
    }

    /// <summary>
    /// показываем анимацию открытия карты
    /// </summary>
    /// <param name="cardInHand"></param>
    /// <param name="instant">нужно пропустить анимацию?</param>
    public void OpenCard(int id, bool instant = false)
    {
        print("OpenCard " + id);
        CardsInHands.Remove(id);
        TimerP.Hide();
        if (!My)
        {
            _id2Open = id;
            if (instant)
            {
                OpenedCard = TableController.Instance.SendCard(id, OpenCardPos, OpenCardPos, EAnimTip.Instant);
            }
            else
            {
                Invoke("OpenCardAnim", 0.3f);
                foreach (CardInHand card in Cards)
                {
                    if (card.gameObject.activeSelf)
                    {
                        card.Drop();
                        break;
                    }
                }
            }
        }
        else
        {
            OpenedCard = TableController.Instance.SendCard(id, TableController.Instance.DraggedCardPosition,Quaternion.identity, OpenCardPos, EAnimTip.Close2Open);
        }
        if (!Away)
            MsgP.HideMsg();
    }
    /// <summary>
    /// показываем анимацию открытия карты
    /// </summary>
    public void OpenCardAnim()
    {
        print("OpenCardAnim " + _id2Open);
        OpenedCard = TableController.Instance.SendCard(_id2Open, TakePoint, OpenCardPos, EAnimTip.Close2Open);
        _id2Open = 0;
    }
    /// <summary>
    /// показываем анимацию взятки или просто добавляем их число 
    /// </summary>
    public void AddVziatka()
    {
        Vziatki++;
        if (Vziatki == 1)
        {
            //проявляем картинку взятки
            /* FloatTweener.TweenTo(0, 1, 10, p =>
             {
                 VziatkaImage.color = new Color(255f, 255f, 255f, p);
             }, Id + 200);*/
            VziatkaImage.ShowTake();
        }
        VziatkaImage.ShowTake();
        VziatkaText.text = Vziatki.ToString();
    }
    /// <summary>
    /// повышаем ставку
    /// </summary>
    /// <param name="sum"></param>
    public void Raise(double sum, bool showMsg = true)
    {
        print("Raise " + sum);
        TimerP.Hide();
        Fishki.gameObject.SetActive(true);
        int kol = (int)Math.Floor(sum / 10) + 1;
        if (kol > 5)
            kol = 5;
        TableController.Instance.SendFishki(Fishki.GetComponent<RectTransform>(), RectTransform, kol);
        if (showMsg)
            MsgP.ShowMsgWhite(GameController.LangManager.GetTextValue("Game.Raise") + sum);
        // MoneyText.text = (Double.Parse(MoneyText.text) + sum).ToString();
        //MsgP.HideMsg();
    }


    /// <summary>
    /// игрок уравнивает
    /// </summary>
    /// <param name="sum"></param>
    public void Call(double sum)
    {
        Fishki.gameObject.SetActive(true);
        TimerP.Hide();
        // MoneyText.text = (Double.Parse(MoneyText.text) + sum).ToString();
        int kol = (int)Math.Floor(sum / 10) + 1;
        if (kol > 5)
            kol = 5;
        TableController.Instance.SendFishki(Fishki.GetComponent<RectTransform>(), RectTransform, kol);
        MsgP.ShowMsgWhite(GameController.LangManager.GetTextValue("Game.Equalized"));
        // MsgP.HideMsg();
    }
    /// <summary>
    /// помечаем какие карты мы можем выкладывать на стол
    /// </summary>
    /// <param name="jSonMutableObject"></param>
    public void SetActiveCards(IJSonMutableObject obj)
    {
        bool flag;
        foreach (CardInHand card in Cards)
        {
            flag = false;
            for (int i = 0; i < obj.Count; i++)
            {
                 print(card.Id + "=" + obj[i].Int32Value);
                if (card.Id == obj[i].Int32Value)
                {
                    flag = true;
                    card.SetActive(true);
                }
            }
            if (!flag)
                card.SetActive(false);
        }
    }
    /// <summary>
    /// позволяем кликать по всем картам
    /// </summary>
    public void UnlockAllCards()
    {
        Invoke("UnlockAll", 1.5f);

    }

    public void UnlockAll()
    {
        foreach (CardInHand card in Cards)
        {
            card.SetActive(true);
        }
    }

    private int _lastCard = 0;
    public void ShowLastCard(int cardId,float interval=0)
    {
        _lastCard = cardId;
        Invoke("ShowLastCardIntervaled",interval);
    }

    public void ShowLastCardIntervaled()
    {
        print("ShowLastCard " + _lastCard);
        if (!My)
        {
            foreach (CardInHand card in Cards)
            {
                card.gameObject.SetActive(false);
            }
            GameObject go = (GameObject)Resources.Load("GUI/CardCont");
            go = (GameObject)Instantiate(go, TakePoint.position, Quaternion.identity);
            Card c = go.GetComponent<Card>();
            c.Back.gameObject.SetActive(false);
            c.Id = _lastCard;
            c.GetComponent<RectTransform>().SetParent(Fishki.transform);
            c.transform.localPosition = Vector3.zero;
            c.transform.localScale=Vector3.one;
            c.LastCardAnim(_lastCard);
            Destroy(go, 3f);
        }
    }
    /// <summary>
    /// убираем все взятки
    /// </summary>
    public void RemoveWins()
    {
        if (VziatkaImage != null)
            VziatkaImage.HideCards();
        VziatkaText.text = "";
    }
    /// <summary>
    /// инициируем как гостя
    /// </summary>
    /// <param name="id"></param>
    /// <param name="login"></param>
    /// <param name="toString"></param>
    /// <param name="flag"></param>
    public void InitGuest(int id, string login, string avatar, string flag)
    {
        print("InitGuest " + id + ":" + flag);
        TimerP.Hide();
        _cardsInHands = 0;
        Vziatki = 0;
        if (Zanaveska != null)
            Zanaveska.SetActive(false);
        CardsInHands = new List<int>();
        UCSS.HTTP.GetTexture(Net.ServerURL + avatar, new EventHandlerHTTPTexture(this.OnAvatarDownloaded), new EventHandlerServiceError(this.OnHTTPError), new EventHandlerServiceTimeOut(this.OnHTTPTimeOut), 10);
        UCSS.HTTP.GetTexture(Net.ServerURL + flag, new EventHandlerHTTPTexture(this.OnFlagDownloaded), new EventHandlerServiceError(this.OnHTTPError), new EventHandlerServiceTimeOut(this.OnHTTPTimeOut), 10);
        if (VziatkaImage != null)
            VziatkaImage.HideCards();
        VziatkaText.text = "";
        MoneyText.text = "";
        Id = id;
        if (NickText != null) NickText.text = login;
        foreach (CardInHand card in Cards)
        {
            card.gameObject.SetActive(false);
        }
        DealerImage.gameObject.SetActive(false);
        MsgP.HideMsg();
        gameObject.SetActive(true);
    }
}
