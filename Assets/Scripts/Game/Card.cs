using System.Collections.Generic;

using UnityEngine;
using System.Collections;

using UnityEngine.UI;

public enum ECardAction
{
    ToPlayerHand,
    ToTable,
    Koloda,
    ShowTrump,
    StayOnTable,
    Remove,
    RemoveAndShowKoloda
}

public class Card : MonoBehaviour
{
    public Image Back;
    public Image Front;
    public Image BigImage;

    public bool InMove = false;
    public Vector2 TargetPosition;
    public Quaternion TargetRotation;
    public RectTransform RTransform;
    public ECardAction ActionOnEnd;
    public bool EmmitCards = true;
    /// <summary>
    /// место куда двигаем карту
    /// </summary>
    public RectTransform Target;
    public Animation Anim;
    private CanvasGroup _canvasG;
    /// <summary>
    /// нужно ли прятать карту в движении
    /// </summary>
    public bool Hide = false;
    /// <summary>
    /// ссылка на игрока которому шлем карту
    /// </summary>
    private PlayerPanel _player;
    //  private RectTransform _rect;
    private int _id;
    private float _rotSpeed = 0;
    public int Id
    {
        get { return _id; }
        set
        {
           // print("card id " + value);
            _id = value;
            if (GameController.CardsID.ContainsKey(_id))
                Front.sprite = GameController.CardsID[value];
        }
    }

    void Awake()
    {
        RTransform = GetComponent<RectTransform>();
        _canvasG = GetComponent<CanvasGroup>();
    }
    /// <summary>
    /// проигрываем анимацию открытия карт
    /// </summary>
    public void OpenCard()
    {
        Anim.Play("Open");
        ActionOnEnd = ECardAction.StayOnTable;
    }

    /// <summary>
    /// показываем сразу открытой и оставляем на столе
    /// </summary>
    /// <param name="endPos"></param>
    public void InstantOpen(RectTransform endPos)
    {
        InMove = false;
        Anim.enabled = false;
        ActionOnEnd = ECardAction.StayOnTable;
        RTransform.position = endPos.position;
        Back.enabled = false;
    }
    /// <summary>
    /// двигаемся к указанной позиции
    /// </summary>
    /// <param name="pos"></param>
    public void MoveTo(RectTransform pos, Vector3 vt, float rot)
    {
        //print("[Card]MoveTo " + this.name + ":" + vt + ":" );
        TargetPosition = pos.position + vt;
        TargetRotation = Quaternion.Euler(0, 0, rot);
        _rotSpeed = (1000 / Vector2.Distance(RTransform.position, TargetPosition)) * 300f;
        FloatTweener.TweenTo(RTransform.localScale.x, pos.localScale.x, 10, p =>
        {
            if (RTransform != null)
                RTransform.localScale = new Vector3(p, p, p);
        }, Id);
        //   var direction = (pos.position - RectTransform.position).normalized;

        // direction.y = 0f;
        /* TargetRotation= Quaternion.LookRotation(direction);
         TargetRotation.x = 0;
         TargetRotation.y = 0;
         print(TargetRotation);*/
        InMove = true;

    }



    public void ReturnKoloda(Vector2 pos)
    {
       //  print("[Card]MoveTo Vector2 " + this.name + ":" + pos);
        TargetPosition = pos;
        TargetRotation = Quaternion.identity;
        InMove = true;
        ActionOnEnd = ECardAction.ShowTrump;
    }

    void Update()
    {
        if (InMove)
        {
            //двигаем к цели
            //  print(RectTransform.position + ":" + TargetPosition);

            RTransform.position = Vector2.MoveTowards(RTransform.position, TargetPosition, 1000f * Time.deltaTime);
            // RectTransform.Rotate(Vector3.forward,30f);
            RTransform.rotation = Quaternion.RotateTowards(RTransform.rotation, TargetRotation, Time.deltaTime * _rotSpeed);
            if (Hide && _canvasG != null)
            {
                _canvasG.alpha -= 0.005f;
                // print(_canvasG.alpha);
            }
            //RectTransform.rotation = Quaternion.Slerp(transform.rotation, TargetRotation, Time.deltaTime*2f);
            //    print(this+":"+RectTransform.rotation + ":" + TargetRotation);
            if (Vector2.Distance(RTransform.position, TargetPosition) < 1f)
            {
                //  print("[Card]EndMove " + Vector2.Distance(RectTransform.position, TargetPosition) + ":" + RectTransform.position + ":" + TargetPosition);
                //достигли цели совершаем следующее действие
                InMove = false;
                switch (ActionOnEnd)
                {
                    case ECardAction.ToPlayerHand:
                        //игроку в руку
                        //  print(Target.gameObject);
                        if (_player != null) _player.AddCard(this);
                        Destroy(this.gameObject);
                        break;
                    case ECardAction.Koloda:
                        if (EmmitCards)
                            TableController.Instance.SendCardsToPlayers();
                        break;
                    case ECardAction.ShowTrump:
                        TableController.Instance.TrumpCard = TableController.Instance.SendCard(TableController.Trump, RTransform,
                        TableController.Instance.TrumpPos, EAnimTip.Close2Open, true);
                        // TableController.Instance.SetActivePlayer(TableController.Dealer.Id);
                        //TableController.Instance.StartAuction();
                        break;
                    case ECardAction.StayOnTable:
                        break;
                    case ECardAction.Remove:
                        //просто убираем карту
                        Destroy(this.gameObject);
                        break;
                    case ECardAction.RemoveAndShowKoloda:
                        //просто убираем карту
                        Destroy(this.gameObject);
                        TableController.Instance.Koloda.gameObject.SetActive(true);
                        break;
                }
                Target = null;
            }
        }
    }
    /// <summary>
    /// указываем цель куда переместить карту
    /// </summary>
    /// <param name="pos"></param>
    public void SetTarget(RectTransform pos, PlayerPanel player = null)
    {
        ActionOnEnd = ECardAction.ToPlayerHand;
        Target = pos;
        _player = player;
    }
    /// <summary>
    /// отпарвяем карту к целе с задержкой
    /// </summary>
    /// <param name="delay"></param>
    public void SendCartWithDelay(float delay)
    {
        Invoke("SendCartInHand", delay);
    }
    /// <summary>
    /// показываем анимацию перемешения к цели заданной через SetTarget
    /// </summary>
    public void SendCartInHand()
    {
        // print(Target+":"+gameObject.name);
        if (Target == null)
        {
            // Debug.Break();
            Destroy(this.gameObject);
        }
        else
        {
            MoveTo(Target, Vector3.zero, 0);
            Hide = true;
        }
    }

    public void Open2Open()
    {
        Anim.enabled = false;
        FloatTweener.TweenTo(0, 1, 10, p =>
        {
            if (_canvasG != null)
                _canvasG.alpha = p;
        }, Id + 100);
    }
    /// <summary>
    /// анимация при пасе
    /// </summary>
    public void Close2Koloda()
    {
        ActionOnEnd = ECardAction.Remove;
        Anim.enabled = false;
        Invoke("Destr",2f);
        FloatTweener.TweenTo(1, 10, 10, p =>
        {
            if (_canvasG != null)
                _canvasG.alpha = p;
        }, Id + 101);
    }

    public void Destr()
    {
        if (this.gameObject!=null)
            Destroy(this.gameObject);
    }
    public void ShowWinAnim()
    {
        print("ShowWinAnim "+Anim);
        if (Anim != null) Anim.Play("Win");
        else if(gameObject!=null)Destroy(gameObject);
    }

    /// <summary>
    /// показываем анимацию последней карты
    /// </summary>
    /// <param name="cardId"></param>
    public void LastCardAnim(int cardId)
    {
        Anim.Play("LastCard");
    }
}
