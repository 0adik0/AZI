using UnityEngine;
using System.Collections;

using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CardAnim : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{

    private Animator _animator;
    private CanvasGroup _canvasGroup;
    public Image Front;
    private CardInHand _card;
    //  private RectTransform _rect;
    private bool _isOpen = false;
    private int _id = 0;
    public bool IsOpen
    {
        get
        {
            if (_animator != null)
            {
                return _animator.GetBool("IsOpen");
            }
            else
            {
                return _isOpen;
            }
        }
        set
        {
            if (_animator != null)
            {
                _animator.SetBool("IsOpen", value);
                if (_canvasGroup != null) _canvasGroup.blocksRaycasts = _canvasGroup.interactable = value;
            }
            else
            {
                _isOpen = value;
                if (_canvasGroup != null) _canvasGroup.blocksRaycasts = _canvasGroup.interactable = value;
            }
        }
    }
    private Quaternion _startRot;

    private Vector3 _startPos;

    public void Init(int id)
    {
        print("Init card " + id);
        _id = id;
        transform.position = _startPos;
        transform.rotation = _startRot;
        if (_animator != null)
        {
            _animator.SetTrigger("Active");
            _animator.ResetTrigger("PutCard");
        }
        Front.sprite = GameController.CardsID[id];

    }
    public void Take()
    {
        if (_animator != null) _animator.SetTrigger("Take");
    }

    public void PutCard()
    {
        if (_animator != null)
        {
            _animator.ResetTrigger("Active");
            _animator.SetTrigger("PutCard");
        }
        else
        {
            print("используем анимацию");

        }
        Invoke("HideCard", 0.5f);
    }
    /// <summary>
    /// отключаем карту
    /// </summary>
    public void HideCard()
    {
        gameObject.SetActive(false);
    }
    public void OnClick(bool needSend = true)
    {
        //       print((TableController.ActivePlayer == TableController.MainPlayer) + ":" + TableController.CurrentRound + ":" + TableController.MainPlayer.CardOpened);
        if (TableController.ActivePlayer == TableController.MainPlayer && TableController.CurrentRound > 1 && !TableController.MainPlayer.CardOpened)
        {
            TableController.MainPlayer.CardOpened = true;
            if (needSend)
            {
                Net.OpenCard(_id);
                // PutCard();
                // Invoke("OpenAnim", 0.3f);
            }


        }
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        TableController.Instance.DraggedCardPosition = Vector3.zero;
        //print("OnBeginDrag "+this);
        transform.rotation = Quaternion.identity;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        //print("OnEndDrag " + this);
        //print(transform.position.y+":"+(transform.position.y < 350 || (TableController.ActivePlayer == TableController.MainPlayer && TableController.CurrentRound > 1 ))+":"+(TableController.ActivePlayer == TableController.MainPlayer)+":"+(TableController.CurrentRound > 1));
        if (transform.position.y < 350 || TableController.ActivePlayer != TableController.MainPlayer || TableController.CurrentRound <= 1)
        {
            transform.position = _startPos;
            transform.rotation = _startRot;
        }
        else
        {
            TableController.Instance.DraggedCardPosition = transform.position;
            OnClick(true);
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        //        print("OnDrag " + this);
        transform.Translate(eventData.delta);
    }
    public void OpenCard()
    {
        print("OpenCard");
        PutCard();
        Invoke("OpenAnimMain", 0.3f);
    }
    public void OpenAnim()
    {
        TableController.ActivePlayer.OpenCard(_id);
    }
    public void OpenAnimMain()
    {
        TableController.MainPlayer.OpenCard(_id);
    }
    public void OnOver()
    {
        //print("OnOver");
        _animator.SetBool("Over", true);
    }
    public void OnEndOver()
    {
        // print("OnEndOver");
        _animator.SetBool("Over", false);
    }
    public void Awake()
    {
        _animator = GetComponent<Animator>();
        _canvasGroup = GetComponent<CanvasGroup>();
        _card = GetComponent<CardInHand>();
        _startRot = transform.rotation;
        _startPos = transform.position;
        //_rect = GetComponent<RectTransform>();
        //  _rect.offsetMax = _rect.offsetMin = new Vector2(0, 0);
    }

}
